// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        05/01/2014
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// NtoNTransportReportViewer class
    /// </summary>
    public partial class NtoNTransportReportViewer : Form
    {
        private string reportFileName = "";
        private string reportFailuresFileName = "";
        private MSCRMNtoNAssociationsTransportManager tpm = new MSCRMNtoNAssociationsTransportManager();
        private NtoNTransportReport report = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNTransportReportViewer"/> class.
        /// </summary>
        public NtoNTransportReportViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNTransportReportViewer"/> class.
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        /// <param name="reportFailuresFileName">Name of the report failures file.</param>
        public NtoNTransportReportViewer(string reportFileName, string reportFailuresFileName)
        {
            this.reportFileName = reportFileName;
            this.reportFailuresFileName = reportFailuresFileName;
            InitializeComponent();
        }

        private void TransportReportViewer_Load(object sender, EventArgs e)
        {
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

            if (reportFileName == "")
                return;

            report = tpm.ReadTransportReport(reportFileName);
            labelTransportationProfileName.Text = report.TransportProfileName;
            labelTransportCompleted.Text = (report.TransportCompleted) ? "Yes" : "No";
            labelTransportStartedAt.Text = report.TransportStartedAt;
            labelTransportFinishedAt.Text = report.TransportFinishedAt;
            labelTransportedIn.Text = report.TransportedIn;
            labelTotalExportedRecords.Text = report.TotalExportedRecords.ToString();
            labelTotalImportedRecords.Text = report.TotalImportedRecords.ToString();
            labelTotalImportFailures.Text = report.TotalImportFailures.ToString();
            
            SortableBindingList<NtoNTransportReportLine> sortedReportLines = new SortableBindingList<NtoNTransportReportLine>(report.ReportLines);
            dataGridView1.DataSource = sortedReportLines;

            if (report.TotalImportFailures > 0 && File.Exists(reportFailuresFileName))
            {
                buttonFailuresReport.Visible = true;
            }
            else
            {
                buttonFailuresReport.Visible = false;
            }
        }

        private void failuresReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(reportFailuresFileName))
            {
                MessageBox.Show("Report File not found!");
                return;
            }

            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = reportFailuresFileName;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    NtoNTransportImportFailuresReportViewer tifrv = new NtoNTransportImportFailuresReportViewer(reportFailuresFileName);
                    tifrv.Show();
                }

                else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Console.WriteLine(ex.Message + ". You do not have permission to access this report file.");
                }
            }
        }

        private void buttonFailuresReport_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(reportFailuresFileName))
            {
                MessageBox.Show("Report File not found!");
                return;
            }

            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = reportFailuresFileName;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    NtoNTransportImportFailuresReportViewer tifrv = new NtoNTransportImportFailuresReportViewer(reportFailuresFileName);
                    tifrv.Show();
                }

                else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Console.WriteLine(ex.Message + ". You do not have permission to access this report file.");
                }
            }
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
                        this.reportFileName = fileLoc;
                        this.reportFailuresFileName = reportFileName.Replace("TransportReport", "ImportFailuresReport");
                        try
                        {
                            report = tpm.ReadTransportReport(reportFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Invalid Report File: \n" + ex.Message);
                        }
                        labelTransportationProfileName.Text = report.TransportProfileName;
                        labelTransportCompleted.Text = (report.TransportCompleted) ? "Yes" : "No";
                        labelTransportStartedAt.Text = report.TransportStartedAt;
                        labelTransportFinishedAt.Text = report.TransportFinishedAt;
                        labelTransportedIn.Text = report.TransportedIn;
                        labelTotalExportedRecords.Text = report.TotalExportedRecords.ToString();
                        labelTotalImportedRecords.Text = report.TotalImportedRecords.ToString();
                        labelTotalImportFailures.Text = report.TotalImportFailures.ToString();

                        SortableBindingList<NtoNTransportReportLine> sortedReportLines = new SortableBindingList<NtoNTransportReportLine>(report.ReportLines);
                        dataGridView1.DataSource = sortedReportLines;

                        //Retrieve Failures Report FileName
                        if (report.TotalImportFailures > 0 && File.Exists(reportFailuresFileName))
                        {
                            buttonFailuresReport.Visible = true;
                        }
                        else
                        {
                            buttonFailuresReport.Visible = false;
                        }
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] filePath = openFileDialog1.FileNames;
                foreach (string fileLoc in filePath)
                {
                    // Code to read the contents of the text file
                    if (File.Exists(fileLoc))
                    {
                        this.reportFileName = fileLoc;
                        this.reportFailuresFileName = reportFileName.Replace("TransportReport", "ImportFailuresReport");
                        try
                        {
                            report = tpm.ReadTransportReport(reportFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Invalid Report File: \n" + ex.Message);
                        }
                        labelTransportationProfileName.Text = report.TransportProfileName;
                        labelTransportCompleted.Text = (report.TransportCompleted) ? "Yes" : "No";
                        labelTransportStartedAt.Text = report.TransportStartedAt;
                        labelTransportFinishedAt.Text = report.TransportFinishedAt;
                        labelTransportedIn.Text = report.TransportedIn;
                        labelTotalExportedRecords.Text = report.TotalExportedRecords.ToString();
                        labelTotalImportedRecords.Text = report.TotalImportedRecords.ToString();
                        labelTotalImportFailures.Text = report.TotalImportFailures.ToString();

                        SortableBindingList<NtoNTransportReportLine> sortedReportLines = new SortableBindingList<NtoNTransportReportLine>(report.ReportLines);
                        dataGridView1.DataSource = sortedReportLines;

                        //Retrieve Failures Report FileName
                        if (report.TotalImportFailures > 0 && File.Exists(reportFailuresFileName))
                        {
                            buttonFailuresReport.Visible = true;
                        }
                        else
                        {
                            buttonFailuresReport.Visible = false;
                        }
                    }
                }
            }
        }
    }
}