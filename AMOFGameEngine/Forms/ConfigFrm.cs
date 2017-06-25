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
using AMOFGameEngine.Localization;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine.Dialogs
{
    public partial class ConfigFrm : Form
    {
        Root r=new Root();

        LOCATE selectedlocate;
        LocateSystem ls = new LocateSystem();
        List<OgreConfigNode> ogreConfigs = new List<OgreConfigNode>();
        List<OgreConfigNode> gameCfgs=new List<OgreConfigNode>(0);
        OgreConfigFileAdapter cfa = new OgreConfigFileAdapter("./ogre.cfg");
        OgreConfigFileAdapter gameCfa = new OgreConfigFileAdapter("./Game.cfg");
        OgreConfigNode defaultRSConfig = new OgreConfigNode();
        bool isEnableMusic;
        bool isEnableSound;
        Dictionary<string, string> GameConfigOptions;

        public ConfigFrm()
        {
            InitializeComponent();
            GameConfigOptions = new Dictionary<string, string>();
        }
        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            ogreConfigs = cfa.ReadConfigData();
            gameCfgs = gameCfa .ReadConfigData();

            foreach (OgreConfigNode node in ogreConfigs)
            {
                if (!string.IsNullOrEmpty(node.Section))
                {
                    cmbSubRenderSys.Items.Add(node.Section);
                }
            }

            for (int i = 0; i < gameCfgs.Count;i++ )
            {
                if (gameCfgs[i].Settings.Count > 0)
                {
                    switch (gameCfgs[i].Section)
                    {
                        case "Audio":
                            foreach (KeyValuePair<string, string> kpl in gameCfgs[i].Settings)
                            {
                                if (kpl.Key == "EnableSound")
                                {
                                    if (kpl.Value == "1")
                                    {
                                        isEnableSound = true;
                                        chkEnableSound.Checked = true;
                                    }
                                    else if (kpl.Value == "0")
                                    {
                                        isEnableSound = false;
                                        chkEnableSound.Checked = false;
                                    }
                                }
                                if (kpl.Key == "EnableMusic")
                                {
                                    if (kpl.Value == "1")
                                    {
                                        isEnableMusic = true;
                                        chkEnableMusic .Checked = true;
                                    }
                                    else if (kpl.Value == "0")
                                    {
                                        isEnableMusic = false;
                                        chkEnableMusic.Checked = false;
                                    }
                                }
                            }

                            break;
                        case "Localized":
                            selectedlocate = ls.ConvertLocateShortStringToLocateInfo(gameCfgs[i].Settings["Current"]);

                            break;
                    }
                }
            }
            if (selectedlocate != LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = ls.CovertLocateInfoToIndex(selectedlocate);

                ls.InitLocateSystem(selectedlocate);// Init Locate System
                ls.IsInit = true;

                tbRenderOpt.TabPages[0].Text = ls.LOC(LocateFileType.GameUI, "Graphic");
                tbRenderOpt.TabPages[1].Text = ls.LOC(LocateFileType.GameUI, "Audio");
                tbRenderOpt.TabPages[2].Text = ls.LOC(LocateFileType.GameUI, "Game");

                lblRenderSys.Text = ls.LOC(LocateFileType.GameUI, "Render SubSystem");
                lblCOO.Text = ls.LOC(LocateFileType.GameUI, "Click On Options");
                lblLang.Text = ls.LOC(LocateFileType.GameUI, "Language");
                gbRenderOpt.Text = ls.LOC(LocateFileType.GameUI, "Render System Options");
            }

            string defaultRenderSystem = cfa.GetDefaultRenderSystem();
            if (!string.IsNullOrEmpty(defaultRenderSystem))
            {
                cmbSubRenderSys.SelectedItem = defaultRenderSystem;
            }
        }

        private void cmbSubRenderSys_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            defaultRSConfig.Section = "";
            Dictionary<string, string> defaultRSSetting = new Dictionary<string, string>();
            defaultRSSetting.Add("Render System", cmbSubRenderSys.SelectedItem.ToString());
            defaultRSConfig.Settings = defaultRSSetting;
            InsetSettingsByIndex();
        }

        private void InsetSettingsByIndex()
        {
            lstConfig.Items.Clear();
            string selectedSubRenderSys=cmbSubRenderSys.SelectedItem.ToString();
            IEnumerable<OgreConfigNode> filterNode = ogreConfigs.Where(o=>o.Section==selectedSubRenderSys);

            foreach( KeyValuePair<string,string> kpl in filterNode.First().Settings )
            {
                string singleSetting = kpl.Key + ":" + kpl.Value;
                lstConfig.Items.Add(singleSetting);
            }
        }

        private void lstConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbValueChange.Enabled = true;
            InsertAvaliableValueByIndex(cmbSubRenderSys.SelectedItem.ToString());
        }

        private void InsertAvaliableValueByIndex(string secName)
        {
            cmbValueChange.Items.Clear();

            string[] tempStrs = lstConfig.SelectedItem.ToString().Split(':');
            ConfigOptionMap configOptionMap=r.GetRenderSystemByName(secName).GetConfigOptions();

            IEnumerable<OgreConfigNode> filterNodes = ogreConfigs.Where(o => o.Section == secName);
            OgreConfigNode currentNode = filterNodes.First();
            Dictionary<string, string> currentSettings = currentNode.Settings;
            Dictionary<string, string>.KeyCollection keys=currentSettings.Keys;
            IEnumerable<string> selectedKey=  keys.Where(o => o == tempStrs[0]);
            string currentKey = selectedKey.First();

            foreach (string psv in configOptionMap[currentKey].possibleValues)
            {
                cmbValueChange.Items.Add(psv);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CheckOptionsAndRun();
        }

        private void CheckOptionsAndRun()
        {
            GameConfigOptions.Add("IsEnableMusic", isEnableMusic.ToString());
            GameConfigOptions.Add("IsEnableSound", isEnableSound.ToString());
            GameConfigOptions.Add("Language", cmbLanguageSelect.SelectedItem.ToString());

            gameCfgs.Where(o => o.Section == "Audio").FirstOrDefault().Settings["EnableSound"] = chkEnableSound.Checked ? "1" : "0";
            gameCfgs.Where(o => o.Section == "Audio").FirstOrDefault().Settings["EnableMusic"] = chkEnableMusic.Checked ? "1" : "0";
            gameCfgs.Where(o => o.Section == "Localized").FirstOrDefault().Settings["Current"] = ls.CovertReadableStringToLocateShortString(cmbLanguageSelect.SelectedItem.ToString());

            //ls.SaveLanguageSettingsToFIle(cmbLanguageSelect.SelectedIndex);
            cfa.SaveConfig(ogreConfigs, cmbSubRenderSys.SelectedItem.ToString());
            gameCfa.SaveConfig(gameCfgs);
            this.Close();

            GameApp app = new GameApp(GameConfigOptions, ls, ogreConfigs, r);
            app.Run();
        }

        private void cmbValueChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValueToListBox(cmbSubRenderSys.SelectedItem.ToString());
        }
        private void UpdateValueToListBox(string secName)
        {
            try
            {
                OgreConfigNode configNode = ogreConfigs.Where(o => o.Section == secName).First();
                Dictionary<string, string> settings = configNode.Settings;
                KeyValuePair<string, string> pi = settings.ElementAt(lstConfig.SelectedIndex);
                string[] tempStrs = lstConfig.SelectedItem.ToString().Split(':');
                settings[tempStrs[0]]=cmbValueChange.SelectedItem.ToString();
                lstConfig.Items.Clear();

                foreach (KeyValuePair<string, string> kpl in settings)
                {
                    string singleSetting = kpl.Key + ":" + kpl.Value;
                    lstConfig.Items.Add(singleSetting);
                }
                OgreConfigNode newConfigNode = new OgreConfigNode();
                newConfigNode.Section = configNode.Section;
                newConfigNode.Settings = settings;
                int indexDeleted=ogreConfigs.IndexOf(configNode);
                ogreConfigs.Remove(configNode);
                ogreConfigs.Insert(indexDeleted, newConfigNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        private void cmbLanguageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void chkEnableMusic_CheckedChanged(object sender, EventArgs e)
        {
            isEnableMusic = chkEnableMusic.Checked;
        }

        private void chkEnableSound_CheckedChanged(object sender, EventArgs e)
        {
            isEnableSound = chkEnableSound.Checked;
        }
        

    }
}
