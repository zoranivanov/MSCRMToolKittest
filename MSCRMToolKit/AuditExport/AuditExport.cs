// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        18/10/2012
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// Audit Export class
    /// </summary>
    public partial class AuditExport : Form
    {
        /// <summary>
        /// The Connections Manager
        /// </summary>
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The current profile
        /// </summary>
        internal MSCRMAuditExportProfile currentProfile;
        /// <summary>
        /// The MSCRM Audit Export Manager
        /// </summary>
        internal MSCRMAuditExportManager man = new MSCRMAuditExportManager();
        /// <summary>
        /// The Environment Audit Structure
        /// </summary>
        private EnvAuditStructure es = null;
        /// <summary>
        /// The selected entity list
        /// </summary>
        internal List<SelectedAuditEntity> SelectedEntityList = new List<SelectedAuditEntity>();
        /// <summary>
        /// The selected entities initial loading
        /// </summary>
        private bool selectedEntitiesInitialLoading = true;
        /// <summary>
        /// The audit type
        /// </summary>
        internal string AuditType = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditExport"/> class.
        /// </summary>
        public AuditExport()
        {
            InitializeComponent();
            LogManager.WriteLog("Audit Export Manager launched.");

            if (man.Profiles != null)
            {
                foreach (MSCRMAuditExportProfile profile in man.Profiles)
                {
                    this.comboBoxProfiles.Items.AddRange(new object[] { profile.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<MSCRMAuditExportProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }
        }

        /// <summary>
        /// Handles the Load event of the AuditExport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AuditExport_Load(object sender, EventArgs e)
        {
            comboBoxEncoding.SelectedIndex = 0;
            dateTimePickerAuditCreatedFrom.Format = DateTimePickerFormat.Custom;
            dateTimePickerAuditCreatedFrom.CustomFormat = "dd/MM/yyyy";
            dateTimePickerAuditCreatedFrom.Value = DateTime.Now;
            dateTimePickerAuditCreatedTo.Format = DateTimePickerFormat.Custom;
            dateTimePickerAuditCreatedTo.CustomFormat = "dd/MM/yyyy";
            dateTimePickerAuditCreatedTo.Value = DateTime.Now;
        }

        /// <summary>
        /// Saves the profile.
        /// </summary>
        /// <returns>True or False if the profile was succesfuly saved.</returns>
        private bool SaveProfile()
        {
            bool result = true;
            //Check that all fields are provided
            if (string.IsNullOrEmpty(textBoxProfileName.Text))
            {
                MessageBox.Show("Profile Name is mandatory!");
                return false;
            }

            //Check that the name of the connection is valid
            if (textBoxProfileName.Text.Contains(" ") ||
                    textBoxProfileName.Text.Contains("\\") ||
                    textBoxProfileName.Text.Contains("/") ||
                    textBoxProfileName.Text.Contains(">") ||
                    textBoxProfileName.Text.Contains("<") ||
                    textBoxProfileName.Text.Contains("?") ||
                    textBoxProfileName.Text.Contains("*") ||
                    textBoxProfileName.Text.Contains(":") ||
                    textBoxProfileName.Text.Contains("|") ||
                    textBoxProfileName.Text.Contains("\"") ||
                    textBoxProfileName.Text.Contains("'")
                    )
            {
                MessageBox.Show("You shouldn't use spaces nor the following characters (\\/<>?*:|\"') in the Profile Name as it will be used to create folders and files.");
                return false;
            }

            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a Source for the Profile");
                return false;
            }

            if (comboBoxAuditTypeExport.SelectedItem == null)
            {
                MessageBox.Show("You must select an Audit Type to Export for the Profile");
                return false;
            }

            if (comboBoxFormat.SelectedItem == null)
            {
                MessageBox.Show("You must select an Export Format for the Profile");
                return false;
            }

            if (checkedListBoxActions.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select a least 1 Action");
                return false;
            }
            if (checkedListBoxOperations.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select a least 1 Operation");
                return false;
            }
            if (checkedListBoxUsers.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select a least 1 User");
                return false;
            }
            if (comboBoxAuditTypeExport.SelectedItem.ToString() != "User Acces Audit" && checkedListBoxEntities.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select a least 1 Entity");
                return false;
            }
            if (comboBoxAuditTypeExport.SelectedItem.ToString() == "Attribute Change History")
            {
                List<SelectedAuditEntity> SAEList = SelectedEntityList.Where(x => (x.SelectedAttributes == null || x.SelectedAttributes.Count == 0)).ToList<SelectedAuditEntity>();

                if (SAEList.Count > 0)
                {
                    MessageBox.Show("You must select 1 Attribute for each selected Entity!");
                    return false;
                }
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Data Export Profile having the same name exist already
                MSCRMAuditExportProfile existingProfile = man.Profiles.Find(d => d.ProfileName.ToLower() == textBoxProfileName.Text.ToLower());
                if (existingProfile != null)
                {
                    MessageBox.Show("Profile with the name " + textBoxProfileName.Text + " exist already. Please select another name");
                    return false;
                }

                MSCRMAuditExportProfile newProfile = new MSCRMAuditExportProfile();
                newProfile.ProfileName = textBoxProfileName.Text;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.setSourceConneciton();
                newProfile.ExportFormat = comboBoxFormat.SelectedItem.ToString();
                newProfile.AuditType = comboBoxAuditTypeExport.SelectedItem.ToString();

                newProfile.Encoding = "Default";
                if (comboBoxEncoding.SelectedIndex == 1)
                    newProfile.Encoding = "UTF8";
                else if (comboBoxEncoding.SelectedIndex == 1)
                    newProfile.Encoding = "Unicode";
                else if (comboBoxEncoding.SelectedIndex == 2)
                    newProfile.Encoding = "ASCII";
                else if (comboBoxEncoding.SelectedIndex == 3)
                    newProfile.Encoding = "BigEndianUnicode";

                newProfile.SelectedActions = new List<int>();
                if (checkedListBoxActions.CheckedItems.Count == checkedListBoxActions.Items.Count)
                {
                    newProfile.AllActionsSelected = true;
                }
                else
                {
                    newProfile.AllActionsSelected = false;
                    foreach (ActionListBoxItem checkedAction in checkedListBoxActions.CheckedItems)
                    {
                        newProfile.SelectedActions.Add(checkedAction.Key);
                    }
                }

                newProfile.SelectedOperations = new List<int>();
                if (checkedListBoxOperations.CheckedItems.Count == checkedListBoxOperations.Items.Count)
                {
                    newProfile.AllOperationsSelected = true;
                }
                else
                {
                    newProfile.AllOperationsSelected = false;
                    for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
                    {
                        if (checkedListBoxOperations.CheckedItems.IndexOf(checkedListBoxOperations.Items[i]) > -1)
                        {
                            KeyValuePair<int, string> u = new KeyValuePair<int, string>(i + 1, checkedListBoxOperations.Items[i].ToString());
                            newProfile.SelectedOperations.Add(u.Key);
                        }
                    }
                }

                newProfile.SelectedUsers = new List<AuditUser>();
                if (checkedListBoxUsers.CheckedItems.Count == checkedListBoxUsers.Items.Count)
                {
                    newProfile.AllUsersSelected = true;
                }
                else
                {
                    newProfile.AllUsersSelected = false;
                    foreach (CheckListBoxItem checkedUser in checkedListBoxUsers.CheckedItems)
                    {
                        AuditUser u = new AuditUser { Id = checkedUser.Id, FullName = checkedUser.Text };
                        newProfile.SelectedUsers.Add(u);
                    }
                }

                newProfile.SelectedEntities = new List<SelectedAuditEntity>();
                if (checkedListBoxEntities.CheckedItems.Count == checkedListBoxEntities.Items.Count)
                    newProfile.AllEntitiesSelected = true;
                else
                    newProfile.AllEntitiesSelected = false;

                foreach (EntityListBoxItem checkedEntity in checkedListBoxEntities.CheckedItems)
                {
                    SelectedAuditEntity ee = new SelectedAuditEntity();
                    ee.LogicalName = checkedEntity.Value;
                    ee.ObjectTypeCode = checkedEntity.ObjectTypeCode;

                    SelectedAuditEntity seForIgnoredAttributes = SelectedEntityList.Find(match => match.LogicalName == checkedEntity.Value);
                    if (seForIgnoredAttributes != null)
                    {
                        ee.SelectedAttributes = seForIgnoredAttributes.SelectedAttributes;
                        ee.Filter = seForIgnoredAttributes.Filter;
                    }

                    newProfile.SelectedEntities.Add(ee);
                }

                if (comboBoxAuditRecordCreatedOnFilter.SelectedItem != null)
                    newProfile.AuditRecordCreatedOnFilter = comboBoxAuditRecordCreatedOnFilter.SelectedItem.ToString();
                newProfile.AuditRecordCreatedOnFilterLastX = numericUpDownLastX.Value;
                newProfile.AuditRecordCreatedOnFilterFrom = dateTimePickerAuditCreatedFrom.Value;
                newProfile.AuditRecordCreatedOnFilterTo = dateTimePickerAuditCreatedTo.Value;

                man.CreateProfile(newProfile);
                currentProfile = newProfile;
                comboBoxProfiles.Items.AddRange(new object[] { newProfile.ProfileName });
                comboBoxProfiles.SelectedItem = newProfile.ProfileName;
            }
            else
            {
                currentProfile.ProfileName = textBoxProfileName.Text;
                currentProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                currentProfile.ExportFormat = comboBoxFormat.SelectedItem.ToString();
                currentProfile.AuditType = comboBoxAuditTypeExport.SelectedItem.ToString();

                currentProfile.Encoding = "Default";
                if (comboBoxEncoding.SelectedIndex == 1)
                    currentProfile.Encoding = "UTF8";
                else if (comboBoxEncoding.SelectedIndex == 1)
                    currentProfile.Encoding = "Unicode";
                else if (comboBoxEncoding.SelectedIndex == 2)
                    currentProfile.Encoding = "ASCII";
                else if (comboBoxEncoding.SelectedIndex == 3)
                    currentProfile.Encoding = "BigEndianUnicode";

                currentProfile.SelectedActions = new List<int>();
                if (checkedListBoxActions.CheckedItems.Count == checkedListBoxActions.Items.Count)
                {
                    currentProfile.AllActionsSelected = true;
                }
                else
                {
                    currentProfile.AllActionsSelected = false;
                    foreach (ActionListBoxItem checkedAction in checkedListBoxActions.CheckedItems)
                    {
                        currentProfile.SelectedActions.Add(checkedAction.Key);
                    }
                }

                currentProfile.SelectedOperations = new List<int>();
                if (checkedListBoxOperations.CheckedItems.Count == checkedListBoxOperations.Items.Count)
                {
                    currentProfile.AllOperationsSelected = true;
                }
                else
                {
                    currentProfile.AllOperationsSelected = false;
                    for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
                    {
                        if (checkedListBoxOperations.CheckedItems.IndexOf(checkedListBoxOperations.Items[i]) > -1)
                        {
                            KeyValuePair<int, string> u = new KeyValuePair<int, string>(i + 1, checkedListBoxOperations.Items[i].ToString());
                            currentProfile.SelectedOperations.Add(u.Key);
                        }
                    }
                }

                currentProfile.SelectedUsers = new List<AuditUser>();
                if (checkedListBoxUsers.CheckedItems.Count == checkedListBoxUsers.Items.Count)
                {
                    currentProfile.AllUsersSelected = true;
                }
                else
                {
                    currentProfile.AllUsersSelected = false;
                    foreach (CheckListBoxItem checkedUser in checkedListBoxUsers.CheckedItems)
                    {
                        AuditUser u = new AuditUser { Id = checkedUser.Id, FullName = checkedUser.Text };
                        currentProfile.SelectedUsers.Add(u);
                    }
                }

                currentProfile.SelectedEntities = new List<SelectedAuditEntity>();
                if (checkedListBoxEntities.CheckedItems.Count == checkedListBoxEntities.Items.Count)
                    currentProfile.AllEntitiesSelected = true;
                else
                    currentProfile.AllEntitiesSelected = false;

                foreach (EntityListBoxItem checkedEntity in checkedListBoxEntities.CheckedItems)
                {
                    SelectedAuditEntity ee = new SelectedAuditEntity();
                    ee.LogicalName = checkedEntity.Value;
                    ee.ObjectTypeCode = checkedEntity.ObjectTypeCode;

                    SelectedAuditEntity seForIgnoredAttributes = SelectedEntityList.Find(match => match.LogicalName == checkedEntity.Value);
                    if (seForIgnoredAttributes != null)
                    {
                        ee.SelectedAttributes = seForIgnoredAttributes.SelectedAttributes;
                        ee.Filter = seForIgnoredAttributes.Filter;
                    }

                    currentProfile.SelectedEntities.Add(ee);
                }

                if (comboBoxAuditRecordCreatedOnFilter.SelectedItem != null)
                    currentProfile.AuditRecordCreatedOnFilter = comboBoxAuditRecordCreatedOnFilter.SelectedItem.ToString();
                currentProfile.AuditRecordCreatedOnFilterLastX = numericUpDownLastX.Value;
                currentProfile.AuditRecordCreatedOnFilterFrom = dateTimePickerAuditCreatedFrom.Value;
                currentProfile.AuditRecordCreatedOnFilterTo = dateTimePickerAuditCreatedTo.Value;

                currentProfile.setSourceConneciton();
                MSCRMAuditExportProfile oldDEP = man.GetProfile(currentProfile.ProfileName);
                man.UpdateProfile(currentProfile);
            }

            //runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = "Profile " + currentProfile.ProfileName + " saved.";
            LogManager.WriteLog("Profile " + currentProfile.ProfileName + " saved.");
            return result;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxConnectionSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            selectedEntitiesInitialLoading = true;
            populateCheckedListBoxes();
            displaySelectedEntities();
            selectedEntitiesInitialLoading = false;
            if (comboBoxConnectionSource.SelectedItem == null)
                buttonLoadEntities.Enabled = false;
            else
                buttonLoadEntities.Enabled = true;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxProfiles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            comboBoxConnectionSource.SelectedItem = null;
            if (comboBoxProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxProfiles.SelectedIndex];
                SelectedEntityList = currentProfile.SelectedEntities;
                textBoxProfileName.Text = currentProfile.ProfileName;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                comboBoxFormat.SelectedItem = currentProfile.ExportFormat;
                comboBoxAuditTypeExport.SelectedItem = currentProfile.AuditType;

                comboBoxEncoding.SelectedItem = "Default";
                if (currentProfile.Encoding == "UTF8")
                    comboBoxEncoding.SelectedItem = "UTF8";
                else if (currentProfile.Encoding == "Unicode")
                    comboBoxEncoding.SelectedItem = "Unicode";
                else if (currentProfile.Encoding == "ASCII")
                    comboBoxEncoding.SelectedItem = "ASCII";
                else if (currentProfile.Encoding == "BigEndianUnicode")
                    comboBoxEncoding.SelectedItem = "BigEndianUnicode";

                comboBoxAuditRecordCreatedOnFilter.SelectedItem = currentProfile.AuditRecordCreatedOnFilter;
                numericUpDownLastX.Value = currentProfile.AuditRecordCreatedOnFilterLastX;
                dateTimePickerAuditCreatedFrom.Value = currentProfile.AuditRecordCreatedOnFilterFrom;
                dateTimePickerAuditCreatedTo.Value = currentProfile.AuditRecordCreatedOnFilterTo;

                deleteProfileToolStripMenuItem.Enabled = true;
                newToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                textBoxProfileName.Enabled = false;
                runProfileToolStripMenuItem.Enabled = true;
                comboBoxAuditTypeExport.Enabled = false;
                comboBoxConnectionSource.Enabled = false;
            }
            else
            {
                currentProfile = null;
                textBoxProfileName.Text = "";
                comboBoxFormat.SelectedItem = null;
                comboBoxAuditTypeExport.SelectedItem = null;
                comboBoxAuditTypeExport.Enabled = true;
                deleteProfileToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                textBoxProfileName.Enabled = true;
                runProfileToolStripMenuItem.Enabled = false;
                comboBoxConnectionSource.Enabled = true;
            }

            buttonOpenInExcel.Visible = false;
            //dataExportReportToolStripMenuItem.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the buttonLoadEntities control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonLoadEntities_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            DialogResult dResTest = MessageBox.Show("Loading the structure may take some time. The application will become unresponsive during this time.\n\nAre you sure you want to load the structure from the Source?", "Confirm Structure Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }

            toolStripStatusLabel1.Text = "Loading structure. Please wait...";
            Application.DoEvents();

            try
            {
                es = man.downloadEnvAuditStructure(comboBoxConnectionSource.SelectedItem.ToString());
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }

            if (es == null)
                return;

            string structureFileName = man.Folder + "\\" + comboBoxConnectionSource.SelectedItem.ToString() + ".xml";
            DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
            labelStructureLastLoadedDate.Text = structureRefreshedOn.ToString();

            populateCheckedListBoxes();

            toolStripStatusLabel1.Text = "Structure successfully loaded";
        }

        /// <summary>
        /// Populates the checked list boxes.
        /// </summary>
        private void populateCheckedListBoxes()
        {
            checkedListBoxActions.Items.Clear();
            checkedListBoxOperations.Items.Clear();
            checkedListBoxUsers.Items.Clear();
            checkedListBoxEntities.Items.Clear();

            //If Structure already downloaded display entities list
            if (comboBoxConnectionSource.SelectedItem == null)
                return;

            try
            {
                es = man.ReadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while reading the struction of connection " + comboBoxConnectionSource.SelectedItem.ToString() + ". The structure file may be corrupted.\r\n\r\n" + ex.Message);
                return;
            }
            labelStructureLastLoadedDate.Text = "never";
            if (es != null)
            {
                string structureFileName = man.Folder + "\\" + comboBoxConnectionSource.SelectedItem + ".xml";
                DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
                labelStructureLastLoadedDate.Text = structureRefreshedOn.ToString();

                //Fill Actions checkedListBox
                foreach (KeyValuePair<int, string> aa in es.Actions)
                {
                    checkedListBoxActions.Items.Add(new ActionListBoxItem { Key = aa.Key, Value = aa.Value });
                    if (currentProfile == null || (currentProfile != null && currentProfile.AllActionsSelected))
                    {
                        checkedListBoxActions.SetItemChecked(checkedListBoxActions.Items.Count - 1, true);
                        continue;
                    }

                    if (currentProfile.SelectedActions == null)
                        continue;

                    foreach (int a in currentProfile.SelectedActions)
                    {
                        if (aa.Key == a)
                            checkedListBoxActions.SetItemChecked(checkedListBoxActions.Items.Count - 1, true);
                    }
                }

                //Fill Operations checkedListBox
                foreach (KeyValuePair<int, string> oo in es.Operations)
                {
                    checkedListBoxOperations.Items.Add(new ActionListBoxItem { Key = oo.Key, Value = oo.Value });
                    if (currentProfile == null || (currentProfile != null && currentProfile.AllOperationsSelected))
                    {
                        checkedListBoxOperations.SetItemChecked(checkedListBoxOperations.Items.Count - 1, true);
                        continue;
                    }

                    if (currentProfile.SelectedOperations == null)
                        continue;

                    foreach (int o in currentProfile.SelectedOperations)
                    {
                        if (oo.Key == o)
                            checkedListBoxOperations.SetItemChecked(checkedListBoxOperations.Items.Count - 1, true);
                    }
                }

                //Fill Users checkedListBox
                foreach (AuditUser u in es.Users)
                {
                    checkedListBoxUsers.Items.Add(new CheckListBoxItem { Id = u.Id, Text = u.FullName });
                    if (currentProfile == null || (currentProfile != null && currentProfile.AllUsersSelected))
                    {
                        checkedListBoxUsers.SetItemChecked(checkedListBoxUsers.Items.Count - 1, true);
                        continue;
                    }

                    if (currentProfile.SelectedUsers == null)
                        continue;

                    foreach (AuditUser ee1 in currentProfile.SelectedUsers)
                    {
                        if (u.Id == ee1.Id)
                            checkedListBoxUsers.SetItemChecked(checkedListBoxUsers.Items.Count - 1, true);
                    }
                }

                //Fill Entities checkedListBox
                foreach (EnvAuditEntity ee in es.Entities)
                {
                    checkedListBoxEntities.Items.AddRange(new object[] { new EntityListBoxItem { Value = ee.LogicalName, ObjectTypeCode = ee.ObjectTypeCode } });

                    if (currentProfile == null)
                        continue;
                    if (currentProfile != null && currentProfile.AllEntitiesSelected)
                    {
                        checkedListBoxEntities.SetItemChecked(checkedListBoxEntities.Items.Count - 1, true);
                        continue;
                    }
                    if (currentProfile.SelectedEntities == null)
                        continue;

                    foreach (SelectedAuditEntity ee1 in currentProfile.SelectedEntities)
                    {
                        if (ee.LogicalName == ee1.LogicalName)
                            checkedListBoxEntities.SetItemChecked(checkedListBoxEntities.Items.Count - 1, true);
                    }
                }

                if (currentProfile == null)
                {
                    comboBoxActionsSelector.SelectedIndex = 0;
                    comboBoxOperationsSelector.SelectedIndex = 0;
                    comboBoxUsersSelector.SelectedIndex = 0;
                    comboBoxEntitiesSelector.SelectedIndex = 1;
                }
                else
                {
                    if (checkedListBoxActions.CheckedItems.Count == checkedListBoxActions.Items.Count)
                        comboBoxActionsSelector.SelectedItem = "All";
                    else if (checkedListBoxActions.CheckedItems.Count == 0)
                        comboBoxActionsSelector.SelectedItem = "None";
                    else
                        comboBoxActionsSelector.SelectedItem = "Custom";

                    if (checkedListBoxOperations.CheckedItems.Count == checkedListBoxOperations.Items.Count)
                        comboBoxOperationsSelector.SelectedItem = "All";
                    else if (checkedListBoxOperations.CheckedItems.Count == 0)
                        comboBoxOperationsSelector.SelectedItem = "None";
                    else
                        comboBoxOperationsSelector.SelectedItem = "Custom";

                    if (checkedListBoxUsers.CheckedItems.Count == checkedListBoxUsers.Items.Count)
                        comboBoxUsersSelector.SelectedItem = "All";
                    else if (checkedListBoxUsers.CheckedItems.Count == 0)
                        comboBoxUsersSelector.SelectedItem = "None";
                    else
                        comboBoxUsersSelector.SelectedItem = "Custom";

                    if (checkedListBoxEntities.CheckedItems.Count == checkedListBoxEntities.Items.Count)
                        comboBoxEntitiesSelector.SelectedItem = "All";
                    else if (checkedListBoxEntities.CheckedItems.Count == 0)
                        comboBoxEntitiesSelector.SelectedItem = "None";
                    else
                        comboBoxEntitiesSelector.SelectedItem = "Custom";
                }
            }
        }

        /// <summary>
        /// Displays the selected entities.
        /// </summary>
        private void displaySelectedEntities()
        {
            //Remove all the combobox and labels
            ArrayList list = new ArrayList(panelSelectedEntities.Controls);
            foreach (Control c in list)
            {
                panelSelectedEntities.Controls.Remove(c);
            }

            if (checkedListBoxEntities.Items.Count == 0)
                return;

            int transportOrderYLocation = 7;
            IOrderedEnumerable<SelectedAuditEntity> se = null;
            List<string> treatedEntities = new List<string>();
            if (currentProfile != null)
            {
                if (currentProfile.SelectedEntities == null)
                    currentProfile.SelectedEntities = new List<SelectedAuditEntity>();
                se = currentProfile.SelectedEntities.OrderBy(ee => ee.LogicalName);

                foreach (SelectedAuditEntity ee in se)
                {
                    EntityListBoxItem elbi = new EntityListBoxItem { ObjectTypeCode = ee.ObjectTypeCode, Value = ee.LogicalName };
                    //check if the SelectedEntity is still checked
                    if (!checkedListBoxEntities.CheckedItems.Contains(elbi))
                        continue;

                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Text = ee.LogicalName;
                    label.Location = new System.Drawing.Point(0, transportOrderYLocation + 4);
                    label.Size = new System.Drawing.Size(210, 15);
                    panelSelectedEntities.Controls.Add(label);

                    if (this.AuditType != "Audit Summary View")
                    {
                        Button setupIgnoredAttributes = new Button();
                        setupIgnoredAttributes.Name = "button_" + ee.LogicalName;
                        setupIgnoredAttributes.Text = "...";
                        setupIgnoredAttributes.Tag = ee.LogicalName;
                        setupIgnoredAttributes.Size = new System.Drawing.Size(25, 23);
                        setupIgnoredAttributes.Location = new System.Drawing.Point(215, transportOrderYLocation - 1);
                        setupIgnoredAttributes.Click += new System.EventHandler(this.showAuditExportDetailsWizard);
                        panelSelectedEntities.Controls.Add(setupIgnoredAttributes);
                    }
                    transportOrderYLocation += 25;
                    treatedEntities.Add(ee.LogicalName);
                }
            }

            foreach (EntityListBoxItem checkedEntity in checkedListBoxEntities.CheckedItems)
            {
                if (treatedEntities.Contains(checkedEntity.Value))
                    continue;
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Text = checkedEntity.Value;
                label.Location = new System.Drawing.Point(0, transportOrderYLocation + 4);
                label.Size = new System.Drawing.Size(210, 15);
                panelSelectedEntities.Controls.Add(label);
                if (this.AuditType != "Audit Summary View")
                {
                    Button setupIgnoredAttributes = new Button();
                    setupIgnoredAttributes.Name = "button_" + checkedEntity;
                    setupIgnoredAttributes.Text = "...";
                    setupIgnoredAttributes.Tag = checkedEntity;
                    setupIgnoredAttributes.Size = new System.Drawing.Size(25, 23);
                    setupIgnoredAttributes.Location = new System.Drawing.Point(215, transportOrderYLocation - 1);
                    setupIgnoredAttributes.Click += new System.EventHandler(this.showAuditExportDetailsWizard);
                    panelSelectedEntities.Controls.Add(setupIgnoredAttributes);
                }
                transportOrderYLocation += 25;
            }
        }

        /// <summary>
        /// Shows the audit export details wizard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void showAuditExportDetailsWizard(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            Button b = (Button)sender;
            string entityName = b.Name.Replace("button_", "");
            AuditExportDetails sia = new AuditExportDetails(this, entityName);
            sia.Show();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the checkedListBoxEntities control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkedListBoxEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (checkedListBoxEntities.CheckedItems.Count == checkedListBoxEntities.Items.Count)
                comboBoxEntitiesSelector.SelectedItem = "All";
            else if (checkedListBoxEntities.CheckedItems.Count == 0)
                comboBoxEntitiesSelector.SelectedItem = "None";
            else
                comboBoxEntitiesSelector.SelectedItem = "Custom";

            CheckedListBox clb = (CheckedListBox)sender;
            EntityListBoxItem elbi = (EntityListBoxItem)clb.SelectedItem;
            if (clb.CheckedItems.Contains(clb.SelectedItem))
            {
                if (SelectedEntityList.Find(m => m.LogicalName == elbi.Value) == null)
                    SelectedEntityList.Add(new SelectedAuditEntity { LogicalName = elbi.Value, ObjectTypeCode = elbi.ObjectTypeCode });
            }
            else
            {
                int index = SelectedEntityList.FindIndex(m => m.LogicalName == elbi.Value);
                if (index != -1)
                    SelectedEntityList.RemoveAt(index);
            }

            //Check if this is not the initial loading to improve performance
            if (!selectedEntitiesInitialLoading)
                displaySelectedEntities();
        }

        /// <summary>
        /// Handles the Click event of the newToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            comboBoxProfiles.SelectedItem = null;
            textBoxProfileName.Text = "";
            textBoxProfileName.Enabled = true;
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxEncoding.SelectedIndex = 0;
            newToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;

            comboBoxUsersSelector.SelectedItem = null;
            comboBoxEntitiesSelector.SelectedItem = null;
            comboBoxActionsSelector.SelectedItem = null;
            comboBoxOperationsSelector.SelectedItem = null;
        }

        /// <summary>
        /// Handles the Click event of the saveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            SaveProfile();
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the deleteProfileToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            string currentProfileName = currentProfile.ProfileName;
            DialogResult dResTest;
            dResTest = MessageBox.Show("Are you sure you want to delete this Profile ?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }
            else
            {
                comboBoxProfiles.Items.Remove(currentProfile.ProfileName);
                comboBoxProfiles.SelectedItem = null;
                man.DeleteProfile(currentProfile);
                currentProfile = null;
                textBoxProfileName.Text = "";
                textBoxProfileName.Enabled = true;
                comboBoxConnectionSource.SelectedItem = null;
                toolStripStatusLabel1.Text = "Profile " + currentProfileName + " deleted";
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxAuditTypeExport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxAuditTypeExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            ComboBox temp = (ComboBox)sender;
            this.AuditType = temp.Text;

            displaySelectedEntities();

            if (this.AuditType == "User Acces Audit")
            {
                //Remove all the combobox and labels
                ArrayList list = new ArrayList(panelSelectedEntities.Controls);
                foreach (Control c in list)
                {
                    panelSelectedEntities.Controls.Remove(c);
                }

                for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
                {
                    ActionListBoxItem Operation = (ActionListBoxItem)checkedListBoxOperations.Items[i];
                    if (Operation.Key == 4)
                        checkedListBoxOperations.SetItemChecked(i, true);
                    else
                        checkedListBoxOperations.SetItemChecked(i, false);
                }

                for (int i = 0; i < checkedListBoxActions.Items.Count; i++)
                {
                    ActionListBoxItem Action = (ActionListBoxItem)checkedListBoxActions.Items[i];
                    if (Action.Key == 64 || Action.Key == 65)
                        checkedListBoxActions.SetItemChecked(i, true);
                    else
                        checkedListBoxActions.SetItemChecked(i, false);
                }

                comboBoxUsersSelector.Enabled = true;
                checkedListBoxUsers.Enabled = true;
                comboBoxEntitiesSelector.Enabled = false;
                comboBoxActionsSelector.Enabled = false;
                comboBoxOperationsSelector.Enabled = false;
                checkedListBoxActions.Enabled = false;
                checkedListBoxOperations.Enabled = false;
                checkedListBoxEntities.Enabled = false;
            }
            else if (this.AuditType == "Audit Summary View")
            {
                //Remove all the labels
                List<Control> list = Controls.OfType<Button>().Cast<Control>().ToList();

                //ArrayList list = new ArrayList(panelSelectedEntities.Controls.);
                foreach (Control c in list)
                {
                    panelSelectedEntities.Controls.Remove(c);
                }

                comboBoxUsersSelector.Enabled = true;
                comboBoxEntitiesSelector.Enabled = true;
                comboBoxActionsSelector.Enabled = true;
                comboBoxOperationsSelector.Enabled = true;
                checkedListBoxActions.Enabled = true;
                checkedListBoxOperations.Enabled = true;
                checkedListBoxEntities.Enabled = true;
                checkedListBoxUsers.Enabled = true;
            }
            else if (this.AuditType == "Attribute Change History")
            {
                comboBoxEntitiesSelector.Enabled = true;
                checkedListBoxEntities.Enabled = true;
                comboBoxUsersSelector.Enabled = false;
                comboBoxActionsSelector.Enabled = false;
                comboBoxOperationsSelector.Enabled = false;
                checkedListBoxActions.Enabled = false;
                checkedListBoxOperations.Enabled = false;
                checkedListBoxUsers.Enabled = false;

                comboBoxOperationsSelector.SelectedItem = "All";
                for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
                {
                    checkedListBoxOperations.SetItemChecked(i, true);
                }

                comboBoxActionsSelector.SelectedItem = "All";
                for (int i = 0; i < checkedListBoxActions.Items.Count; i++)
                {
                    checkedListBoxActions.SetItemChecked(i, true);
                }

                comboBoxUsersSelector.SelectedItem = "All";
                for (int i = 0; i < checkedListBoxUsers.Items.Count; i++)
                {
                    checkedListBoxUsers.SetItemChecked(i, true);
                }
            }
            else if (this.AuditType == "Record Change History")
            {
                comboBoxEntitiesSelector.Enabled = true;
                checkedListBoxEntities.Enabled = true;
                comboBoxUsersSelector.Enabled = false;
                comboBoxActionsSelector.Enabled = false;
                comboBoxOperationsSelector.Enabled = false;
                checkedListBoxActions.Enabled = false;
                checkedListBoxOperations.Enabled = false;
                checkedListBoxUsers.Enabled = false;
            }
            else
            {
                comboBoxUsersSelector.Enabled = true;
                comboBoxEntitiesSelector.Enabled = true;
                comboBoxActionsSelector.Enabled = true;
                comboBoxOperationsSelector.Enabled = true;
                checkedListBoxActions.Enabled = true;
                checkedListBoxOperations.Enabled = true;
                checkedListBoxEntities.Enabled = true;
                checkedListBoxUsers.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxFormat control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxEncoding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the checkedListBoxUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkedListBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (checkedListBoxUsers.CheckedItems.Count == checkedListBoxUsers.Items.Count)
                comboBoxUsersSelector.SelectedItem = "All";
            else if (checkedListBoxUsers.CheckedItems.Count == 0)
                comboBoxUsersSelector.SelectedItem = "None";
            else
                comboBoxUsersSelector.SelectedItem = "Custom";
        }

        /// <summary>
        /// Handles the Click event of the runProfileToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;
            List<ControlEnabled> cList = new List<ControlEnabled>();
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                cList.Add(new ControlEnabled { Control = c, Enabled = c.Enabled });
                c.Enabled = false;
            }

            buttonOpenInExcel.Visible = false;
            toolStripStatusLabel1.Text = "Running Profile " + currentProfile.ProfileName + ". Please wait...";
            Application.DoEvents();
            try
            {
                man.RunProfile(currentProfile);
                buttonOpenInExcel.Visible = true;
                //dataExportReportToolStripMenuItem.Visible = true;
                toolStripStatusLabel1.Text = "Audit exported. Click on the \"Open in Excel\" button to see the result.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }

            foreach (ControlEnabled ce in cList)
            {
                ce.Control.Enabled = ce.Enabled;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxUsersSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxUsersSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUsersSelector.SelectedIndex == 2)
                return;
            bool checkAll = (comboBoxUsersSelector.SelectedIndex == 0);

            for (int i = 0; i < checkedListBoxUsers.Items.Count; i++)
            {
                checkedListBoxUsers.SetItemChecked(i, checkAll);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxEntitiesSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxEntitiesSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEntitiesSelector.SelectedIndex == 2)
                return;
            bool checkAll = (comboBoxEntitiesSelector.SelectedIndex == 0);
            SelectedEntityList = new List<SelectedAuditEntity>();

            for (int i = 0; i < checkedListBoxEntities.Items.Count; i++)
            {
                checkedListBoxEntities.SetItemChecked(i, checkAll);
                if (!checkAll)
                    continue;

                EntityListBoxItem elbi = (EntityListBoxItem)checkedListBoxEntities.Items[i];
                SelectedEntityList.Add(new SelectedAuditEntity { LogicalName = elbi.Value, ObjectTypeCode = elbi.ObjectTypeCode });
            }

            displaySelectedEntities();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxActionsSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxActionsSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxActionsSelector.SelectedIndex == 2)
                return;
            bool checkAll = (comboBoxActionsSelector.SelectedIndex == 0);

            for (int i = 0; i < checkedListBoxActions.Items.Count; i++)
            {
                checkedListBoxActions.SetItemChecked(i, checkAll);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxOperationsSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxOperationsSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOperationsSelector.SelectedIndex == 2)
                return;
            bool checkAll = (comboBoxOperationsSelector.SelectedIndex == 0);

            for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
            {
                checkedListBoxOperations.SetItemChecked(i, checkAll);
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonOpenInExcel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonOpenInExcel_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(man.fileName))
            {
                MessageBox.Show("File not found!");
                return;
            }

            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = man.fileName;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    MessageBox.Show("Excel is not installed on this computer!");
                }

                else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Console.WriteLine(ex.Message + ". You do not have permission to access this file.");
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the checkedListBoxActions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkedListBoxActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (checkedListBoxActions.CheckedItems.Count == checkedListBoxActions.Items.Count)
                comboBoxActionsSelector.SelectedItem = "All";
            else if (checkedListBoxActions.CheckedItems.Count == 0)
                comboBoxActionsSelector.SelectedItem = "None";
            else
                comboBoxActionsSelector.SelectedItem = "Custom";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the checkedListBoxOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkedListBoxOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (checkedListBoxOperations.CheckedItems.Count == checkedListBoxOperations.Items.Count)
                comboBoxOperationsSelector.SelectedItem = "All";
            else if (checkedListBoxOperations.CheckedItems.Count == 0)
                comboBoxOperationsSelector.SelectedItem = "None";
            else
                comboBoxOperationsSelector.SelectedItem = "Custom";
        }

        /// <summary>
        /// Handles the Click event of the viewLogToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        /// <summary>
        /// Handles the Click event of the logArchiveToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        /// <summary>
        /// Handles the Click event of the helpToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#auditexportmanager";
            Process.Start(startInfo);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxAuditDate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxAuditDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = comboBoxAuditRecordCreatedOnFilter.SelectedItem.ToString();
            if (SelectedItem == "Last X Days" || SelectedItem == "Last X Months" || SelectedItem == "Last X Years")
            {
                numericUpDownLastX.Visible = true;
                labelAuditCreatedFrom.Visible = false;
                labelAuditCreatedTo.Visible = false;
                dateTimePickerAuditCreatedFrom.Visible = false;
                dateTimePickerAuditCreatedTo.Visible = false;
            }
            else if (SelectedItem == "Between Dates")
            {
                labelAuditCreatedFrom.Visible = true;
                labelAuditCreatedTo.Visible = true;
                dateTimePickerAuditCreatedFrom.Visible = true;
                dateTimePickerAuditCreatedTo.Visible = true;
                numericUpDownLastX.Visible = false;
            }
            else
            {
                numericUpDownLastX.Visible = false;
                labelAuditCreatedFrom.Visible = false;
                labelAuditCreatedTo.Visible = false;
                dateTimePickerAuditCreatedFrom.Visible = false;
                dateTimePickerAuditCreatedTo.Visible = false;
            }
        }
    }

    /// <summary>
    /// Check List Box Item class
    /// </summary>
    public class CheckListBoxItem
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public Guid Id;
        /// <summary>
        /// The text
        /// </summary>
        public string Text;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>
    /// Action List Box Item class
    /// </summary>
    public class ActionListBoxItem
    {
        /// <summary>
        /// The key
        /// </summary>
        public int Key;
        /// <summary>
        /// The value
        /// </summary>
        public string Value;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Entity List Box Item class
    /// </summary>
    public class EntityListBoxItem
    {
        /// <summary>
        /// The object type code
        /// </summary>
        public int ObjectTypeCode;
        /// <summary>
        /// The value
        /// </summary>
        public string Value;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// Control Enabled class
    /// </summary>
    public class ControlEnabled
    {
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public Control Control { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ControlEnabled"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }
    }
}