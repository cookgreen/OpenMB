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
        OgreConfigFileAdapter cfa = new OgreConfigFileAdapter("./ogre.cfg");
        OgreConfigNode defaultRSConfig = new OgreConfigNode();
        bool isEnableMusic;
        bool isEnableSound;
        Dictionary<string, string> GameConfigOptions;

        public ConfigFrm()
        {
            InitializeComponent();
            GameConfigOptions = new Dictionary<string, string>();
            isEnableSound = chkEnableSound.Checked;
            isEnableMusic = chkEnableMusic.Checked;
        }
        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            selectedlocate = ls.GetLanguageFromFile();
            if (selectedlocate != LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = ls.CovertLocateInfoToIndex(selectedlocate);

                ls.InitLocateSystem(selectedlocate);// Init Locate System
                ls.IsInit = true;

                tbRenderOpt.TabPages[0].Text = ls.CreateLocateString("22161220");
                tbRenderOpt.TabPages[1].Text = ls.CreateLocateString("22161226");
                tbRenderOpt.TabPages[2].Text = ls.CreateLocateString("22161224");

                lblRenderSys.Text = ls.CreateLocateString("22161221");
                lblCOO.Text = ls.CreateLocateString("22161223");
                lblLang.Text = ls.CreateLocateString("22161225");
                gbRenderOpt.Text = ls.CreateLocateString("22161222");
            }

            ogreConfigs = cfa.ReadConfigData();

            foreach (OgreConfigNode node in ogreConfigs)
            {
                if (!string.IsNullOrEmpty(node.Section))
                {
                    cmbSubRenderSys.Items.Add(node.Section);
                }
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

            ls.SaveLanguageSettingsToFIle(cmbLanguageSelect.SelectedIndex);
            cfa.SaveConfig(ogreConfigs, cmbSubRenderSys.SelectedItem.ToString());
            this.Close();

            GameApp app = new GameApp(GameConfigOptions);
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
