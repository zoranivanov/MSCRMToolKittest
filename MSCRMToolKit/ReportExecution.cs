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
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// ReportExecution
    /// </summary>
    public partial class ReportExecution : Form
    {
        private OrganizationServiceProxy _serviceProxy = null;
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        private List<MSCRMReport> reportsList = new List<MSCRMReport>();
        private Guid selectedReportId = Guid.Empty;
        internal MSCRMReportExecutionProfile currentProfile;
        internal MSCRMReportExecutionManager man = new MSCRMReportExecutionManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportExecution"/> class.
        /// </summary>
        public ReportExecution()
        {
            InitializeComponent();
            LogManager.WriteLog("Solution Export Manager launched.");

            if (man.Profiles != null)
            {
                foreach (MSCRMReportExecutionProfile profile in man.Profiles)
                {
                    this.comboBoxProfiles.Items.AddRange(new object[] { profile.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<MSCRMReportExecutionProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProfiles.SelectedItem = null;
            textBoxProfileName.Text = "";
            textBoxProfileName.Enabled = true;
            comboBoxConnectionSource.SelectedItem = null;
            newToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
        }

        private bool SaveProfile()
        {
            bool result = true;
            //Check that all fields are provided
            if (string.IsNullOrEmpty(textBoxProfileName.Text))
            {
                MessageBox.Show("Report Execution Profile Name is mandatory!");
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
                MessageBox.Show("You shouldn't use spaces nor the following characters (\\/<>?*:|\"') in the Report Execution Profile Name as it will be used to create folders and files.");
                return false;
            }

            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a Source for the Profile");
                return false;
            }

            if (this.selectedReportId == Guid.Empty)
            {
                MessageBox.Show("You must select a Report for the Profile");
                return false;
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Solution Transport Profile having the same name exist already
                MSCRMReportExecutionProfile profile = man.Profiles.Find(p => p.ProfileName.ToLower() == textBoxProfileName.Text.ToLower());
                if (profile != null)
                {
                    MessageBox.Show("Report Execution Profile with the name " + textBoxProfileName.Text + " exist already. Please select another name");
                    return false;
                }

                MSCRMReportExecutionProfile newProfile = new MSCRMReportExecutionProfile();
                newProfile.ProfileName = textBoxProfileName.Text;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.SelectedReprortId = this.selectedReportId;
                newProfile.setSourceConneciton();

                man.CreateProfile(newProfile);
                comboBoxProfiles.Items.AddRange(new object[] { newProfile.ProfileName });
                comboBoxProfiles.SelectedItem = newProfile.ProfileName;
                currentProfile = newProfile;
            }
            else
            {
                currentProfile.ProfileName = textBoxProfileName.Text;
                currentProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                currentProfile.SelectedReprortId = this.selectedReportId;
                currentProfile.setSourceConneciton();
                MSCRMReportExecutionProfile oldDEP = man.GetProfile(currentProfile.ProfileName);
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = "Report Execution Profile " + currentProfile.ProfileName + " saved.";
            return result;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentProfileName = currentProfile.ProfileName;
            DialogResult dResTest;
            dResTest = MessageBox.Show("Are you sure you want to delete this Report Execution Profile ?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

                toolStripStatusLabel1.Text = "Report Execution Profile " + currentProfileName + " deleted";
                LogManager.WriteLog("Report Execution Profile " + currentProfileName + " deleted");
            }
        }

        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //find the newest log file
            if (!Directory.Exists("log"))
                Directory.CreateDirectory("log");
            DirectoryInfo logFolder = new DirectoryInfo("log");
            if ((Directory.GetFiles("log").Length == 0))
                LogManager.WriteLog("Initializing log.");

            FileInfo logFileName = logFolder.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();

            //open the log file in notepad
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "NOTEPAD.EXE";
            startInfo.Arguments = logFileName.FullName;
            Process.Start(startInfo);
        }

        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("log"))
                Directory.CreateDirectory("log");
            DirectoryInfo logFolder = new DirectoryInfo("log");

            //open the log folder in explorer
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EXPLORER.EXE";
            startInfo.Arguments = logFolder.ToString(); ;
            Process.Start(startInfo);
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            if (comboBoxProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxProfiles.SelectedIndex];
                this.selectedReportId = currentProfile.SelectedReprortId;
                textBoxProfileName.Text = currentProfile.ProfileName;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                deleteProfileToolStripMenuItem.Enabled = true;
                newToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                textBoxProfileName.Enabled = false;
                runProfileToolStripMenuItem.Enabled = true;
                toolStripStatusLabel1.Text = "Report Execution Profile " + currentProfile.ProfileName + " loaded.";
                LogManager.WriteLog("Report Execution Profile " + currentProfile.ProfileName + " loaded.");
            }
            else
            {
                currentProfile = null;
                textBoxProfileName.Text = "";
                deleteProfileToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                textBoxProfileName.Enabled = true;
                runProfileToolStripMenuItem.Enabled = false;
                this.selectedReportId = Guid.Empty;
            }
        }

        private void buttonLoadReports_Click(object sender, EventArgs e)
        {
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a connection before loading Reports!");
                return;
            }

            toolStripStatusLabel1.Text = "Loading reports. Please wait...";
            Application.DoEvents();

            reportsList = new List<MSCRMReport>();
            dataGridView1.DataSource = reportsList;

            try
            {
                MSCRMConnection connection = cm.MSCRMConnections[comboBoxConnectionSource.SelectedIndex];
                _serviceProxy = cm.connect(connection);

                QueryExpression queryReports = new QueryExpression
                {
                    EntityName = "report",
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression(),
                };

                EntityCollection reports = _serviceProxy.RetrieveMultiple(queryReports);

                foreach (Entity report in reports.Entities)
                {
                    string description = report.Attributes.Contains("description") ? (string)report["description"] : "";
                    MSCRMReport MSCRMReport = new MSCRMReport
                    {
                        Id = report.Id,
                        Name = (string)report["name"],
                        Description = description
                    };
                    reportsList.Add(MSCRMReport);
                }
                man.WriteReports(comboBoxConnectionSource.SelectedItem.ToString(), reportsList);
                dataGridView1.DataSource = reportsList.ToList();
                toolStripStatusLabel1.Text = "Reports loaded.";
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
        }

        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            reportsList = new List<MSCRMReport>();
            dataGridView1.DataSource = reportsList;
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                return;
            }
            List<MSCRMReport> reports = man.ReadReports(comboBoxConnectionSource.SelectedItem.ToString());
            reportsList = reports;
            dataGridView1.DataSource = reportsList;
            //Check selected solution
            if (currentProfile != null)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString() == currentProfile.SelectedReprortId.ToString())
                    {
                        DataGridViewCell cbc = row.Cells[0];
                        cbc.Value = true;
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = false;
            }
            dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
            selectedReportId = new Guid(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
        }
    }
}