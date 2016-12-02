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
            this.label2 = new System.Windows.Forms.Label();
            this.cmbValueChange = new System.Windows.Forms.ComboBox();
            this.tbRenderOpt = new System.Windows.Forms.TabControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstConfigOpt = new System.Windows.Forms.ListBox();
            this.cmbSubRenderSys = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLanguageSelect = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tbRenderOpt.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.pictureBox1.Location = new System.Drawing.Point(-1, -20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(487, 183);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "[Click On Option]:";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstConfigOpt);
            this.groupBox1.Location = new System.Drawing.Point(8, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Render System Options";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Render Subsystem:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cmbValueChange);
            this.tabPage1.Controls.Add(this.cmbSubRenderSys);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabPage1.Size = new System.Drawing.Size(454, 211);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Graphic";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(454, 211);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Game";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Language:";
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
            this.groupBox1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbValueChange;
        private System.Windows.Forms.TabControl tbRenderOpt;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSubRenderSys;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstConfigOpt;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cmbLanguageSelect;
        private System.Windows.Forms.Label label3;
    }
}