using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Mogre;
using OpenMB.Forms.Controller;
using OpenMB.Localization;
using OpenMB.Utilities;
using OpenMB.Core;

namespace OpenMB.Forms
{
    public partial class frmConfigure : Form
    {
        private string mod;
        private frmConfigureController controller;
        public frmConfigureController Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
                cmbSubRenderSys.DataSource = controller.GraphicConfig.RenderSystemNames;
                cmbSubRenderSys.DataBindings.Add("SelectedItem", controller.GraphicConfig, "RenderSystem");
                lstConfig.DataSource = controller.GraphicConfig.RenderParams;
                cmbValueChange.DataSource = controller.GraphicConfig.PossibleValues;
                cmbValueChange.DataBindings.Add("SelectedItem", controller.GraphicConfig, "CurrentPossibleValue");
                chkEnableMusic.DataBindings.Add("Checked", controller.AudioConfig, "IsEnableMusic");
                chkEnableSound.DataBindings.Add("Checked", controller.AudioConfig, "IsEnableSound");
                cmbLanguageSelect.DataSource = controller.GameConfig.AvaliableLocates;
                cmbLanguageSelect.DataBindings.Add("SelectedItem", controller.GameConfig, "CurrentSelectedLocate");
                chkEnableEditMode.DataBindings.Add("checked", controller.GameConfig, "IsEnableEditMode");
                resourceFileSystemList.DataSource = controller.ResourceConfig.FileSystemResources;
                resourceZipList.DataSource = controller.ResourceConfig.ZipResources;
            }
        }
        public frmConfigure(string mod = null)
        {
            InitializeComponent();
            this.mod = mod;
        }
        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            controller.Init();
            LocateSystem.Instance.InitLocateSystem(controller.CurrentLoacte);// Init Locate System
            controller.InitLocates();

            if (controller.CurrentLoacte != LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = LocateSystem.Instance.CovertLocateInfoToIndex(controller.CurrentLoacte);
                
                tpGraphic.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_graphic");
                tpResource.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_resource");
                tpAudio.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_audio");
                tpGame.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_game");
            
                lblRenderSys.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_rendersystem");
                lblCOO.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_click_on_options");
                lblLang.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_language");

                chkEnableSound.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_enable_sound");
                chkEnableMusic.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_enable_music");

                groupFileSystem.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_filesystem");
                groupZip.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_zip");
                gbLocalization.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_localization");
                gbRenderOpt.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_render_options");
                gbMusicSound.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_music_sound");
                gbAdvanced.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_advanced_options");
                
                btnAddFileSystemResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_add");
                btnModifyFileSystemResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_modify");
                btnDeleteFileSystemResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_delete");
                btnAddZipResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_add");
                btnModifyZipResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_modify");
                btnDeleteZipResourceLoc.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_delete");
                btnOK.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_ok");
                btnCancel.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_cancel");

                chkEnableEditMode.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_is_enable_edit_mode");
                chkEnableCheatMode.Text = LocateSystem.Instance.GetLocalizedString(LocateFileType.GameUI, "ui_is_enable_cheat_mode");
            }
        }

        private void cmbSubRenderSys_SelectedIndexChanged(object sender, EventArgs e)
        {
            controller.GetGraphicSettingsByName(cmbSubRenderSys.SelectedItem.ToString());
        }

        private void lstConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConfig.SelectedItem != null)
            {
                cmbValueChange.Enabled = true;
                controller.InsertPossibleValue(cmbSubRenderSys.SelectedItem.ToString(),
                                               lstConfig.SelectedItem.ToString().Split(':')[0],
                                               lstConfig.SelectedItem.ToString().Split(':')[1]);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Hide();
            GameConfigXml gameOptions = controller.SaveConfigure();
            GameApp app = new GameApp(gameOptions, mod);
            app.Run();
        }
        private void cmbValueChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbValueChange.SelectedItem != null && cmbValueChange.SelectedItem.ToString() != lstConfig.SelectedItem.ToString().Split(':')[1])
            {
                controller.UpdateGraphicConfigByValue(cmbSubRenderSys.SelectedItem.ToString(),
                                                      lstConfig.SelectedItem.ToString().Split(':')[0],
                                                      cmbValueChange.SelectedItem.ToString());
            }
        }

        private void frmConfigure_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void rendererPluginList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void resourceFileSystemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resourceFileSystemList.SelectedItem != null)
            {
                btnModifyFileSystemResourceLoc.Enabled = true;
                btnDeleteFileSystemResourceLoc.Enabled = true;
            }
            else
            {
                btnModifyFileSystemResourceLoc.Enabled = false;
                btnDeleteFileSystemResourceLoc.Enabled = false;
            }
        }

        private void resourceZipList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resourceZipList.SelectedItem != null)
            {
                btnModifyZipResourceLoc.Enabled = true;
                btnDeleteZipResourceLoc.Enabled = true;
            }
            else
            {
                btnModifyZipResourceLoc.Enabled = false;
                btnDeleteZipResourceLoc.Enabled = false;
            }
        }

        private void btnAddFileSystemResourceLoc_Click(object sender, EventArgs e)
        {
            frmRelativeFileFolderBrowser resourceLocEditor = new frmRelativeFileFolderBrowser();
            string rootDir = null;
            if (controller.ResourceConfig.ResourceRootDir.StartsWith(".") &&
                !controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
            {
                rootDir = controller.ResourceConfig.ResourceRootDir.Substring(1);
            }
            else
            {
                if (controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir.Substring(2);
                }
                else
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir;
                }
            }
            resourceLocEditor.InitFullPath = Environment.CurrentDirectory + "\\" + rootDir;
            resourceLocEditor.ShowType = ShowType.ShowFolder;
            if (resourceLocEditor.ShowDialog() == DialogResult.OK)
            {
                controller.ResourceConfig.FileSystemResources.Add(resourceLocEditor.RelativePath);
            }
        }

        private void btnModifyFileSystemResourceLoc_Click(object sender, EventArgs e)
        {
            frmRelativeFileFolderBrowser resourceLocEditor = new frmRelativeFileFolderBrowser();
            string rootDir = null;
            if (controller.ResourceConfig.ResourceRootDir.StartsWith(".") &&
                !controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
            {
                rootDir = controller.ResourceConfig.ResourceRootDir.Substring(1);
            }
            else
            {
                if (controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir.Substring(2);
                }
                else
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir;
                }
            }
            resourceLocEditor.ShowType = ShowType.ShowFolder;
            if (resourceLocEditor.ShowDialog() == DialogResult.OK)
            {
                controller.ResourceConfig.FileSystemResources.Add(resourceLocEditor.RelativePath);
            }
        }

        private void btnDeleteFileSystemResourceLoc_Click(object sender, EventArgs e)
        {

        }

        private void btnAddZipResourceLoc_Click(object sender, EventArgs e)
        {
            frmRelativeFileFolderBrowser resourceLocEditor = new frmRelativeFileFolderBrowser();
            string rootDir = null;
            if (controller.ResourceConfig.ResourceRootDir.StartsWith(".") &&
                !controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
            {
                rootDir = controller.ResourceConfig.ResourceRootDir.Substring(1);
            }
            else
            {
                if (controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir.Substring(2);
                }
                else
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir;
                }
            }
            resourceLocEditor.InitFullPath = Environment.CurrentDirectory + "\\" + rootDir;
            resourceLocEditor.ShowType = ShowType.ShowFile;
            resourceLocEditor.ShowDialog();
        }

        private void btnModifyZipResourceLoc_Click(object sender, EventArgs e)
        {
            frmRelativeFileFolderBrowser resourceLocEditor = new frmRelativeFileFolderBrowser();
            string rootDir = null;
            if (controller.ResourceConfig.ResourceRootDir.StartsWith(".") &&
                !controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
            {
                rootDir = controller.ResourceConfig.ResourceRootDir.Substring(1);
            }
            else
            {
                if (controller.ResourceConfig.ResourceRootDir.StartsWith(".."))
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir.Substring(2);
                }
                else
                {
                    rootDir = controller.ResourceConfig.ResourceRootDir;
                }
            }
            resourceLocEditor.InitFullPath = Environment.CurrentDirectory + "\\" + rootDir;
            resourceLocEditor.ShowType = ShowType.ShowFile;
            resourceLocEditor.ShowDialog();
        }

        private void btnDeleteZipResourceLoc_Click(object sender, EventArgs e)
        {

        }

        private void btnAutoDetectPlugins_Click(object sender, EventArgs e)
        {

        }
    }
}
