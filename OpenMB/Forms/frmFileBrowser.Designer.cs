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
            this.lbCurrent.Location = new System.Drawing.Point(13, 21);
            this.lbCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCurrent.Name = "lbCurrent";
            this.lbCurrent.Size = new System.Drawing.Size(71, 15);
            this.lbCurrent.TabIndex = 0;
            this.lbCurrent.Text = "Current:";
            // 
            // txtResource
            // 
            this.txtResource.Location = new System.Drawing.Point(92, 18);
            this.txtResource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtResource.Name = "txtResource";
            this.txtResource.ReadOnly = true;
            this.txtResource.Size = new System.Drawing.Size(310, 25);
            this.txtResource.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(390, 362);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(282, 362);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 29);
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
            this.groupBox1.Location = new System.Drawing.Point(16, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(474, 56);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // btnUp
            // 
            this.btnUp.Enabled = false;
            this.btnUp.Location = new System.Drawing.Point(410, 14);
            this.btnUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(64, 29);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // fileFolderList
            // 
            this.fileFolderList.HideSelection = false;
            this.fileFolderList.LargeImageList = this.imageList1;
            this.fileFolderList.Location = new System.Drawing.Point(16, 65);
            this.fileFolderList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileFolderList.Name = "fileFolderList";
            this.fileFolderList.Size = new System.Drawing.Size(474, 255);
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
            this.txtFileFolderName.Location = new System.Drawing.Point(196, 329);
            this.txtFileFolderName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFileFolderName.Name = "txtFileFolderName";
            this.txtFileFolderName.Size = new System.Drawing.Size(294, 25);
            this.txtFileFolderName.TabIndex = 7;
            // 
            // lbFileFolderName
            // 
            this.lbFileFolderName.AutoSize = true;
            this.lbFileFolderName.Location = new System.Drawing.Point(13, 332);
            this.lbFileFolderName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFileFolderName.Name = "lbFileFolderName";
            this.lbFileFolderName.Size = new System.Drawing.Size(159, 15);
            this.lbFileFolderName.TabIndex = 8;
            this.lbFileFolderName.Text = "Selected File Name:";
            // 
            // frmFileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 401);
            this.Controls.Add(this.lbFileFolderName);
            this.Controls.Add(this.txtFileFolderName);
            this.Controls.Add(this.fileFolderList);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFileBrowser";
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