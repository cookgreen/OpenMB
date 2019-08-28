namespace OpenMB.Forms
{
    partial class frmResourceLocEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtResource = new System.Windows.Forms.TextBox();
            this.btnChooseResource = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resource Location:";
            // 
            // txtResource
            // 
            this.txtResource.Location = new System.Drawing.Point(129, 14);
            this.txtResource.Name = "txtResource";
            this.txtResource.ReadOnly = true;
            this.txtResource.Size = new System.Drawing.Size(330, 21);
            this.txtResource.TabIndex = 1;
            // 
            // btnChooseResource
            // 
            this.btnChooseResource.Location = new System.Drawing.Point(465, 12);
            this.btnChooseResource.Name = "btnChooseResource";
            this.btnChooseResource.Size = new System.Drawing.Size(38, 23);
            this.btnChooseResource.TabIndex = 2;
            this.btnChooseResource.Text = "...";
            this.btnChooseResource.UseVisualStyleBackColor = true;
            this.btnChooseResource.Click += new System.EventHandler(this.btnChooseResource_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(436, 52);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(355, 52);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtResource);
            this.groupBox1.Controls.Add(this.btnChooseResource);
            this.groupBox1.Location = new System.Drawing.Point(2, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 45);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // frmResourceLocEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 87);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResourceLocEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resource Location Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtResource;
        private System.Windows.Forms.Button btnChooseResource;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}