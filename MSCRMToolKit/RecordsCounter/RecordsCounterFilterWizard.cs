using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// RecordsCounterFilterWizard class
    /// </summary>
    public partial class RecordsCounterFilterWizard : Form
    {
        /// <summary>
        /// The entity
        /// </summary>
        private string entity;
        /// <summary>
        /// The rc
        /// </summary>
        private RecordsCounter rc;
        /// <summary>
        /// The selected entities
        /// </summary>
        public List<RecordLine> selectedEntities = new List<RecordLine>();
        /// <summary>
        /// The rl
        /// </summary>
        private RecordLine rl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsCounterFilterWizard"/> class.
        /// </summary>
        /// <param name="rc">The rc.</param>
        /// <param name="entityName">Name of the entity.</param>
        public RecordsCounterFilterWizard(RecordsCounter rc, string entityName)
        {
            InitializeComponent();
            this.entity = entityName;
            this.rc = rc;
            labelEntityName.Text = "Filter for entity: " + entity;
            rl = rc.selectedEntities.Find(se => se.Entity == entity);
            xmlEditor1.Text = rl.Filter;
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
            rl.Filter = xmlEditor1.Text;
            int index = rc.selectedEntities.FindIndex(se => se.Entity == entity);
            rc.selectedEntities[index] = rl;
            //rc.selectedEntities = selectedEntities;
            this.Dispose();
        }
    }
}