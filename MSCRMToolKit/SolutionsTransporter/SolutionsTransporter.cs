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

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
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
    /// SolutionsTransporter class
    /// </summary>
    public partial class SolutionsTransporter : Form
    {
        private OrganizationServiceProxy _serviceProxy = null;
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        private List<MSCRMSolution> solutionsLst = new List<MSCRMSolution>();
        internal MSCRMSolutionsTransportProfile currentProfile;
        internal MSCRMSolutionsTransportManager man = new MSCRMSolutionsTransportManager();

        private BackgroundWorker bwExport = new BackgroundWorker();
        private BackgroundWorker bwImport = new BackgroundWorker();
        private BackgroundWorker bwSolutionImport = new BackgroundWorker();

        ImportSolutionRequest impSolReqWithMonitoring = null;

        //private bool exportInProgress = false;
        //private bool importInProgress = false;
        private bool solutionImportInProgress = false;
        private bool transportStopped = false;
        private bool transportRunning = false;

        private string StatusMessage = "";
        private string importSourceFolder = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionsTransporter"/> class.
        /// </summary>
        public SolutionsTransporter()
        {
            InitializeComponent();

            LogManager.WriteLog("Solutions Transporter launched.");

            if (man.Profiles != null)
            {
                foreach (MSCRMSolutionsTransportProfile profile in man.Profiles)
                {
                    this.comboBoxProfiles.Items.AddRange(new object[] { profile.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<MSCRMSolutionsTransportProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                this.comboBoxConnectionTarget.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }
            dataGridView1.AutoGenerateColumns = false;

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

            bwImport.WorkerSupportsCancellation = true;
            bwSolutionImport.DoWork += new DoWorkEventHandler(bwSolutionImport_DoWork);
            bwSolutionImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSolutionImport_RunWorkerCompleted);
        }      

        #region Transport threads

        private void bwSolutionImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            solutionImportInProgress = false;
        }

        private void bwSolutionImport_DoWork(object sender, DoWorkEventArgs e)
        {
            if (impSolReqWithMonitoring == null)
                return;
            try
            {             
                MSCRMConnection connection = currentProfile.getTargetConneciton();
                OrganizationServiceProxy _serviceProxy1 = cm.connect(connection);
                _serviceProxy1.Execute(impSolReqWithMonitoring);
            }
            catch(Exception ex)
            {
                transportStopped = true;
                LogManager.WriteLog("Error ! Detail : " + ex.Message);
                MessageBox.Show("Error ! Detail : " + ex.Message);
                toolStripStatusLabel1.Text = "Transport stopped.";
            }            
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                toolStripStatusLabel1.Text = "Transport stopped.";
            }
            else if (e.Cancelled)
            {
                toolStripStatusLabel1.Text = "Transport canceled";
            }
            else
            {
                toolStripStatusLabel1.Text = "Import Finished";
            }

            transportRunning = false;
            buttonStopTransport.Visible = false;
            runProfileToolStripMenuItem.Enabled = true;
            pgbState.Visible = false;
            SetFields(true);

            //get all import logs
            if (importSourceFolder == "")
                return;

            //string[] importlogFilesNames = Directory.GetFiles(importSourceFolder, "*.xml");

            DirectoryInfo DirInfo = new DirectoryInfo(importSourceFolder);
            FileInfo[] importlogFilesNames = DirInfo.GetFiles("*.xml").OrderByDescending(p => p.CreationTime).ToArray();

            if (importlogFilesNames.Count() == 0)
                return;
            foreach (FileInfo importlogFileName in importlogFilesNames)
            {
                ToolStripMenuItem ImportLog1ToolStripMenuItem = new ToolStripMenuItem();
                ImportLog1ToolStripMenuItem.Name = importlogFileName.Name;
                ImportLog1ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
                ImportLog1ToolStripMenuItem.Text = importlogFileName.Name;
                ImportLog1ToolStripMenuItem.ToolTipText = importlogFileName.FullName;
                ImportLog1ToolStripMenuItem.Click += new System.EventHandler(this.openImportLogToolStripMenuItem_Click);
                this.importLogsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {ImportLog1ToolStripMenuItem});

                this.importLogsToolStripMenuItem.Visible = true;
            }
        }

        private void bwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = StatusMessage;
            if (currentProfile.Operation == 2)
                this.pgbState.Value = e.ProgressPercentage / 2+50;
            else
                this.pgbState.Value = e.ProgressPercentage;
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //Import Data
            ImportGUI(worker, e);
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
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
                toolStripStatusLabel1.Text = "Transport canceled.";
                LogManager.WriteLog("Transport canceled.");
                SetFields(true);
                transportRunning = false;
                transportStopped = true;
                buttonStopTransport.Visible = false;
                runProfileToolStripMenuItem.Enabled = true;
                pgbState.Visible = false;
            }
            else
            {
                toolStripStatusLabel1.Text = "Export Finished";

                if (currentProfile.Operation == 2)
                {
                    if (!transportStopped)
                    {
                        transportRunning = true;
                        SetFields(false);
                        toolStripStatusLabel1.Text = "Initializing Solutions Import";
                        label2.Visible = true;
                        label3.Visible = true;
                        label4.Visible = true;
                        bwImport.RunWorkerAsync();
                    }
                    else
                    {
                        bwImport.CancelAsync();
                        transportRunning = false;
                        SetFields(true);
                        buttonStopTransport.Visible = false;
                        runProfileToolStripMenuItem.Enabled = true;
                        pgbState.Visible = false;
                    }
                }
                else
                {
                    SetFields(true);
                    transportRunning = false;
                    buttonStopTransport.Visible = false;
                    runProfileToolStripMenuItem.Enabled = true;
                    pgbState.Visible = false;
                }
            }
        }

        private void bwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = StatusMessage;
            if (currentProfile.Operation == 2)
                this.pgbState.Value = e.ProgressPercentage / 2;
            else
                this.pgbState.Value = e.ProgressPercentage;
        }

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            transportStopped = false;
            BackgroundWorker worker = sender as BackgroundWorker;
            //Export data
            ExportGUI(worker, e);            
        }

        private void ExportGUI(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //Set Data export folder
                if (!Directory.Exists(currentProfile.SolutionExportFolder))
                    Directory.CreateDirectory(currentProfile.SolutionExportFolder);

                MSCRMConnection connection = currentProfile.getSourceConneciton();
                _serviceProxy = cm.connect(connection);



                ////perform a retrievemultiple on solutioncomponent entities for the given solution
                //RetrieveMultipleRequest rmr = new RetrieveMultipleRequest();
                //RetrieveMultipleResponse resp = new RetrieveMultipleResponse();
                //QueryExpression query = new QueryExpression()
                //{
                //    EntityName = "solutioncomponent",
                //    ColumnSet = new ColumnSet(true),
                //    Criteria = new FilterExpression
                //    {
                //        FilterOperator = LogicalOperator.And,
                //        Conditions =
                //   {
                //       new ConditionExpression
                //       {
                //           AttributeName = "objectid",
                //           Operator = ConditionOperator.Equal,
                //           Values = { "b00c4576-d156-424b-b438-f118500ba058" }
                //       }
                //   }
                //                }
                //            };
                //rmr.Query = query;
                //resp = (RetrieveMultipleResponse)_serviceProxy.Execute(rmr);

                ////use RemoveSolutionComponentRequest to remove solution elements from solution.
                //foreach (Entity sc in resp.EntityCollection.Entities)
                //{

                //    RemoveSolutionComponentRequest removereq = new RemoveSolutionComponentRequest();
                //    removereq.SolutionUniqueName = "CXO_NEW";

                //    removereq.ComponentId = ((Guid)sc["objectid"]);
                //    removereq.ComponentType = ((OptionSetValue)sc["componenttype"]).Value;
                //    RemoveSolutionComponentResponse removeresp = (RemoveSolutionComponentResponse)_serviceProxy.Execute(removereq);
                //}





                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                //Download fresh list of solutions for versions update
                List<MSCRMSolution> solutions = man.DownloadSolutions(connection);

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                DateTime now = DateTime.Now;
                string folderName = String.Format("{0:yyyyMMddHHmmss}", now);

                if (!Directory.Exists(currentProfile.SolutionExportFolder + "\\" + folderName))
                    Directory.CreateDirectory(currentProfile.SolutionExportFolder + "\\" + folderName);

                //Check if customizations are to be published
                if (currentProfile.PublishAllCustomizationsSource)
                {
                    LogManager.WriteLog("Publishing all Customizations on " + connection.ConnectionName + " ...");
                    StatusMessage = "Publishing all Customizations on " + connection.ConnectionName + " ...";
                    worker.ReportProgress(0);

                    PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
                    _serviceProxy.Execute(publishRequest);
                }

                int solutionsCpt = 0;
                foreach (string SolutionName in currentProfile.SelectedSolutionsNames)
                {
                    solutionsCpt++;
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    LogManager.WriteLog("Exporting Solution " + SolutionName + " from " + connection.ConnectionName);
                    StatusMessage = "Exporting Solution " + SolutionName + " from " + connection.ConnectionName;
                    worker.ReportProgress(solutionsCpt * 100 / currentProfile.SelectedSolutionsNames.Count);

                    ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
                    exportSolutionRequest.Managed = currentProfile.ExportAsManaged;
                    exportSolutionRequest.SolutionName = SolutionName;
                    exportSolutionRequest.ExportAutoNumberingSettings = currentProfile.ExportAutoNumberingSettings;
                    exportSolutionRequest.ExportCalendarSettings = currentProfile.ExportCalendarSettings;
                    exportSolutionRequest.ExportCustomizationSettings = currentProfile.ExportCustomizationSettings;
                    exportSolutionRequest.ExportEmailTrackingSettings = currentProfile.ExportEmailTrackingSettings;
                    exportSolutionRequest.ExportGeneralSettings = currentProfile.ExportGeneralSettings;
                    exportSolutionRequest.ExportIsvConfig = currentProfile.ExportIsvConfig;
                    exportSolutionRequest.ExportMarketingSettings = currentProfile.ExportMarketingSettings;
                    exportSolutionRequest.ExportOutlookSynchronizationSettings = currentProfile.ExportOutlookSynchronizationSettings;
                    exportSolutionRequest.ExportRelationshipRoles = currentProfile.ExportRelationshipRoles;

                    string managed = "";
                    if (currentProfile.ExportAsManaged)
                        managed = "managed";
                    MSCRMSolution selectedSolution = solutions.Find(s => s.UniqueName == SolutionName);
                    string selectedSolutionVersion = selectedSolution.Version.Replace(".", "_");
                    ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)_serviceProxy.Execute(exportSolutionRequest);
                    byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
                    string filename = SolutionName + "_" + selectedSolutionVersion + "_" + managed + ".zip";
                    File.WriteAllBytes(currentProfile.SolutionExportFolder + "\\" + folderName + "\\" + filename, exportXml);
                    LogManager.WriteLog("Export finished for Solution: " + SolutionName + ". Exported file: " + filename);
                    StatusMessage = "Export finished for Solution: " + SolutionName + ". Exported file: " + filename;
                    worker.ReportProgress(solutionsCpt * 100 / currentProfile.SelectedSolutionsNames.Count);

                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                LogManager.WriteLog("Export finished for Profile: " + currentProfile.ProfileName);
                StatusMessage = "Export finished for Profile: " + currentProfile.ProfileName;
                worker.ReportProgress(solutionsCpt * 100 / currentProfile.SelectedSolutionsNames.Count);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                throw;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                }
                throw;
            }
        }

        private void ImportGUI(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try 
            {
                //Check if there is a solutions to import
                if (Directory.Exists(currentProfile.SolutionExportFolder))
                {
                    IOrderedEnumerable<string> subDirectories = Directory.GetDirectories(currentProfile.SolutionExportFolder).OrderByDescending(x => x);
                    if (subDirectories.Count<string>() == 0)
                    {
                        LogManager.WriteLog("There are no solutions for import.");
                        StatusMessage = "There are no solutions for import.";
                        return;
                    }

                    //Check which solutions to import: Newest, Oldest, specific exprot date solutions
                    string solutionsToImportFolder = "";
                    if (currentProfile.SolutionsToImport == "Newest")
                        solutionsToImportFolder = subDirectories.ElementAt(0);
                    else if (currentProfile.SolutionsToImport == "Oldest")
                        solutionsToImportFolder = subDirectories.ElementAt(subDirectories.Count<string>() - 1);
                    else
                        solutionsToImportFolder = subDirectories.First(s => s.EndsWith(currentProfile.SolutionsToImport));

                    if (solutionsToImportFolder == "")
                    {
                        LogManager.WriteLog("The specified solutions to import were not found.");
                        StatusMessage = "The specified solutions to import were not found.";
                        return;
                    }

                    //get all solutions archives
                    string[] solutionsArchivesNames = Directory.GetFiles(solutionsToImportFolder, "*.zip");
                    if (solutionsArchivesNames.Count<string>() == 0)
                    {
                        LogManager.WriteLog("There are no solutions for import.");
                        StatusMessage = "There are no solutions for import.";
                        return;
                    }

                    string[] pathList = solutionsToImportFolder.Split(new Char[] { '\\' });
                    string DirectoryName = pathList[pathList.Count<string>() - 1];
                    LogManager.WriteLog("Importing solutions from " + DirectoryName);
                
                    MSCRMConnection connection = currentProfile.getTargetConneciton();
                    _serviceProxy = cm.connect(connection);
                    LogManager.WriteLog("Start importing solutions in " + connection.ConnectionName);

                    int solutionsCpt = 0;

                    foreach (string solutionArchiveName in solutionsArchivesNames)
                    {                 
                        bool selectedsolutionfound = false;
                        foreach (string solutionname in currentProfile.SelectedSolutionsNames)
                        {
                            if (Path.GetFileName(solutionArchiveName).StartsWith(solutionname))
                                selectedsolutionfound = true;
                        }

                        if(!selectedsolutionfound)
                            continue;

                        solutionsCpt++;

                        //Import Solution
                        LogManager.WriteLog("Importing solution archive " + Path.GetFileName(solutionArchiveName) + " into " + connection.ConnectionName);
                        StatusMessage = "Importing solution archive " + Path.GetFileName(solutionArchiveName) + " into " + connection.ConnectionName;
                        //worker.ReportProgress(solutionsCpt * 100 / currentProfile.SelectedSolutionsNames.Count);

                        byte[] fileBytes = File.ReadAllBytes(solutionArchiveName);

                        Guid ImportJobId = Guid.NewGuid();

                        impSolReqWithMonitoring = new ImportSolutionRequest()
                        {
                            CustomizationFile = fileBytes,
                            OverwriteUnmanagedCustomizations = currentProfile.OverwriteUnmanagedCustomizations,
                            PublishWorkflows = currentProfile.PublishWorkflows,
                            ImportJobId = ImportJobId
                        };

                        if (bwSolutionImport.IsBusy != true)
                            bwSolutionImport.RunWorkerAsync();
                        solutionImportInProgress = true;

                        while (solutionImportInProgress)
                        {
                            if (transportStopped)
                              return;
                            int progress = GetImportProgress(ImportJobId, connection);
                            worker.ReportProgress(progress);
                            System.Threading.Thread.Sleep(5000);
                        }

                        //if (transportStopped)
                          //  return;

                        //Entity ImportJob = _serviceProxy.Retrieve("importjob", ImportJobId, new ColumnSet(true));
                        ////File.WriteAllText(solutionsToImportFolder + "\\importlog_ORIGINAL_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + ".xml", (string)ImportJob["data"]);
                        //impSolReqWithMonitoring = null;

                        //RetrieveFormattedImportJobResultsRequest importLogRequest = new RetrieveFormattedImportJobResultsRequest()
                        //{
                        //    ImportJobId = ImportJobId
                        //};

                        //RetrieveFormattedImportJobResultsResponse importLogResponse = (RetrieveFormattedImportJobResultsResponse)_serviceProxy.Execute(importLogRequest);

                        //DateTime now = DateTime.Now;
                        //string timeNow = String.Format("{0:yyyyMMddHHmmss}", now);

                        //string exportedSolutionFileName = solutionsToImportFolder + "\\importlog_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + "_" + timeNow + ".xml";

                        ////Fix bad Status and Message export
                        //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        //doc.LoadXml((string)ImportJob["data"]);
                        //String SolutionImportResult = doc.SelectSingleNode("//solutionManifest/result/@result").Value;

                        //File.WriteAllText(exportedSolutionFileName, importLogResponse.FormattedResults);
                        writeLogFile(ImportJobId, solutionsToImportFolder, solutionArchiveName);

                        importSourceFolder = solutionsToImportFolder;

                        LogManager.WriteLog("Solution " + Path.GetFileName(solutionArchiveName) + " was imported with success in " + connection.ConnectionName);
                    }

                    //Check if customizations are to be published
                    if (currentProfile.PublishAllCustomizationsTarget)
                    {
                        LogManager.WriteLog("Publishing all Customizations on " + connection.ConnectionName + " ...");
                        PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
                        _serviceProxy.Execute(publishRequest);
                    }

                    LogManager.WriteLog("Solutions Import finished for Profile: " + currentProfile.ProfileName);
                }
                else
                {
                    LogManager.WriteLog("There are no solutions for import.");
                    return;
                }
            }
            catch(Exception ex)
            {
                transportStopped = true;
                LogManager.WriteLog("Error ! Detail : " + ex.Message);
                MessageBox.Show("Error ! Detail : " + ex.Message);
                toolStripStatusLabel1.Text = "Transport stopped.";
            }
        }

        private void writeLogFile(Guid ImportJobId, string solutionsToImportFolder, string solutionArchiveName)
        {
            Entity ImportJob = _serviceProxy.Retrieve("importjob", ImportJobId, new ColumnSet(true));
            //File.WriteAllText(solutionsToImportFolder + "\\importlog_ORIGINAL_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + ".xml", (string)ImportJob["data"]);
            impSolReqWithMonitoring = null;

            RetrieveFormattedImportJobResultsRequest importLogRequest = new RetrieveFormattedImportJobResultsRequest()
            {
                ImportJobId = ImportJobId
            };

            RetrieveFormattedImportJobResultsResponse importLogResponse = (RetrieveFormattedImportJobResultsResponse)_serviceProxy.Execute(importLogRequest);

            DateTime now = DateTime.Now;
            string timeNow = String.Format("{0:yyyyMMddHHmmss}", now);

            string exportedSolutionFileName = solutionsToImportFolder + "\\importlog_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + "_" + timeNow + ".xml";

            //Fix bad Status and Message export
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml((string)ImportJob["data"]);
            String SolutionImportResult = doc.SelectSingleNode("//solutionManifest/result/@result").Value;

            File.WriteAllText(exportedSolutionFileName, importLogResponse.FormattedResults);
        }
       
        #endregion Transport threads

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBoxProfiles.SelectedItem = null;
            textBoxProfileName.Text = "";
            textBoxSelectedFolder.Text = "";
            textBoxProfileName.Enabled = true;
            comboBoxConnectionSource.SelectedItem = null;
            newToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;

            this.importLogsToolStripMenuItem.DropDownItems.Clear();
            this.importLogsToolStripMenuItem.Visible = false;
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

            if (comboBoxOperation.SelectedItem == null)
            {
                MessageBox.Show("The Operation is Mandatory!");
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

            if (this.textBoxSelectedFolder.Text == "")
            {
                MessageBox.Show("You must select an Export Folder for the Profile");
                return false;
            }

            if (comboBoxOperation.SelectedItem.ToString() != "Export Solutions")
            {
                if (comboBoxConnectionTarget.SelectedItem == null)
                {
                    MessageBox.Show("The Target Connection is Mandatory!");
                    return false;
                }
                if (comboBoxSolutionsToImport.SelectedItem == null)
                {
                    MessageBox.Show("The Solutions to Import are Mandatory!");
                    return false;
                }
            }

            dataGridView1.EndEdit();

            MSCRMSolutionsTransportProfile tempProfile = new MSCRMSolutionsTransportProfile();
            tempProfile.ProfileName = textBoxProfileName.Text;
            tempProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();

            tempProfile.SelectedSolutionsNames = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCell cbc = row.Cells[0];
                
                if ((bool)cbc.Value)
                {
                    MSCRMSolution ms = (MSCRMSolution)row.DataBoundItem;
                    tempProfile.SelectedSolutionsNames.Add(ms.UniqueName);
                }
            }
            if (tempProfile.SelectedSolutionsNames.Count == 0)
            {
                MessageBox.Show("You must select at least 1 Solution for the Profile");
                return false;
            }

            tempProfile.SolutionExportFolder = textBoxSelectedFolder.Text;
            tempProfile.ExportAsManaged = checkBoxExportAsManaged.Checked;
            tempProfile.Operation = comboBoxOperation.SelectedIndex;
            tempProfile.ExportAutoNumberingSettings = checkedListBoxSettings.GetItemChecked(0);
            tempProfile.ExportCalendarSettings = checkedListBoxSettings.GetItemChecked(1);
            tempProfile.ExportCustomizationSettings = checkedListBoxSettings.GetItemChecked(2);
            tempProfile.ExportEmailTrackingSettings = checkedListBoxSettings.GetItemChecked(3);
            tempProfile.ExportGeneralSettings = checkedListBoxSettings.GetItemChecked(4);
            tempProfile.ExportMarketingSettings = checkedListBoxSettings.GetItemChecked(5);
            tempProfile.ExportOutlookSynchronizationSettings = checkedListBoxSettings.GetItemChecked(6);
            tempProfile.ExportRelationshipRoles = checkedListBoxSettings.GetItemChecked(7);
            tempProfile.ExportIsvConfig = checkedListBoxSettings.GetItemChecked(8);
            tempProfile.setSourceConneciton();
            tempProfile.PublishAllCustomizationsSource = checkBoxPublishAllCustomizationsSource.Checked;

            if (comboBoxConnectionTarget.SelectedItem != null)
            {
                tempProfile.TargetConnectionName = comboBoxConnectionTarget.SelectedItem.ToString();
                tempProfile.setTargetConneciton();
            }
            if (comboBoxSolutionsToImport.SelectedItem != null)
                tempProfile.SolutionsToImport = comboBoxSolutionsToImport.SelectedItem.ToString();
            tempProfile.PublishAllCustomizationsTarget = checkBoxPublishAllCustomizationsTarget.Checked;
            tempProfile.PublishWorkflows = checkBoxPublishWorkflows.Checked;
            tempProfile.OverwriteUnmanagedCustomizations = checkBoxOverwriteUnmanagedCustomizations.Checked;
            

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Solution Transport Profile having the same name exist already
                MSCRMSolutionsTransportProfile profile = man.Profiles.Find(p => p.ProfileName.ToLower() == textBoxProfileName.Text.ToLower());
                if (profile != null)
                {
                    MessageBox.Show("Profile with the name " + textBoxProfileName.Text + " exist already. Please select another name");
                    return false;
                }             

                man.CreateProfile(tempProfile);
                comboBoxProfiles.Items.AddRange(new object[] { tempProfile.ProfileName });
                comboBoxProfiles.SelectedItem = tempProfile.ProfileName;
                currentProfile = tempProfile;
            }
            else
            {
                currentProfile = tempProfile;
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = "Profile " + currentProfile.ProfileName + " saved.";
            return result;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentProfileName = currentProfile.ProfileName;
            DialogResult dResTest;
            dResTest = MessageBox.Show("Deleting this Profile will delete also delete all the exported files in the Export Folder.\r\nAre you sure you want to delete this Solution Transport Profile ?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }

            comboBoxProfiles.Items.Remove(currentProfile.ProfileName);
            comboBoxProfiles.SelectedItem = null;
            man.DeleteProfile(currentProfile);
            currentProfile = null;
            textBoxProfileName.Text = "";
            textBoxProfileName.Enabled = true;
            textBoxSelectedFolder.Text = "";
            comboBoxConnectionSource.SelectedItem = null;
            deleteProfileToolStripMenuItem.Enabled = false;
            newToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            labelSolutionsRefreshedOn.Text = "";
            runProfileToolStripMenuItem.Enabled = false;
            checkBoxExportAsManaged.Checked = false;
            comboBoxOperation.SelectedIndex = -1;
            checkBoxPublishAllCustomizationsSource.Checked = false;
            checkedListBoxSettings.SetItemChecked(0, false);
            checkedListBoxSettings.SetItemChecked(1, false);
            checkedListBoxSettings.SetItemChecked(2, false);
            checkedListBoxSettings.SetItemChecked(3, false);
            checkedListBoxSettings.SetItemChecked(4, false);
            checkedListBoxSettings.SetItemChecked(5, false);
            checkedListBoxSettings.SetItemChecked(6, false);
            checkedListBoxSettings.SetItemChecked(7, false);
            checkedListBoxSettings.SetItemChecked(8, false);
            comboBoxConnectionTarget.SelectedIndex = -1;
            comboBoxSolutionsToImport.SelectedIndex = -1;
            checkBoxPublishAllCustomizationsTarget.Checked = false;
            checkBoxPublishWorkflows.Checked = false;
            checkBoxOverwriteUnmanagedCustomizations.Checked = false;

            this.importLogsToolStripMenuItem.DropDownItems.Clear();
            this.importLogsToolStripMenuItem.Visible = false;

            toolStripStatusLabel1.Text = "Profile " + currentProfileName + " deleted";
            LogManager.WriteLog("Profile " + currentProfileName + " deleted");
        }

        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;
            
            toolStripStatusLabel1.Text = "Running Profile: " + currentProfile.ProfileName + ". Please wait...";
            LogManager.WriteLog("Running Profile: " + currentProfile.ProfileName);

            this.importLogsToolStripMenuItem.DropDownItems.Clear();
            this.importLogsToolStripMenuItem.Visible = false;

            Application.DoEvents();

            try
            {
                //man.RunProfile(currentProfile);
                if (currentProfile.Operation == 0 || currentProfile.Operation == 2)
                {
                    if (!transportRunning)
                    {
                        buttonStopTransport.Visible = true;
                        runProfileToolStripMenuItem.Enabled = false;
                        pgbState.Visible = true;
                        SetFields(false);

                        toolStripStatusLabel1.Text = "Initializing Solutions Export";
                        if (bwExport.IsBusy != true)
                            bwExport.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Export already running!");
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

                        toolStripStatusLabel1.Text = "Initializing Solutions Import";
                        label2.Visible = true;
                        label3.Visible = true;
                        label4.Visible = true;
                        if (bwImport.IsBusy != true)
                            bwImport.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Import already running!");
                    }
                }

                //toolStripStatusLabel1.Text = "Solution Transport finished.";
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

        private void SetFields(bool state)
        {
            foreach (Control c in groupBox1.Controls)
            {
                c.Enabled = state;
            }
            foreach (Control c in groupBox2.Controls)
            {
                c.Enabled = state;
            }
            pgbState.Value = 0;
        }

        private void comboBoxSolutionExportProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            if (comboBoxProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxProfiles.SelectedIndex];
                textBoxProfileName.Text = currentProfile.ProfileName;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                deleteProfileToolStripMenuItem.Enabled = true;
                newToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                textBoxProfileName.Enabled = false;
                runProfileToolStripMenuItem.Enabled = true;
                textBoxSelectedFolder.Text = currentProfile.SolutionExportFolder;

                checkBoxExportAsManaged.Checked = currentProfile.ExportAsManaged;
                comboBoxOperation.SelectedIndex = currentProfile.Operation;
                checkBoxPublishAllCustomizationsSource.Checked = currentProfile.PublishAllCustomizationsSource;

                checkedListBoxSettings.SetItemChecked(0, currentProfile.ExportAutoNumberingSettings);
                checkedListBoxSettings.SetItemChecked(1, currentProfile.ExportCalendarSettings);
                checkedListBoxSettings.SetItemChecked(2, currentProfile.ExportCustomizationSettings);
                checkedListBoxSettings.SetItemChecked(3, currentProfile.ExportEmailTrackingSettings);
                checkedListBoxSettings.SetItemChecked(4, currentProfile.ExportGeneralSettings);
                checkedListBoxSettings.SetItemChecked(5, currentProfile.ExportMarketingSettings);
                checkedListBoxSettings.SetItemChecked(6, currentProfile.ExportOutlookSynchronizationSettings);
                checkedListBoxSettings.SetItemChecked(7, currentProfile.ExportRelationshipRoles);
                checkedListBoxSettings.SetItemChecked(8, currentProfile.ExportIsvConfig);

                //Solution to import combobox update
                this.comboBoxSolutionsToImport.Items.Clear();
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Newest" });
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Oldest" });
                if (Directory.Exists(currentProfile.SolutionExportFolder))
                {
                    IOrderedEnumerable<string> subDirectories = Directory.GetDirectories(currentProfile.SolutionExportFolder).OrderByDescending(x => x);
                    foreach (string DirectoryPath in subDirectories)
                    {
                        string[] pathList = DirectoryPath.Split(new Char[] { '\\' });
                        string DirectoryName = pathList[pathList.Count<string>() - 1];
                        this.comboBoxSolutionsToImport.Items.AddRange(new object[] { DirectoryName });
                    }
                }

                comboBoxConnectionTarget.SelectedItem = currentProfile.TargetConnectionName;
                if (currentProfile.Operation == 2)
                    comboBoxSolutionsToImport.SelectedItem = "Newest";
                else
                    comboBoxSolutionsToImport.SelectedItem = currentProfile.SolutionsToImport;
                checkBoxPublishAllCustomizationsTarget.Checked = currentProfile.PublishAllCustomizationsTarget;
                checkBoxPublishWorkflows.Checked = currentProfile.PublishWorkflows;
                checkBoxOverwriteUnmanagedCustomizations.Checked = currentProfile.OverwriteUnmanagedCustomizations;                

                toolStripStatusLabel1.Text = "Solution Transport Profile " + currentProfile.ProfileName + " loaded.";
                LogManager.WriteLog("Solution Transport Profile " + currentProfile.ProfileName + " loaded.");
            }
            else
            {
                currentProfile = null;
                textBoxProfileName.Text = "";
                deleteProfileToolStripMenuItem.Enabled = false;
                newToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                textBoxProfileName.Enabled = true;
                labelSolutionsRefreshedOn.Text = "";
                runProfileToolStripMenuItem.Enabled = false;
                comboBoxConnectionSource.SelectedIndex = -1;                
                checkBoxExportAsManaged.Checked = false;
                comboBoxOperation.SelectedIndex = -1;
                checkBoxPublishAllCustomizationsSource.Checked = false;
                checkedListBoxSettings.SetItemChecked(0, false);
                checkedListBoxSettings.SetItemChecked(1, false);
                checkedListBoxSettings.SetItemChecked(2, false);
                checkedListBoxSettings.SetItemChecked(3, false);
                checkedListBoxSettings.SetItemChecked(4, false);
                checkedListBoxSettings.SetItemChecked(5, false);
                checkedListBoxSettings.SetItemChecked(6, false);
                checkedListBoxSettings.SetItemChecked(7, false);
                checkedListBoxSettings.SetItemChecked(8, false);
                comboBoxConnectionTarget.SelectedIndex = -1;
                checkBoxPublishAllCustomizationsTarget.Checked = false;
                checkBoxPublishWorkflows.Checked = false;
                checkBoxOverwriteUnmanagedCustomizations.Checked = false;

                //Solution to import combobox update
                this.comboBoxSolutionsToImport.Items.Clear();
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Newest" });
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Oldest" });
            }

            this.importLogsToolStripMenuItem.DropDownItems.Clear();
            this.importLogsToolStripMenuItem.Visible = false;
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        private void logArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        private void buttonLoadSolutions_Click(object sender, EventArgs e)
        {
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a connection before loading Solutions!");
                return;
            }

            toolStripStatusLabel1.Text = "Loading solutions. Please wait...";
            Application.DoEvents();
                        
            dataGridView1.DataSource = new List<MSCRMSolution>();

            try
            {
                solutionsLst = man.DownloadSolutions(cm.MSCRMConnections[comboBoxConnectionSource.SelectedIndex]);
                SortableBindingList<MSCRMSolution> sorted = new SortableBindingList<MSCRMSolution>(solutionsLst.ToList());
                dataGridView1.DataSource = sorted;

                //Check selected solution
                if (currentProfile != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (currentProfile.SelectedSolutionsNames.IndexOf(row.Cells[2].Value.ToString()) > -1)
                        {
                            DataGridViewCell cbc = row.Cells[0];
                            cbc.Value = true;
                        }
                    }
                }

                dataGridView1.ClearSelection();

                List<MSCRMSolution> solutionsFromDisc = man.ReadSolutions(comboBoxConnectionSource.SelectedItem.ToString());
                if (solutionsFromDisc.Count == 0)
                {
                    labelSolutionsRefreshedOn.Text = "never";
                }
                else
                {
                    DateTime structureRefreshedOn = File.GetLastWriteTime(man.Folder + "\\" + comboBoxConnectionSource.SelectedItem.ToString() + ".xml");
                    labelSolutionsRefreshedOn.Text = structureRefreshedOn.ToString();
                }

                toolStripStatusLabel1.Text = "Solutions loaded.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
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
        }

        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            solutionsLst = new List<MSCRMSolution>();
            dataGridView1.DataSource = solutionsLst;
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                return;
            }
            
            List<MSCRMSolution> solutions = man.ReadSolutions(comboBoxConnectionSource.SelectedItem.ToString());
            if (solutions.Count == 0)
            {
                labelSolutionsRefreshedOn.Text = "never";
            }
            else
            {
                DateTime structureRefreshedOn = File.GetLastWriteTime(man.Folder + "\\" + comboBoxConnectionSource.SelectedItem.ToString() + ".xml");
                labelSolutionsRefreshedOn.Text = structureRefreshedOn.ToString();
            }
            solutionsLst = solutions;

            SortableBindingList<MSCRMSolution> sorted = new SortableBindingList<MSCRMSolution>(solutionsLst);
            dataGridView1.DataSource = sorted;

            //Check selected solution
            if (currentProfile != null)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (currentProfile.SelectedSolutionsNames.IndexOf(row.Cells[2].Value.ToString()) > -1)
                    {
                        DataGridViewCell cbc = row.Cells[0];
                        cbc.Value = true;
                    }
                }
            }           

            dataGridView1.ClearSelection();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#solutionstransporter";
            Process.Start(startInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string composedPath = "";
            // New FolderBrowserDialog instance
            FolderBrowserDialog Fld = new FolderBrowserDialog();

            // Set initial selected folder
            if (textBoxSelectedFolder.Text != "")
                Fld.SelectedPath = textBoxSelectedFolder.Text;
            else
            {
                string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                appPath = appPath.Replace("file:\\", "");

                composedPath = appPath + "\\" + man.Folder + "\\" + textBoxProfileName.Text;

                if (!Directory.Exists(composedPath))
                    Directory.CreateDirectory(composedPath);

                Fld.SelectedPath = composedPath;
            }

            // Show the "Make new folder" button
            Fld.ShowNewFolderButton = true;
            if (Fld.ShowDialog() == DialogResult.OK)
            {
                textBoxSelectedFolder.Text = Fld.SelectedPath;
                if (composedPath != "" && Fld.SelectedPath != composedPath && Directory.GetFiles(composedPath).Length == 0)
                {
                    Directory.Delete(composedPath);
                }
            }
        }

        private void comboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOperation.SelectedItem == null || comboBoxOperation.SelectedItem.ToString() == "Import Solutions")
            {
                comboBoxConnectionTarget.Enabled = true;
                comboBoxSolutionsToImport.Enabled = true;
                checkBoxPublishAllCustomizationsTarget.Enabled = true;
                checkBoxPublishWorkflows.Enabled = true;
                checkBoxOverwriteUnmanagedCustomizations.Enabled = true;

                //Solution to import combobox update
                this.comboBoxSolutionsToImport.Items.Clear();
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Newest" });
                this.comboBoxSolutionsToImport.Items.AddRange(new object[] { "Oldest" });
                if (currentProfile != null && Directory.Exists(currentProfile.SolutionExportFolder))
                {
                    IOrderedEnumerable<string> subDirectories = Directory.GetDirectories(currentProfile.SolutionExportFolder).OrderByDescending(x => x);
                    foreach (string DirectoryPath in subDirectories)
                    {
                        string[] pathList = DirectoryPath.Split(new Char[] { '\\' });
                        string DirectoryName = pathList[pathList.Count<string>() - 1];
                        this.comboBoxSolutionsToImport.Items.AddRange(new object[] { DirectoryName });
                    }
                }
            }
            else if (comboBoxOperation.SelectedItem.ToString() == "Export Solutions")
            {
                comboBoxConnectionTarget.Enabled = false;
                comboBoxSolutionsToImport.Enabled = false;
                checkBoxPublishAllCustomizationsTarget.Enabled = false;
                checkBoxPublishWorkflows.Enabled = false;
                checkBoxOverwriteUnmanagedCustomizations.Enabled = false;
            }
            else if (comboBoxOperation.SelectedItem.ToString() == "Transport Solutions")
            {
                comboBoxConnectionTarget.Enabled = true;                
                checkBoxPublishAllCustomizationsTarget.Enabled = true;
                checkBoxPublishWorkflows.Enabled = true;
                checkBoxOverwriteUnmanagedCustomizations.Enabled = true;
                comboBoxSolutionsToImport.SelectedItem = "Newest";
                comboBoxSolutionsToImport.Enabled = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void buttonStopTransport_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Stopping Transport";
            transportStopped = true;
            bwExport.CancelAsync();
            bwImport.CancelAsync();
        }

        private int GetImportProgress(Guid ImportJobId, MSCRMConnection targetConnection)
        {
            int progress = 0;
            //Get Source Default Transaction Currency
            string fetchXml = string.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                      <entity name='importjob'>
                                        <attribute name='progress' />
                                        <order attribute='createdon' descending='true' />
                                        <filter type='and'>
											<condition attribute='importjobid' operator='eq' value='{0}' />
										</filter>
                                      </entity>
                                    </fetch> ", ImportJobId);

            using (OrganizationServiceProxy _serviceProxyImport = cm.connect(targetConnection))
            {
                EntityCollection result = _serviceProxyImport.RetrieveMultiple(new FetchExpression(fetchXml));
            
                if (result.Entities.Count < 1)
                    return 0;

                List<MSCRMSolutionImportJob> ImportJobsList = new List<MSCRMSolutionImportJob>();
                Entity ImportJob = result.Entities[0];
                if (ImportJob.Contains("progress"))
                    progress = Convert.ToInt32((Double)ImportJob["progress"]);
            }
            return progress;
        }

        private void openImportLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filpath = "";

            ToolStripMenuItem ImportLog1ToolStripMenuItem = (ToolStripMenuItem)sender;
            filpath = ImportLog1ToolStripMenuItem.ToolTipText;

            //Check if the file exist
            if (!File.Exists(filpath))
            {
                MessageBox.Show("File not found!");
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = filpath;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                LogManager.WriteLog("Excel not found on this computer: " + ex.Message+ ". Trying to open the file with Internet Explorer.");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "IEXPLORE.EXE";
                startInfo.Arguments = filpath;
                Process.Start(startInfo);
            }
        }
    }
}