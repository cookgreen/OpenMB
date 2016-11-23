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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstConfigOpt = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSubRenderSys = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbValueChange = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(318, 380);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstConfigOpt);
            this.groupBox1.Location = new System.Drawing.Point(12, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Render System Options";
            // 
            // lstConfigOpt
            // 
            this.lstConfigOpt.FormattingEnabled = true;
            this.lstConfigOpt.ItemHeight = 12;
            this.lstConfigOpt.Location = new System.Drawing.Point(6, 20);
            this.lstConfigOpt.Name = "lstConfigOpt";
            this.lstConfigOpt.Size = new System.Drawing.Size(447, 100);
            this.lstConfigOpt.TabIndex = 0;
            this.lstConfigOpt.SelectedIndexChanged += new System.EventHandler(this.lstConfigOpt_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(399, 380);
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
            this.pictureBox1.Size = new System.Drawing.Size(487, 207);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Render Subsystem:";
            // 
            // cmbSubRenderSys
            // 
            this.cmbSubRenderSys.DisplayMember = "sssssss";
            this.cmbSubRenderSys.FormattingEnabled = true;
            this.cmbSubRenderSys.Location = new System.Drawing.Point(155, 187);
            this.cmbSubRenderSys.Name = "cmbSubRenderSys";
            this.cmbSubRenderSys.Size = new System.Drawing.Size(294, 20);
            this.cmbSubRenderSys.TabIndex = 5;
            this.cmbSubRenderSys.SelectedIndexChanged += new System.EventHandler(this.cmbSubRenderSys_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "[Click On Option]:";
            // 
            // cmbValueChange
            // 
            this.cmbValueChange.Enabled = false;
            this.cmbValueChange.FormattingEnabled = true;
            this.cmbValueChange.Location = new System.Drawing.Point(231, 353);
            this.cmbValueChange.Name = "cmbValueChange";
            this.cmbValueChange.Size = new System.Drawing.Size(234, 20);
            this.cmbValueChange.TabIndex = 8;
            this.cmbValueChange.SelectedIndexChanged += new System.EventHandler(this.cmbValueChange_SelectedIndexChanged);
            // 
            // ConfigFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 415);
            this.Controls.Add(this.cmbValueChange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSubRenderSys);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Name = "ConfigFrm";
            this.Text = "AMOF Game Engine Setup";
            this.Load += new System.EventHandler(this.ConfigFrm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSubRenderSys;
        private System.Windows.Forms.ListBox lstConfigOpt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbValueChange;
    }
}