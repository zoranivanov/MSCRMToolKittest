namespace MSCRMToolKit
{
    partial class TransportReportViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransportReportViewer));
            this.transportReportLineBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelTransportationProfileName = new System.Windows.Forms.Label();
            this.labelTransportCompleted = new System.Windows.Forms.Label();
            this.labelTransportStartedAt = new System.Windows.Forms.Label();
            this.labelTransportFinishedAt = new System.Windows.Forms.Label();
            this.labelTransportedIn = new System.Windows.Forms.Label();
            this.labelTotalExportedRecords = new System.Windows.Forms.Label();
            this.labelTotalImportedRecords = new System.Windows.Forms.Label();
            this.labelTotalImportFailures = new System.Windows.Forms.Label();
            this.buttonFailuresReport = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.entityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportStartedAtDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportFinishedAtDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportedInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exportedRecordsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importStartedAtDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importFinishedAtDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importedInDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importedRecordsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importFailuresDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.transportReportLineBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // transportReportLineBindingSource
            // 
            this.transportReportLineBindingSource.DataSource = typeof(TransportReportLine);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelTransportationProfileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTransportCompleted, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTransportStartedAt, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTransportFinishedAt, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTransportedIn, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTotalExportedRecords, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTotalImportedRecords, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelTotalImportFailures, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonFailuresReport, 4, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(937, 356);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transportation Profile:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Transport Started At:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(421, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Transport Finished At:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Transported In:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(421, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Total Exported Records:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Toral Imported Records:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(421, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total Import Failures:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(421, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Transport Completed:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.entityDataGridViewTextBoxColumn,
            this.exportStartedAtDataGridViewTextBoxColumn,
            this.exportFinishedAtDataGridViewTextBoxColumn,
            this.exportedInDataGridViewTextBoxColumn,
            this.exportedRecordsDataGridViewTextBoxColumn,
            this.importStartedAtDataGridViewTextBoxColumn,
            this.importFinishedAtDataGridViewTextBoxColumn,
            this.importedInDataGridViewTextBoxColumn,
            this.importedRecordsDataGridViewTextBoxColumn,
            this.importFailuresDataGridViewTextBoxColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 5);
            this.dataGridView1.DataSource = this.transportReportLineBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 103);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(931, 250);
            this.dataGridView1.TabIndex = 0;
            // 
            // labelTransportationProfileName
            // 
            this.labelTransportationProfileName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTransportationProfileName.AutoSize = true;
            this.labelTransportationProfileName.Location = new System.Drawing.Point(163, 6);
            this.labelTransportationProfileName.Name = "labelTransportationProfileName";
            this.labelTransportationProfileName.Size = new System.Drawing.Size(0, 13);
            this.labelTransportationProfileName.TabIndex = 9;
            // 
            // labelTransportCompleted
            // 
            this.labelTransportCompleted.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTransportCompleted.AutoSize = true;
            this.labelTransportCompleted.Location = new System.Drawing.Point(581, 6);
            this.labelTransportCompleted.Name = "labelTransportCompleted";
            this.labelTransportCompleted.Size = new System.Drawing.Size(0, 13);
            this.labelTransportCompleted.TabIndex = 10;
            // 
            // labelTransportStartedAt
            // 
            this.labelTransportStartedAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTransportStartedAt.AutoSize = true;
            this.labelTransportStartedAt.Location = new System.Drawing.Point(163, 31);
            this.labelTransportStartedAt.Name = "labelTransportStartedAt";
            this.labelTransportStartedAt.Size = new System.Drawing.Size(0, 13);
            this.labelTransportStartedAt.TabIndex = 11;
            // 
            // labelTransportFinishedAt
            // 
            this.labelTransportFinishedAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTransportFinishedAt.AutoSize = true;
            this.labelTransportFinishedAt.Location = new System.Drawing.Point(581, 31);
            this.labelTransportFinishedAt.Name = "labelTransportFinishedAt";
            this.labelTransportFinishedAt.Size = new System.Drawing.Size(0, 13);
            this.labelTransportFinishedAt.TabIndex = 12;
            // 
            // labelTransportedIn
            // 
            this.labelTransportedIn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTransportedIn.AutoSize = true;
            this.labelTransportedIn.Location = new System.Drawing.Point(163, 56);
            this.labelTransportedIn.Name = "labelTransportedIn";
            this.labelTransportedIn.Size = new System.Drawing.Size(0, 13);
            this.labelTransportedIn.TabIndex = 13;
            // 
            // labelTotalExportedRecords
            // 
            this.labelTotalExportedRecords.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTotalExportedRecords.AutoSize = true;
            this.labelTotalExportedRecords.Location = new System.Drawing.Point(581, 56);
            this.labelTotalExportedRecords.Name = "labelTotalExportedRecords";
            this.labelTotalExportedRecords.Size = new System.Drawing.Size(0, 13);
            this.labelTotalExportedRecords.TabIndex = 14;
            // 
            // labelTotalImportedRecords
            // 
            this.labelTotalImportedRecords.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTotalImportedRecords.AutoSize = true;
            this.labelTotalImportedRecords.Location = new System.Drawing.Point(163, 81);
            this.labelTotalImportedRecords.Name = "labelTotalImportedRecords";
            this.labelTotalImportedRecords.Size = new System.Drawing.Size(0, 13);
            this.labelTotalImportedRecords.TabIndex = 15;
            // 
            // labelTotalImportFailures
            // 
            this.labelTotalImportFailures.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTotalImportFailures.AutoSize = true;
            this.labelTotalImportFailures.Location = new System.Drawing.Point(581, 81);
            this.labelTotalImportFailures.Name = "labelTotalImportFailures";
            this.labelTotalImportFailures.Size = new System.Drawing.Size(0, 13);
            this.labelTotalImportFailures.TabIndex = 16;
            // 
            // buttonFailuresReport
            // 
            this.buttonFailuresReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFailuresReport.Location = new System.Drawing.Point(840, 73);
            this.buttonFailuresReport.Name = "buttonFailuresReport";
            this.tableLayoutPanel1.SetRowSpan(this.buttonFailuresReport, 2);
            this.buttonFailuresReport.Size = new System.Drawing.Size(94, 24);
            this.buttonFailuresReport.TabIndex = 17;
            this.buttonFailuresReport.Text = "Failures Report";
            this.buttonFailuresReport.UseVisualStyleBackColor = true;
            this.buttonFailuresReport.Visible = false;
            this.buttonFailuresReport.Click += new System.EventHandler(this.buttonFailuresReport_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(937, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openToolStripMenuItem.Text = "&Open Report";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(181, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "XML Files (*.xml)|";
            // 
            // entityDataGridViewTextBoxColumn
            // 
            this.entityDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.entityDataGridViewTextBoxColumn.DataPropertyName = "Entity";
            this.entityDataGridViewTextBoxColumn.HeaderText = "Entity";
            this.entityDataGridViewTextBoxColumn.Name = "entityDataGridViewTextBoxColumn";
            this.entityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // exportStartedAtDataGridViewTextBoxColumn
            // 
            this.exportStartedAtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.exportStartedAtDataGridViewTextBoxColumn.DataPropertyName = "ExportStartedAt";
            this.exportStartedAtDataGridViewTextBoxColumn.HeaderText = "ExportStartedAt";
            this.exportStartedAtDataGridViewTextBoxColumn.Name = "exportStartedAtDataGridViewTextBoxColumn";
            this.exportStartedAtDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // exportFinishedAtDataGridViewTextBoxColumn
            // 
            this.exportFinishedAtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.exportFinishedAtDataGridViewTextBoxColumn.DataPropertyName = "ExportFinishedAt";
            this.exportFinishedAtDataGridViewTextBoxColumn.HeaderText = "ExportFinishedAt";
            this.exportFinishedAtDataGridViewTextBoxColumn.Name = "exportFinishedAtDataGridViewTextBoxColumn";
            this.exportFinishedAtDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // exportedInDataGridViewTextBoxColumn
            // 
            this.exportedInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.exportedInDataGridViewTextBoxColumn.DataPropertyName = "ExportedIn";
            this.exportedInDataGridViewTextBoxColumn.HeaderText = "ExportedIn";
            this.exportedInDataGridViewTextBoxColumn.Name = "exportedInDataGridViewTextBoxColumn";
            this.exportedInDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // exportedRecordsDataGridViewTextBoxColumn
            // 
            this.exportedRecordsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.exportedRecordsDataGridViewTextBoxColumn.DataPropertyName = "ExportedRecords";
            this.exportedRecordsDataGridViewTextBoxColumn.HeaderText = "ExportedRecords";
            this.exportedRecordsDataGridViewTextBoxColumn.Name = "exportedRecordsDataGridViewTextBoxColumn";
            this.exportedRecordsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importStartedAtDataGridViewTextBoxColumn
            // 
            this.importStartedAtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.importStartedAtDataGridViewTextBoxColumn.DataPropertyName = "ImportStartedAt";
            this.importStartedAtDataGridViewTextBoxColumn.HeaderText = "ImportStartedAt";
            this.importStartedAtDataGridViewTextBoxColumn.Name = "importStartedAtDataGridViewTextBoxColumn";
            this.importStartedAtDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importFinishedAtDataGridViewTextBoxColumn
            // 
            this.importFinishedAtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.importFinishedAtDataGridViewTextBoxColumn.DataPropertyName = "ImportFinishedAt";
            this.importFinishedAtDataGridViewTextBoxColumn.HeaderText = "ImportFinishedAt";
            this.importFinishedAtDataGridViewTextBoxColumn.Name = "importFinishedAtDataGridViewTextBoxColumn";
            this.importFinishedAtDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importedInDataGridViewTextBoxColumn
            // 
            this.importedInDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.importedInDataGridViewTextBoxColumn.DataPropertyName = "ImportedIn";
            this.importedInDataGridViewTextBoxColumn.HeaderText = "ImportedIn";
            this.importedInDataGridViewTextBoxColumn.Name = "importedInDataGridViewTextBoxColumn";
            this.importedInDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importedRecordsDataGridViewTextBoxColumn
            // 
            this.importedRecordsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.importedRecordsDataGridViewTextBoxColumn.DataPropertyName = "ImportedRecords";
            this.importedRecordsDataGridViewTextBoxColumn.HeaderText = "ImportedRecords";
            this.importedRecordsDataGridViewTextBoxColumn.Name = "importedRecordsDataGridViewTextBoxColumn";
            this.importedRecordsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importFailuresDataGridViewTextBoxColumn
            // 
            this.importFailuresDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.importFailuresDataGridViewTextBoxColumn.DataPropertyName = "ImportFailures";
            this.importFailuresDataGridViewTextBoxColumn.HeaderText = "ImportFailures";
            this.importFailuresDataGridViewTextBoxColumn.Name = "importFailuresDataGridViewTextBoxColumn";
            this.importFailuresDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TransportReportViewer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 380);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TransportReportViewer";
            this.Text = "Transport Report Viewer";
            this.Load += new System.EventHandler(this.TransportReportViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.transportReportLineBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource transportReportLineBindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelTransportationProfileName;
        private System.Windows.Forms.Label labelTransportCompleted;
        private System.Windows.Forms.Label labelTransportStartedAt;
        private System.Windows.Forms.Label labelTransportFinishedAt;
        private System.Windows.Forms.Label labelTransportedIn;
        private System.Windows.Forms.Label labelTotalExportedRecords;
        private System.Windows.Forms.Label labelTotalImportedRecords;
        private System.Windows.Forms.Label labelTotalImportFailures;
        private System.Windows.Forms.Button buttonFailuresReport;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportStartedAtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportFinishedAtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportedInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn exportedRecordsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importStartedAtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importFinishedAtDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importedInDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importedRecordsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn importFailuresDataGridViewTextBoxColumn;
    }
}