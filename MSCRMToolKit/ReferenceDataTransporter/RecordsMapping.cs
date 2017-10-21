// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        22/10/2013
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
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// RecordsMapping class
    /// </summary>
    public partial class RecordsMapping : Form
    {
        private ReferenceDataTransporter rdt;
        private List<RecordMapping> rm;
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The _service proxy source
        /// </summary>
        public OrganizationServiceProxy _serviceProxySource;
        /// <summary>
        /// The _service proxy target
        /// </summary>
        public OrganizationServiceProxy _serviceProxyTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsMapping"/> class.
        /// </summary>
        /// <param name="rdt">The RDT.</param>
        public RecordsMapping(ReferenceDataTransporter rdt)
        {
            this.rdt = rdt;

            //Strange datagridview behaviour fix
            bool tempRecordMappingAdded = false;

            if (rdt.currentProfile.RecordMappings == null)
            {
                tempRecordMappingAdded = true;
                rdt.currentProfile.RecordMappings = new List<RecordMapping>();
                RecordMapping rd = new RecordMapping { SourceRecordId = Guid.Empty, TargetRecordId = Guid.Empty };
                rdt.currentProfile.RecordMappings.Add(rd);
            }
            rm = rdt.currentProfile.RecordMappings;

            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;

            //Setup Entities Combobox Binding source
            List<string> entitiesList = new List<string>();

            foreach (EnvEntity ee in rdt.es.Entities)
                entitiesList.Add(ee.EntityName);
            BindingSource bindingSourceEntities = new BindingSource();
            bindingSourceEntities.DataSource = entitiesList;
            DataGridViewComboBoxColumn ColumnEntities = (DataGridViewComboBoxColumn)dataGridView1.Columns[0];
            ColumnEntities.DataSource = bindingSourceEntities;

            dataGridView1.DataSource = rm;

            //Cleanup temporary record mapping
            if (tempRecordMappingAdded)
            {
                rm.RemoveAt(0);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = rm;
            }
        }

        private void toolStripMenuItemAddRecordMapping_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            RecordMapping rr = new RecordMapping();
            rr.SourceRecordId = Guid.Empty;
            rr.TargetRecordId = Guid.Empty;
            rm.Add(rr);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = rm;
        }

        private void toolStripMenuItemDeleteRecordMapping_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
                return;

            DialogResult dResTest;
            dResTest = MessageBox.Show("Are you sure you want to delete this Record Mapping ?", "Confirm Record Mapping Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }
            else
            {
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                rm.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = rm;
            }
        }

        private void toolStripButtonOK_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            this.Dispose();
        }

        private void mapDefaultTransactionCurrencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.EndEdit();
                Guid SourceTransactionCurrencyId = Guid.Empty;
                Guid TargetTransactionCurrencyId = Guid.Empty;
                //Get Source Default Transaction Currency
                string fetchCurrency = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='1'>
                                  <entity name='transactioncurrency'>
                                    <attribute name='transactioncurrencyid' />
                                    <attribute name='createdon' />
                                    <order attribute='createdon' descending='false' />
                                  </entity>
                                </fetch> ";
                MSCRMConnection connectionSource = rdt.currentProfile.getSourceConneciton();
                _serviceProxySource = cm.connect(connectionSource);

                EntityCollection resultSource = _serviceProxySource.RetrieveMultiple(new FetchExpression(fetchCurrency));
                foreach (var s in resultSource.Entities) { SourceTransactionCurrencyId = (Guid)s.Attributes["transactioncurrencyid"]; }

                //Get Target Default Transaction Currency
                MSCRMConnection connectionTarget = rdt.currentProfile.getTargetConneciton();
                _serviceProxyTarget = cm.connect(connectionTarget);

                EntityCollection resultTarget = _serviceProxyTarget.RetrieveMultiple(new FetchExpression(fetchCurrency));
                foreach (var t in resultTarget.Entities) { TargetTransactionCurrencyId = (Guid)t.Attributes["transactioncurrencyid"]; }

                //Add the mapping
                RecordMapping rr = new RecordMapping();
                rr.EntityName = "transactioncurrency";
                rr.SourceRecordId = SourceTransactionCurrencyId;
                rr.TargetRecordId = TargetTransactionCurrencyId;
                rm.Add(rr);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = rm;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mapping error: " + ex.Message);
            }
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.EndEdit();
                Guid SourceBUId = Guid.Empty;
                Guid TargetBUId = Guid.Empty;
                //Get Source Default Transaction Currency
                string fetchBU = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='1'>
                                      <entity name='businessunit'>
                                        <attribute name='businessunitid' />
                                        <attribute name='createdon' />
                                        <order attribute='createdon' descending='false' />
                                      </entity>
                                    </fetch> ";
                MSCRMConnection connectionSource = rdt.currentProfile.getSourceConneciton();
                _serviceProxySource = cm.connect(connectionSource);

                EntityCollection resultSource = _serviceProxySource.RetrieveMultiple(new FetchExpression(fetchBU));
                foreach (var s in resultSource.Entities) { SourceBUId = (Guid)s.Attributes["businessunitid"]; }

                //Get Target Default Transaction Currency
                MSCRMConnection connectionTarget = rdt.currentProfile.getTargetConneciton();
                _serviceProxyTarget = cm.connect(connectionTarget);

                EntityCollection resultTarget = _serviceProxyTarget.RetrieveMultiple(new FetchExpression(fetchBU));
                foreach (var t in resultTarget.Entities) { TargetBUId = (Guid)t.Attributes["businessunitid"]; }

                //Add the mapping
                RecordMapping rr = new RecordMapping();
                rr.EntityName = "businessunit";
                rr.SourceRecordId = SourceBUId;
                rr.TargetRecordId = TargetBUId;
                rm.Add(rr);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = rm;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mapping error: " + ex.Message);
            }
        }
    }
}