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

				tpGraphic.Text = "ui_graphic".ToUILocalizedString();
				tpResource.Text = "ui_resource".ToUILocalizedString();
				tpAudio.Text = "ui_audio".ToUILocalizedString();
				tpGame.Text = "ui_game".ToUILocalizedString();

				lblRenderSys.Text = "ui_rendersystem".ToUILocalizedString();
				lblCOO.Text = "ui_click_on_options".ToUILocalizedString();
				lblLang.Text = "ui_language".ToUILocalizedString();

				chkEnableSound.Text = "ui_enable_sound".ToUILocalizedString();
				chkEnableMusic.Text = "ui_enable_music".ToUILocalizedString();

				groupFileSystem.Text = "ui_filesystem".ToUILocalizedString();
				groupZip.Text = "ui_zip".ToUILocalizedString();
				gbLocalization.Text = "ui_localization".ToUILocalizedString();
				gbRenderOpt.Text = "ui_render_options".ToUILocalizedString();
				gbMusicSound.Text = "ui_music_sound".ToUILocalizedString();
				gbAdvanced.Text = "ui_advanced_options".ToUILocalizedString();

				btnAddFileSystemResourceLoc.Text = "ui_add".ToUILocalizedString();
				btnModifyFileSystemResourceLoc.Text = "ui_modify".ToUILocalizedString();
				btnDeleteFileSystemResourceLoc.Text = "ui_delete".ToUILocalizedString();
				btnAddZipResourceLoc.Text = "ui_add".ToUILocalizedString();
				btnModifyZipResourceLoc.Text = "ui_modify".ToUILocalizedString();
				btnDeleteZipResourceLoc.Text = "ui_delete".ToUILocalizedString();
				btnOK.Text = "ui_ok".ToUILocalizedString();
				btnCancel.Text = "ui_cancel".ToUILocalizedString();

				chkEnableEditMode.Text = "ui_is_enable_edit_mode".ToUILocalizedString();
				chkEnableCheatMode.Text = "ui_is_enable_cheat_mode".ToUILocalizedString();
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
