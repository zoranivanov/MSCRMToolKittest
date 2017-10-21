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
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// NtoNTransportImportFailuresReportViewer class
    /// </summary>
    public partial class NtoNTransportImportFailuresReportViewer : Form
    {
        private string filename = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNTransportImportFailuresReportViewer"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public NtoNTransportImportFailuresReportViewer(string filename)
        {
            this.filename = filename;
            InitializeComponent();
        }

        private void TransportImportFailuresReportViewer_Load(object sender, EventArgs e)
        {
            MSCRMNtoNAssociationsTransportManager tpm = new MSCRMNtoNAssociationsTransportManager();
            List<NtoNRelationshipsImportFailure> aa = tpm.ReadImportFailuresReport(filename);
            dataGridView1.DataSource = aa;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
            {
                object value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "IEXPLORE.EXE";
                startInfo.Arguments = value.ToString();
                Process.Start(startInfo);
            }
        }
    }
}