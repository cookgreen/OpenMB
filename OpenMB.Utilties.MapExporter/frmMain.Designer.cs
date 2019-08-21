namespace OpenMB.Utilties.MapExporter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMapXmlPath = new System.Windows.Forms.TextBox();
            this.btnChooseMapXml = new System.Windows.Forms.Button();
            this.btnChooseMBMapTxt = new System.Windows.Forms.Button();
            this.txtMBMapPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMapXmlPath);
            this.groupBox1.Controls.Add(this.btnChooseMapXml);
            this.groupBox1.Controls.Add(this.btnChooseMBMapTxt);
            this.groupBox1.Controls.Add(this.txtMBMapPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtMapXmlPath
            // 
            this.txtMapXmlPath.Location = new System.Drawing.Point(161, 41);
            this.txtMapXmlPath.Name = "txtMapXmlPath";
            this.txtMapXmlPath.ReadOnly = true;
            this.txtMapXmlPath.Size = new System.Drawing.Size(250, 21);
            this.txtMapXmlPath.TabIndex = 5;
            // 
            // btnChooseMapXml
            // 
            this.btnChooseMapXml.Location = new System.Drawing.Point(417, 38);
            this.btnChooseMapXml.Name = "btnChooseMapXml";
            this.btnChooseMapXml.Size = new System.Drawing.Size(45, 23);
            this.btnChooseMapXml.TabIndex = 4;
            this.btnChooseMapXml.Text = "...";
            this.btnChooseMapXml.UseVisualStyleBackColor = true;
            this.btnChooseMapXml.Click += new System.EventHandler(this.btnChooseMapXml_Click);
            // 
            // btnChooseMBMapTxt
            // 
            this.btnChooseMBMapTxt.Location = new System.Drawing.Point(417, 12);
            this.btnChooseMBMapTxt.Name = "btnChooseMBMapTxt";
            this.btnChooseMBMapTxt.Size = new System.Drawing.Size(45, 23);
            this.btnChooseMBMapTxt.TabIndex = 3;
            this.btnChooseMBMapTxt.Text = "...";
            this.btnChooseMBMapTxt.UseVisualStyleBackColor = true;
            this.btnChooseMBMapTxt.Click += new System.EventHandler(this.btnChooseMBMapTxt_Click);
            // 
            // txtMBMapPath
            // 
            this.txtMBMapPath.Location = new System.Drawing.Point(161, 14);
            this.txtMBMapPath.Name = "txtMBMapPath";
            this.txtMBMapPath.ReadOnly = true;
            this.txtMBMapPath.Size = new System.Drawing.Size(250, 21);
            this.txtMBMapPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Generate xml path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "MountBlade map.txt path:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(405, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(324, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 120);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Map Exporter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMapXmlPath;
        private System.Windows.Forms.Button btnChooseMapXml;
        private System.Windows.Forms.Button btnChooseMBMapTxt;
        private System.Windows.Forms.TextBox txtMBMapPath;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}