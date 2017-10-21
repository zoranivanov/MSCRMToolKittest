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
using System.IO;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// DataExportReportViewer class
    /// </summary>
    public partial class DataExportReportViewer : Form
    {
        private string reportFileName = "";
        private MSCRMDataExportManager dem = new MSCRMDataExportManager();
        private DataExportReport report = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExportReportViewer"/> class.
        /// </summary>
        public DataExportReportViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExportReportViewer"/> class.
        /// </summary>
        /// <param name="reportFileName">Name of the report file.</param>
        public DataExportReportViewer(string reportFileName)
        {
            this.reportFileName = reportFileName;
            InitializeComponent();
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
                        try
                        {
                            report = dem.ReadReport(reportFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Invalid Report File: \n" + ex.Message);
                        }
                        labelDataExportProfileName.Text = report.DataExportProfileName;
                        labelExportCompleted.Text = (report.DataExportCompleted) ? "Yes" : "No";
                        labelExportStartedAt.Text = report.DataExportStartedAt;
                        labelExportFinishedAt.Text = report.DataExportFinishedAt;
                        labelExportedIn.Text = report.DataExportedIn;
                        labelExportedRecords.Text = report.TotalExportedRecords.ToString();
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void DataExportReport_Load(object sender, EventArgs e)
        {
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;

            if (reportFileName == "")
                return;

            report = dem.ReadReport(reportFileName);
            labelDataExportProfileName.Text = report.DataExportProfileName;
            labelExportCompleted.Text = (report.DataExportCompleted) ? "Yes" : "No";
            labelExportStartedAt.Text = report.DataExportStartedAt;
            labelExportFinishedAt.Text = report.DataExportFinishedAt;
            labelExportedIn.Text = report.DataExportedIn;
            labelExportedRecords.Text = report.TotalExportedRecords.ToString();
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
                        try
                        {
                            report = dem.ReadReport(reportFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Invalid Report File: \n" + ex.Message);
                        }
                        labelDataExportProfileName.Text = report.DataExportProfileName;
                        labelExportCompleted.Text = (report.DataExportCompleted) ? "Yes" : "No";
                        labelExportStartedAt.Text = report.DataExportStartedAt;
                        labelExportFinishedAt.Text = report.DataExportFinishedAt;
                        labelExportedIn.Text = report.DataExportedIn;
                        labelExportedRecords.Text = report.TotalExportedRecords.ToString();
                    }
                }
            }
        }
    }
}