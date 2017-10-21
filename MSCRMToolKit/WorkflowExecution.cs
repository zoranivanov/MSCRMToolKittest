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
    /// WorkflowExecution form
    /// </summary>
    public partial class WorkflowExecution : Form
    {
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        internal MSCRMWorkflowExecutionProfile currentProfile;
        internal MSCRMWorkflowExecutionManager man = new MSCRMWorkflowExecutionManager();
        private List<MSCRMWorkflow> Workflows = new List<MSCRMWorkflow>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowExecution"/> class.
        /// </summary>
        public WorkflowExecution()
        {
            InitializeComponent();
            LogManager.WriteLog("Workflow Execution Manager launched.");

            if (man.Profiles != null)
            {
                foreach (MSCRMWorkflowExecutionProfile profile in man.Profiles)
                {
                    this.comboBoxProfiles.Items.AddRange(new object[] { profile.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<MSCRMWorkflowExecutionProfile>();
            }

            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProfiles.SelectedItem = null;
            textBoxProfileName.Text = "";
            textBoxProfileName.Enabled = true;
            comboBoxConnectionSource.SelectedItem = null;
            xmlEditor1.Text = "";
            newToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
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

            if (comboBoxWorkflows.SelectedItem == null)
            {
                MessageBox.Show("You must select a Workflow for the Profile");
                return false;
            }

            //Vérify that the query was provided
            if (xmlEditor1.Text == "")
            {
                MessageBox.Show("You must provide a Query for the records!");
                return false;
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Data Export Profile having the same name exist already
                MSCRMWorkflowExecutionProfile existingProfile = man.Profiles.Find(d => d.ProfileName.ToLower() == textBoxProfileName.Text.ToLower());
                if (existingProfile != null)
                {
                    MessageBox.Show("Profile with the name " + textBoxProfileName.Text + " exist already. Please select another name");
                    return false;
                }

                MSCRMWorkflowExecutionProfile newProfile = new MSCRMWorkflowExecutionProfile();
                newProfile.ProfileName = textBoxProfileName.Text;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.setSourceConneciton();
                newProfile.WorkflowId = this.Workflows.Find(w => w.Name == comboBoxWorkflows.SelectedItem.ToString()).Id;
                newProfile.WorkflowName = comboBoxWorkflows.SelectedItem.ToString();
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
                currentProfile.WorkflowId = this.Workflows.Find(w => w.Name == comboBoxWorkflows.SelectedItem.ToString()).Id;
                currentProfile.WorkflowName = comboBoxWorkflows.SelectedItem.ToString();
                currentProfile.FetchXMLQuery = xmlEditor1.Text;
                currentProfile.setSourceConneciton();
                MSCRMWorkflowExecutionProfile oldDEP = man.GetProfile(currentProfile.ProfileName);
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = "Profile " + currentProfile.ProfileName + " saved.";
            LogManager.WriteLog("Profile " + currentProfile.ProfileName + " saved.");
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

        private void executeWokflowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;

            toolStripStatusLabel1.Text = "Running Profile: " + currentProfile.ProfileName + ". Please wait...";
            Application.DoEvents();

            try
            {
                man.RunProfile(currentProfile);
                toolStripStatusLabel1.Text = "All workflows were launched.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                toolStripStatusLabel1.Text = "Error.";
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    toolStripStatusLabel1.Text = "Error.";
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                    toolStripStatusLabel1.Text = "Error.";
                }
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

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            if (comboBoxProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxProfiles.SelectedIndex];
                textBoxProfileName.Text = currentProfile.ProfileName;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                comboBoxWorkflows.SelectedItem = currentProfile.WorkflowName;
                xmlEditor1.Text = currentProfile.FetchXMLQuery;
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
                comboBoxWorkflows.SelectedItem = null;
                deleteProfileToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                textBoxProfileName.Enabled = true;
                runProfileToolStripMenuItem.Enabled = false;
            }
        }

        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxWorkflows.Items.Clear();
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                return;
            }
            List<MSCRMWorkflow> workflows = man.ReadWorkflows(comboBoxConnectionSource.SelectedItem.ToString());
            this.Workflows = workflows;
            foreach (MSCRMWorkflow workflow in workflows)
            {
                this.comboBoxWorkflows.Items.AddRange(new object[] { workflow.Name });
            }
        }

        private void buttonLoadWorkflows_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxConnectionSource.SelectedItem == null)
                {
                    MessageBox.Show("You must select a connection before loading workflows!");
                    return;
                }

                toolStripStatusLabel1.Text = "Loading workflows. Please wait...";
                Application.DoEvents();

                string fetchXMLQuery = @"
                    <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                    <entity name='workflow'><attribute name='workflowid' /><attribute name='name' />
                    <order attribute='name' descending='false' /><filter type='and'>
                    <condition attribute='type' operator='eq' value='1' />
                    <condition attribute='category' operator='eq' value='0' />
                    <condition attribute='statecode' operator='eq' value='1' />
                    <condition attribute='ondemand' operator='eq' value='1' />
                    </filter></entity></fetch>";

                MSCRMConnection connection = cm.MSCRMConnections[comboBoxConnectionSource.SelectedIndex];
                man._serviceProxy = cm.connect(connection);

                EntityCollection result = man._serviceProxy.RetrieveMultiple(new FetchExpression(fetchXMLQuery));

                List<MSCRMWorkflow> workflows = new List<MSCRMWorkflow>();
                foreach (var workflow in result.Entities)
                {
                    //this.comboBoxWorkflows.Items.AddRange(new object[] { (string)workflow["name"] });
                    workflows.Add(new MSCRMWorkflow { Id = (Guid)workflow["workflowid"], Name = (string)workflow["name"] });
                }
                man.WriteWorkflows(comboBoxConnectionSource.SelectedItem.ToString(), workflows);

                //Read workflows
                this.comboBoxWorkflows.Items.Clear();
                List<MSCRMWorkflow> readedWorkflows = man.ReadWorkflows(comboBoxConnectionSource.SelectedItem.ToString());
                foreach (MSCRMWorkflow workflow in readedWorkflows)
                {
                    this.comboBoxWorkflows.Items.AddRange(new object[] { workflow.Name });
                }
                this.Workflows = readedWorkflows;

                toolStripStatusLabel1.Text = "Workflows loaded.";
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    toolStripStatusLabel1.Text = "Error.";
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                    toolStripStatusLabel1.Text = "Error.";
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#workflowexecutionmanager";
            Process.Start(startInfo);
        }
    }
}