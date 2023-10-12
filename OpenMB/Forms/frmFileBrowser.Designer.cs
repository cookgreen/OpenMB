namespace OpenMB.Forms
{
    partial class frmFileBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileBrowser));
            this.lbCurrent = new System.Windows.Forms.Label();
            this.txtResource = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.fileFolderList = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtFileFolderName = new System.Windows.Forms.TextBox();
            this.lbFileFolderName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCurrent
            // 
            this.lbCurrent.AutoSize = true;
            this.lbCurrent.Location = new System.Drawing.Point(10, 17);
            this.lbCurrent.Name = "lbCurrent";
            this.lbCurrent.Size = new System.Drawing.Size(53, 12);
            this.lbCurrent.TabIndex = 0;
            this.lbCurrent.Text = "Current:";
            // 
            // txtResource
            // 
            this.txtResource.Location = new System.Drawing.Point(69, 14);
            this.txtResource.Name = "txtResource";
            this.txtResource.ReadOnly = true;
            this.txtResource.Size = new System.Drawing.Size(338, 21);
            this.txtResource.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(403, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(322, 290);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUp);
            this.groupBox1.Controls.Add(this.lbCurrent);
            this.groupBox1.Controls.Add(this.txtResource);
            this.groupBox1.Location = new System.Drawing.Point(12, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 45);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnUp
            // 
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(412, 12);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(48, 23);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // fileFolderList
            // 
            this.fileFolderList.LargeImageList = this.imageList1;
            this.fileFolderList.Location = new System.Drawing.Point(12, 52);
            this.fileFolderList.Name = "fileFolderList";
            this.fileFolderList.Size = new System.Drawing.Size(466, 205);
            this.fileFolderList.SmallImageList = this.imageList1;
            this.fileFolderList.TabIndex = 6;
            this.fileFolderList.UseCompatibleStateImageBehavior = false;
            this.fileFolderList.SelectedIndexChanged += new System.EventHandler(this.fileFolderList_SelectedIndexChanged);
            this.fileFolderList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fileFolderList_MouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "file.png");
            this.imageList1.Images.SetKeyName(2, "zip.png");
            // 
            // txtFileFolderName
            // 
            this.txtFileFolderName.Location = new System.Drawing.Point(147, 263);
            this.txtFileFolderName.Name = "txtFileFolderName";
            this.txtFileFolderName.Size = new System.Drawing.Size(331, 21);
            this.txtFileFolderName.TabIndex = 7;
            // 
            // lbFileFolderName
            // 
            this.lbFileFolderName.AutoSize = true;
            this.lbFileFolderName.Location = new System.Drawing.Point(10, 266);
            this.lbFileFolderName.Name = "lbFileFolderName";
            this.lbFileFolderName.Size = new System.Drawing.Size(119, 12);
            this.lbFileFolderName.TabIndex = 8;
            this.lbFileFolderName.Text = "Selected File Name:";
            // 
            // frmRelativeFileFolderBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 321);
            this.Controls.Add(this.lbFileFolderName);
            this.Controls.Add(this.txtFileFolderName);
            this.Controls.Add(this.fileFolderList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRelativeFileFolderBrowser";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Browse";
            this.Load += new System.EventHandler(this.frmRelativeFileFolderBrowser_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCurrent;
        private System.Windows.Forms.TextBox txtResource;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ListView fileFolderList;
        private System.Windows.Forms.TextBox txtFileFolderName;
        private System.Windows.Forms.Label lbFileFolderName;
        private System.Windows.Forms.ImageList imageList1;
    }
}