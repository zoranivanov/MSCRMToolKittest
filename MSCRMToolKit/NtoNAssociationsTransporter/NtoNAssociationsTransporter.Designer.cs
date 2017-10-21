namespace MSCRMToolKit
{
    partial class NtoNAssociationsTransporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NtoNAssociationsTransporter));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transportReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxOperation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLoadEntities = new System.Windows.Forms.Button();
            this.comboBoxConnectionSource = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelStructureLastLoadedDate = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxConnectionTarget = new System.Windows.Forms.ComboBox();
            this.textBoxTransportationProfileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.relationshipSchemaNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.intersectEntityNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entity1LogicalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entity1IntersectAttributeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entity2LogicalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entity2IntersectAttributeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ntoNRelationshipBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonStopTransport = new System.Windows.Forms.Button();
            this.labelImportFailures = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelImportSuccess = new System.Windows.Forms.Label();
            this.pgbState = new System.Windows.Forms.ProgressBar();
            this.comboBoxTransportationProfiles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntoNRelationshipBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.runProfileToolStripMenuItem,
            this.transportReportToolStripMenuItem,
            this.logToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1039, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(138, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteProfileToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // deleteProfileToolStripMenuItem
            // 
            this.deleteProfileToolStripMenuItem.Enabled = false;
            this.deleteProfileToolStripMenuItem.Name = "deleteProfileToolStripMenuItem";
            this.deleteProfileToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deleteProfileToolStripMenuItem.Text = "Delete Profile";
            this.deleteProfileToolStripMenuItem.Click += new System.EventHandler(this.deleteProfileToolStripMenuItem_Click);
            // 
            // runProfileToolStripMenuItem
            // 
            this.runProfileToolStripMenuItem.Enabled = false;
            this.runProfileToolStripMenuItem.Name = "runProfileToolStripMenuItem";
            this.runProfileToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.runProfileToolStripMenuItem.Text = "Run Profile";
            this.runProfileToolStripMenuItem.Click += new System.EventHandler(this.runProfileToolStripMenuItem_Click);
            // 
            // transportReportToolStripMenuItem
            // 
            this.transportReportToolStripMenuItem.Name = "transportReportToolStripMenuItem";
            this.transportReportToolStripMenuItem.Size = new System.Drawing.Size(108, 20);
            this.transportReportToolStripMenuItem.Text = "Transport Report";
            this.transportReportToolStripMenuItem.Visible = false;
            this.transportReportToolStripMenuItem.Click += new System.EventHandler(this.transportReportToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogToolStripMenuItem,
            this.logArchiveToolStripMenuItem});
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.logToolStripMenuItem.Text = "Log";
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // logArchiveToolStripMenuItem
            // 
            this.logArchiveToolStripMenuItem.Name = "logArchiveToolStripMenuItem";
            this.logArchiveToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.logArchiveToolStripMenuItem.Text = "Log Archive";
            this.logArchiveToolStripMenuItem.Click += new System.EventHandler(this.logArchiveToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 378);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1039, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1039, 354);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxOperation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.textBoxTransportationProfileName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 314);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transport Profile Properties:";
            // 
            // comboBoxOperation
            // 
            this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperation.FormattingEnabled = true;
            this.comboBoxOperation.Items.AddRange(new object[] {
            "Export Data",
            "Import Data",
            "Transport Data"});
            this.comboBoxOperation.Location = new System.Drawing.Point(23, 96);
            this.comboBoxOperation.Name = "comboBoxOperation";
            this.comboBoxOperation.Size = new System.Drawing.Size(149, 21);
            this.comboBoxOperation.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Operation:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonLoadEntities);
            this.groupBox3.Controls.Add(this.comboBoxConnectionSource);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.labelStructureLastLoadedDate);
            this.groupBox3.Location = new System.Drawing.Point(12, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(170, 110);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source:";
            // 
            // buttonLoadEntities
            // 
            this.buttonLoadEntities.Enabled = false;
            this.buttonLoadEntities.Location = new System.Drawing.Point(10, 75);
            this.buttonLoadEntities.Name = "buttonLoadEntities";
            this.buttonLoadEntities.Size = new System.Drawing.Size(150, 23);
            this.buttonLoadEntities.TabIndex = 7;
            this.buttonLoadEntities.Text = "Load Source Structure";
            this.buttonLoadEntities.UseVisualStyleBackColor = true;
            this.buttonLoadEntities.Click += new System.EventHandler(this.buttonLoadEntities_Click);
            // 
            // comboBoxConnectionSource
            // 
            this.comboBoxConnectionSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionSource.FormattingEnabled = true;
            this.comboBoxConnectionSource.Location = new System.Drawing.Point(10, 19);
            this.comboBoxConnectionSource.Name = "comboBoxConnectionSource";
            this.comboBoxConnectionSource.Size = new System.Drawing.Size(150, 21);
            this.comboBoxConnectionSource.TabIndex = 2;
            this.comboBoxConnectionSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxConnectionSource_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Structure loaded on:";
            // 
            // labelStructureLastLoadedDate
            // 
            this.labelStructureLastLoadedDate.AutoSize = true;
            this.labelStructureLastLoadedDate.Location = new System.Drawing.Point(8, 59);
            this.labelStructureLastLoadedDate.Name = "labelStructureLastLoadedDate";
            this.labelStructureLastLoadedDate.Size = new System.Drawing.Size(34, 13);
            this.labelStructureLastLoadedDate.TabIndex = 21;
            this.labelStructureLastLoadedDate.Text = "never";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxConnectionTarget);
            this.groupBox4.Location = new System.Drawing.Point(12, 244);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(170, 55);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Target:";
            // 
            // comboBoxConnectionTarget
            // 
            this.comboBoxConnectionTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionTarget.FormattingEnabled = true;
            this.comboBoxConnectionTarget.Location = new System.Drawing.Point(9, 19);
            this.comboBoxConnectionTarget.Name = "comboBoxConnectionTarget";
            this.comboBoxConnectionTarget.Size = new System.Drawing.Size(151, 21);
            this.comboBoxConnectionTarget.TabIndex = 3;
            // 
            // textBoxTransportationProfileName
            // 
            this.textBoxTransportationProfileName.Location = new System.Drawing.Point(23, 54);
            this.textBoxTransportationProfileName.Name = "textBoxTransportationProfileName";
            this.textBoxTransportationProfileName.Size = new System.Drawing.Size(149, 20);
            this.textBoxTransportationProfileName.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(201, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(835, 314);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Available N to N Relationships";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedDataGridViewCheckBoxColumn,
            this.relationshipSchemaNameDataGridViewTextBoxColumn,
            this.intersectEntityNameDataGridViewTextBoxColumn,
            this.entity1LogicalNameDataGridViewTextBoxColumn,
            this.entity1IntersectAttributeDataGridViewTextBoxColumn,
            this.entity2LogicalNameDataGridViewTextBoxColumn,
            this.entity2IntersectAttributeDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.ntoNRelationshipBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(829, 295);
            this.dataGridView1.TabIndex = 0;
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.selectedDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "#";
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            this.selectedDataGridViewCheckBoxColumn.Width = 25;
            // 
            // relationshipSchemaNameDataGridViewTextBoxColumn
            // 
            this.relationshipSchemaNameDataGridViewTextBoxColumn.DataPropertyName = "RelationshipSchemaName";
            this.relationshipSchemaNameDataGridViewTextBoxColumn.HeaderText = "RelationshipSchemaName";
            this.relationshipSchemaNameDataGridViewTextBoxColumn.Name = "relationshipSchemaNameDataGridViewTextBoxColumn";
            // 
            // intersectEntityNameDataGridViewTextBoxColumn
            // 
            this.intersectEntityNameDataGridViewTextBoxColumn.DataPropertyName = "IntersectEntityName";
            this.intersectEntityNameDataGridViewTextBoxColumn.HeaderText = "IntersectEntityName";
            this.intersectEntityNameDataGridViewTextBoxColumn.Name = "intersectEntityNameDataGridViewTextBoxColumn";
            // 
            // entity1LogicalNameDataGridViewTextBoxColumn
            // 
            this.entity1LogicalNameDataGridViewTextBoxColumn.DataPropertyName = "Entity1LogicalName";
            this.entity1LogicalNameDataGridViewTextBoxColumn.HeaderText = "Entity1LogicalName";
            this.entity1LogicalNameDataGridViewTextBoxColumn.Name = "entity1LogicalNameDataGridViewTextBoxColumn";
            // 
            // entity1IntersectAttributeDataGridViewTextBoxColumn
            // 
            this.entity1IntersectAttributeDataGridViewTextBoxColumn.DataPropertyName = "Entity1IntersectAttribute";
            this.entity1IntersectAttributeDataGridViewTextBoxColumn.HeaderText = "Entity1IntersectAttribute";
            this.entity1IntersectAttributeDataGridViewTextBoxColumn.Name = "entity1IntersectAttributeDataGridViewTextBoxColumn";
            // 
            // entity2LogicalNameDataGridViewTextBoxColumn
            // 
            this.entity2LogicalNameDataGridViewTextBoxColumn.DataPropertyName = "Entity2LogicalName";
            this.entity2LogicalNameDataGridViewTextBoxColumn.HeaderText = "Entity2LogicalName";
            this.entity2LogicalNameDataGridViewTextBoxColumn.Name = "entity2LogicalNameDataGridViewTextBoxColumn";
            // 
            // entity2IntersectAttributeDataGridViewTextBoxColumn
            // 
            this.entity2IntersectAttributeDataGridViewTextBoxColumn.DataPropertyName = "Entity2IntersectAttribute";
            this.entity2IntersectAttributeDataGridViewTextBoxColumn.HeaderText = "Entity2IntersectAttribute";
            this.entity2IntersectAttributeDataGridViewTextBoxColumn.Name = "entity2IntersectAttributeDataGridViewTextBoxColumn";
            // 
            // ntoNRelationshipBindingSource
            // 
            this.ntoNRelationshipBindingSource.DataSource = typeof(NtoNRelationship);
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.buttonStopTransport);
            this.panel1.Controls.Add(this.labelImportFailures);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labelImportSuccess);
            this.panel1.Controls.Add(this.pgbState);
            this.panel1.Controls.Add(this.comboBoxTransportationProfiles);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1033, 28);
            this.panel1.TabIndex = 2;
            // 
            // buttonStopTransport
            // 
            this.buttonStopTransport.Location = new System.Drawing.Point(316, 3);
            this.buttonStopTransport.Name = "buttonStopTransport";
            this.buttonStopTransport.Size = new System.Drawing.Size(91, 23);
            this.buttonStopTransport.TabIndex = 29;
            this.buttonStopTransport.Text = "Stop Transport";
            this.buttonStopTransport.UseVisualStyleBackColor = true;
            this.buttonStopTransport.Visible = false;
            this.buttonStopTransport.Click += new System.EventHandler(this.buttonStopTransport_Click);
            // 
            // labelImportFailures
            // 
            this.labelImportFailures.AutoSize = true;
            this.labelImportFailures.Location = new System.Drawing.Point(541, 8);
            this.labelImportFailures.Name = "labelImportFailures";
            this.labelImportFailures.Size = new System.Drawing.Size(0, 13);
            this.labelImportFailures.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(425, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "OK:";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "KO:";
            this.label4.Visible = false;
            // 
            // labelImportSuccess
            // 
            this.labelImportSuccess.AutoSize = true;
            this.labelImportSuccess.Location = new System.Drawing.Point(447, 8);
            this.labelImportSuccess.Name = "labelImportSuccess";
            this.labelImportSuccess.Size = new System.Drawing.Size(0, 13);
            this.labelImportSuccess.TabIndex = 31;
            // 
            // pgbState
            // 
            this.pgbState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbState.Location = new System.Drawing.Point(801, 8);
            this.pgbState.Name = "pgbState";
            this.pgbState.Size = new System.Drawing.Size(220, 10);
            this.pgbState.TabIndex = 2;
            this.pgbState.Visible = false;
            // 
            // comboBoxTransportationProfiles
            // 
            this.comboBoxTransportationProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTransportationProfiles.FormattingEnabled = true;
            this.comboBoxTransportationProfiles.Location = new System.Drawing.Point(109, 4);
            this.comboBoxTransportationProfiles.Name = "comboBoxTransportationProfiles";
            this.comboBoxTransportationProfiles.Size = new System.Drawing.Size(185, 21);
            this.comboBoxTransportationProfiles.TabIndex = 1;
            this.comboBoxTransportationProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransportationProfiles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Transport Profiles:";
            // 
            // NtoNAssociationsTransporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 400);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "NtoNAssociationsTransporter";
            this.Text = "N:N Associations Transporter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ntoNRelationshipBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxTransportationProfiles;
        private System.Windows.Forms.ComboBox comboBoxOperation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonLoadEntities;
        private System.Windows.Forms.ComboBox comboBoxConnectionSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelStructureLastLoadedDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBoxConnectionTarget;
        private System.Windows.Forms.TextBox textBoxTransportationProfileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem deleteProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logArchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ProgressBar pgbState;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem runProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transportReportToolStripMenuItem;
        private System.Windows.Forms.Button buttonStopTransport;
        private System.Windows.Forms.Label labelImportFailures;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelImportSuccess;
        private System.Windows.Forms.BindingSource ntoNRelationshipBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relationshipSchemaNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn intersectEntityNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entity1LogicalNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entity1IntersectAttributeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entity2LogicalNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entity2IntersectAttributeDataGridViewTextBoxColumn;
    }
}