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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMToolkit main application
    /// </summary>
    public partial class MSCRMToolKit : Form
    {
        /// <summary>
        /// The MSCRM connections
        /// </summary>
        private List<MSCRMConnection> MSCRMConnections = new List<MSCRMConnection>();
        /// <summary>
        /// The  Connections Manager
        /// </summary>
        private MSCRMConnectionsManager cm = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMToolKit" /> class.
        /// </summary>
        public MSCRMToolKit()
        {
            //Set the application directory as the current directory
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            appPath = appPath.Replace("file:\\", "");
            Directory.SetCurrentDirectory(appPath);

            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the connectionsManagerToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void connectionsManagerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            cm = new MSCRMConnectionsManager();
            cm.LaunchConnectionsManagerGUI();
        }

        /// <summary>
        /// Handles the Click event of the referenceDataTransporterToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void referenceDataTransporterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReferenceDataTransporter rdt = new ReferenceDataTransporter();
            rdt.Show();
        }

        /// <summary>
        /// Handles the Click event of the exportEntitiesStructureToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exportEntitiesStructureToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EntitiesStructureExport ese = new EntitiesStructureExport();
            ese.Show();
        }

        /// <summary>
        /// Handles the 1 event of the viewLogToolStripMenuItem_Click control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void viewLogToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        /// <summary>
        /// Handles the Click event of the logArchivesToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void logArchivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the helpToolStripMenuItem1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki";
            Process.Start(startInfo);
        }

        /// <summary>
        /// Handles the Click event of the dataExportManagerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dataExportManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataExport de = new DataExport();
            de.Show();
        }

        /// <summary>
        /// Handles the Click event of the referenceDataTransportReportViewerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void referenceDataTransportReportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransportReportViewer tpv = new TransportReportViewer();
            tpv.Show();
        }

        /// <summary>
        /// Handles the Click event of the dataExportReportViewerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void dataExportReportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataExportReportViewer derv = new DataExportReportViewer();
            derv.Show();
        }

        /// <summary>
        /// Handles the Click event of the serverSettingsToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void serverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeploymentProperties ss = new DeploymentProperties();
            ss.Show();
        }

        /// <summary>
        /// Handles the Click event of the solutionExportToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void solutionExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionsTransporter sem = new SolutionsTransporter();
            sem.Show();
        }

        /// <summary>
        /// Handles the Click event of the workflowLauncherToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void workflowLauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkflowExecution we = new WorkflowExecution();
            we.Show();
        }

        /// <summary>
        /// Handles the Click event of the recordsCounterToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void recordsCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsCounter rc = new RecordsCounter();
            rc.Show();
        }

        /// <summary>
        /// Handles the Click event of the auditExportManagerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void auditExportManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditExport ae = new AuditExport();
            ae.Show();
        }

        /// <summary>
        /// Handles the Click event of the nNRelationshipsTransporterToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void nNRelationshipsTransporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NtoNAssociationsTransporter nnrt = new NtoNAssociationsTransporter();
            nnrt.Show();
        }

        /// <summary>
        /// Handles the Click event of the nNReferenceDataTransportReportViewerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void nNReferenceDataTransportReportViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NtoNTransportReportViewer tpv = new NtoNTransportReportViewer();
            tpv.Show();
        }

        /// <summary>
        /// Handles the Click event of the solutionsImportJobsViewerToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void solutionsImportJobsViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionsImportJobsViewer sijv = new SolutionsImportJobsViewer();
            sijv.Show();
        }
    }
}