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

using System;
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
    /// DataExport form
    /// </summary>
    public partial class DataExport : Form
    {
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        internal MSCRMDataExportProfile currentProfile;
        internal MSCRMDataExportManager man = new MSCRMDataExportManager();
        private List<ControlEnabled> cList = new List<ControlEnabled>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExport"/> class.
        /// </summary>
        public DataExport()
        {
            InitializeComponent();
            LogManager.WriteLog("Data Export Manager launched.");

            if (man.Profiles != null)
            {
                foreach (MSCRMDataExportProfile profile in man.Profiles)
                {
                    this.comboBoxProfiles.Items.AddRange(new object[] { profile.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<MSCRMDataExportProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }
        }

        private void DataExport_Load(object sender, EventArgs e)
        {
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
            comboBoxFieldSeparator.SelectedIndex = 2;
            comboBoxDataSeparator.SelectedIndex = 1;
            comboBoxEncoding.SelectedIndex = 0;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                foreach (string fileLoc in filePaths)
                {
                    // Code to read the contents of the text file
                    if (File.Exists(fileLoc))
                    {
                        using (TextReader tr = new StreamReader(fileLoc))
                        {
                            //MessageBox.Show(tr.ReadToEnd());
                            xmlEditor1.Text = tr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

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

            if (comboBoxFormat.SelectedItem == null)
            {
                MessageBox.Show("You must select an Export Format for the Profile");
                return false;
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Data Export Profile having the same name exist already
                MSCRMDataExportProfile existingProfile = man.Profiles.Find(d => d.ProfileName.ToLower() == textBoxProfileName.Text.ToLower());
                if (existingProfile != null)
                {
                    MessageBox.Show("Profile with the name " + textBoxProfileName.Text + " exist already. Please select another name");
                    return false;
                }

                MSCRMDataExportProfile newProfile = new MSCRMDataExportProfile();
                newProfile.ProfileName = textBoxProfileName.Text;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.setSourceConneciton();
                newProfile.ExportFormat = comboBoxFormat.SelectedItem.ToString();
                if (comboBoxFieldSeparator.SelectedIndex == 0)
                    newProfile.FieldSeparator = ",";
                else if (comboBoxFieldSeparator.SelectedIndex == 1)
                    newProfile.FieldSeparator = ":";
                else if (comboBoxFieldSeparator.SelectedIndex == 2)
                    newProfile.FieldSeparator = ";";
                else if (comboBoxFieldSeparator.SelectedIndex == 3)
                    newProfile.FieldSeparator = "	";

                if (comboBoxDataSeparator.SelectedIndex == 0)
                    newProfile.DataSeparator = "'";
                else if (comboBoxDataSeparator.SelectedIndex == 1)
                    newProfile.DataSeparator = "\"";
                else if (comboBoxDataSeparator.SelectedIndex == 2)
                    newProfile.DataSeparator = "";

                newProfile.Encoding = "Default";
                if (comboBoxEncoding.SelectedIndex == 1)
                    newProfile.Encoding = "UTF8";
                else if (comboBoxEncoding.SelectedIndex == 1)
                    newProfile.Encoding = "Unicode";
                else if (comboBoxEncoding.SelectedIndex == 2)
                    newProfile.Encoding = "ASCII";
                else if (comboBoxEncoding.SelectedIndex == 3)
                    newProfile.Encoding = "BigEndianUnicode";

                newProfile.FetchXMLQuery = xmlEditor1.Text;
                man.CreateProfile(newProfile);
                comboBoxProfiles.Items.AddRange(new object[] { newProfile.ProfileName });
                comboBoxProfiles.SelectedItem = newProfile.ProfileName;
                currentProfile = newProfile;
            }
            else
            {
                currentProfile.ProfileName = textBoxProfileName.Text;
                currentProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                currentProfile.ExportFormat = comboBoxFormat.SelectedItem.ToString();
                if (comboBoxFieldSeparator.SelectedIndex == 0)
                    currentProfile.FieldSeparator = ",";
                else if (comboBoxFieldSeparator.SelectedIndex == 1)
                    currentProfile.FieldSeparator = ":";
                else if (comboBoxFieldSeparator.SelectedIndex == 2)
                    currentProfile.FieldSeparator = ";";
                else if (comboBoxFieldSeparator.SelectedIndex == 3)
                    currentProfile.FieldSeparator = "	";

                if (comboBoxDataSeparator.SelectedIndex == 0)
                    currentProfile.DataSeparator = "'";
                else if (comboBoxDataSeparator.SelectedIndex == 1)
                    currentProfile.DataSeparator = "\"";
                else if (comboBoxDataSeparator.SelectedIndex == 2)
                    currentProfile.DataSeparator = "";

                currentProfile.Encoding = "Default";
                if (comboBoxEncoding.SelectedIndex == 1)
                    currentProfile.Encoding = "UTF8";
                else if (comboBoxEncoding.SelectedIndex == 1)
                    currentProfile.Encoding = "Unicode";
                else if (comboBoxEncoding.SelectedIndex == 2)
                    currentProfile.Encoding = "ASCII";
                else if (comboBoxEncoding.SelectedIndex == 3)
                    currentProfile.Encoding = "BigEndianUnicode";

                currentProfile.FetchXMLQuery = xmlEditor1.Text;
                currentProfile.setSourceConneciton();
                MSCRMDataExportProfile oldDEP = man.GetProfile(currentProfile.ProfileName);
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = "Profile " + currentProfile.ProfileName + " saved.";
            LogManager.WriteLog("Profile " + currentProfile.ProfileName + " saved.");
            return result;
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                xmlEditor1.Text = "";
                textBoxProfileName.Enabled = true;
                comboBoxConnectionSource.SelectedItem = null;
                toolStripStatusLabel1.Text = "Profile " + currentProfileName + " deleted";
            }
        }

        private void comboBoxDataExportProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            if (comboBoxProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxProfiles.SelectedIndex];
                textBoxProfileName.Text = currentProfile.ProfileName;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                comboBoxFormat.SelectedItem = currentProfile.ExportFormat;
                xmlEditor1.Text = currentProfile.FetchXMLQuery;

                if (currentProfile.FieldSeparator == ",")
                    comboBoxFieldSeparator.SelectedItem = "Comma (,)";
                else if (currentProfile.FieldSeparator == ";")
                    comboBoxFieldSeparator.SelectedItem = "Semi-colon (;)";
                else if (currentProfile.FieldSeparator == @"\t")
                    comboBoxFieldSeparator.SelectedItem = "Tab (\t)";

                if (currentProfile.DataSeparator == "'")
                    comboBoxDataSeparator.SelectedItem = "Signle Quote (')";
                else if (currentProfile.DataSeparator == "\"")
                    comboBoxDataSeparator.SelectedItem = "Double Quote (\")";
                else if (currentProfile.DataSeparator == "")
                    comboBoxDataSeparator.SelectedItem = "None";

                comboBoxEncoding.SelectedItem = "Default";
                if (currentProfile.Encoding == "UTF8")
                    comboBoxEncoding.SelectedItem = "UTF8";
                else if (currentProfile.Encoding == "Unicode")
                    comboBoxEncoding.SelectedItem = "Unicode";
                else if (currentProfile.Encoding == "ASCII")
                    comboBoxEncoding.SelectedItem = "ASCII";
                else if (currentProfile.Encoding == "BigEndianUnicode")
                    comboBoxEncoding.SelectedItem = "BigEndianUnicode";

                deleteProfileToolStripMenuItem.Enabled = true;
                newToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                textBoxProfileName.Enabled = false;
                runProfileToolStripMenuItem.Enabled = true;
            }
            else
            {
                currentProfile = null;
                textBoxProfileName.Text = "";
                comboBoxFormat.SelectedItem = null;
                deleteProfileToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                textBoxProfileName.Enabled = true;
                runProfileToolStripMenuItem.Enabled = false;
            }

            buttonOpenInExcel.Visible = false;
            dataExportReportToolStripMenuItem.Visible = false;
        }

        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonOpenInExcel.Visible = false;
            dataExportReportToolStripMenuItem.Visible = false;

            if (!SaveProfile())
                return;

            setStateAllControls(false);

            toolStripStatusLabel1.Text = "Exporting Data. Please wait...";
            Application.DoEvents();

            try
            {
                man.RunProfile(currentProfile);
                if (man.ExportedRecordsNumber > 0)
                    buttonOpenInExcel.Visible = true;
                dataExportReportToolStripMenuItem.Visible = true;
                toolStripStatusLabel1.Text = "Export finished. Exported " + man.ExportedRecordsNumber + " record(s).";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                toolStripStatusLabel1.Text = "Error...";
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
                toolStripStatusLabel1.Text = "Error...";
            }

            setStateAllControls(true);
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProfiles.SelectedItem = null;
            textBoxProfileName.Text = "";
            textBoxProfileName.Enabled = true;
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxFieldSeparator.SelectedIndex = 2;
            comboBoxDataSeparator.SelectedIndex = 1;
            comboBoxEncoding.SelectedIndex = 0;
            xmlEditor1.Text = "";
            newToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private void buttonExportInExcel_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(man.ExportedDataFileName))
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
                startInfo.Arguments = man.ExportedDataFileName;
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

        private void dataExportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(man.ReportFileName))
            {
                MessageBox.Show("File not found!");
                return;
            }

            DataExportReportViewer derv = new DataExportReportViewer(man.ReportFileName);
            derv.Show();
            return;
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFormat.SelectedItem != null && comboBoxFormat.SelectedItem.ToString() == "CSV")
            {
                label4.Visible = true;
                comboBoxFieldSeparator.Visible = true;
                label5.Visible = true;
                comboBoxDataSeparator.Visible = true;
            }
            else
            {
                label4.Visible = false;
                comboBoxFieldSeparator.Visible = false;
                label5.Visible = false;
                comboBoxDataSeparator.Visible = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#dataexportmanager";
            Process.Start(startInfo);
        }

        private void setStateAllControls(bool newState)
        {
            if (newState)
            {
                foreach (ControlEnabled ce in cList)
                {
                    ce.Control.Enabled = ce.Enabled;
                }
            }
            else
            {
                cList = new List<ControlEnabled>();
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    cList.Add(new ControlEnabled { Control = c, Enabled = c.Enabled });
                    c.Enabled = false;
                }
            }
        }
    }
}