
namespace KBFEditor
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportMesh = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportMaterialScript = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportTexture = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuImportSkeleton = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.entryTypeTabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.entryList = new System.Windows.Forms.ListBox();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAboutKBFEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.entryTypeTabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(1067, 28);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "Main Menu";
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
			// mnuFileNew
			// 
			this.mnuFileNew.Name = "mnuFileNew";
			this.mnuFileNew.Size = new System.Drawing.Size(224, 26);
			this.mnuFileNew.Text = "New";
			this.mnuFileNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Name = "mnuFileOpen";
			this.mnuFileOpen.Size = new System.Drawing.Size(224, 26);
			this.mnuFileOpen.Text = "Open";
			this.mnuFileOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuFileSave
			// 
			this.mnuFileSave.Enabled = false;
			this.mnuFileSave.Name = "mnuFileSave";
			this.mnuFileSave.Size = new System.Drawing.Size(224, 26);
			this.mnuFileSave.Text = "Save";
			this.mnuFileSave.Click += new System.EventHandler(this.mnuSaveFile_Click);
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
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImportMesh,
            this.mnuImportMaterialScript,
            this.mnuImportTexture,
            this.mnuImportSkeleton});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// mnuImportMesh
			// 
			this.mnuImportMesh.Enabled = false;
			this.mnuImportMesh.Name = "mnuImportMesh";
			this.mnuImportMesh.Size = new System.Drawing.Size(253, 26);
			this.mnuImportMesh.Text = "Import Mesh";
			this.mnuImportMesh.Click += new System.EventHandler(this.mnuImportMesh_Click);
			// 
			// mnuImportMaterialScript
			// 
			this.mnuImportMaterialScript.Enabled = false;
			this.mnuImportMaterialScript.Name = "mnuImportMaterialScript";
			this.mnuImportMaterialScript.Size = new System.Drawing.Size(253, 26);
			this.mnuImportMaterialScript.Text = "Import Material Script";
			this.mnuImportMaterialScript.Click += new System.EventHandler(this.mnuImportMaterialScript_Click);
			// 
			// mnuImportTexture
			// 
			this.mnuImportTexture.Enabled = false;
			this.mnuImportTexture.Name = "mnuImportTexture";
			this.mnuImportTexture.Size = new System.Drawing.Size(253, 26);
			this.mnuImportTexture.Text = "Import Texture";
			this.mnuImportTexture.Click += new System.EventHandler(this.mnuImportTexture_Click);
			// 
			// mnuImportSkeleton
			// 
			this.mnuImportSkeleton.Enabled = false;
			this.mnuImportSkeleton.Name = "mnuImportSkeleton";
			this.mnuImportSkeleton.Size = new System.Drawing.Size(253, 26);
			this.mnuImportSkeleton.Text = "Import Skeleton";
			this.mnuImportSkeleton.Click += new System.EventHandler(this.mnuImportSkeleton_Click);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 28);
			this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.entryTypeTabControl);
			this.splitContainer2.Size = new System.Drawing.Size(1067, 534);
			this.splitContainer2.SplitterDistance = 571;
			this.splitContainer2.SplitterWidth = 5;
			this.splitContainer2.TabIndex = 1;
			// 
			// entryTypeTabControl
			// 
			this.entryTypeTabControl.Controls.Add(this.tabPage1);
			this.entryTypeTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.entryTypeTabControl.Location = new System.Drawing.Point(0, 0);
			this.entryTypeTabControl.Margin = new System.Windows.Forms.Padding(4);
			this.entryTypeTabControl.Name = "entryTypeTabControl";
			this.entryTypeTabControl.SelectedIndex = 0;
			this.entryTypeTabControl.Size = new System.Drawing.Size(571, 534);
			this.entryTypeTabControl.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.entryList);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
			this.tabPage1.Size = new System.Drawing.Size(563, 505);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// entryList
			// 
			this.entryList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.entryList.FormattingEnabled = true;
			this.entryList.ItemHeight = 15;
			this.entryList.Location = new System.Drawing.Point(4, 4);
			this.entryList.Margin = new System.Windows.Forms.Padding(4);
			this.entryList.Name = "entryList";
			this.entryList.Size = new System.Drawing.Size(555, 497);
			this.entryList.TabIndex = 0;
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAboutKBFEditor});
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// mnuAboutKBFEditor
			// 
			this.mnuAboutKBFEditor.Name = "mnuAboutKBFEditor";
			this.mnuAboutKBFEditor.Size = new System.Drawing.Size(224, 26);
			this.mnuAboutKBFEditor.Text = "About KBF Editor";
			// 
			// mnuFileSaveAs
			// 
			this.mnuFileSaveAs.Enabled = false;
			this.mnuFileSaveAs.Name = "mnuFileSaveAs";
			this.mnuFileSaveAs.Size = new System.Drawing.Size(224, 26);
			this.mnuFileSaveAs.Text = "Save As";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 562);
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.mainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenu;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "KBFEditor";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.entryTypeTabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuImportMesh;
        private System.Windows.Forms.ToolStripMenuItem mnuImportMaterialScript;
        private System.Windows.Forms.ToolStripMenuItem mnuImportTexture;
        private System.Windows.Forms.ToolStripMenuItem mnuImportSkeleton;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNew;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TabControl entryTypeTabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.ListBox entryList;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuAboutKBFEditor;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSaveAs;
	}
}

