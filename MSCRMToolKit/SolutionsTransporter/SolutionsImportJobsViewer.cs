// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        08/02/2014
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// Solutions Import Jobs Viewer class
    /// </summary>
    public partial class SolutionsImportJobsViewer : Form
    {
        /// <summary>
        /// The CRM service proxy
        /// </summary>
        private OrganizationServiceProxy _serviceProxy = null;
        /// <summary>
        /// The Connections Manager
        /// </summary>
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The Workspace folder
        /// </summary>
        private string Folder = "SolutionsImportJobsViewer";

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionsImportJobsViewer"/> class.
        /// </summary>
        public SolutionsImportJobsViewer()
        {
            InitializeComponent();
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
            }
            dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
        }

        /// <summary>
        /// Handles the Click event of the buttonLoadSolutionsImportJobs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonLoadSolutionsImportJobs_Click(object sender, EventArgs e)
        {
            try
            {
                List<MSCRMSolutionImportJob> Lst = new List<MSCRMSolutionImportJob>();
                dataGridView1.DataSource = Lst;

                if (comboBoxConnectionSource.SelectedItem == null)
                {
                    MessageBox.Show("You must select a connection before loading the Solution Import Jobs!");
                    return;
                }

                toolStripStatusLabel1.Text = "Loading Solution Import Jobs. Please wait...";
                Application.DoEvents();

                MSCRMConnection connection = cm.MSCRMConnections[comboBoxConnectionSource.SelectedIndex];
                _serviceProxy = cm.connect(connection);

                //Get Source Default Transaction Currency
                string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                      <entity name='importjob'>
                                        <attribute name='completedon' />
                                        <attribute name='createdby' />
                                        <attribute name='data' />
                                        <attribute name='importjobid' />
                                        <attribute name='modifiedon' />
                                        <attribute name='progress' />
                                        <attribute name='solutionname' />
                                        <attribute name='startedon' />
                                        <order attribute='createdon' descending='true' />
                                      </entity>
                                    </fetch> ";


                EntityCollection result = _serviceProxy.RetrieveMultiple(new FetchExpression(fetchXml));

                if (result.Entities.Count < 1)
                {
                    MessageBox.Show("There are no Solution Import Jobs!");
                    return;
                }
                List<MSCRMSolutionImportJob> ImportJobsList = new List<MSCRMSolutionImportJob>();
                foreach (Entity ImportJob in result.Entities)
                {
                    MSCRMSolutionImportJob job = new MSCRMSolutionImportJob();
                    if (ImportJob.Contains("completedon"))
                        job.completedon = (DateTime)ImportJob["completedon"];
                    if (ImportJob.Contains("createdby"))
                        job.createdby = ((EntityReference)ImportJob["createdby"]).Name;
                    if (ImportJob.Contains("data"))
                        job.data = (String)ImportJob["data"];
                    if (ImportJob.Contains("importjobid"))
                        job.importjobid = (Guid)ImportJob["importjobid"];
                    if (ImportJob.Contains("modifiedon"))
                        job.modifiedon = (DateTime)ImportJob["modifiedon"];
                    if (ImportJob.Contains("progress"))
                        job.progress = Math.Round((Double)ImportJob["progress"], 2);
                    if (ImportJob.Contains("solutionname"))
                        job.solutionname = (String)ImportJob["solutionname"];
                    if (ImportJob.Contains("startedon"))
                        job.startedon = (DateTime)ImportJob["startedon"];

                    if (job.importjobid != null && job.importjobid != Guid.Empty)
                        ImportJobsList.Add(job);
                }

                SortableBindingList<MSCRMSolutionImportJob> sorted = new SortableBindingList<MSCRMSolutionImportJob>(ImportJobsList);
                dataGridView1.DataSource = sorted;

                toolStripStatusLabel1.Text = "Loaded " + result.Entities.Count + " Import Jobs.";
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
        /// Handles the CellClick event of the dataGridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 && e.ColumnIndex == 8)
                    return;

                if (e.ColumnIndex == this.dataGridView1.Rows[0].Cells.Count - 1)
                {
                    //MessageBox.Show((e.RowIndex + 1) + "  Row  " + (e.ColumnIndex + 1) + "  Column button clicked " + this.dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    RetrieveFormattedImportJobResultsRequest importLogRequest = new RetrieveFormattedImportJobResultsRequest()
                    {
                        ImportJobId = new Guid(this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString())
                    };
                    RetrieveFormattedImportJobResultsResponse importLogResponse = (RetrieveFormattedImportJobResultsResponse)_serviceProxy.Execute(importLogRequest);

                    if (!Directory.Exists(Folder))
                        Directory.CreateDirectory(Folder);
                    string SolutionName = this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                    string FileName = Folder + "\\importlog_" + Path.GetFileNameWithoutExtension(SolutionName) + ".xml";

                    File.WriteAllText(FileName, importLogResponse.FormattedResults);

                    //Check if the file exist
                    if (!File.Exists(FileName))
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
                        startInfo.Arguments = FileName;
                        Process.Start(startInfo);
                    }
                    catch (Win32Exception ex)
                    {
                        if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.FileName = "NOTEPAD.EXE";
                            startInfo.Arguments = FileName;
                            Process.Start(startInfo);
                        }
                        else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                        {
                            MessageBox.Show(ex.Message + ". You do not have permission to access this file.");
                        }
                    }
                }
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

        /// <summary>
        /// Handles the Click event of the helpToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#solutionsimportjobsviewer";
            Process.Start(startInfo);
        }
    }

    /// <summary>
    /// MSCRM Solution Import Job
    /// </summary>
    public class MSCRMSolutionImportJob
    {
        /// <summary>
        /// Gets or sets the completedon.
        /// </summary>
        /// <value>
        /// The completedon.
        /// </value>
        public DateTime completedon { get; set; }
        /// <summary>
        /// Gets or sets the createdby.
        /// </summary>
        /// <value>
        /// The createdby.
        /// </value>
        public String createdby { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public String data { get; set; }
        /// <summary>
        /// Gets or sets the importjobid.
        /// </summary>
        /// <value>
        /// The importjobid.
        /// </value>
        public Guid importjobid { get; set; }
        /// <summary>
        /// Gets or sets the modifiedon.
        /// </summary>
        /// <value>
        /// The modifiedon.
        /// </value>
        public DateTime modifiedon { get; set; }
        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        public Double progress { get; set; }
        /// <summary>
        /// Gets or sets the solutionname.
        /// </summary>
        /// <value>
        /// The solutionname.
        /// </value>
        public String solutionname { get; set; }
        /// <summary>
        /// Gets or sets the startedon.
        /// </summary>
        /// <value>
        /// The startedon.
        /// </value>
        public DateTime startedon { get; set; }
        /// <summary>
        /// Gets or sets the download log.
        /// </summary>
        /// <value>
        /// The download log.
        /// </value>
        public Button DownloadLog { get; set; }
    }
}
