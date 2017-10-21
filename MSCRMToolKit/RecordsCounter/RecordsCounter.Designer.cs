namespace MSCRMToolKit
{
    partial class RecordsCounter
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getRecordsNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openInExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonDownloadEntitiesFromSource = new System.Windows.Forms.Button();
            this.comboBoxConnectionSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxEntities = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panelSelectedEntities = new System.Windows.Forms.Panel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.importFailureBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.importFailureBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importFailureBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importFailureBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.getRecordsNumberToolStripMenuItem,
            this.openInExcelToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(625, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.quitterToolStripMenuItem.Text = "&Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // getRecordsNumberToolStripMenuItem
            // 
            this.getRecordsNumberToolStripMenuItem.Name = "getRecordsNumberToolStripMenuItem";
            this.getRecordsNumberToolStripMenuItem.Size = new System.Drawing.Size(129, 20);
            this.getRecordsNumberToolStripMenuItem.Text = "Get Records Number";
            this.getRecordsNumberToolStripMenuItem.Click += new System.EventHandler(this.getRecordsNumberToolStripMenuItem_Click);
            // 
            // openInExcelToolStripMenuItem
            // 
            this.openInExcelToolStripMenuItem.Name = "openInExcelToolStripMenuItem";
            this.openInExcelToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.openInExcelToolStripMenuItem.Text = "Open in Excel";
            this.openInExcelToolStripMenuItem.Visible = false;
            this.openInExcelToolStripMenuItem.Click += new System.EventHandler(this.openInExcelToolStripMenuItem_Click);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 396);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(625, 22);
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(625, 372);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.buttonDownloadEntitiesFromSource);
            this.panel1.Controls.Add(this.comboBoxConnectionSource);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 34);
            this.panel1.TabIndex = 2;
            // 
            // buttonDownloadEntitiesFromSource
            // 
            this.buttonDownloadEntitiesFromSource.Location = new System.Drawing.Point(248, 5);
            this.buttonDownloadEntitiesFromSource.Name = "buttonDownloadEntitiesFromSource";
            this.buttonDownloadEntitiesFromSource.Size = new System.Drawing.Size(118, 23);
            this.buttonDownloadEntitiesFromSource.TabIndex = 2;
            this.buttonDownloadEntitiesFromSource.Text = "(Re) Load Entities";
            this.buttonDownloadEntitiesFromSource.UseVisualStyleBackColor = true;
            this.buttonDownloadEntitiesFromSource.Click += new System.EventHandler(this.buttonDownloadEntitiesFromSource_Click);
            // 
            // comboBoxConnectionSource
            // 
            this.comboBoxConnectionSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnectionSource.FormattingEnabled = true;
            this.comboBoxConnectionSource.Location = new System.Drawing.Point(53, 6);
            this.comboBoxConnectionSource.Name = "comboBoxConnectionSource";
            this.comboBoxConnectionSource.Size = new System.Drawing.Size(174, 21);
            this.comboBoxConnectionSource.TabIndex = 1;
            this.comboBoxConnectionSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxConnectionSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBoxEntities);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 326);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entities";
            // 
            // checkedListBoxEntities
            // 
            this.checkedListBoxEntities.CheckOnClick = true;
            this.checkedListBoxEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxEntities.FormattingEnabled = true;
            this.checkedListBoxEntities.Location = new System.Drawing.Point(3, 16);
            this.checkedListBoxEntities.Name = "checkedListBoxEntities";
            this.checkedListBoxEntities.Size = new System.Drawing.Size(300, 307);
            this.checkedListBoxEntities.TabIndex = 0;
            this.checkedListBoxEntities.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxEntities_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelSelectedEntities);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(315, 43);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 326);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Entities";
            // 
            // panelSelectedEntities
            // 
            this.panelSelectedEntities.AutoScroll = true;
            this.panelSelectedEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectedEntities.Location = new System.Drawing.Point(3, 16);
            this.panelSelectedEntities.Name = "panelSelectedEntities";
            this.panelSelectedEntities.Size = new System.Drawing.Size(301, 307);
            this.panelSelectedEntities.TabIndex = 0;
            // 
            // RecordsCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 418);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RecordsCounter";
            this.Text = "Records Counter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importFailureBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importFailureBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxConnectionSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDownloadEntitiesFromSource;
        private System.Windows.Forms.BindingSource importFailureBindingSource1;
        private System.Windows.Forms.BindingSource importFailureBindingSource;
        private System.Windows.Forms.ToolStripMenuItem getRecordsNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openInExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.CheckedListBox checkedListBoxEntities;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panelSelectedEntities;
    }
}