namespace AMOFGameEngine.Dialogs
{
    partial class ConfigFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigFrm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbAMGELogo = new System.Windows.Forms.PictureBox();
            this.lblCOO = new System.Windows.Forms.Label();
            this.cmbValueChange = new System.Windows.Forms.ComboBox();
            this.tbRenderOpt = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblRenderSys = new System.Windows.Forms.Label();
            this.cmbSubRenderSys = new System.Windows.Forms.ComboBox();
            this.gbRenderOpt = new System.Windows.Forms.GroupBox();
            this.lstConfig = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnableMusic = new System.Windows.Forms.CheckBox();
            this.chkEnableSound = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbLanguageSelect = new System.Windows.Forms.ComboBox();
            this.lblLang = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbAMGELogo)).BeginInit();
            this.tbRenderOpt.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbRenderOpt.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(318, 417);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(399, 417);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbAMGELogo
            // 
            this.pbAMGELogo.Image = ((System.Drawing.Image)(resources.GetObject("pbAMGELogo.Image")));
            this.pbAMGELogo.Location = new System.Drawing.Point(1, 3);
            this.pbAMGELogo.Name = "pbAMGELogo";
            this.pbAMGELogo.Size = new System.Drawing.Size(481, 160);
            this.pbAMGELogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAMGELogo.TabIndex = 3;
            this.pbAMGELogo.TabStop = false;
            // 
            // lblCOO
            // 
            this.lblCOO.AutoSize = true;
            this.lblCOO.Location = new System.Drawing.Point(74, 182);
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
            this.cmbValueChange.Location = new System.Drawing.Point(209, 179);
            this.cmbValueChange.Name = "cmbValueChange";
            this.cmbValueChange.Size = new System.Drawing.Size(234, 20);
            this.cmbValueChange.TabIndex = 8;
            this.cmbValueChange.SelectedIndexChanged += new System.EventHandler(this.cmbValueChange_SelectedIndexChanged);
            // 
            // tbRenderOpt
            // 
            this.tbRenderOpt.Controls.Add(this.tabPage1);
            this.tbRenderOpt.Controls.Add(this.tabPage2);
            this.tbRenderOpt.Controls.Add(this.tabPage3);
            this.tbRenderOpt.Location = new System.Drawing.Point(12, 169);
            this.tbRenderOpt.Name = "tbRenderOpt";
            this.tbRenderOpt.SelectedIndex = 0;
            this.tbRenderOpt.Size = new System.Drawing.Size(462, 237);
            this.tbRenderOpt.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblRenderSys);
            this.tabPage1.Controls.Add(this.cmbValueChange);
            this.tabPage1.Controls.Add(this.cmbSubRenderSys);
            this.tabPage1.Controls.Add(this.lblCOO);
            this.tabPage1.Controls.Add(this.gbRenderOpt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage1.Size = new System.Drawing.Size(454, 211);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Graphic";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.cmbSubRenderSys.Size = new System.Drawing.Size(294, 20);
            this.cmbSubRenderSys.TabIndex = 5;
            this.cmbSubRenderSys.SelectedIndexChanged += new System.EventHandler(this.cmbSubRenderSys_SelectedIndexChanged);
            // 
            // gbRenderOpt
            // 
            this.gbRenderOpt.Controls.Add(this.lstConfig);
            this.gbRenderOpt.Location = new System.Drawing.Point(8, 39);
            this.gbRenderOpt.Name = "gbRenderOpt";
            this.gbRenderOpt.Size = new System.Drawing.Size(440, 134);
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
            this.lstConfig.Size = new System.Drawing.Size(428, 100);
            this.lstConfig.TabIndex = 0;
            this.lstConfig.SelectedIndexChanged += new System.EventHandler(this.lstConfig_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(454, 211);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Audio";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEnableMusic);
            this.groupBox1.Controls.Add(this.chkEnableSound);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 199);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Music&&Sound";
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
            this.chkEnableMusic.CheckedChanged += new System.EventHandler(this.chkEnableMusic_CheckedChanged);
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
            this.chkEnableSound.CheckedChanged += new System.EventHandler(this.chkEnableSound_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(454, 211);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Game";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(6, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(442, 139);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbLanguageSelect);
            this.groupBox2.Controls.Add(this.lblLang);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(442, 54);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Localization";
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
            this.cmbLanguageSelect.SelectedIndexChanged += new System.EventHandler(this.cmbLanguageSelect_SelectedIndexChanged);
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
            // ConfigFrm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 452);
            this.Controls.Add(this.tbRenderOpt);
            this.Controls.Add(this.pbAMGELogo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMOFGameEngine Option";
            this.Load += new System.EventHandler(this.ConfigFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbAMGELogo)).EndInit();
            this.tbRenderOpt.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gbRenderOpt.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pbAMGELogo;
        private System.Windows.Forms.Label lblCOO;
        private System.Windows.Forms.ComboBox cmbValueChange;
        private System.Windows.Forms.TabControl tbRenderOpt;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblRenderSys;
        private System.Windows.Forms.ComboBox cmbSubRenderSys;
        private System.Windows.Forms.GroupBox gbRenderOpt;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cmbLanguageSelect;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ListBox lstConfig;
        private System.Windows.Forms.CheckBox chkEnableSound;
        private System.Windows.Forms.CheckBox chkEnableMusic;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}