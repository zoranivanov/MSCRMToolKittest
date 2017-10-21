namespace MSCRMToolKit
{
    partial class ReferenceDataTransporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReferenceDataTransporter));
            this.checkedListBoxEntities = new System.Windows.Forms.CheckedListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxOperation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLoadEntities = new System.Windows.Forms.Button();
            this.comboBoxConnectionSource = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelStructureLastLoadedDate = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRecordsMapping = new System.Windows.Forms.Button();
            this.comboBoxConnectionTarget = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxImportMode = new System.Windows.Forms.ComboBox();
            this.textBoxTransportationProfileName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelImportFailures = new System.Windows.Forms.Label();
            this.pgbState = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelImportSuccess = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxTransportationProfiles = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transportReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonStopTransport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panelTransportOrder = new System.Windows.Forms.Panel();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxEntities
            // 
            this.checkedListBoxEntities.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBoxEntities.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxEntities.CheckOnClick = true;
            this.checkedListBoxEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxEntities.FormattingEnabled = true;
            this.checkedListBoxEntities.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxEntities.Name = "checkedListBoxEntities";
            this.checkedListBoxEntities.Size = new System.Drawing.Size(229, 346);
            this.checkedListBoxEntities.TabIndex = 6;
            this.checkedListBoxEntities.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxEntites_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboBoxOperation);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.groupBox3);
            this.groupBox6.Controls.Add(this.groupBox2);
            this.groupBox6.Controls.Add(this.textBoxTransportationProfileName);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(6, 40);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(192, 371);
            this.groupBox6.TabIndex = 19;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Transport Profile Properties:";
            // 
            // comboBoxOperation
            // 
            this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperation.FormattingEnabled = true;
            this.comboBoxOperation.Items.AddRange(new object[] {
            "Export Data",
            "Import Data",
            "Transport Data"});
            this.comboBoxOperation.Location = new System.Drawing.Point(23, 78);
            this.comboBoxOperation.Name = "comboBoxOperation";
            this.comboBoxOperation.Size = new System.Drawing.Size(149, 21);
            this.comboBoxOperation.TabIndex = 30;
            this.comboBoxOperation.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Operation:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonLoadEntities);
            this.groupBox3.Controls.Add(this.comboBoxConnectionSource);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.labelStructureLastLoadedDate);
            this.groupBox3.Location = new System.Drawing.Point(12, 111);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(170, 110);
            this.groupBox3.TabIndex = 28;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRecordsMapping);
            this.groupBox2.Controls.Add(this.comboBoxConnectionTarget);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.comboBoxImportMode);
            this.groupBox2.Location = new System.Drawing.Point(12, 226);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 135);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target:";
            // 
            // buttonRecordsMapping
            // 
            this.buttonRecordsMapping.Enabled = false;
            this.buttonRecordsMapping.Location = new System.Drawing.Point(9, 101);
            this.buttonRecordsMapping.Name = "buttonRecordsMapping";
            this.buttonRecordsMapping.Size = new System.Drawing.Size(151, 23);
            this.buttonRecordsMapping.TabIndex = 26;
            this.buttonRecordsMapping.Text = "Records Mapping";
            this.buttonRecordsMapping.UseVisualStyleBackColor = true;
            this.buttonRecordsMapping.Click += new System.EventHandler(this.buttonRecordsMapping_Click);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Import Mode:";
            // 
            // comboBoxImportMode
            // 
            this.comboBoxImportMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxImportMode.FormattingEnabled = true;
            this.comboBoxImportMode.Items.AddRange(new object[] {
            "Create",
            "Create or Update",
            "Update"});
            this.comboBoxImportMode.Location = new System.Drawing.Point(9, 66);
            this.comboBoxImportMode.Name = "comboBoxImportMode";
            this.comboBoxImportMode.Size = new System.Drawing.Size(151, 21);
            this.comboBoxImportMode.TabIndex = 24;
            // 
            // textBoxTransportationProfileName
            // 
            this.textBoxTransportationProfileName.Location = new System.Drawing.Point(23, 36);
            this.textBoxTransportationProfileName.Name = "textBoxTransportationProfileName";
            this.textBoxTransportationProfileName.Size = new System.Drawing.Size(149, 20);
            this.textBoxTransportationProfileName.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Name:";
            // 
            // labelImportFailures
            // 
            this.labelImportFailures.AutoSize = true;
            this.labelImportFailures.Location = new System.Drawing.Point(518, 7);
            this.labelImportFailures.Name = "labelImportFailures";
            this.labelImportFailures.Size = new System.Drawing.Size(0, 13);
            this.labelImportFailures.TabIndex = 28;
            // 
            // pgbState
            // 
            this.pgbState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbState.Location = new System.Drawing.Point(605, 7);
            this.pgbState.Name = "pgbState";
            this.pgbState.Size = new System.Drawing.Size(150, 10);
            this.pgbState.TabIndex = 22;
            this.pgbState.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(495, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "KO:";
            this.label3.Visible = false;
            // 
            // labelImportSuccess
            // 
            this.labelImportSuccess.AutoSize = true;
            this.labelImportSuccess.Location = new System.Drawing.Point(424, 7);
            this.labelImportSuccess.Name = "labelImportSuccess";
            this.labelImportSuccess.Size = new System.Drawing.Size(0, 13);
            this.labelImportSuccess.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(402, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "OK:";
            this.label2.Visible = false;
            // 
            // comboBoxTransportationProfiles
            // 
            this.comboBoxTransportationProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTransportationProfiles.FormattingEnabled = true;
            this.comboBoxTransportationProfiles.Location = new System.Drawing.Point(92, 4);
            this.comboBoxTransportationProfiles.Name = "comboBoxTransportationProfiles";
            this.comboBoxTransportationProfiles.Size = new System.Drawing.Size(180, 21);
            this.comboBoxTransportationProfiles.TabIndex = 20;
            this.comboBoxTransportationProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransportationProfiles_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Transport Profiles:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 441);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(767, 22);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
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
            this.menuStrip1.Size = new System.Drawing.Size(767, 24);
            this.menuStrip1.TabIndex = 30;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.toolStripSeparator7,
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
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(138, 6);
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
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 417);
            this.tableLayoutPanel1.TabIndex = 31;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.buttonStopTransport);
            this.panel1.Controls.Add(this.comboBoxTransportationProfiles);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labelImportFailures);
            this.panel1.Controls.Add(this.pgbState);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labelImportSuccess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 28);
            this.panel1.TabIndex = 22;
            // 
            // buttonStopTransport
            // 
            this.buttonStopTransport.Location = new System.Drawing.Point(295, 2);
            this.buttonStopTransport.Name = "buttonStopTransport";
            this.buttonStopTransport.Size = new System.Drawing.Size(91, 23);
            this.buttonStopTransport.TabIndex = 22;
            this.buttonStopTransport.Text = "Stop Transport";
            this.buttonStopTransport.UseVisualStyleBackColor = true;
            this.buttonStopTransport.Visible = false;
            this.buttonStopTransport.Click += new System.EventHandler(this.buttonStopTransport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxEntities);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(204, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(241, 371);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Entities:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panelTransportOrder);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(451, 40);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox4.Size = new System.Drawing.Size(310, 371);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected Entities and Transport Order:";
            // 
            // panelTransportOrder
            // 
            this.panelTransportOrder.AutoScroll = true;
            this.panelTransportOrder.BackColor = System.Drawing.SystemColors.Control;
            this.panelTransportOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTransportOrder.Location = new System.Drawing.Point(6, 19);
            this.panelTransportOrder.Name = "panelTransportOrder";
            this.panelTransportOrder.Padding = new System.Windows.Forms.Padding(3);
            this.panelTransportOrder.Size = new System.Drawing.Size(298, 346);
            this.panelTransportOrder.TabIndex = 0;
            // 
            // ReferenceDataTransporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 463);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReferenceDataTransporter";
            this.Text = "Reference Data Transporter";
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox checkedListBoxEntities;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox comboBoxOperation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonLoadEntities;
        private System.Windows.Forms.ComboBox comboBoxConnectionSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelStructureLastLoadedDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxConnectionTarget;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxImportMode;
        private System.Windows.Forms.TextBox textBoxTransportationProfileName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxTransportationProfiles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar pgbState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelImportSuccess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelImportFailures;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panelTransportOrder;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem deleteProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logArchiveToolStripMenuItem;
        private System.Windows.Forms.Button buttonStopTransport;
        private System.Windows.Forms.ToolStripMenuItem runProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transportReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Button buttonRecordsMapping;
    }
}