namespace MSCRMToolKit
{
    partial class SolutionsTransporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionsTransporter));
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
            this.importLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logArchivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UniqueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publisherDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mSCRMSolutionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkedListBoxSettings = new System.Windows.Forms.CheckedListBox();
            this.checkBoxPublishAllCustomizationsSource = new System.Windows.Forms.CheckBox();
            this.comboBoxConnectionSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLoadSolutions = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSelectedFolder = new System.Windows.Forms.TextBox();
            this.labelSolutionsRefreshedOn = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxExportAsManaged = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxOverwriteUnmanagedCustomizations = new System.Windows.Forms.CheckBox();
            this.checkBoxPublishWorkflows = new System.Windows.Forms.CheckBox();
            this.checkBoxPublishAllCustomizationsTarget = new System.Windows.Forms.CheckBox();
            this.comboBoxConnectionTarget = new System.Windows.Forms.ComboBox();
            this.comboBoxSolutionsToImport = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxOperation = new System.Windows.Forms.ComboBox();
            this.textBoxProfileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pgbState = new System.Windows.Forms.ProgressBar();
            this.buttonStopTransport = new System.Windows.Forms.Button();
            this.comboBoxProfiles = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSCRMSolutionBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.runProfileToolStripMenuItem,
            this.importLogsToolStripMenuItem,
            this.logToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1015, 24);
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
            // importLogsToolStripMenuItem
            // 
            this.importLogsToolStripMenuItem.Name = "importLogsToolStripMenuItem";
            this.importLogsToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.importLogsToolStripMenuItem.Text = "Import Logs";
            this.importLogsToolStripMenuItem.Visible = false;
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogToolStripMenuItem,
            this.logArchivesToolStripMenuItem});
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.logToolStripMenuItem.Text = "Log";
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.viewLogToolStripMenuItem_Click);
            // 
            // logArchivesToolStripMenuItem
            // 
            this.logArchivesToolStripMenuItem.Name = "logArchivesToolStripMenuItem";
            this.logArchivesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logArchivesToolStripMenuItem.Text = "Log Archives";
            this.logArchivesToolStripMenuItem.Click += new System.EventHandler(this.logArchivesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1015, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 377F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1015, 487);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(380, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(632, 441);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Solutions:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.displayNameDataGridViewTextBoxColumn,
            this.UniqueName,
            this.publisherDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.mSCRMSolutionBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(626, 422);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Select";
            this.dataGridViewCheckBoxColumn1.HeaderText = "#";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 20;
            // 
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.HeaderText = "Display Name";
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // UniqueName
            // 
            this.UniqueName.DataPropertyName = "UniqueName";
            this.UniqueName.HeaderText = "Unique Name";
            this.UniqueName.Name = "UniqueName";
            this.UniqueName.ReadOnly = true;
            // 
            // publisherDataGridViewTextBoxColumn
            // 
            this.publisherDataGridViewTextBoxColumn.DataPropertyName = "Publisher";
            this.publisherDataGridViewTextBoxColumn.HeaderText = "Publisher";
            this.publisherDataGridViewTextBoxColumn.Name = "publisherDataGridViewTextBoxColumn";
            this.publisherDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mSCRMSolutionBindingSource
            // 
            this.mSCRMSolutionBindingSource.DataSource = typeof(MSCRMSolution);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBoxOperation);
            this.groupBox2.Controls.Add(this.textBoxProfileName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 43);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(371, 441);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Solutions Transport Profile Properties";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.checkedListBoxSettings);
            this.groupBox4.Controls.Add(this.checkBoxPublishAllCustomizationsSource);
            this.groupBox4.Controls.Add(this.comboBoxConnectionSource);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.buttonLoadSolutions);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.textBoxSelectedFolder);
            this.groupBox4.Controls.Add(this.labelSolutionsRefreshedOn);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.checkBoxExportAsManaged);
            this.groupBox4.Location = new System.Drawing.Point(7, 69);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(355, 209);
            this.groupBox4.TabIndex = 44;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Source";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(192, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Settings:";
            // 
            // checkedListBoxSettings
            // 
            this.checkedListBoxSettings.CheckOnClick = true;
            this.checkedListBoxSettings.FormattingEnabled = true;
            this.checkedListBoxSettings.Items.AddRange(new object[] {
            "AutoNumbering",
            "Calendar",
            "Customization",
            "EmailTracking",
            "General",
            "Marketing",
            "OutlookSynchronization",
            "RelationshipRoles",
            "IsvConfig"});
            this.checkedListBoxSettings.Location = new System.Drawing.Point(192, 33);
            this.checkedListBoxSettings.Name = "checkedListBoxSettings";
            this.checkedListBoxSettings.Size = new System.Drawing.Size(155, 139);
            this.checkedListBoxSettings.TabIndex = 14;
            // 
            // checkBoxPublishAllCustomizationsSource
            // 
            this.checkBoxPublishAllCustomizationsSource.AutoSize = true;
            this.checkBoxPublishAllCustomizationsSource.Location = new System.Drawing.Point(9, 182);
            this.checkBoxPublishAllCustomizationsSource.Name = "checkBoxPublishAllCustomizationsSource";
            this.checkBoxPublishAllCustomizationsSource.Size = new System.Drawing.Size(218, 17);
            this.checkBoxPublishAllCustomizationsSource.TabIndex = 12;
            this.checkBoxPublishAllCustomizationsSource.Text = "Publish All Customizations (before export)";
            this.checkBoxPublishAllCustomizationsSource.UseVisualStyleBackColor = true;
            // 
            // comboBoxConnectionSource
            // 
            this.comboBoxConnectionSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionSource.FormattingEnabled = true;
            this.comboBoxConnectionSource.Location = new System.Drawing.Point(8, 33);
            this.comboBoxConnectionSource.Name = "comboBoxConnectionSource";
            this.comboBoxConnectionSource.Size = new System.Drawing.Size(155, 21);
            this.comboBoxConnectionSource.TabIndex = 1;
            this.comboBoxConnectionSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxConnectionSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source Connection:";
            // 
            // buttonLoadSolutions
            // 
            this.buttonLoadSolutions.Location = new System.Drawing.Point(8, 89);
            this.buttonLoadSolutions.Name = "buttonLoadSolutions";
            this.buttonLoadSolutions.Size = new System.Drawing.Size(159, 23);
            this.buttonLoadSolutions.TabIndex = 5;
            this.buttonLoadSolutions.Text = "(Re) Load Solutions";
            this.buttonLoadSolutions.UseVisualStyleBackColor = true;
            this.buttonLoadSolutions.Click += new System.EventHandler(this.buttonLoadSolutions_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Refreshed on:";
            // 
            // textBoxSelectedFolder
            // 
            this.textBoxSelectedFolder.Location = new System.Drawing.Point(8, 136);
            this.textBoxSelectedFolder.Name = "textBoxSelectedFolder";
            this.textBoxSelectedFolder.Size = new System.Drawing.Size(128, 20);
            this.textBoxSelectedFolder.TabIndex = 7;
            // 
            // labelSolutionsRefreshedOn
            // 
            this.labelSolutionsRefreshedOn.AutoSize = true;
            this.labelSolutionsRefreshedOn.Location = new System.Drawing.Point(9, 73);
            this.labelSolutionsRefreshedOn.Name = "labelSolutionsRefreshedOn";
            this.labelSolutionsRefreshedOn.Size = new System.Drawing.Size(0, 13);
            this.labelSolutionsRefreshedOn.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Export Folder:";
            // 
            // checkBoxExportAsManaged
            // 
            this.checkBoxExportAsManaged.AutoSize = true;
            this.checkBoxExportAsManaged.Location = new System.Drawing.Point(9, 163);
            this.checkBoxExportAsManaged.Name = "checkBoxExportAsManaged";
            this.checkBoxExportAsManaged.Size = new System.Drawing.Size(165, 17);
            this.checkBoxExportAsManaged.TabIndex = 9;
            this.checkBoxExportAsManaged.Text = "Export Solutions As Managed";
            this.checkBoxExportAsManaged.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.checkBoxOverwriteUnmanagedCustomizations);
            this.groupBox3.Controls.Add(this.checkBoxPublishWorkflows);
            this.groupBox3.Controls.Add(this.checkBoxPublishAllCustomizationsTarget);
            this.groupBox3.Controls.Add(this.comboBoxConnectionTarget);
            this.groupBox3.Controls.Add(this.comboBoxSolutionsToImport);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(7, 294);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 130);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Target Connection:";
            // 
            // checkBoxOverwriteUnmanagedCustomizations
            // 
            this.checkBoxOverwriteUnmanagedCustomizations.AutoSize = true;
            this.checkBoxOverwriteUnmanagedCustomizations.Location = new System.Drawing.Point(9, 105);
            this.checkBoxOverwriteUnmanagedCustomizations.Name = "checkBoxOverwriteUnmanagedCustomizations";
            this.checkBoxOverwriteUnmanagedCustomizations.Size = new System.Drawing.Size(205, 17);
            this.checkBoxOverwriteUnmanagedCustomizations.TabIndex = 45;
            this.checkBoxOverwriteUnmanagedCustomizations.Text = "Overwrite Unmanaged Customizations";
            this.checkBoxOverwriteUnmanagedCustomizations.UseVisualStyleBackColor = true;
            // 
            // checkBoxPublishWorkflows
            // 
            this.checkBoxPublishWorkflows.AutoSize = true;
            this.checkBoxPublishWorkflows.Location = new System.Drawing.Point(9, 87);
            this.checkBoxPublishWorkflows.Name = "checkBoxPublishWorkflows";
            this.checkBoxPublishWorkflows.Size = new System.Drawing.Size(173, 17);
            this.checkBoxPublishWorkflows.TabIndex = 44;
            this.checkBoxPublishWorkflows.Text = "Activate processes and plugins";
            this.checkBoxPublishWorkflows.UseVisualStyleBackColor = true;
            // 
            // checkBoxPublishAllCustomizationsTarget
            // 
            this.checkBoxPublishAllCustomizationsTarget.AutoSize = true;
            this.checkBoxPublishAllCustomizationsTarget.Location = new System.Drawing.Point(9, 69);
            this.checkBoxPublishAllCustomizationsTarget.Name = "checkBoxPublishAllCustomizationsTarget";
            this.checkBoxPublishAllCustomizationsTarget.Size = new System.Drawing.Size(208, 17);
            this.checkBoxPublishAllCustomizationsTarget.TabIndex = 43;
            this.checkBoxPublishAllCustomizationsTarget.Text = "Publish All Customizations (after import)";
            this.checkBoxPublishAllCustomizationsTarget.UseVisualStyleBackColor = true;
            // 
            // comboBoxConnectionTarget
            // 
            this.comboBoxConnectionTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionTarget.FormattingEnabled = true;
            this.comboBoxConnectionTarget.Location = new System.Drawing.Point(8, 36);
            this.comboBoxConnectionTarget.Name = "comboBoxConnectionTarget";
            this.comboBoxConnectionTarget.Size = new System.Drawing.Size(157, 21);
            this.comboBoxConnectionTarget.TabIndex = 39;
            // 
            // comboBoxSolutionsToImport
            // 
            this.comboBoxSolutionsToImport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSolutionsToImport.FormattingEnabled = true;
            this.comboBoxSolutionsToImport.Items.AddRange(new object[] {
            "Newest",
            "Oldest"});
            this.comboBoxSolutionsToImport.Location = new System.Drawing.Point(192, 36);
            this.comboBoxSolutionsToImport.Name = "comboBoxSolutionsToImport";
            this.comboBoxSolutionsToImport.Size = new System.Drawing.Size(155, 21);
            this.comboBoxSolutionsToImport.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(192, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Solutions to Import:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Operation:";
            // 
            // comboBoxOperation
            // 
            this.comboBoxOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperation.FormattingEnabled = true;
            this.comboBoxOperation.Items.AddRange(new object[] {
            "Export Solutions",
            "Import Solutions",
            "Transport Solutions"});
            this.comboBoxOperation.Location = new System.Drawing.Point(199, 37);
            this.comboBoxOperation.Name = "comboBoxOperation";
            this.comboBoxOperation.Size = new System.Drawing.Size(155, 21);
            this.comboBoxOperation.TabIndex = 37;
            this.comboBoxOperation.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperation_SelectedIndexChanged);
            // 
            // textBoxProfileName
            // 
            this.textBoxProfileName.Location = new System.Drawing.Point(15, 37);
            this.textBoxProfileName.Name = "textBoxProfileName";
            this.textBoxProfileName.Size = new System.Drawing.Size(156, 20);
            this.textBoxProfileName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.pgbState);
            this.panel1.Controls.Add(this.buttonStopTransport);
            this.panel1.Controls.Add(this.comboBoxProfiles);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1009, 34);
            this.panel1.TabIndex = 4;
            // 
            // pgbState
            // 
            this.pgbState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pgbState.Location = new System.Drawing.Point(780, 11);
            this.pgbState.Name = "pgbState";
            this.pgbState.Size = new System.Drawing.Size(220, 10);
            this.pgbState.TabIndex = 31;
            this.pgbState.Visible = false;
            // 
            // buttonStopTransport
            // 
            this.buttonStopTransport.Location = new System.Drawing.Point(683, 4);
            this.buttonStopTransport.Name = "buttonStopTransport";
            this.buttonStopTransport.Size = new System.Drawing.Size(91, 23);
            this.buttonStopTransport.TabIndex = 30;
            this.buttonStopTransport.Text = "Stop Transport";
            this.buttonStopTransport.UseVisualStyleBackColor = true;
            this.buttonStopTransport.Visible = false;
            this.buttonStopTransport.Click += new System.EventHandler(this.buttonStopTransport_Click);
            // 
            // comboBoxProfiles
            // 
            this.comboBoxProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProfiles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxProfiles.FormattingEnabled = true;
            this.comboBoxProfiles.Location = new System.Drawing.Point(142, 4);
            this.comboBoxProfiles.Name = "comboBoxProfiles";
            this.comboBoxProfiles.Size = new System.Drawing.Size(151, 21);
            this.comboBoxProfiles.TabIndex = 1;
            this.comboBoxProfiles.SelectedIndexChanged += new System.EventHandler(this.comboBoxSolutionExportProfiles_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Solutions Transport Profiles:";
            // 
            // SolutionsTransporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 533);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SolutionsTransporter";
            this.Text = "Solutions Transporter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSCRMSolutionBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBoxConnectionSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxProfileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxProfiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem deleteProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logArchivesToolStripMenuItem;
        private System.Windows.Forms.Button buttonLoadSolutions;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxSelectedFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxExportAsManaged;
        private System.Windows.Forms.BindingSource mSCRMSolutionBindingSource;
        private System.Windows.Forms.Label labelSolutionsRefreshedOn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOperation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxConnectionTarget;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSolutionsToImport;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox checkedListBoxSettings;
        private System.Windows.Forms.CheckBox checkBoxPublishAllCustomizationsSource;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxPublishAllCustomizationsTarget;
        private System.Windows.Forms.CheckBox checkBoxOverwriteUnmanagedCustomizations;
        private System.Windows.Forms.CheckBox checkBoxPublishWorkflows;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UniqueName;
        private System.Windows.Forms.DataGridViewTextBoxColumn publisherDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonStopTransport;
        private System.Windows.Forms.ProgressBar pgbState;
        private System.Windows.Forms.ToolStripMenuItem importLogsToolStripMenuItem;
    }
}