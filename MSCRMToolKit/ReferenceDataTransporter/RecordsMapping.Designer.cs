namespace MSCRMToolKit
{
    partial class RecordsMapping
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordsMapping));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.entityNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sourceRecordIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetRecordIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordMappingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemAddRecordMapping = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteRecordMapping = new System.Windows.Forms.ToolStripMenuItem();
            this.quickMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapDefaultTransactionCurrencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordMappingBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.entityNameDataGridViewTextBoxColumn,
            this.sourceRecordIdDataGridViewTextBoxColumn,
            this.targetRecordIdDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.recordMappingBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(722, 189);
            this.dataGridView1.TabIndex = 0;
            // 
            // entityNameDataGridViewTextBoxColumn
            // 
            this.entityNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.entityNameDataGridViewTextBoxColumn.DataPropertyName = "EntityName";
            this.entityNameDataGridViewTextBoxColumn.HeaderText = "Entity Name";
            this.entityNameDataGridViewTextBoxColumn.Name = "entityNameDataGridViewTextBoxColumn";
            this.entityNameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.entityNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // sourceRecordIdDataGridViewTextBoxColumn
            // 
            this.sourceRecordIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sourceRecordIdDataGridViewTextBoxColumn.DataPropertyName = "SourceRecordId";
            this.sourceRecordIdDataGridViewTextBoxColumn.HeaderText = "Source Record Id";
            this.sourceRecordIdDataGridViewTextBoxColumn.Name = "sourceRecordIdDataGridViewTextBoxColumn";
            // 
            // targetRecordIdDataGridViewTextBoxColumn
            // 
            this.targetRecordIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.targetRecordIdDataGridViewTextBoxColumn.DataPropertyName = "TargetRecordId";
            this.targetRecordIdDataGridViewTextBoxColumn.HeaderText = "Target Record Id";
            this.targetRecordIdDataGridViewTextBoxColumn.Name = "targetRecordIdDataGridViewTextBoxColumn";
            // 
            // recordMappingBindingSource
            // 
            this.recordMappingBindingSource.DataSource = typeof(RecordMapping);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAddRecordMapping,
            this.toolStripMenuItemDeleteRecordMapping,
            this.quickMappingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(728, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemAddRecordMapping
            // 
            this.toolStripMenuItemAddRecordMapping.Name = "toolStripMenuItemAddRecordMapping";
            this.toolStripMenuItemAddRecordMapping.Size = new System.Drawing.Size(132, 20);
            this.toolStripMenuItemAddRecordMapping.Text = "Add Record Mapping";
            this.toolStripMenuItemAddRecordMapping.Click += new System.EventHandler(this.toolStripMenuItemAddRecordMapping_Click);
            // 
            // toolStripMenuItemDeleteRecordMapping
            // 
            this.toolStripMenuItemDeleteRecordMapping.Name = "toolStripMenuItemDeleteRecordMapping";
            this.toolStripMenuItemDeleteRecordMapping.Size = new System.Drawing.Size(143, 20);
            this.toolStripMenuItemDeleteRecordMapping.Text = "Delete Record Mapping";
            this.toolStripMenuItemDeleteRecordMapping.Click += new System.EventHandler(this.toolStripMenuItemDeleteRecordMapping_Click);
            // 
            // quickMappingToolStripMenuItem
            // 
            this.quickMappingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapDefaultTransactionCurrencyToolStripMenuItem,
            this.mapToolStripMenuItem});
            this.quickMappingToolStripMenuItem.Name = "quickMappingToolStripMenuItem";
            this.quickMappingToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.quickMappingToolStripMenuItem.Text = "Quick Mapping";
            // 
            // mapDefaultTransactionCurrencyToolStripMenuItem
            // 
            this.mapDefaultTransactionCurrencyToolStripMenuItem.Name = "mapDefaultTransactionCurrencyToolStripMenuItem";
            this.mapDefaultTransactionCurrencyToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.mapDefaultTransactionCurrencyToolStripMenuItem.Text = "Map Base Transaction Currency";
            this.mapDefaultTransactionCurrencyToolStripMenuItem.Click += new System.EventHandler(this.mapDefaultTransactionCurrencyToolStripMenuItem_Click);
            // 
            // mapToolStripMenuItem
            // 
            this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            this.mapToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.mapToolStripMenuItem.Text = "Map Base Business Unit";
            this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOK});
            this.toolStrip1.Location = new System.Drawing.Point(0, 219);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(728, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonOK
            // 
            this.toolStripButtonOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOK.Image")));
            this.toolStripButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOK.Name = "toolStripButtonOK";
            this.toolStripButtonOK.Size = new System.Drawing.Size(27, 22);
            this.toolStripButtonOK.Text = "OK";
            this.toolStripButtonOK.Click += new System.EventHandler(this.toolStripButtonOK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(728, 195);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // RecordsMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 244);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RecordsMapping";
            this.Text = "Records Mapping";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordMappingBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource recordMappingBindingSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn entityNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceRecordIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn targetRecordIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddRecordMapping;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteRecordMapping;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.ToolStripMenuItem quickMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapDefaultTransactionCurrencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
    }
}