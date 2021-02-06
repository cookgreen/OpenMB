
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportMesh = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportMaterialScript = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportTexture = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportSkeleton = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.entryTypeTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.entryList = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.entryTypeTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSaveFile,
            this.toolStripMenuItem1,
            this.mnuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(180, 22);
            this.mnuNew.Text = "New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(180, 22);
            this.mnuOpen.Text = "Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSaveFile
            // 
            this.mnuSaveFile.Enabled = false;
            this.mnuSaveFile.Name = "mnuSaveFile";
            this.mnuSaveFile.Size = new System.Drawing.Size(180, 22);
            this.mnuSaveFile.Text = "Save";
            this.mnuSaveFile.Click += new System.EventHandler(this.mnuSaveFile_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(180, 22);
            this.mnuExit.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuImportMesh,
            this.mnuImportMaterialScript,
            this.mnuImportTexture,
            this.mnuImportSkeleton});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // mnuImportMesh
            // 
            this.mnuImportMesh.Enabled = false;
            this.mnuImportMesh.Name = "mnuImportMesh";
            this.mnuImportMesh.Size = new System.Drawing.Size(205, 22);
            this.mnuImportMesh.Text = "Import Mesh";
            this.mnuImportMesh.Click += new System.EventHandler(this.mnuImportMesh_Click);
            // 
            // mnuImportMaterialScript
            // 
            this.mnuImportMaterialScript.Enabled = false;
            this.mnuImportMaterialScript.Name = "mnuImportMaterialScript";
            this.mnuImportMaterialScript.Size = new System.Drawing.Size(205, 22);
            this.mnuImportMaterialScript.Text = "Import Material Script";
            this.mnuImportMaterialScript.Click += new System.EventHandler(this.mnuImportMaterialScript_Click);
            // 
            // mnuImportTexture
            // 
            this.mnuImportTexture.Enabled = false;
            this.mnuImportTexture.Name = "mnuImportTexture";
            this.mnuImportTexture.Size = new System.Drawing.Size(205, 22);
            this.mnuImportTexture.Text = "Import Texture";
            this.mnuImportTexture.Click += new System.EventHandler(this.mnuImportTexture_Click);
            // 
            // mnuImportSkeleton
            // 
            this.mnuImportSkeleton.Enabled = false;
            this.mnuImportSkeleton.Name = "mnuImportSkeleton";
            this.mnuImportSkeleton.Size = new System.Drawing.Size(205, 22);
            this.mnuImportSkeleton.Text = "Import Skeleton";
            this.mnuImportSkeleton.Click += new System.EventHandler(this.mnuImportSkeleton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 425);
            this.splitContainer1.SplitterDistance = 465;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.entryTypeTabControl);
            this.splitContainer2.Size = new System.Drawing.Size(465, 425);
            this.splitContainer2.SplitterDistance = 249;
            this.splitContainer2.TabIndex = 0;
            // 
            // entryTypeTabControl
            // 
            this.entryTypeTabControl.Controls.Add(this.tabPage1);
            this.entryTypeTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entryTypeTabControl.Location = new System.Drawing.Point(0, 0);
            this.entryTypeTabControl.Name = "entryTypeTabControl";
            this.entryTypeTabControl.SelectedIndex = 0;
            this.entryTypeTabControl.Size = new System.Drawing.Size(249, 425);
            this.entryTypeTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.entryList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(241, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // entryList
            // 
            this.entryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entryList.FormattingEnabled = true;
            this.entryList.ItemHeight = 12;
            this.entryList.Location = new System.Drawing.Point(3, 3);
            this.entryList.Name = "entryList";
            this.entryList.Size = new System.Drawing.Size(235, 393);
            this.entryList.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KBFEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.entryTypeTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveFile;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuImportMesh;
        private System.Windows.Forms.ToolStripMenuItem mnuImportMaterialScript;
        private System.Windows.Forms.ToolStripMenuItem mnuImportTexture;
        private System.Windows.Forms.ToolStripMenuItem mnuImportSkeleton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl entryTypeTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox entryList;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
    }
}

