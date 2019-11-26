namespace OpenMB.Forms
{
    partial class frmConfigure
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigure));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pbAMGELogo = new System.Windows.Forms.PictureBox();
			this.lblCOO = new System.Windows.Forms.Label();
			this.cmbValueChange = new System.Windows.Forms.ComboBox();
			this.tbRenderOpt = new System.Windows.Forms.TabControl();
			this.tpGraphic = new System.Windows.Forms.TabPage();
			this.lblRenderSys = new System.Windows.Forms.Label();
			this.cmbSubRenderSys = new System.Windows.Forms.ComboBox();
			this.gbRenderOpt = new System.Windows.Forms.GroupBox();
			this.lstConfig = new System.Windows.Forms.ListBox();
			this.tpGame = new System.Windows.Forms.TabPage();
			this.gbAdvanced = new System.Windows.Forms.GroupBox();
			this.chkEnableCheatMode = new System.Windows.Forms.CheckBox();
			this.chkEnableEditMode = new System.Windows.Forms.CheckBox();
			this.gbLocalization = new System.Windows.Forms.GroupBox();
			this.cmbLanguageSelect = new System.Windows.Forms.ComboBox();
			this.lblLang = new System.Windows.Forms.Label();
			this.tpAudio = new System.Windows.Forms.TabPage();
			this.gbMusicSound = new System.Windows.Forms.GroupBox();
			this.chkEnableMusic = new System.Windows.Forms.CheckBox();
			this.chkEnableSound = new System.Windows.Forms.CheckBox();
			this.tpResource = new System.Windows.Forms.TabPage();
			this.groupZip = new System.Windows.Forms.GroupBox();
			this.btnDeleteZipResourceLoc = new System.Windows.Forms.Button();
			this.resourceZipList = new System.Windows.Forms.ListBox();
			this.btnAddZipResourceLoc = new System.Windows.Forms.Button();
			this.btnModifyZipResourceLoc = new System.Windows.Forms.Button();
			this.groupFileSystem = new System.Windows.Forms.GroupBox();
			this.resourceFileSystemList = new System.Windows.Forms.ListBox();
			this.btnDeleteFileSystemResourceLoc = new System.Windows.Forms.Button();
			this.btnAddFileSystemResourceLoc = new System.Windows.Forms.Button();
			this.btnModifyFileSystemResourceLoc = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pbAMGELogo)).BeginInit();
			this.tbRenderOpt.SuspendLayout();
			this.tpGraphic.SuspendLayout();
			this.gbRenderOpt.SuspendLayout();
			this.tpGame.SuspendLayout();
			this.gbAdvanced.SuspendLayout();
			this.gbLocalization.SuspendLayout();
			this.tpAudio.SuspendLayout();
			this.gbMusicSound.SuspendLayout();
			this.tpResource.SuspendLayout();
			this.groupZip.SuspendLayout();
			this.groupFileSystem.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(322, 425);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(403, 425);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pbAMGELogo
			// 
			this.pbAMGELogo.Image = global::OpenMB.Properties.Resources.logo;
			this.pbAMGELogo.Location = new System.Drawing.Point(1, -1);
			this.pbAMGELogo.Name = "pbAMGELogo";
			this.pbAMGELogo.Size = new System.Drawing.Size(481, 173);
			this.pbAMGELogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbAMGELogo.TabIndex = 3;
			this.pbAMGELogo.TabStop = false;
			// 
			// lblCOO
			// 
			this.lblCOO.AutoSize = true;
			this.lblCOO.Location = new System.Drawing.Point(98, 189);
			this.lblCOO.Name = "lblCOO";
			this.lblCOO.Size = new System.Drawing.Size(113, 12);
			this.lblCOO.TabIndex = 7;
			this.lblCOO.Text = "[Click On Option]:";
			// 
			// cmbValueChange
			// 
			this.cmbValueChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbValueChange.Enabled = false;
			this.cmbValueChange.FormattingEnabled = true;
			this.cmbValueChange.Location = new System.Drawing.Point(232, 186);
			this.cmbValueChange.Name = "cmbValueChange";
			this.cmbValueChange.Size = new System.Drawing.Size(234, 20);
			this.cmbValueChange.TabIndex = 8;
			this.cmbValueChange.SelectedIndexChanged += new System.EventHandler(this.cmbValueChange_SelectedIndexChanged);
			// 
			// tbRenderOpt
			// 
			this.tbRenderOpt.Controls.Add(this.tpGraphic);
			this.tbRenderOpt.Controls.Add(this.tpGame);
			this.tbRenderOpt.Controls.Add(this.tpAudio);
			this.tbRenderOpt.Controls.Add(this.tpResource);
			this.tbRenderOpt.Location = new System.Drawing.Point(1, 169);
			this.tbRenderOpt.Name = "tbRenderOpt";
			this.tbRenderOpt.SelectedIndex = 0;
			this.tbRenderOpt.Size = new System.Drawing.Size(481, 250);
			this.tbRenderOpt.TabIndex = 9;
			// 
			// tpGraphic
			// 
			this.tpGraphic.Controls.Add(this.lblRenderSys);
			this.tpGraphic.Controls.Add(this.cmbValueChange);
			this.tpGraphic.Controls.Add(this.cmbSubRenderSys);
			this.tpGraphic.Controls.Add(this.lblCOO);
			this.tpGraphic.Controls.Add(this.gbRenderOpt);
			this.tpGraphic.Location = new System.Drawing.Point(4, 22);
			this.tpGraphic.Name = "tpGraphic";
			this.tpGraphic.Padding = new System.Windows.Forms.Padding(3);
			this.tpGraphic.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tpGraphic.Size = new System.Drawing.Size(473, 224);
			this.tpGraphic.TabIndex = 0;
			this.tpGraphic.Text = "Graphic";
			this.tpGraphic.UseVisualStyleBackColor = true;
			// 
			// lblRenderSys
			// 
			this.lblRenderSys.AutoSize = true;
			this.lblRenderSys.Location = new System.Drawing.Point(6, 17);
			this.lblRenderSys.Name = "lblRenderSys";
			this.lblRenderSys.Size = new System.Drawing.Size(107, 12);
			this.lblRenderSys.TabIndex = 4;
			this.lblRenderSys.Text = "Render Subsystem:";
			// 
			// cmbSubRenderSys
			// 
			this.cmbSubRenderSys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSubRenderSys.FormattingEnabled = true;
			this.cmbSubRenderSys.Location = new System.Drawing.Point(119, 14);
			this.cmbSubRenderSys.Name = "cmbSubRenderSys";
			this.cmbSubRenderSys.Size = new System.Drawing.Size(347, 20);
			this.cmbSubRenderSys.TabIndex = 5;
			this.cmbSubRenderSys.SelectedIndexChanged += new System.EventHandler(this.cmbSubRenderSys_SelectedIndexChanged);
			// 
			// gbRenderOpt
			// 
			this.gbRenderOpt.Controls.Add(this.lstConfig);
			this.gbRenderOpt.Location = new System.Drawing.Point(8, 39);
			this.gbRenderOpt.Name = "gbRenderOpt";
			this.gbRenderOpt.Size = new System.Drawing.Size(458, 141);
			this.gbRenderOpt.TabIndex = 1;
			this.gbRenderOpt.TabStop = false;
			this.gbRenderOpt.Text = "Render System Options";
			// 
			// lstConfig
			// 
			this.lstConfig.FormattingEnabled = true;
			this.lstConfig.ItemHeight = 12;
			this.lstConfig.Location = new System.Drawing.Point(6, 20);
			this.lstConfig.Name = "lstConfig";
			this.lstConfig.Size = new System.Drawing.Size(446, 112);
			this.lstConfig.TabIndex = 0;
			this.lstConfig.SelectedIndexChanged += new System.EventHandler(this.lstConfig_SelectedIndexChanged);
			// 
			// tpGame
			// 
			this.tpGame.Controls.Add(this.gbAdvanced);
			this.tpGame.Controls.Add(this.gbLocalization);
			this.tpGame.Location = new System.Drawing.Point(4, 22);
			this.tpGame.Name = "tpGame";
			this.tpGame.Padding = new System.Windows.Forms.Padding(3);
			this.tpGame.Size = new System.Drawing.Size(473, 224);
			this.tpGame.TabIndex = 2;
			this.tpGame.Text = "Game";
			this.tpGame.UseVisualStyleBackColor = true;
			// 
			// gbAdvanced
			// 
			this.gbAdvanced.Controls.Add(this.chkEnableCheatMode);
			this.gbAdvanced.Controls.Add(this.chkEnableEditMode);
			this.gbAdvanced.Location = new System.Drawing.Point(6, 66);
			this.gbAdvanced.Name = "gbAdvanced";
			this.gbAdvanced.Size = new System.Drawing.Size(460, 152);
			this.gbAdvanced.TabIndex = 3;
			this.gbAdvanced.TabStop = false;
			this.gbAdvanced.Text = "Advanced Options";
			// 
			// chkEnableCheatMode
			// 
			this.chkEnableCheatMode.AutoSize = true;
			this.chkEnableCheatMode.Location = new System.Drawing.Point(8, 53);
			this.chkEnableCheatMode.Name = "chkEnableCheatMode";
			this.chkEnableCheatMode.Size = new System.Drawing.Size(126, 16);
			this.chkEnableCheatMode.TabIndex = 1;
			this.chkEnableCheatMode.Text = "Enable Cheat Mode";
			this.chkEnableCheatMode.UseVisualStyleBackColor = true;
			// 
			// chkEnableEditMode
			// 
			this.chkEnableEditMode.AutoSize = true;
			this.chkEnableEditMode.Location = new System.Drawing.Point(8, 20);
			this.chkEnableEditMode.Name = "chkEnableEditMode";
			this.chkEnableEditMode.Size = new System.Drawing.Size(120, 16);
			this.chkEnableEditMode.TabIndex = 0;
			this.chkEnableEditMode.Text = "Enable Edit Mode";
			this.chkEnableEditMode.UseVisualStyleBackColor = true;
			// 
			// gbLocalization
			// 
			this.gbLocalization.Controls.Add(this.cmbLanguageSelect);
			this.gbLocalization.Controls.Add(this.lblLang);
			this.gbLocalization.Location = new System.Drawing.Point(6, 6);
			this.gbLocalization.Name = "gbLocalization";
			this.gbLocalization.Size = new System.Drawing.Size(460, 54);
			this.gbLocalization.TabIndex = 2;
			this.gbLocalization.TabStop = false;
			this.gbLocalization.Text = "Localization";
			// 
			// cmbLanguageSelect
			// 
			this.cmbLanguageSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLanguageSelect.FormattingEnabled = true;
			this.cmbLanguageSelect.Items.AddRange(new object[] {
            "English",
            "Simple Chinese"});
			this.cmbLanguageSelect.Location = new System.Drawing.Point(85, 20);
			this.cmbLanguageSelect.Name = "cmbLanguageSelect";
			this.cmbLanguageSelect.Size = new System.Drawing.Size(236, 20);
			this.cmbLanguageSelect.TabIndex = 1;
			// 
			// lblLang
			// 
			this.lblLang.AutoSize = true;
			this.lblLang.Location = new System.Drawing.Point(6, 23);
			this.lblLang.Name = "lblLang";
			this.lblLang.Size = new System.Drawing.Size(59, 12);
			this.lblLang.TabIndex = 0;
			this.lblLang.Text = "Language:";
			// 
			// tpAudio
			// 
			this.tpAudio.Controls.Add(this.gbMusicSound);
			this.tpAudio.Location = new System.Drawing.Point(4, 22);
			this.tpAudio.Name = "tpAudio";
			this.tpAudio.Padding = new System.Windows.Forms.Padding(3);
			this.tpAudio.Size = new System.Drawing.Size(473, 224);
			this.tpAudio.TabIndex = 1;
			this.tpAudio.Text = "Audio";
			this.tpAudio.UseVisualStyleBackColor = true;
			// 
			// gbMusicSound
			// 
			this.gbMusicSound.Controls.Add(this.chkEnableMusic);
			this.gbMusicSound.Controls.Add(this.chkEnableSound);
			this.gbMusicSound.Location = new System.Drawing.Point(6, 6);
			this.gbMusicSound.Name = "gbMusicSound";
			this.gbMusicSound.Size = new System.Drawing.Size(460, 212);
			this.gbMusicSound.TabIndex = 2;
			this.gbMusicSound.TabStop = false;
			this.gbMusicSound.Text = "Music&&Sound";
			// 
			// chkEnableMusic
			// 
			this.chkEnableMusic.AutoSize = true;
			this.chkEnableMusic.Checked = true;
			this.chkEnableMusic.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableMusic.Location = new System.Drawing.Point(6, 52);
			this.chkEnableMusic.Name = "chkEnableMusic";
			this.chkEnableMusic.Size = new System.Drawing.Size(96, 16);
			this.chkEnableMusic.TabIndex = 1;
			this.chkEnableMusic.Text = "Enable Music";
			this.chkEnableMusic.UseVisualStyleBackColor = true;
			// 
			// chkEnableSound
			// 
			this.chkEnableSound.AutoSize = true;
			this.chkEnableSound.Checked = true;
			this.chkEnableSound.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEnableSound.Location = new System.Drawing.Point(6, 20);
			this.chkEnableSound.Name = "chkEnableSound";
			this.chkEnableSound.Size = new System.Drawing.Size(96, 16);
			this.chkEnableSound.TabIndex = 0;
			this.chkEnableSound.Text = "Enable Sound";
			this.chkEnableSound.UseVisualStyleBackColor = true;
			// 
			// tpResource
			// 
			this.tpResource.Controls.Add(this.groupZip);
			this.tpResource.Controls.Add(this.groupFileSystem);
			this.tpResource.Location = new System.Drawing.Point(4, 22);
			this.tpResource.Name = "tpResource";
			this.tpResource.Padding = new System.Windows.Forms.Padding(3);
			this.tpResource.Size = new System.Drawing.Size(473, 224);
			this.tpResource.TabIndex = 3;
			this.tpResource.Text = "Resources";
			this.tpResource.UseVisualStyleBackColor = true;
			// 
			// groupZip
			// 
			this.groupZip.Controls.Add(this.btnDeleteZipResourceLoc);
			this.groupZip.Controls.Add(this.resourceZipList);
			this.groupZip.Controls.Add(this.btnAddZipResourceLoc);
			this.groupZip.Controls.Add(this.btnModifyZipResourceLoc);
			this.groupZip.Location = new System.Drawing.Point(238, 6);
			this.groupZip.Name = "groupZip";
			this.groupZip.Size = new System.Drawing.Size(228, 212);
			this.groupZip.TabIndex = 7;
			this.groupZip.TabStop = false;
			this.groupZip.Text = "Zip";
			// 
			// btnDeleteZipResourceLoc
			// 
			this.btnDeleteZipResourceLoc.Enabled = false;
			this.btnDeleteZipResourceLoc.Location = new System.Drawing.Point(130, 183);
			this.btnDeleteZipResourceLoc.Name = "btnDeleteZipResourceLoc";
			this.btnDeleteZipResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnDeleteZipResourceLoc.TabIndex = 6;
			this.btnDeleteZipResourceLoc.Text = "Delete";
			this.btnDeleteZipResourceLoc.UseVisualStyleBackColor = true;
			this.btnDeleteZipResourceLoc.Click += new System.EventHandler(this.btnDeleteZipResourceLoc_Click);
			// 
			// resourceZipList
			// 
			this.resourceZipList.FormattingEnabled = true;
			this.resourceZipList.ItemHeight = 12;
			this.resourceZipList.Location = new System.Drawing.Point(6, 17);
			this.resourceZipList.Name = "resourceZipList";
			this.resourceZipList.Size = new System.Drawing.Size(216, 160);
			this.resourceZipList.TabIndex = 1;
			this.resourceZipList.SelectedIndexChanged += new System.EventHandler(this.resourceZipList_SelectedIndexChanged);
			// 
			// btnAddZipResourceLoc
			// 
			this.btnAddZipResourceLoc.Location = new System.Drawing.Point(6, 183);
			this.btnAddZipResourceLoc.Name = "btnAddZipResourceLoc";
			this.btnAddZipResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnAddZipResourceLoc.TabIndex = 4;
			this.btnAddZipResourceLoc.Text = "Add";
			this.btnAddZipResourceLoc.UseVisualStyleBackColor = true;
			this.btnAddZipResourceLoc.Click += new System.EventHandler(this.btnAddZipResourceLoc_Click);
			// 
			// btnModifyZipResourceLoc
			// 
			this.btnModifyZipResourceLoc.Enabled = false;
			this.btnModifyZipResourceLoc.Location = new System.Drawing.Point(68, 183);
			this.btnModifyZipResourceLoc.Name = "btnModifyZipResourceLoc";
			this.btnModifyZipResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnModifyZipResourceLoc.TabIndex = 5;
			this.btnModifyZipResourceLoc.Text = "Modify";
			this.btnModifyZipResourceLoc.UseVisualStyleBackColor = true;
			this.btnModifyZipResourceLoc.Click += new System.EventHandler(this.btnModifyZipResourceLoc_Click);
			// 
			// groupFileSystem
			// 
			this.groupFileSystem.Controls.Add(this.resourceFileSystemList);
			this.groupFileSystem.Controls.Add(this.btnDeleteFileSystemResourceLoc);
			this.groupFileSystem.Controls.Add(this.btnAddFileSystemResourceLoc);
			this.groupFileSystem.Controls.Add(this.btnModifyFileSystemResourceLoc);
			this.groupFileSystem.Location = new System.Drawing.Point(7, 8);
			this.groupFileSystem.Name = "groupFileSystem";
			this.groupFileSystem.Size = new System.Drawing.Size(225, 210);
			this.groupFileSystem.TabIndex = 6;
			this.groupFileSystem.TabStop = false;
			this.groupFileSystem.Text = "FileSystem";
			// 
			// resourceFileSystemList
			// 
			this.resourceFileSystemList.FormattingEnabled = true;
			this.resourceFileSystemList.ItemHeight = 12;
			this.resourceFileSystemList.Location = new System.Drawing.Point(6, 15);
			this.resourceFileSystemList.Name = "resourceFileSystemList";
			this.resourceFileSystemList.Size = new System.Drawing.Size(213, 160);
			this.resourceFileSystemList.TabIndex = 0;
			this.resourceFileSystemList.SelectedIndexChanged += new System.EventHandler(this.resourceFileSystemList_SelectedIndexChanged);
			// 
			// btnDeleteFileSystemResourceLoc
			// 
			this.btnDeleteFileSystemResourceLoc.Enabled = false;
			this.btnDeleteFileSystemResourceLoc.Location = new System.Drawing.Point(130, 181);
			this.btnDeleteFileSystemResourceLoc.Name = "btnDeleteFileSystemResourceLoc";
			this.btnDeleteFileSystemResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnDeleteFileSystemResourceLoc.TabIndex = 3;
			this.btnDeleteFileSystemResourceLoc.Text = "Delete";
			this.btnDeleteFileSystemResourceLoc.UseVisualStyleBackColor = true;
			this.btnDeleteFileSystemResourceLoc.Click += new System.EventHandler(this.btnDeleteFileSystemResourceLoc_Click);
			// 
			// btnAddFileSystemResourceLoc
			// 
			this.btnAddFileSystemResourceLoc.Location = new System.Drawing.Point(6, 181);
			this.btnAddFileSystemResourceLoc.Name = "btnAddFileSystemResourceLoc";
			this.btnAddFileSystemResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnAddFileSystemResourceLoc.TabIndex = 1;
			this.btnAddFileSystemResourceLoc.Text = "Add";
			this.btnAddFileSystemResourceLoc.UseVisualStyleBackColor = true;
			this.btnAddFileSystemResourceLoc.Click += new System.EventHandler(this.btnAddFileSystemResourceLoc_Click);
			// 
			// btnModifyFileSystemResourceLoc
			// 
			this.btnModifyFileSystemResourceLoc.Enabled = false;
			this.btnModifyFileSystemResourceLoc.Location = new System.Drawing.Point(68, 181);
			this.btnModifyFileSystemResourceLoc.Name = "btnModifyFileSystemResourceLoc";
			this.btnModifyFileSystemResourceLoc.Size = new System.Drawing.Size(56, 23);
			this.btnModifyFileSystemResourceLoc.TabIndex = 2;
			this.btnModifyFileSystemResourceLoc.Text = "Modify";
			this.btnModifyFileSystemResourceLoc.UseVisualStyleBackColor = true;
			this.btnModifyFileSystemResourceLoc.Click += new System.EventHandler(this.btnModifyFileSystemResourceLoc_Click);
			// 
			// frmConfigure
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(483, 455);
			this.Controls.Add(this.tbRenderOpt);
			this.Controls.Add(this.pbAMGELogo);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmConfigure";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Engine Option";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfigure_FormClosed);
			this.Load += new System.EventHandler(this.ConfigFrm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pbAMGELogo)).EndInit();
			this.tbRenderOpt.ResumeLayout(false);
			this.tpGraphic.ResumeLayout(false);
			this.tpGraphic.PerformLayout();
			this.gbRenderOpt.ResumeLayout(false);
			this.tpGame.ResumeLayout(false);
			this.gbAdvanced.ResumeLayout(false);
			this.gbAdvanced.PerformLayout();
			this.gbLocalization.ResumeLayout(false);
			this.gbLocalization.PerformLayout();
			this.tpAudio.ResumeLayout(false);
			this.gbMusicSound.ResumeLayout(false);
			this.gbMusicSound.PerformLayout();
			this.tpResource.ResumeLayout(false);
			this.groupZip.ResumeLayout(false);
			this.groupFileSystem.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pbAMGELogo;
        private System.Windows.Forms.Label lblCOO;
        private System.Windows.Forms.ComboBox cmbValueChange;
        private System.Windows.Forms.TabControl tbRenderOpt;
        private System.Windows.Forms.TabPage tpGraphic;
        private System.Windows.Forms.Label lblRenderSys;
        private System.Windows.Forms.ComboBox cmbSubRenderSys;
        private System.Windows.Forms.GroupBox gbRenderOpt;
        private System.Windows.Forms.TabPage tpAudio;
        private System.Windows.Forms.TabPage tpGame;
        private System.Windows.Forms.ComboBox cmbLanguageSelect;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ListBox lstConfig;
        private System.Windows.Forms.CheckBox chkEnableSound;
        private System.Windows.Forms.CheckBox chkEnableMusic;
        private System.Windows.Forms.GroupBox gbMusicSound;
        private System.Windows.Forms.GroupBox gbAdvanced;
        private System.Windows.Forms.GroupBox gbLocalization;
        private System.Windows.Forms.CheckBox chkEnableEditMode;
        private System.Windows.Forms.CheckBox chkEnableCheatMode;
        private System.Windows.Forms.TabPage tpResource;
        private System.Windows.Forms.Button btnDeleteFileSystemResourceLoc;
        private System.Windows.Forms.Button btnModifyFileSystemResourceLoc;
        private System.Windows.Forms.Button btnAddFileSystemResourceLoc;
        private System.Windows.Forms.GroupBox groupZip;
        private System.Windows.Forms.GroupBox groupFileSystem;
        private System.Windows.Forms.ListBox resourceFileSystemList;
        private System.Windows.Forms.ListBox resourceZipList;
        private System.Windows.Forms.Button btnDeleteZipResourceLoc;
        private System.Windows.Forms.Button btnAddZipResourceLoc;
        private System.Windows.Forms.Button btnModifyZipResourceLoc;
    }
}