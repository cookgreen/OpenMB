namespace AMOFGameEngine
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCOO = new System.Windows.Forms.Label();
            this.cmbValueChange = new System.Windows.Forms.ComboBox();
            this.tbRenderOpt = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblRenderSys = new System.Windows.Forms.Label();
            this.cmbSubRenderSys = new System.Windows.Forms.ComboBox();
            this.gbRenderOpt = new System.Windows.Forms.GroupBox();
            this.lstConfigOpt = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmbLanguageSelect = new System.Windows.Forms.ComboBox();
            this.lblLang = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tbRenderOpt.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbRenderOpt.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-1, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(487, 160);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
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
            this.cmbSubRenderSys.DisplayMember = "sssssss";
            this.cmbSubRenderSys.FormattingEnabled = true;
            this.cmbSubRenderSys.Location = new System.Drawing.Point(119, 14);
            this.cmbSubRenderSys.Name = "cmbSubRenderSys";
            this.cmbSubRenderSys.Size = new System.Drawing.Size(294, 20);
            this.cmbSubRenderSys.TabIndex = 5;
            this.cmbSubRenderSys.SelectedIndexChanged += new System.EventHandler(this.cmbSubRenderSys_SelectedIndexChanged);
            // 
            // gbRenderOpt
            // 
            this.gbRenderOpt.Controls.Add(this.lstConfigOpt);
            this.gbRenderOpt.Location = new System.Drawing.Point(8, 39);
            this.gbRenderOpt.Name = "gbRenderOpt";
            this.gbRenderOpt.Size = new System.Drawing.Size(440, 134);
            this.gbRenderOpt.TabIndex = 1;
            this.gbRenderOpt.TabStop = false;
            this.gbRenderOpt.Text = "Render System Options";
            // 
            // lstConfigOpt
            // 
            this.lstConfigOpt.FormattingEnabled = true;
            this.lstConfigOpt.ItemHeight = 12;
            this.lstConfigOpt.Location = new System.Drawing.Point(12, 20);
            this.lstConfigOpt.Name = "lstConfigOpt";
            this.lstConfigOpt.Size = new System.Drawing.Size(414, 100);
            this.lstConfigOpt.TabIndex = 0;
            this.lstConfigOpt.SelectedIndexChanged += new System.EventHandler(this.lstConfigOpt_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(454, 211);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Audio";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmbLanguageSelect);
            this.tabPage3.Controls.Add(this.lblLang);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(454, 211);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Game";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmbLanguageSelect
            // 
            this.cmbLanguageSelect.FormattingEnabled = true;
            this.cmbLanguageSelect.Items.AddRange(new object[] {
            "English",
            "Simple Chinese"});
            this.cmbLanguageSelect.Location = new System.Drawing.Point(100, 22);
            this.cmbLanguageSelect.Name = "cmbLanguageSelect";
            this.cmbLanguageSelect.Size = new System.Drawing.Size(236, 20);
            this.cmbLanguageSelect.TabIndex = 1;
            this.cmbLanguageSelect.SelectedIndexChanged += new System.EventHandler(this.cmbLanguageSelect_SelectedIndexChanged);
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Location = new System.Drawing.Point(21, 25);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(59, 12);
            this.lblLang.TabIndex = 0;
            this.lblLang.Text = "Language:";
            // 
            // ConfigFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 452);
            this.Controls.Add(this.tbRenderOpt);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ConfigFrm";
            this.Text = "AMOF Game Engine Setup";
            this.Load += new System.EventHandler(this.ConfigFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tbRenderOpt.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gbRenderOpt.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCOO;
        private System.Windows.Forms.ComboBox cmbValueChange;
        private System.Windows.Forms.TabControl tbRenderOpt;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblRenderSys;
        private System.Windows.Forms.ComboBox cmbSubRenderSys;
        private System.Windows.Forms.GroupBox gbRenderOpt;
        private System.Windows.Forms.ListBox lstConfigOpt;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cmbLanguageSelect;
        private System.Windows.Forms.Label lblLang;
    }
}