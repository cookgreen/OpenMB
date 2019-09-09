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
    public enum ShowType
    {
        ShowFile,
        ShowFolder
    }
    public partial class frmRelativeFileFolderBrowser : Form
    {
        private Stack<string> pathStack;
        private string currentFullPath;
        private string currentRelativePath;
        private List<FileSystemInfo> currentFileFolders;
        public string InitFullPath { get; set; }
        public ShowType ShowType { get; set; }
        public string Filter { get; set; }
        public string RelativePath { get { return currentRelativePath; } }
        public frmRelativeFileFolderBrowser()
        {
            InitializeComponent();

            lbFileFolderName.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_selected_file_name");
            lbCurrent.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_current_loc");
            btnOK.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_ok");
            btnCancel.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_cancel");
            btnUp.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_up");
            Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_title");
        }

        private void frmRelativeFileFolderBrowser_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InitFullPath))
            {
                throw new Exception("InitFullPath can't be empty!");
            }

            pathStack = new Stack<string>();
            txtResource.Text = Path.GetDirectoryName(InitFullPath);

            DirectoryInfo dii = new DirectoryInfo(InitFullPath);
            txtResource.Text = dii.Name;
            currentRelativePath = dii.Name;
            currentFullPath = InitFullPath;
            switch(ShowType)
            {
                case ShowType.ShowFile:
                    lbFileFolderName.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_selected_file_name");
                    break;
                case ShowType.ShowFolder:
                    lbFileFolderName.Text = Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_selected_folder_name");
                    break;
            }
            pathStack.Push(currentFullPath);
            RefreshFileFolder();
        }

        private void RefreshFileFolder()
        {
            txtResource.Text = currentRelativePath;
            fileFolderList.Items.Clear();

            DirectoryInfo di = new DirectoryInfo(currentFullPath);
            currentFileFolders = di.EnumerateFileSystemInfos().ToList();

            foreach (var fileFolder in currentFileFolders)
            {
                ListViewItem item = new ListViewItem();
                item.Text = fileFolder.Name;
                if (fileFolder.Attributes == FileAttributes.Directory)
                {
                    item.ImageIndex = 0;
                    fileFolderList.Items.Add(item);
                }
                else
                {
                    string extension = Path.GetExtension(fileFolder.Name);
                    if (extension == "zip")
                    {
                        item.ImageIndex = 2;
                    }
                    else
                    {
                        item.ImageIndex = 1;
                    }
                    if (ShowType == ShowType.ShowFile)
                    {
                        fileFolderList.Items.Add(item);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (fileFolderList.SelectedIndices.Count == 0 && 
                ShowType == ShowType.ShowFile)
            {
                MessageBox.Show(
                    Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_file_browser_err_must_select_valid_file"), 
                    Localization.LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameUI, "ui_error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }
            if (ShowType == ShowType.ShowFile)
            {
                currentRelativePath += "/" + 
                    fileFolderList.Items[fileFolderList.SelectedIndices[0]].Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void fileFolderList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (fileFolderList.SelectedIndices.Count != 0)
            {
                var fileFolderName = fileFolderList.Items[fileFolderList.SelectedIndices[0]].Text;
                var extension = Path.GetExtension(fileFolderName);
                if (string.IsNullOrEmpty(extension))
                {
                    currentFullPath += "\\" + fileFolderName;
                    currentRelativePath += "/" + fileFolderName;
                    pathStack.Push(currentFullPath);
                    btnUp.Enabled = true;
                    RefreshFileFolder();
                }
            }
        }

        private void fileFolderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileFolderList.SelectedIndices.Count == 0)
            {
                return;
            }

            switch(ShowType)
            {
                case ShowType.ShowFile:
                    var extension = Path.GetExtension(fileFolderList.Items[fileFolderList.SelectedIndices[0]].Text);
                    if (string.IsNullOrEmpty(extension))
                    {
                        txtFileFolderName.Text = null;
                    }
                    else
                    {
                        txtFileFolderName.Text = fileFolderList.Items[fileFolderList.SelectedIndices[0]].Text;
                    }
                    break;
                case ShowType.ShowFolder:
                    txtFileFolderName.Text = fileFolderList.Items[fileFolderList.SelectedIndices[0]].Text;
                    break;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            pathStack.Pop();
            currentFullPath = pathStack.Peek();
            RefreshFileFolder();
            if (pathStack.Count == 1)
            {
                btnUp.Enabled = false;
            }
        }
    }
}
