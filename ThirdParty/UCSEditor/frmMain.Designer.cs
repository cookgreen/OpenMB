namespace UCSEditor
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menus = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditAddNewLine = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEditDeleteThisLine = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAboutUCSEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbGoogleTranslationAPILanguages = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitWinMain = new System.Windows.Forms.SplitContainer();
            this.lsvLocateInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtKey = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSuggestion = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtLocalizedText = new System.Windows.Forms.RichTextBox();
            this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menus.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitWinMain)).BeginInit();
            this.splitWinMain.Panel1.SuspendLayout();
            this.splitWinMain.Panel2.SuspendLayout();
            this.splitWinMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menus
            // 
            this.menus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menus.Location = new System.Drawing.Point(0, 0);
            this.menus.Name = "menus";
            this.menus.Size = new System.Drawing.Size(1045, 28);
            this.menus.TabIndex = 1;
            this.menus.Text = "Menus";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNew,
            this.mnuFileOpen,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.toolStripMenuItem1,
            this.mnuFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(48, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.Size = new System.Drawing.Size(224, 26);
            this.mnuFileOpen.Text = "Open";
            this.mnuFileOpen.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Enabled = false;
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.Size = new System.Drawing.Size(224, 26);
            this.mnuFileSave.Text = "Save";
            this.mnuFileSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Enabled = false;
            this.mnuFileSaveAs.Name = "mnuFileSaveAs";
            this.mnuFileSaveAs.Size = new System.Drawing.Size(224, 26);
            this.mnuFileSaveAs.Text = "Save As";
            this.mnuFileSaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(221, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(224, 26);
            this.mnuFileExit.Text = "Exit";
            this.mnuFileExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditAddNewLine,
            this.mnuEditDeleteThisLine});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // mnuEditAddNewLine
            // 
            this.mnuEditAddNewLine.Enabled = false;
            this.mnuEditAddNewLine.Name = "mnuEditAddNewLine";
            this.mnuEditAddNewLine.Size = new System.Drawing.Size(224, 26);
            this.mnuEditAddNewLine.Text = "Add New Line";
            this.mnuEditAddNewLine.Click += new System.EventHandler(this.addNewLineToolStripMenuItem_Click);
            // 
            // mnuEditDeleteThisLine
            // 
            this.mnuEditDeleteThisLine.Enabled = false;
            this.mnuEditDeleteThisLine.Name = "mnuEditDeleteThisLine";
            this.mnuEditDeleteThisLine.Size = new System.Drawing.Size(224, 26);
            this.mnuEditDeleteThisLine.Text = "Delete this Line";
            this.mnuEditDeleteThisLine.Click += new System.EventHandler(this.deleteThisLineToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAboutUCSEditor});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // mnuAboutUCSEditor
            // 
            this.mnuAboutUCSEditor.Name = "mnuAboutUCSEditor";
            this.mnuAboutUCSEditor.Size = new System.Drawing.Size(224, 26);
            this.mnuAboutUCSEditor.Text = "About UCSEditor";
            this.mnuAboutUCSEditor.Click += new System.EventHandler(this.aboutUCSEditorToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbGoogleTranslationAPILanguages});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1045, 28);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(324, 25);
            this.toolStripLabel1.Text = "Choose the language you want translate to:";
            // 
            // cmbGoogleTranslationAPILanguages
            // 
            this.cmbGoogleTranslationAPILanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGoogleTranslationAPILanguages.Name = "cmbGoogleTranslationAPILanguages";
            this.cmbGoogleTranslationAPILanguages.Size = new System.Drawing.Size(160, 28);
            this.cmbGoogleTranslationAPILanguages.SelectedIndexChanged += new System.EventHandler(this.cmbGoogleTranslationAPILanguages_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitWinMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1045, 713);
            this.panel1.TabIndex = 3;
            // 
            // splitWinMain
            // 
            this.splitWinMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitWinMain.Location = new System.Drawing.Point(0, 0);
            this.splitWinMain.Margin = new System.Windows.Forms.Padding(4);
            this.splitWinMain.Name = "splitWinMain";
            this.splitWinMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitWinMain.Panel1
            // 
            this.splitWinMain.Panel1.Controls.Add(this.lsvLocateInfo);
            // 
            // splitWinMain.Panel2
            // 
            this.splitWinMain.Panel2.Controls.Add(this.splitContainer1);
            this.splitWinMain.Size = new System.Drawing.Size(1045, 713);
            this.splitWinMain.SplitterDistance = 229;
            this.splitWinMain.SplitterWidth = 5;
            this.splitWinMain.TabIndex = 1;
            // 
            // lsvLocateInfo
            // 
            this.lsvLocateInfo.BackColor = System.Drawing.Color.White;
            this.lsvLocateInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvLocateInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvLocateInfo.FullRowSelect = true;
            this.lsvLocateInfo.GridLines = true;
            this.lsvLocateInfo.HideSelection = false;
            this.lsvLocateInfo.Location = new System.Drawing.Point(0, 0);
            this.lsvLocateInfo.Margin = new System.Windows.Forms.Padding(4);
            this.lsvLocateInfo.Name = "lsvLocateInfo";
            this.lsvLocateInfo.Size = new System.Drawing.Size(1045, 229);
            this.lsvLocateInfo.TabIndex = 0;
            this.lsvLocateInfo.UseCompatibleStateImageBehavior = false;
            this.lsvLocateInfo.View = System.Windows.Forms.View.Details;
            this.lsvLocateInfo.SelectedIndexChanged += new System.EventHandler(this.lsvLocateInfo_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Key";
            this.columnHeader1.Width = 384;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Text";
            this.columnHeader2.Width = 382;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1045, 479);
            this.splitContainer1.SplitterDistance = 517;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtKey);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(517, 479);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key";
            // 
            // txtKey
            // 
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKey.Location = new System.Drawing.Point(4, 22);
            this.txtKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(509, 453);
            this.txtKey.TabIndex = 0;
            this.txtKey.Text = "";
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(523, 479);
            this.splitContainer2.SplitterDistance = 163;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSuggestion);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(523, 163);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Suggestion";
            // 
            // txtSuggestion
            // 
            this.txtSuggestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSuggestion.Location = new System.Drawing.Point(4, 22);
            this.txtSuggestion.Margin = new System.Windows.Forms.Padding(4);
            this.txtSuggestion.Name = "txtSuggestion";
            this.txtSuggestion.ReadOnly = true;
            this.txtSuggestion.Size = new System.Drawing.Size(515, 137);
            this.txtSuggestion.TabIndex = 0;
            this.txtSuggestion.Text = "No Suggestion";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLocalizedText);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(523, 311);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Text";
            // 
            // txtLocalizedText
            // 
            this.txtLocalizedText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocalizedText.Location = new System.Drawing.Point(4, 22);
            this.txtLocalizedText.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocalizedText.Name = "txtLocalizedText";
            this.txtLocalizedText.Size = new System.Drawing.Size(515, 285);
            this.txtLocalizedText.TabIndex = 0;
            this.txtLocalizedText.Text = "";
            this.txtLocalizedText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLocalizedText_KeyDown);
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Name = "mnuFileNew";
            this.mnuFileNew.Size = new System.Drawing.Size(224, 26);
            this.mnuFileNew.Text = "New";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 769);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menus;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UCSEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.menus.ResumeLayout(false);
            this.menus.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitWinMain.Panel1.ResumeLayout(false);
            this.splitWinMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitWinMain)).EndInit();
            this.splitWinMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menus;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuEditAddNewLine;
        private System.Windows.Forms.ToolStripMenuItem mnuEditDeleteThisLine;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuAboutUCSEditor;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbGoogleTranslationAPILanguages;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitWinMain;
        private System.Windows.Forms.ListView lsvLocateInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txtKey;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtSuggestion;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox txtLocalizedText;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNew;
    }
}