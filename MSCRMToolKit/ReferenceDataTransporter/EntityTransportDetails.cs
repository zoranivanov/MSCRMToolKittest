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
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// SetupIgnoredAttributes class
    /// </summary>
    public partial class SetupIgnoredAttributes : Form
    {
        /// <summary>
        /// The RDT
        /// </summary>
        private ReferenceDataTransporter rdt;
        /// <summary>
        /// The entity
        /// </summary>
        private string entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupIgnoredAttributes"/> class.
        /// </summary>
        /// <param name="rdt">The RDT.</param>
        /// <param name="entity">The entity.</param>
        public SetupIgnoredAttributes(ReferenceDataTransporter rdt, string entity)
        {
            this.rdt = rdt;
            this.entity = entity;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the SetupIgnoredAttributes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SetupIgnoredAttributes_Load(object sender, EventArgs e)
        {
            Control[] ComboBoxes = rdt.Controls.Find("comboBoxConnectionSource", true);
            if (ComboBoxes.Length != 1)
                return;
            ComboBox sourceCB = (ComboBox)ComboBoxes[0];
            EnvStructure es = rdt.man.ReadEnvStructure(sourceCB.SelectedItem.ToString());
            List<EnvEntity> eeList = es.Entities;
            EnvEntity ee = eeList.Find(eP => eP.EntityName == this.entity);
            labelEntityName.Text = "Entity: " + ee.EntityName;
            SelectedEntity se = null;
            if (rdt.currentProfile != null)
                se = rdt.currentProfile.SelectedEntities.Find(eP => eP.EntityName == this.entity);
            foreach (string Attribute in ee.Attributes)
            {
                checkedListBoxAttributes.Items.AddRange(new object[] { Attribute });
                checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, true);
                if (se == null)
                    continue;
                if (se.IgnoredAttributes == null)
                    continue;
                string ignoredAttribute = se.IgnoredAttributes.Find(a => a == Attribute);
                if (ignoredAttribute == null)
                    checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, true);
                else
                    checkedListBoxAttributes.SetItemChecked(checkedListBoxAttributes.Items.Count - 1, false);
            }

            //Display Export Filter
            if (se != null && se.Filter != null && se.Filter != "")
                xmlEditor1.Text = se.Filter;
        }

        /// <summary>
        /// Handles the Click event of the toolStripButton2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<string> ignoredAttributes = new List<string>();
            //for (int j = 0; j < checkedListBoxAttributes.CheckedItems.Count; j++)
            //{
            //    ignoredAttributes.Add(checkedListBoxAttributes.CheckedItems[j].ToString());
            //}

            for (int j = 0; j < checkedListBoxAttributes.Items.Count; j++)
            {
                if (!checkedListBoxAttributes.CheckedItems.Contains(checkedListBoxAttributes.Items[j]))
                    ignoredAttributes.Add(checkedListBoxAttributes.Items[j].ToString());
            }

            if (ignoredAttributes.Count < 1)
                ignoredAttributes = null;

            SelectedEntity se = new SelectedEntity
            {
                EntityName = this.entity,
                IgnoredAttributes = ignoredAttributes,
                Filter = xmlEditor1.Text
            };

            int index = rdt.TemporarySelectedEntityListForIgnoredAttributes.FindIndex(match => match.EntityName == this.entity);
            if (index > -1)
                rdt.TemporarySelectedEntityListForIgnoredAttributes[index] = se;
            else
                rdt.TemporarySelectedEntityListForIgnoredAttributes.Add(se);

            this.Dispose();
        }
    }
}