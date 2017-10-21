namespace MSCRMToolKit
{
    partial class DeploymentProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeploymentProperties));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownAggregateQueryRecordLimit = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxAutomaticallyInstallDatabaseUpdates = new System.Windows.Forms.CheckBox();
            this.checkBoxAutomaticallyReprovisionLanguagePacks = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxOrganizationProperties = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxLicenceanduserinformation = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonSaveServerProperties = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSslHeader = new System.Windows.Forms.TextBox();
            this.checkBoxPostViaExternalRouter = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownPostponeAppFabricRequestsInMinutes = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownMaxResultsPerCollection = new System.Windows.Forms.NumericUpDown();
            this.checkBoxNlbEnabled = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownMaxExpandCount = new System.Windows.Forms.NumericUpDown();
            this.checkBoxDisableUserInfoClaim = new System.Windows.Forms.CheckBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAggregateQueryRecordLimit)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPostponeAppFabricRequestsInMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxResultsPerCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxExpandCount)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(615, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 435);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(615, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Deployment:";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(7, 54);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(152, 21);
            this.comboBoxSource.TabIndex = 4;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Save Deployment Properties";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "AggregateQueryRecordLimit:";
            // 
            // numericUpDownAggregateQueryRecordLimit
            // 
            this.numericUpDownAggregateQueryRecordLimit.Location = new System.Drawing.Point(168, 16);
            this.numericUpDownAggregateQueryRecordLimit.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.numericUpDownAggregateQueryRecordLimit.Name = "numericUpDownAggregateQueryRecordLimit";
            this.numericUpDownAggregateQueryRecordLimit.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownAggregateQueryRecordLimit.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(165, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 59);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Important note:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(425, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // checkBoxAutomaticallyInstallDatabaseUpdates
            // 
            this.checkBoxAutomaticallyInstallDatabaseUpdates.AutoSize = true;
            this.checkBoxAutomaticallyInstallDatabaseUpdates.Location = new System.Drawing.Point(6, 42);
            this.checkBoxAutomaticallyInstallDatabaseUpdates.Name = "checkBoxAutomaticallyInstallDatabaseUpdates";
            this.checkBoxAutomaticallyInstallDatabaseUpdates.Size = new System.Drawing.Size(201, 17);
            this.checkBoxAutomaticallyInstallDatabaseUpdates.TabIndex = 10;
            this.checkBoxAutomaticallyInstallDatabaseUpdates.Text = "AutomaticallyInstallDatabaseUpdates";
            this.checkBoxAutomaticallyInstallDatabaseUpdates.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutomaticallyReprovisionLanguagePacks
            // 
            this.checkBoxAutomaticallyReprovisionLanguagePacks.AutoSize = true;
            this.checkBoxAutomaticallyReprovisionLanguagePacks.Location = new System.Drawing.Point(6, 65);
            this.checkBoxAutomaticallyReprovisionLanguagePacks.Name = "checkBoxAutomaticallyReprovisionLanguagePacks";
            this.checkBoxAutomaticallyReprovisionLanguagePacks.Size = new System.Drawing.Size(222, 17);
            this.checkBoxAutomaticallyReprovisionLanguagePacks.TabIndex = 11;
            this.checkBoxAutomaticallyReprovisionLanguagePacks.Text = "AutomaticallyReprovisionLanguagePacks";
            this.checkBoxAutomaticallyReprovisionLanguagePacks.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxAutomaticallyReprovisionLanguagePacks);
            this.groupBox2.Controls.Add(this.checkBoxAutomaticallyInstallDatabaseUpdates);
            this.groupBox2.Controls.Add(this.numericUpDownAggregateQueryRecordLimit);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(10, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 118);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Deployment Properties:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxOrganizationProperties);
            this.groupBox3.Location = new System.Drawing.Point(313, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(293, 215);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Organization Properties:";
            // 
            // textBoxOrganizationProperties
            // 
            this.textBoxOrganizationProperties.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxOrganizationProperties.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOrganizationProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOrganizationProperties.Location = new System.Drawing.Point(3, 16);
            this.textBoxOrganizationProperties.Multiline = true;
            this.textBoxOrganizationProperties.Name = "textBoxOrganizationProperties";
            this.textBoxOrganizationProperties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOrganizationProperties.Size = new System.Drawing.Size(287, 196);
            this.textBoxOrganizationProperties.TabIndex = 14;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxLicenceanduserinformation);
            this.groupBox5.Location = new System.Drawing.Point(316, 88);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(290, 118);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "License and user information";
            // 
            // textBoxLicenceanduserinformation
            // 
            this.textBoxLicenceanduserinformation.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicenceanduserinformation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicenceanduserinformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLicenceanduserinformation.Location = new System.Drawing.Point(3, 16);
            this.textBoxLicenceanduserinformation.Multiline = true;
            this.textBoxLicenceanduserinformation.Name = "textBoxLicenceanduserinformation";
            this.textBoxLicenceanduserinformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLicenceanduserinformation.Size = new System.Drawing.Size(284, 99);
            this.textBoxLicenceanduserinformation.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonSaveServerProperties);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.textBoxSslHeader);
            this.groupBox4.Controls.Add(this.checkBoxPostViaExternalRouter);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.numericUpDownPostponeAppFabricRequestsInMinutes);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.numericUpDownMaxResultsPerCollection);
            this.groupBox4.Controls.Add(this.checkBoxNlbEnabled);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.numericUpDownMaxExpandCount);
            this.groupBox4.Controls.Add(this.checkBoxDisableUserInfoClaim);
            this.groupBox4.Location = new System.Drawing.Point(10, 212);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(297, 215);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Server Properties:";
            // 
            // buttonSaveServerProperties
            // 
            this.buttonSaveServerProperties.Location = new System.Drawing.Point(9, 186);
            this.buttonSaveServerProperties.Name = "buttonSaveServerProperties";
            this.buttonSaveServerProperties.Size = new System.Drawing.Size(184, 23);
            this.buttonSaveServerProperties.TabIndex = 11;
            this.buttonSaveServerProperties.Text = "Save Server Properties";
            this.buttonSaveServerProperties.UseVisualStyleBackColor = true;
            this.buttonSaveServerProperties.Click += new System.EventHandler(this.buttonSaveServerProperties_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "SslHeader:";
            // 
            // textBoxSslHeader
            // 
            this.textBoxSslHeader.Location = new System.Drawing.Point(83, 84);
            this.textBoxSslHeader.Name = "textBoxSslHeader";
            this.textBoxSslHeader.Size = new System.Drawing.Size(205, 20);
            this.textBoxSslHeader.TabIndex = 9;
            // 
            // checkBoxPostViaExternalRouter
            // 
            this.checkBoxPostViaExternalRouter.AutoSize = true;
            this.checkBoxPostViaExternalRouter.Location = new System.Drawing.Point(9, 162);
            this.checkBoxPostViaExternalRouter.Name = "checkBoxPostViaExternalRouter";
            this.checkBoxPostViaExternalRouter.Size = new System.Drawing.Size(132, 17);
            this.checkBoxPostViaExternalRouter.TabIndex = 8;
            this.checkBoxPostViaExternalRouter.Text = "PostViaExternalRouter";
            this.checkBoxPostViaExternalRouter.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(194, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "PostponeAppFabricRequestsInMinutes:";
            // 
            // numericUpDownPostponeAppFabricRequestsInMinutes
            // 
            this.numericUpDownPostponeAppFabricRequestsInMinutes.Location = new System.Drawing.Point(209, 60);
            this.numericUpDownPostponeAppFabricRequestsInMinutes.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.numericUpDownPostponeAppFabricRequestsInMinutes.Name = "numericUpDownPostponeAppFabricRequestsInMinutes";
            this.numericUpDownPostponeAppFabricRequestsInMinutes.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownPostponeAppFabricRequestsInMinutes.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "MaxResultsPerCollection:";
            // 
            // numericUpDownMaxResultsPerCollection
            // 
            this.numericUpDownMaxResultsPerCollection.Location = new System.Drawing.Point(209, 37);
            this.numericUpDownMaxResultsPerCollection.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.numericUpDownMaxResultsPerCollection.Name = "numericUpDownMaxResultsPerCollection";
            this.numericUpDownMaxResultsPerCollection.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownMaxResultsPerCollection.TabIndex = 3;
            // 
            // checkBoxNlbEnabled
            // 
            this.checkBoxNlbEnabled.AutoSize = true;
            this.checkBoxNlbEnabled.Location = new System.Drawing.Point(9, 135);
            this.checkBoxNlbEnabled.Name = "checkBoxNlbEnabled";
            this.checkBoxNlbEnabled.Size = new System.Drawing.Size(81, 17);
            this.checkBoxNlbEnabled.TabIndex = 5;
            this.checkBoxNlbEnabled.Text = "NlbEnabled";
            this.checkBoxNlbEnabled.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "MaxExpandCount:";
            // 
            // numericUpDownMaxExpandCount
            // 
            this.numericUpDownMaxExpandCount.Location = new System.Drawing.Point(209, 13);
            this.numericUpDownMaxExpandCount.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.numericUpDownMaxExpandCount.Name = "numericUpDownMaxExpandCount";
            this.numericUpDownMaxExpandCount.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownMaxExpandCount.TabIndex = 1;
            // 
            // checkBoxDisableUserInfoClaim
            // 
            this.checkBoxDisableUserInfoClaim.AutoSize = true;
            this.checkBoxDisableUserInfoClaim.Location = new System.Drawing.Point(9, 108);
            this.checkBoxDisableUserInfoClaim.Name = "checkBoxDisableUserInfoClaim";
            this.checkBoxDisableUserInfoClaim.Size = new System.Drawing.Size(126, 17);
            this.checkBoxDisableUserInfoClaim.TabIndex = 0;
            this.checkBoxDisableUserInfoClaim.Text = "DisableUserInfoClaim";
            this.checkBoxDisableUserInfoClaim.UseVisualStyleBackColor = true;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // DeploymentProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 457);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DeploymentProperties";
            this.Text = "Deployment Properties (On-Premise only)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAggregateQueryRecordLimit)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPostponeAppFabricRequestsInMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxResultsPerCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxExpandCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownAggregateQueryRecordLimit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxAutomaticallyInstallDatabaseUpdates;
        private System.Windows.Forms.CheckBox checkBoxAutomaticallyReprovisionLanguagePacks;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxOrganizationProperties;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxSslHeader;
        private System.Windows.Forms.CheckBox checkBoxPostViaExternalRouter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownPostponeAppFabricRequestsInMinutes;
        private System.Windows.Forms.CheckBox checkBoxNlbEnabled;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxResultsPerCollection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxExpandCount;
        private System.Windows.Forms.CheckBox checkBoxDisableUserInfoClaim;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxLicenceanduserinformation;
        private System.Windows.Forms.Button buttonSaveServerProperties;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}