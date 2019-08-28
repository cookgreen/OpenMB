using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMB.Forms
{
    public partial class frmResourceLocEditor : Form
    {
        public string ResourceLoc;
        private string resourceRootDir;
        private string type;
        public frmResourceLocEditor(string resourceRootDir, string type, string filePath = null)
        {
            InitializeComponent();
            this.resourceRootDir = resourceRootDir;
            this.type = type;
            if (!string.IsNullOrEmpty(filePath))
            {
                txtResource.Text = filePath;
            }
        }

        private void btnChooseResource_Click(object sender, EventArgs e)
        {
            switch (type)
            {
                case "FileSystem":
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                    folderDialog.ShowNewFolderButton = false;
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtResource.Text = folderDialog.SelectedPath;
                    }
                    break;
                case "Zip":
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "Zip File|*.zip";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        txtResource.Text = fileDialog.FileName;
                    }
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
