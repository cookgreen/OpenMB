namespace AMOFGameEngine.Utilities.LocateFileEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitWinMain = new System.Windows.Forms.SplitContainer();
            this.lsvLocateInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtLocalizedText = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.menus = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteThisLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutUCSEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitWinMain)).BeginInit();
            this.splitWinMain.Panel1.SuspendLayout();
            this.splitWinMain.Panel2.SuspendLayout();
            this.splitWinMain.SuspendLayout();
            this.menus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitWinMain
            // 
            this.splitWinMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitWinMain.Location = new System.Drawing.Point(0, 25);
            this.splitWinMain.Name = "splitWinMain";
            this.splitWinMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitWinMain.Panel1
            // 
            this.splitWinMain.Panel1.Controls.Add(this.lsvLocateInfo);
            // 
            // splitWinMain.Panel2
            // 
            this.splitWinMain.Panel2.Controls.Add(this.txtLocalizedText);
            this.splitWinMain.Panel2.Controls.Add(this.splitter1);
            this.splitWinMain.Panel2.Controls.Add(this.txtKey);
            this.splitWinMain.Size = new System.Drawing.Size(784, 445);
            this.splitWinMain.SplitterDistance = 278;
            this.splitWinMain.TabIndex = 0;
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
            this.lsvLocateInfo.Location = new System.Drawing.Point(0, 0);
            this.lsvLocateInfo.Name = "lsvLocateInfo";
            this.lsvLocateInfo.Size = new System.Drawing.Size(784, 278);
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
            // txtLocalizedText
            // 
            this.txtLocalizedText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLocalizedText.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLocalizedText.Location = new System.Drawing.Point(405, 0);
            this.txtLocalizedText.Multiline = true;
            this.txtLocalizedText.Name = "txtLocalizedText";
            this.txtLocalizedText.Size = new System.Drawing.Size(379, 163);
            this.txtLocalizedText.TabIndex = 2;
            this.txtLocalizedText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLocalizedText_KeyDown);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(400, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 163);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // txtKey
            // 
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtKey.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtKey.Location = new System.Drawing.Point(0, 0);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(400, 163);
            this.txtKey.TabIndex = 0;
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            // 
            // menus
            // 
            this.menus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menus.Location = new System.Drawing.Point(0, 0);
            this.menus.Name = "menus";
            this.menus.Size = new System.Drawing.Size(784, 25);
            this.menus.TabIndex = 1;
            this.menus.Text = "Menus";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(118, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewLineToolStripMenuItem,
            this.deleteThisLineToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(42, 21);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addNewLineToolStripMenuItem
            // 
            this.addNewLineToolStripMenuItem.Enabled = false;
            this.addNewLineToolStripMenuItem.Name = "addNewLineToolStripMenuItem";
            this.addNewLineToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addNewLineToolStripMenuItem.Text = "Add New Line";
            this.addNewLineToolStripMenuItem.Click += new System.EventHandler(this.addNewLineToolStripMenuItem_Click);
            // 
            // deleteThisLineToolStripMenuItem
            // 
            this.deleteThisLineToolStripMenuItem.Enabled = false;
            this.deleteThisLineToolStripMenuItem.Name = "deleteThisLineToolStripMenuItem";
            this.deleteThisLineToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteThisLineToolStripMenuItem.Text = "Delete this Line";
            this.deleteThisLineToolStripMenuItem.Click += new System.EventHandler(this.deleteThisLineToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutUCSEditorToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(55, 21);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutUCSEditorToolStripMenuItem
            // 
            this.aboutUCSEditorToolStripMenuItem.Name = "aboutUCSEditorToolStripMenuItem";
            this.aboutUCSEditorToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.aboutUCSEditorToolStripMenuItem.Text = "About UCSEditor";
            this.aboutUCSEditorToolStripMenuItem.Click += new System.EventHandler(this.aboutUCSEditorToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 470);
            this.Controls.Add(this.splitWinMain);
            this.Controls.Add(this.menus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menus;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UCSEditor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.splitWinMain.Panel1.ResumeLayout(false);
            this.splitWinMain.Panel2.ResumeLayout(false);
            this.splitWinMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitWinMain)).EndInit();
            this.splitWinMain.ResumeLayout(false);
            this.menus.ResumeLayout(false);
            this.menus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitWinMain;
        private System.Windows.Forms.ListView lsvLocateInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.MenuStrip menus;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteThisLineToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox txtLocalizedText;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutUCSEditorToolStripMenuItem;
    }
}