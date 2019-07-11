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
            LocateSystem.Singleton.InitLocateSystem(controller.CurrentLoacte);// Init Locate System
            controller.InitLocates();

            if (controller.CurrentLoacte != LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = LocateSystem.Singleton.CovertLocateInfoToIndex(controller.CurrentLoacte);
                
                tbRenderOpt.TabPages[0].Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_graphic");
                tbRenderOpt.TabPages[1].Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_audio");
                tbRenderOpt.TabPages[2].Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_game");
            
                lblRenderSys.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_rendersystem");
                lblCOO.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_click_on_options");
                lblLang.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_language");

                chkEnableSound.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_enable_sound");
                chkEnableMusic.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_enable_music");

                gbLocalization.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_localization");
                gbRenderOpt.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_render_options");
                gbMusicSound.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_music_sound");
                gbAdvanced.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_advanced_options");
            
                btnOK.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_ok");
                btnCancel.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_cancel");

                chkEnableEditMode.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_is_enable_edit_mode");
                chkEnableCheatMode.Text = LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameUI, "ui_is_enable_cheat_mode");
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
    }
}
