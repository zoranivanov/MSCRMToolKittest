using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// AuditExportDetails class
    /// </summary>
    public partial class AuditExportDetails : Form
    {
        private AuditExport au;
        private string entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditExportDetails"/> class.
        /// </summary>
        /// <param name="au">The au.</param>
        /// <param name="entity">The entity.</param>
        public AuditExportDetails(AuditExport au, string entity)
        {
            this.au = au;
            this.entity = entity;
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<string> SelectedAttributes = new List<string>();

            if (au.AuditType == "Attribute Change History")
            {
                if (checkedListBoxAttributes.CheckedItems.Count != 1)
                {
                    MessageBox.Show("You must select 1 Attribute for this Entity!");
                    return;
                }

                for (int j = 0; j < checkedListBoxAttributes.CheckedItems.Count; j++)
                {
                    SelectedAttributes.Add(checkedListBoxAttributes.CheckedItems[j].ToString());
                }
            }

            if (SelectedAttributes.Count < 1)
                SelectedAttributes = null;

            SelectedAuditEntity se = new SelectedAuditEntity
            {
                LogicalName = this.entity,
                SelectedAttributes = SelectedAttributes,
                Filter = xmlEditor1.Text
            };

            int index = au.SelectedEntityList.FindIndex(match => match.LogicalName == this.entity);
            if (index > -1)
                au.SelectedEntityList[index] = se;
            else
                au.SelectedEntityList.Add(se);

            this.Dispose();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void AuditExportDetails_Load(object sender, EventArgs e)
        {
            Control[] ComboBoxes = au.Controls.Find("comboBoxConnectionSource", true);
            if (ComboBoxes.Length != 1)
                return;
            ComboBox sourceCB = (ComboBox)ComboBoxes[0];
            EnvAuditStructure es = au.man.ReadEnvStructure(sourceCB.SelectedItem.ToString());
            List<EnvAuditEntity> eeList = es.Entities;
            EnvAuditEntity ee = eeList.Find(eP => eP.LogicalName == this.entity);
            SelectedAuditEntity se = null;
            if (au.currentProfile != null)
                se = au.currentProfile.SelectedEntities.Find(eP => eP.LogicalName == this.entity);

            foreach (string Attribute in ee.Attributes)
            {
                checkedListBoxAttributes.Items.AddRange(new object[] { Attribute });
                checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, au.AuditType != "Attribute Change History");
                //Enable all Attributes selection for Audit Summary Export type
                //if (au.AuditType != "Attribute Change History")
                //    continue;
                if (se == null)
                    continue;
                if (se.SelectedAttributes == null)
                    continue;
                string selectedAttribute = se.SelectedAttributes.Find(a => a == Attribute);
                if (selectedAttribute == null)
                    checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, false);
                else
                    checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, true);
            }

            //Disable Attributes selection for Audit Summary Export type
            if (au.AuditType != "Attribute Change History")
                checkedListBoxAttributes.Enabled = false;

            //Display Export Filter
            if (se != null && se.Filter != null && se.Filter != "")
                xmlEditor1.Text = se.Filter;
        }

        private void checkedListBoxAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            if (au.AuditType == "Attribute Change History")
            {
                for (int i = 0; i < checkedListBoxAttributes.Items.Count; i++)
                {
                    if (checkedListBoxAttributes.Items[i] != clb.SelectedItem)
                        checkedListBoxAttributes.SetItemChecked(i, false);
                    else
                        checkedListBoxAttributes.SetItemChecked(i, true);
                };
            }
        }
    }
}