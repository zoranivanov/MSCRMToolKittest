// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        01/07/2012
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Forms;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// ReferenceDataTransporter class
    /// </summary>
    public partial class ReferenceDataTransporter : Form
    {
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        internal TransportationProfile currentProfile;
        internal MSCRMTransportationProfilesManager man = new MSCRMTransportationProfilesManager();
        internal List<SelectedEntity> TemporarySelectedEntityListForIgnoredAttributes = new List<SelectedEntity>();
        internal EnvStructure es = null;
        private BackgroundWorker bwExport = new BackgroundWorker();
        private BackgroundWorker bwImport = new BackgroundWorker();
        private OrganizationServiceProxy _serviceProxy;
        private string currentlyTransportedEntity = "";
        private int totalImportFailures = 0;
        private int totalImportSuccess = 0;
        private int totalTreatedRecords = 0;
        private bool transportStopped = false;
        private bool transportRunning = false;
        private bool selectedEntitiesInitialLoading = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataTransporter"/> class.
        /// </summary>
        public ReferenceDataTransporter()
        {
            InitializeComponent();
            LogManager.WriteLog("Reference Data Transporter launched.");

            if (man.Profiles != null)
            {
                foreach (TransportationProfile tp in man.Profiles)
                {
                    this.comboBoxTransportationProfiles.Items.AddRange(new object[] { tp.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<TransportationProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                this.comboBoxConnectionTarget.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }
            buttonLoadEntities.Enabled = false;
            comboBoxOperation.SelectedIndex = 2;
            comboBoxImportMode.SelectedIndex = 1;
            bwExport.WorkerReportsProgress = true;
            bwExport.WorkerSupportsCancellation = true;
            bwExport.DoWork += new DoWorkEventHandler(bwExport_DoWork);
            bwExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwExport_ProgressChanged);
            bwExport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwExport_RunWorkerCompleted);

            bwImport.WorkerReportsProgress = true;
            bwImport.WorkerSupportsCancellation = true;
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwImport_ProgressChanged);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
        }

        #region Funcitons

        private void populateCheckedEntities()
        {
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
                foreach (EnvEntity ee in es.Entities)
                {
                    checkedListBoxEntities.Items.AddRange(new object[] { ee.EntityName });
                    if (currentProfile == null)
                        continue;
                    if (currentProfile.SelectedEntities != null)
                    {
                        foreach (SelectedEntity ee1 in currentProfile.SelectedEntities)
                        {
                            if (ee.EntityName == ee1.EntityName)
                            {
                                checkedListBoxEntities.SetItemChecked(checkedListBoxEntities.Items.Count - 1, true);
                            }
                        }
                    }
                }
            }
        }

        private bool SaveProfile()
        {
            bool result = true;
            //Check that all fields are provided
            if (string.IsNullOrEmpty(textBoxTransportationProfileName.Text))
            {
                MessageBox.Show("Transportation Profile Name is mandatory!");
                return false;
            }

            //Check that the name of te connection is valid
            if (textBoxTransportationProfileName.Text.Contains(" ") ||
                    textBoxTransportationProfileName.Text.Contains("\\") ||
                    textBoxTransportationProfileName.Text.Contains("/") ||
                    textBoxTransportationProfileName.Text.Contains(">") ||
                    textBoxTransportationProfileName.Text.Contains("<") ||
                    textBoxTransportationProfileName.Text.Contains("?") ||
                    textBoxTransportationProfileName.Text.Contains("*") ||
                    textBoxTransportationProfileName.Text.Contains(":") ||
                    textBoxTransportationProfileName.Text.Contains("|") ||
                    textBoxTransportationProfileName.Text.Contains("\"") ||
                    textBoxTransportationProfileName.Text.Contains("'")
                    )
            {
                MessageBox.Show("You shouldn't use spaces nor the following characters (\\/<>?*:|\"') in the Transportation Profile Name as it will be used to create folders and files.");
                return false;
            }

            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a Source for the Profile");
                return false;
            }

            if (comboBoxConnectionTarget.SelectedItem == null && comboBoxOperation.SelectedItem.ToString() != "Export Data")
            {
                MessageBox.Show("You must select a Target for the Profile");
                return false;
            }

            //Check for Transport Order Conflicts
            foreach (ComboBox cb in panelTransportOrder.Controls.OfType<ComboBox>())
            {
                int numberOfOccurencies = 0;
                int currentComboSelectedIndex = cb.SelectedIndex;
                foreach (ComboBox cb1 in panelTransportOrder.Controls.OfType<ComboBox>())
                {
                    if (currentComboSelectedIndex == cb1.SelectedIndex)
                    {
                        numberOfOccurencies++;
                    }

                    if (numberOfOccurencies > 1)
                    {
                        MessageBox.Show("The entity \"" + cb.SelectedItem + "\" is declared more than once int the Transport Order. Fix this before saving.");
                        return false;
                    }
                }
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Connection having the same name exist already
                foreach (TransportationProfile tp in man.Profiles)
                {
                    if (tp.ProfileName.ToLower() == textBoxTransportationProfileName.Text.ToLower())
                    {
                        MessageBox.Show("Transportation Profile with the name " + textBoxTransportationProfileName.Text + " exist already. Please select another name");
                        return false;
                    }
                }

                TransportationProfile newProfile = new TransportationProfile();
                newProfile.ProfileName = textBoxTransportationProfileName.Text;
                newProfile.ImportMode = comboBoxImportMode.SelectedIndex;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.setSourceConneciton();
                if (comboBoxOperation.SelectedItem.ToString() != "Export Data")
                {
                    newProfile.TargetConnectionName = comboBoxConnectionTarget.SelectedItem.ToString();
                    newProfile.setTargetConneciton();
                }
                newProfile.SelectedEntities = new List<SelectedEntity>();

                foreach (string checkedEntity in checkedListBoxEntities.CheckedItems)
                {
                    SelectedEntity ee = new SelectedEntity();
                    ee.EntityName = checkedEntity;
                    ee.ExportedRecords = 0;

                    foreach (ComboBox cb in panelTransportOrder.Controls.OfType<ComboBox>())
                    {
                        if (cb.SelectedItem.ToString() == ee.EntityName)
                        {
                            ee.TransportOrder = (int)cb.Tag;
                            break;
                        }
                    }

                    SelectedEntity seForIgnoredAttributes = TemporarySelectedEntityListForIgnoredAttributes.Find(match => match.EntityName == checkedEntity);
                    if (seForIgnoredAttributes != null)
                    {
                        ee.IgnoredAttributes = seForIgnoredAttributes.IgnoredAttributes;
                        ee.Filter = seForIgnoredAttributes.Filter;
                    }

                    newProfile.SelectedEntities.Add(ee);
                }

                newProfile.Operation = comboBoxOperation.SelectedIndex;
                man.CreateProfile(newProfile);
                comboBoxTransportationProfiles.Items.AddRange(new object[] { newProfile.ProfileName });
                comboBoxTransportationProfiles.SelectedItem = newProfile.ProfileName;
            }
            else
            {
                currentProfile.ProfileName = textBoxTransportationProfileName.Text;
                currentProfile.ImportMode = comboBoxImportMode.SelectedIndex;
                currentProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                currentProfile.setSourceConneciton();
                if (comboBoxOperation.SelectedItem.ToString() != "Export Data")
                {
                    currentProfile.TargetConnectionName = comboBoxConnectionTarget.SelectedItem.ToString();
                    currentProfile.setTargetConneciton();
                }

                //Backup Export Records numbers if existing
                List<SelectedEntity> backupSelectedEntites = currentProfile.SelectedEntities;

                currentProfile.SelectedEntities = new List<SelectedEntity>();

                TransportationProfile oldProfile = man.GetProfile(currentProfile.ProfileName);
                foreach (string checkedEntity in checkedListBoxEntities.CheckedItems)
                {
                    SelectedEntity ee = currentProfile.getSelectedEntity(checkedEntity);
                    if (ee == null)
                    {
                        ee = new SelectedEntity();
                        ee.EntityName = checkedEntity;

                        //Restore Export Records numbers if existing
                        SelectedEntity tse = backupSelectedEntites.Find(b => b.EntityName == checkedEntity);
                        if (tse != null && tse.ExportedRecords > 0)
                        {
                            ee.ExportedRecords = tse.ExportedRecords;
                        }
                        else
                        {
                            ee.ExportedRecords = 0;
                        }

                        foreach (ComboBox cb in panelTransportOrder.Controls.OfType<ComboBox>())
                        {
                            if (cb.SelectedItem.ToString() == ee.EntityName)
                            {
                                ee.TransportOrder = (int)cb.Tag;
                                break;
                            }
                        }

                        SelectedEntity seForIgnoredAttributes = TemporarySelectedEntityListForIgnoredAttributes.Find(match => match.EntityName == checkedEntity);
                        if (seForIgnoredAttributes != null)
                        {
                            ee.IgnoredAttributes = seForIgnoredAttributes.IgnoredAttributes;
                            ee.Filter = seForIgnoredAttributes.Filter;
                        }
                        currentProfile.SelectedEntities.Add(ee);
                    }
                    else
                    {
                        foreach (ComboBox cb in panelTransportOrder.Controls.OfType<ComboBox>())
                        {
                            if (cb.SelectedItem.ToString() == ee.EntityName)
                            {
                                ee.TransportOrder = (int)cb.Tag;
                                break;
                            }
                        }
                    }
                }

                //Records mapping cleanup
                if (currentProfile.RecordMappings != null)
                {
                    List<int> RecordMappingsForRemovalIndexes = new List<int>();
                    for (int i = 0; i < currentProfile.RecordMappings.Count; i++)
                    {
                        if (currentProfile.RecordMappings[i].SourceRecordId == Guid.Empty && currentProfile.RecordMappings[i].TargetRecordId == Guid.Empty)
                            RecordMappingsForRemovalIndexes.Add(i);
                    }

                    for (int removeIndex = RecordMappingsForRemovalIndexes.Count - 1; removeIndex >= 0; removeIndex--)
                        currentProfile.RecordMappings.RemoveAt(removeIndex);

                    if (currentProfile.RecordMappings.Count == 0)
                        currentProfile.RecordMappings = null;
                }

                currentProfile.Operation = comboBoxOperation.SelectedIndex;
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel.Text = "Transportation Profile " + currentProfile.ProfileName + " saved.";
            LogManager.WriteLog("Transportation Profile " + currentProfile.ProfileName + " saved.");
            return result;
        }

        private void SetFields(bool state)
        {
            comboBoxTransportationProfiles.Enabled = state;
            comboBoxConnectionSource.Enabled = state;
            comboBoxConnectionTarget.Enabled = state;
            comboBoxImportMode.Enabled = state;
            comboBoxOperation.Enabled = state;
            buttonLoadEntities.Enabled = state;
            deleteProfileToolStripMenuItem.Enabled = state;
            checkedListBoxEntities.Enabled = state;
            foreach (ComboBox cb in panelTransportOrder.Controls.OfType<ComboBox>())
            {
                cb.Enabled = state;
            }
            foreach (Button b in panelTransportOrder.Controls.OfType<Button>())
            {
                b.Enabled = state;
            }
            pgbState.Value = 0;
            buttonRecordsMapping.Enabled = state;
        }

        private void displaySelectedEntitiesTransportOrder()
        {
            //Remove all the combobox and labels
            ArrayList list = new ArrayList(panelTransportOrder.Controls);
            foreach (Control c in list)
            {
                panelTransportOrder.Controls.Remove(c);
            }

            if (checkedListBoxEntities.Items.Count == 0)
                return;

            int transportOrderYLocation = 7;
            int orderCpt = 1;
            IOrderedEnumerable<SelectedEntity> se = null;
            List<string> treatedEntities = new List<string>();
            if (currentProfile != null)
            {
                if (currentProfile.SelectedEntities == null)
                    currentProfile.SelectedEntities = new List<SelectedEntity>();
                se = currentProfile.SelectedEntities.OrderBy(ee => ee.TransportOrder);

                foreach (SelectedEntity ee in se)
                {
                    //check if the SelectedEntity is still checked
                    if (!checkedListBoxEntities.CheckedItems.Contains(ee.EntityName))
                        continue;

                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Text = orderCpt.ToString() + ".";
                    label.Location = new System.Drawing.Point(0, transportOrderYLocation + 4);
                    label.Size = new System.Drawing.Size(26, 15);
                    panelTransportOrder.Controls.Add(label);

                    ComboBox cb = new ComboBox();
                    cb.Name = "combo_" + orderCpt;
                    cb.Tag = orderCpt;
                    cb.Location = new System.Drawing.Point(26, transportOrderYLocation);
                    cb.Size = new System.Drawing.Size(220, 15);
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (string checkedEntity1 in checkedListBoxEntities.CheckedItems)
                    {
                        cb.Items.Add(checkedEntity1);
                    }
                    cb.SelectedItem = ee.EntityName;
                    panelTransportOrder.Controls.Add(cb);

                    Button setupIgnoredAttributes = new Button();
                    setupIgnoredAttributes.Name = "button_" + orderCpt;
                    setupIgnoredAttributes.Text = "...";
                    setupIgnoredAttributes.Tag = ee.EntityName;
                    setupIgnoredAttributes.Size = new System.Drawing.Size(25, 23);
                    setupIgnoredAttributes.Location = new System.Drawing.Point(250, transportOrderYLocation - 1);
                    setupIgnoredAttributes.Click += new System.EventHandler(this.showSetupIgnoredAttributesWizard);
                    panelTransportOrder.Controls.Add(setupIgnoredAttributes);

                    orderCpt++;
                    transportOrderYLocation += 25;
                    treatedEntities.Add(ee.EntityName);
                }
            }

            foreach (string checkedEntity in checkedListBoxEntities.CheckedItems)
            {
                if (treatedEntities.Contains(checkedEntity))
                    continue;
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Text = orderCpt.ToString() + ".";
                label.Location = new System.Drawing.Point(0, transportOrderYLocation + 4);
                label.Size = new System.Drawing.Size(26, 15);
                panelTransportOrder.Controls.Add(label);

                ComboBox cb = new ComboBox();
                cb.Name = "combo_" + orderCpt;
                cb.Tag = orderCpt;
                cb.Location = new System.Drawing.Point(26, transportOrderYLocation);
                cb.Size = new System.Drawing.Size(220, 15);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (string checkedEntity1 in checkedListBoxEntities.CheckedItems)
                {
                    cb.Items.Add(checkedEntity1);
                }
                cb.SelectedItem = checkedEntity;
                panelTransportOrder.Controls.Add(cb);

                Button setupIgnoredAttributes = new Button();
                setupIgnoredAttributes.Name = "button_" + orderCpt;
                setupIgnoredAttributes.Text = "...";
                setupIgnoredAttributes.Tag = checkedEntity;
                setupIgnoredAttributes.Size = new System.Drawing.Size(25, 23);
                setupIgnoredAttributes.Location = new System.Drawing.Point(250, transportOrderYLocation - 1);
                setupIgnoredAttributes.Click += new System.EventHandler(this.showSetupIgnoredAttributesWizard);
                panelTransportOrder.Controls.Add(setupIgnoredAttributes);

                orderCpt++;
                transportOrderYLocation += 25;
            }
        }

        private void showSetupIgnoredAttributesWizard(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string comboBoxName = "combo_" + b.Name.Replace("button_", "");
            Control[] combos = panelTransportOrder.Controls.Find(comboBoxName, false);
            if (combos != null && combos.Length == 1)
            {
                ComboBox cb = (ComboBox)combos[0];
                SetupIgnoredAttributes sia = new SetupIgnoredAttributes(this, cb.SelectedItem.ToString());
                sia.Show();
            }
            else
            {
                MessageBox.Show("Error while loading the Attributes list!");
            }
        }

        private void buttonLoadEntities_Click(object sender, EventArgs e)
        {
            DialogResult dResTest = MessageBox.Show("Loading the structure may take some time. The application will become unresponsive during this time.\n\nAre you sure you want to load the structure from the Source?", "Confirm Structure Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }

            toolStripStatusLabel.Text = "Loading structure. Please wait...";
            Application.DoEvents();

            try
            {
                es = man.downloadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
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
            //Display entities list
            checkedListBoxEntities.Items.Clear();
            foreach (EnvEntity ee in es.Entities)
            {
                checkedListBoxEntities.Items.AddRange(new object[] { ee.EntityName });
            }

            toolStripStatusLabel.Text = "Structure successfully loaded";
        }

        #endregion Funcitons

        #region Transport Profile Properties Controls

        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedEntitiesInitialLoading = true;
            populateCheckedEntities();
            displaySelectedEntitiesTransportOrder();
            selectedEntitiesInitialLoading = false;
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                buttonLoadEntities.Enabled = false;
            }
            else
            {
                buttonLoadEntities.Enabled = true;
            }
        }

        private void comboBoxTransportationProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxConnectionTarget.SelectedItem = null;
            if (comboBoxTransportationProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxTransportationProfiles.SelectedIndex];
                textBoxTransportationProfileName.Text = currentProfile.ProfileName;
                comboBoxImportMode.SelectedIndex = currentProfile.ImportMode;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                comboBoxConnectionTarget.SelectedItem = currentProfile.TargetConnectionName;
                TemporarySelectedEntityListForIgnoredAttributes = currentProfile.SelectedEntities;
                deleteProfileToolStripMenuItem.Enabled = true;
                textBoxTransportationProfileName.Enabled = false;
                comboBoxOperation.SelectedIndex = currentProfile.Operation;
                runProfileToolStripMenuItem.Enabled = true;
            }
            else
            {
                currentProfile = null;
                textBoxTransportationProfileName.Text = "";
                comboBoxOperation.SelectedIndex = 2;
                comboBoxImportMode.SelectedIndex = 1;
                deleteProfileToolStripMenuItem.Enabled = false;
                textBoxTransportationProfileName.Enabled = true;
                labelStructureLastLoadedDate.Text = "never";
                runProfileToolStripMenuItem.Enabled = false;
            }
        }

        private void checkedListBoxEntites_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clean TemporarySelectedEntityListForIgnoredAttributes
            List<SelectedEntity> markedForRemoval = new List<SelectedEntity>();
            foreach (SelectedEntity se in TemporarySelectedEntityListForIgnoredAttributes)
            {
                if (!checkedListBoxEntities.CheckedItems.Contains(se.EntityName))
                    markedForRemoval.Add(se);
            }
            foreach (SelectedEntity se in markedForRemoval)
            {
                TemporarySelectedEntityListForIgnoredAttributes.Remove(se);
            }

            //Check if this is not the initial loading to improve performance
            if (!selectedEntitiesInitialLoading)
                displaySelectedEntitiesTransportOrder();
        }

        private void comboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOperation.SelectedItem.ToString() == "Export Data")
            {
                comboBoxConnectionTarget.Enabled = false;
                comboBoxImportMode.Enabled = false;
                buttonRecordsMapping.Enabled = false;
            }
            else
            {
                comboBoxConnectionTarget.Enabled = true;
                comboBoxImportMode.Enabled = true;
                buttonRecordsMapping.Enabled = true;
            }
        }

        private void buttonRecordsMapping_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;

            RecordsMapping rm = new RecordsMapping(this);
            rm.Show();
        }

        #endregion Transport Profile Properties Controls

        #region Transport threads

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            transportStopped = false;
            BackgroundWorker worker = sender as BackgroundWorker;
            //Export data
            ExportGUI(man.ReportFileName, worker, e);
        }

        private void bwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pgbState.Value = e.ProgressPercentage;
            toolStripStatusLabel.Text = "Exporting: " + currentlyTransportedEntity;
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                transportReportToolStripMenuItem.Visible = true;
                transportRunning = false;
                SetFields(true);
                transportStopped = true;
                buttonStopTransport.Visible = false;
                runProfileToolStripMenuItem.Enabled = true;
                pgbState.Visible = false;
            }
            else if (e.Cancelled)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                toolStripStatusLabel.Text = "Transport canceled";
                transportReportToolStripMenuItem.Visible = true;
                SetFields(true);
                transportRunning = false;
                transportStopped = true;
                buttonStopTransport.Visible = false;
                runProfileToolStripMenuItem.Enabled = true;
                pgbState.Visible = false;
            }
            else
            {
                toolStripStatusLabel.Text = "Export Finished";

                if (currentProfile.Operation == 2)
                {
                    if (!transportStopped)
                    {
                        transportRunning = true;
                        SetFields(false);
                        toolStripStatusLabel.Text = "Initializing Data Import";
                        label2.Visible = true;
                        label3.Visible = true;
                        bwImport.RunWorkerAsync();
                    }
                    else
                    {
                        bwImport.CancelAsync();
                        transportRunning = false;
                        SetFields(true);
                        buttonStopTransport.Visible = false;
                        runProfileToolStripMenuItem.Enabled = true;
                        transportReportToolStripMenuItem.Visible = true;
                        pgbState.Visible = false;
                        TransportReport report = man.ReadTransportReport(man.ReportFileName);
                        report.TransportFinishedAt = DateTime.Now.ToString();
                        report.TransportCompleted = true;
                        TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                        report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                        man.WriteTransportReport(report, man.ReportFileName);
                    }
                }
                else
                {
                    SetFields(true);
                    transportRunning = false;
                    transportReportToolStripMenuItem.Visible = true;
                    buttonStopTransport.Visible = false;
                    runProfileToolStripMenuItem.Enabled = true;
                    pgbState.Visible = false;
                    TransportReport report = man.ReadTransportReport(man.ReportFileName);
                    report.TransportFinishedAt = DateTime.Now.ToString();
                    report.TransportCompleted = true;
                    TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                    report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                    man.WriteTransportReport(report, man.ReportFileName);
                }
            }
        }

        private int ExportGUI(string transportReportFileName, BackgroundWorker worker, DoWorkEventArgs e)
        {
            TransportReport report = null;
            try
            {
                report = new TransportReport(man.ReportFileName);
                //Get Transport Report
                if (File.Exists(man.ReportFileName))
                {
                    report = man.ReadTransportReport(man.ReportFileName);
                }

                //Clean Data folder
                string dataExportFolder = man.Folder + "\\" + currentProfile.ProfileName + "\\Data";
                if (Directory.Exists(dataExportFolder))
                {
                    Directory.Delete(dataExportFolder, true);
                }
                Directory.CreateDirectory(dataExportFolder);

                MSCRMConnection connection = currentProfile.getSourceConneciton();
                EnvStructure es = man.ReadEnvStructure(currentProfile.SourceConnectionName);
                man._serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                List<TransportReportLine> TransportReport = new List<TransportReportLine>();
                currentProfile.TotalExportedRecords = 0;
                //Mesure export time
                DateTime exportStartDT = DateTime.Now;

                LogManager.WriteLog("Start exporting data from " + connection.ConnectionName);

                int recordCount = 0;
                if (es != null)
                {
                    int treatedEntities = 1;

                    //Order export according to profile's transport order
                    IOrderedEnumerable<SelectedEntity> orderedSelectedEntities = currentProfile.SelectedEntities.OrderBy(se => se.TransportOrder);

                    foreach (SelectedEntity ee in orderedSelectedEntities)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return 0;
                        }

                        int percentage = 0;
                        if (currentProfile.SelectedEntities.Count != 0)
                            percentage = (int)(100 * treatedEntities / currentProfile.SelectedEntities.Count);
                        worker.ReportProgress(percentage);
                        treatedEntities++;

                        currentlyTransportedEntity = ee.EntityName;
                        LogManager.WriteLog("Exporting data for entity " + ee.EntityName);
                        DateTime entityExportStartDT = DateTime.Now;
                        string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                        fetchXml += "<entity name='" + ee.EntityName + "'>";
                        //Get Entity structure
                        EnvEntity strE = new EnvEntity();
                        foreach (EnvEntity envE in es.Entities)
                        {
                            if (envE.EntityName == ee.EntityName)
                            {
                                strE = envE;
                                break;
                            }
                        }

                        //Create fetchXML Query
                        foreach (string ea in strE.Attributes)
                        {
                            if (ee.IgnoredAttributes == null)
                            {
                                fetchXml += "<attribute name='" + ea + "' />";
                            }
                            else if (!ee.IgnoredAttributes.Contains(ea))
                            {
                                fetchXml += "<attribute name='" + ea + "' />";
                            }
                        }

                        //Add Query filter
                        fetchXml += ee.Filter;
                        fetchXml += "</entity></fetch>";
                        int recordCountPerEntity = man.ExportEntity(currentProfile, fetchXml);
                        recordCount += recordCountPerEntity;
                        DateTime entityExportEndDT = DateTime.Now;
                        TimeSpan ts = entityExportEndDT - entityExportStartDT;
                        TransportReportLine transportReportLine = new TransportReportLine();
                        transportReportLine.Entity = ee.EntityName;
                        transportReportLine.ExportedRecords = recordCountPerEntity;
                        report.TotalExportedRecords += recordCountPerEntity;
                        transportReportLine.ExportedIn = ts.ToString().Substring(0, 10);
                        transportReportLine.ExportStartedAt = entityExportStartDT.ToString();
                        transportReportLine.ExportFinishedAt = entityExportEndDT.ToString();
                        report.ReportLines.Add(transportReportLine);
                        man.WriteTransportReport(report, transportReportFileName);
                    }
                }

                TimeSpan exportTimeSpan = DateTime.Now - exportStartDT;
                LogManager.WriteLog("Export finished for " + currentProfile.SourceConnectionName + ". Exported " + recordCount + " records in " + exportTimeSpan.ToString().Substring(0, 10));
                return recordCount;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //Stop Transport threads if running
                transportStopped = true;
                e.Cancel = true;
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                //Stop Transport threads if running
                transportStopped = true;
                e.Cancel = true;
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                    LogManager.WriteLog("Error:" + ex.Message);
                }
            }

            return 0;
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //Import Data
            ImportGUI(man.ReportFileName, worker, e);
        }

        private void bwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pgbState.Value = e.ProgressPercentage;
            toolStripStatusLabel.Text = "Importing: " + currentlyTransportedEntity;
            labelImportFailures.Text = totalImportFailures.ToString(); ;
            labelImportSuccess.Text = totalImportSuccess.ToString();
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                toolStripStatusLabel.Text = "Transport stopped.";
            }
            else if (e.Cancelled)
            {
                toolStripStatusLabel.Text = "Transport canceled";
            }
            else
            {
                toolStripStatusLabel.Text = "Import Finished";
                TransportReport report = man.ReadTransportReport(man.ReportFileName);
                report.TransportFinishedAt = DateTime.Now.ToString();
                report.TransportCompleted = true;
                TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                man.WriteTransportReport(report, man.ReportFileName);
            }

            transportRunning = false;
            buttonStopTransport.Visible = false;
            runProfileToolStripMenuItem.Enabled = true;
            pgbState.Visible = false;
            transportReportToolStripMenuItem.Visible = true;
            SetFields(true);
        }

        private int ImportGUI(string transporimportFailurestReportFileName, BackgroundWorker worker, DoWorkEventArgs e)
        {
            totalTreatedRecords = 0;
            totalImportFailures = 0;
            totalImportSuccess = 0;
            int ReconnectionRetryCount = 5;

            try
            {
                TransportReport report = new TransportReport(man.ReportFileName);
                //Get Transport Report
                if (File.Exists(man.ReportFileName))
                {
                    report = man.ReadTransportReport(man.ReportFileName);
                }

                MSCRMConnection connection = currentProfile.getTargetConneciton(); ;
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                LogManager.WriteLog("Start importing data in " + connection.ConnectionName);

                //Mesure import time
                DateTime importStartDT = DateTime.Now;

                //Order import according to profile's import order
                IOrderedEnumerable<SelectedEntity> orderedSelectedEntities = currentProfile.SelectedEntities.OrderBy(se => se.TransportOrder);

                foreach (SelectedEntity ee in orderedSelectedEntities)
                {
                    //Check if there are any records to import
                    if (ee.ExportedRecords == 0)
                    {
                        continue;
                    }

                    //Mesure import time
                    DateTime entityImportStartDT = DateTime.Now;
                    currentlyTransportedEntity = ee.EntityName;

                    string entityFolderPath = man.Folder + "\\" + currentProfile.ProfileName + "\\Data\\" + ee.EntityName;
                    string[] filePaths = Directory.GetFiles(entityFolderPath, "*.xml");

                    LogManager.WriteLog("Importing " + ee.EntityName + " records.");
                    int treatedRecordsForEntity = 0;
                    int importedRecordsForEntity = 0;
                    int importFailuresForEntity = 0;

                    foreach (string filePath in filePaths)
                    {
                        List<Type> knownTypes = new List<Type>();
                        knownTypes.Add(typeof(Entity));

                        XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                        XRQ.MaxStringContentLength = int.MaxValue;

                        using (FileStream fs = new FileStream(filePath, FileMode.Open))
                        using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                        {
                            DataContractSerializer ser = new DataContractSerializer(typeof(EntityCollection), knownTypes);
                            EntityCollection fromDisk = (EntityCollection)ser.ReadObject(reader, true);

                            foreach (Entity en in fromDisk.Entities)
                            {
                                if (worker.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return 0;
                                }

                                //Records mapping for the Lookup attributes
                                Entity entity = man.MapRecords(currentProfile, en);

                                string executingOperation = "";
                                try
                                {
                                    if (currentProfile.ImportMode == 0)
                                    {
                                        executingOperation = "Create";
                                        service.Create(entity);
                                    }
                                    else if (currentProfile.ImportMode == 1)
                                    {
                                        try
                                        {
                                            executingOperation = "Update";
                                            service.Update(entity);
                                        }
                                        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
                                        {
                                            executingOperation = "Create";
                                            service.Create(entity);
                                        }
                                    }
                                    else if (currentProfile.ImportMode == 2)
                                    {
                                        executingOperation = "Update";
                                        service.Update(entity);
                                    }
                                    importedRecordsForEntity++;
                                    totalImportSuccess++;
                                }
                                catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
                                {
                                    totalImportFailures++;
                                    importFailuresForEntity++;
                                    ImportFailure failure = new ImportFailure
                                    {
                                        CreatedOn = DateTime.Now.ToString(),
                                        EntityName = ee.EntityName,
                                        Operation = executingOperation,
                                        Reason = ex.Detail.Message,
                                        Url = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
                                    };
                                    report.TotalImportFailures += 1;
                                    //Insert the Failure line in the Failures Report
                                    man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                                }
                                catch (Exception ex)
                                {
                                    //Check if the authentification session is expired
                                    if (ex.InnerException != null && ex.InnerException.Message.StartsWith("ID3242"))
                                    {
                                        LogManager.WriteLog("Error:The CRM authentication session expired. Reconnection attempt n° " + ReconnectionRetryCount);
                                        ReconnectionRetryCount--;
                                        //On 5 failed reconnections exit
                                        if (ReconnectionRetryCount == 0)
                                            throw;

                                        _serviceProxy = cm.connect(connection);
                                        service = (IOrganizationService)_serviceProxy;
                                        LogManager.WriteLog("Error:The CRM authentication session expired.");
                                        totalImportFailures++;
                                        importFailuresForEntity++;
                                        ImportFailure failure = new ImportFailure
                                        {
                                            CreatedOn = DateTime.Now.ToString(),
                                            EntityName = ee.EntityName,
                                            Operation = executingOperation,
                                            Reason = ex.InnerException.Message,
                                            Url = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
                                        };
                                        report.TotalImportFailures += 1;
                                        //Insert the Failure line in the Failures Report
                                        man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                                totalTreatedRecords++;
                                treatedRecordsForEntity++;
                                man.updateTransportReport(report, ee, importedRecordsForEntity, importFailuresForEntity, entityImportStartDT);

                                int percentage = 0;
                                if (currentProfile.TotalExportedRecords != 0)
                                    percentage = totalTreatedRecords * 100 / currentProfile.TotalExportedRecords;
                                worker.ReportProgress(percentage);
                            }
                        }
                    }
                    LogManager.WriteLog("Treated " + treatedRecordsForEntity + " " + ee.EntityName + " records with " + importedRecordsForEntity + " successfully imported records and " + importFailuresForEntity + " failures.");
                }

                TimeSpan importTimeSpan = DateTime.Now - importStartDT;
                LogManager.WriteLog("Import finished for " + connection.ConnectionName + ". Treated " + totalTreatedRecords + " records in " + importTimeSpan.ToString().Substring(0, 10) + ". Successfuly imported " + totalImportSuccess + " records and " + totalImportFailures + " failures.");
                //tpm.WriteTransportReport(report,tpm.transportReportFileName);
                return 0;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
            return 0;
        }

        /*
        private int ImportGUIMultipleRequest(string transporimportFailurestReportFileName, BackgroundWorker worker, DoWorkEventArgs e)
        {
            totalTreatedRecords = 0;
            totalImportFailures = 0;
            totalImportSuccess = 0;
            int ReconnectionRetryCount = 5;

            try
            {
                TransportReport report = new TransportReport(man.ReportFileName);
                //Get Transport Report
                if (File.Exists(man.ReportFileName))
                {
                    report = man.ReadTransportReport(man.ReportFileName);
                }

                MSCRMConnection connection = currentProfile.getTargetConneciton(); ;
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                LogManager.WriteLog("Start importing data in " + connection.ConnectionName);

                //Mesure import time
                DateTime importStartDT = DateTime.Now;

                //Order import according to profile's import order
                IOrderedEnumerable<SelectedEntity> orderedSelectedEntities = currentProfile.SelectedEntities.OrderBy(se => se.TransportOrder);

                foreach (SelectedEntity ee in orderedSelectedEntities)
                {
                    //Check if there are any records to import
                    if (ee.ExportedRecords == 0)
                    {
                        continue;
                    }

                    //Mesure import time
                    DateTime entityImportStartDT = DateTime.Now;
                    currentlyTransportedEntity = ee.EntityName;

                    string entityFolderPath = man.Folder + "\\" + currentProfile.ProfileName + "\\Data\\" + ee.EntityName;
                    string[] filePaths = Directory.GetFiles(entityFolderPath, "*.xml");

                    LogManager.WriteLog("Importing " + ee.EntityName + " records.");
                    int treatedRecordsForEntity = 0;
                    int importedRecordsForEntity = 0;
                    int importFailuresForEntity = 0;
                    string executingOperation = "";

                    foreach (string filePath in filePaths)
                    {
                        List<Type> knownTypes = new List<Type>();
                        knownTypes.Add(typeof(Entity));
                        FileStream fs = new FileStream(filePath, FileMode.Open);
                        DataContractSerializer ser = new DataContractSerializer(typeof(EntityCollection), knownTypes);
                        XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                        XRQ.MaxStringContentLength = int.MaxValue;
                        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ);
                        EntityCollection fromDisk = (EntityCollection)ser.ReadObject(reader, true);
                        reader.Close();
                        fs.Close();

                        int batchSizeCounter = 0;
                        int batchSizeLimit = 50;
                        EntityCollection input = new EntityCollection();

                        foreach (Entity en in fromDisk.Entities)
                        {
                            if (batchSizeCounter > batchSizeLimit)
                            {
                                //Run Multiple Execute request

                                #region Execute Multiple with Results

                                // Create an ExecuteMultipleRequest object.
                                ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
                                {
                                    // Assign settings that define execution behavior: continue on error, return responses.
                                    Settings = new ExecuteMultipleSettings()
                                    {
                                        ContinueOnError = true,
                                        ReturnResponses = true
                                    },
                                    // Create an empty organization request collection.
                                    Requests = new OrganizationRequestCollection()
                                };

                                executingOperation = "";
                                // Add a CreateRequest for each entity to the request collection.
                                foreach (var entity1 in input.Entities)
                                {
                                    if (currentProfile.ImportMode == 0)
                                    {
                                        executingOperation = "Create";
                                        service.Create(entity1);
                                    }
                                    else if (currentProfile.ImportMode == 1)
                                    {
                                        try
                                        {
                                            executingOperation = "Update";
                                            service.Update(entity1);
                                        }
                                        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
                                        {
                                            executingOperation = "Create";
                                            service.Create(entity1);
                                        }
                                    }
                                    else if (currentProfile.ImportMode == 2)
                                    {
                                        executingOperation = "Update";
                                        service.Update(entity1);
                                    }
                                    CreateRequest createRequest = new CreateRequest { Target = entity1 };
                                    requestWithResults.Requests.Add(createRequest);
                                }

                                // Execute all the requests in the request collection using a single web method call.
                                ExecuteMultipleResponse responseWithResults =
                                    (ExecuteMultipleResponse)_serviceProxy.Execute(requestWithResults);

                                // Display the results returned in the responses.
                                foreach (var responseItem in responseWithResults.Responses)
                                {
                                    // A valid response.
                                    if (responseItem.Response != null)
                                        DisplayResponse(requestWithResults.Requests[responseItem.RequestIndex], responseItem.Response);

                                    // An error has occurred.
                                    else if (responseItem.Fault != null)
                                        DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                                            responseItem.RequestIndex, responseItem.Fault);
                                }
                                //</snippetExecuteMultiple1>

                                #endregion Execute Multiple with Results

                                batchSizeCounter = 0;
                            }

                            input.Entities.Add(en);

                            if (worker.CancellationPending)
                            {
                                e.Cancel = true;
                                return 0;
                            }

                            //Records mapping for the Lookup attributes
                            Entity entity = man.MapRecords(currentProfile, en);

                            try
                            {
                                if (currentProfile.ImportMode == 0)
                                {
                                    executingOperation = "Create";
                                    service.Create(entity);
                                }
                                else if (currentProfile.ImportMode == 1)
                                {
                                    try
                                    {
                                        executingOperation = "Update";
                                        service.Update(entity);
                                    }
                                    catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
                                    {
                                        executingOperation = "Create";
                                        service.Create(entity);
                                    }
                                }
                                else if (currentProfile.ImportMode == 2)
                                {
                                    executingOperation = "Update";
                                    service.Update(entity);
                                }
                                importedRecordsForEntity++;
                                totalImportSuccess++;
                            }
                            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
                            {
                                totalImportFailures++;
                                importFailuresForEntity++;
                                ImportFailure failure = new ImportFailure
                                {
                                    CreatedOn = DateTime.Now.ToString(),
                                    EntityName = ee.EntityName,
                                    Operation = executingOperation,
                                    Reason = ex.Detail.Message,
                                    Url = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
                                };
                                report.TotalImportFailures += 1;
                                //Insert the Failure line in the Failures Report
                                man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                            }
                            catch (Exception ex)
                            {
                                //Check if the authentification session is expired
                                if (ex.InnerException != null && ex.InnerException.Message.StartsWith("ID3242"))
                                {
                                    LogManager.WriteLog("Error:The CRM authentication session expired. Reconnection attempt n° " + ReconnectionRetryCount);
                                    ReconnectionRetryCount--;
                                    //On 5 failed reconnections exit
                                    if (ReconnectionRetryCount == 0)
                                        throw ex;

                                    _serviceProxy = cm.connect(connection);
                                    service = (IOrganizationService)_serviceProxy;
                                    LogManager.WriteLog("Error:The CRM authentication session expired.");
                                    totalImportFailures++;
                                    importFailuresForEntity++;
                                    ImportFailure failure = new ImportFailure
                                    {
                                        CreatedOn = DateTime.Now.ToString(),
                                        EntityName = ee.EntityName,
                                        Operation = executingOperation,
                                        Reason = ex.InnerException.Message,
                                        Url = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
                                    };
                                    report.TotalImportFailures += 1;
                                    //Insert the Failure line in the Failures Report
                                    man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                            totalTreatedRecords++;
                            treatedRecordsForEntity++;
                            man.updateTransportReport(report, ee, importedRecordsForEntity, importFailuresForEntity, entityImportStartDT);

                            int percentage = 0;
                            if (currentProfile.TotalExportedRecords != 0)
                                percentage = totalTreatedRecords * 100 / currentProfile.TotalExportedRecords;
                            worker.ReportProgress(percentage);

                            batchSizeCounter++;
                        }
                    }
                    LogManager.WriteLog("Treated " + treatedRecordsForEntity + " " + ee.EntityName + " records with " + importedRecordsForEntity + " successfully imported records and " + importFailuresForEntity + " failures.");
                }

                TimeSpan importTimeSpan = DateTime.Now - importStartDT;
                LogManager.WriteLog("Import finished for " + connection.ConnectionName + ". Treated " + totalTreatedRecords + " records in " + importTimeSpan.ToString().Substring(0, 10) + ". Successfuly imported " + totalImportSuccess + " records and " + totalImportFailures + " failures.");
                //tpm.WriteTransportReport(report,tpm.transportReportFileName);
                return 0;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// Display the response of an organization message request.
        /// </summary>
        /// <param name="organizationRequest">The organization message request.</param>
        /// <param name="organizationResponse">The organization message response.</param>
        private void DisplayResponse(OrganizationRequest organizationRequest, OrganizationResponse organizationResponse)
        {
            Console.WriteLine("Created " + ((Account)organizationRequest.Parameters["Target"]).Name
                + " with account id as " + organizationResponse.Results["id"].ToString());
            //_newAccountIds.Add(new Guid(organizationResponse.Results["id"].ToString()));
        }

        /// <summary>
        /// Display the fault that resulted from processing an organization message request.
        /// </summary>
        /// <param name="organizationRequest">The organization message request.</param>
        /// <param name="count">nth request number from ExecuteMultiple request</param>
        /// <param name="organizationServiceFault">A WCF fault.</param>
        private void DisplayFault(OrganizationRequest organizationRequest, int count,
            OrganizationServiceFault organizationServiceFault)
        {
            Console.WriteLine("A fault occurred when processing {1} request, at index {0} in the request collection with a fault message: {2}", count + 1,
                organizationRequest.RequestName,
                organizationServiceFault.Message);
        }
        */

        #endregion Transport threads

        #region Menu Controls

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Stop Transport threads if running
            bwExport.CancelAsync();
            bwImport.CancelAsync();
            this.Dispose();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentProfile = null;
            textBoxTransportationProfileName.Text = "";
            comboBoxImportMode.SelectedIndex = 0;
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxConnectionTarget.SelectedItem = null;
            comboBoxTransportationProfiles.SelectedItem = null;
            comboBoxOperation.SelectedIndex = 2;
            comboBoxImportMode.SelectedIndex = 1;
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentTransportationProfileName = currentProfile.ProfileName;
            DialogResult dResTest;
            dResTest = MessageBox.Show("Are you sure you want to delete this Transportation Profile ?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }
            else
            {
                comboBoxTransportationProfiles.Items.Remove(currentProfile.ProfileName);
                comboBoxTransportationProfiles.SelectedItem = null;
                man.DeleteProfile(currentProfile);
                currentProfile = null;
                textBoxTransportationProfileName.Text = "";
                labelStructureLastLoadedDate.Text = "never";
                textBoxTransportationProfileName.Enabled = true;
                comboBoxConnectionSource.SelectedItem = null;
                comboBoxConnectionTarget.SelectedItem = null;
                comboBoxOperation.SelectedIndex = 2;
                comboBoxImportMode.SelectedIndex = 1;
                toolStripStatusLabel.Text = "Transportation Profile " + currentTransportationProfileName + " deleted";
            }
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;

            LogManager.WriteLog("Running Transportation Profile: " + currentProfile.ProfileName);

            //Check if there are selected entities to transport
            if (currentProfile.SelectedEntities == null || currentProfile.SelectedEntities.Count == 0)
            {
                MessageBox.Show("No entities selected for transport. Select the entities and then run the profile");
                LogManager.WriteLog("No entities selected for transport. Select the entities and then run the profile");
                return;
            }

            DateTime now = DateTime.Now;
            man.ReportFileName = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports\\TransportReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";
            man.importFailuresReportFileName = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports\\ImportFailuresReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";

            //Initialize Execution Reports folder
            string executionReportsFolder = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports";
            if (!Directory.Exists(executionReportsFolder))
            {
                Directory.CreateDirectory(executionReportsFolder);
            }

            //Create Transport Report
            TransportReport tr = new TransportReport(currentProfile.ProfileName);
            man.WriteTransportReport(tr, man.ReportFileName);

            transportReportToolStripMenuItem.Visible = false;
            labelImportSuccess.Text = "";
            labelImportFailures.Text = "";
            label2.Visible = false;
            label3.Visible = false;

            if (currentProfile.Operation == 0 || currentProfile.Operation == 2)
            {
                if (!transportRunning)
                {
                    buttonStopTransport.Visible = true;
                    runProfileToolStripMenuItem.Enabled = false;
                    pgbState.Visible = true;
                    SetFields(false);

                    toolStripStatusLabel.Text = "Initializing Data Export";
                    if (bwExport.IsBusy != true)
                        bwExport.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Transport already running!");
                }
            }
            else if (currentProfile.Operation == 1)
            {
                if (!transportRunning)
                {
                    buttonStopTransport.Visible = true;
                    runProfileToolStripMenuItem.Enabled = false;
                    pgbState.Visible = true;
                    SetFields(false);

                    toolStripStatusLabel.Text = "Initializing Data Import";
                    label2.Visible = true;
                    label3.Visible = true;
                    if (bwImport.IsBusy != true)
                        bwImport.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Transport already running!");
                }
            }
        }

        private void transportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(man.ReportFileName))
            {
                MessageBox.Show("Report File not found!");
                return;
            }

            TransportReportViewer tpv = new TransportReportViewer(man.ReportFileName, man.importFailuresReportFileName);
            tpv.Show();
            return;
        }

        private void buttonStopTransport_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "Stopping Transport";
            transportStopped = true;
            bwExport.CancelAsync();
            bwImport.CancelAsync();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#referencedatatransporter";
            Process.Start(startInfo);
        }

        #endregion Menu Controls
    }
}