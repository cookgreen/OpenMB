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
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        Root r=new Root();
        List<OgreConfigNode> OgreConfigs = new List<OgreConfigNode>();
        List<NameValuePairList> pl = new List<NameValuePairList>();
        NameValuePairList paramTemp;
        private OgreConfigNode OgreConfig;
        OgreConfigFileAdapter cfa = new OgreConfigFileAdapter("./ogre.cfg");

        string path = "./language.txt";
        public ConfigFrm()
        {
            InitializeComponent();
        }
        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            LOCATE selectedlocate = LocateSystem.getLanguageFromFile();
            if (selectedlocate != LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = LocateSystem.CovertLocateInfoToIndex(selectedlocate);

                LocateSystem.InitLocateSystem(selectedlocate);// Init Locate System
                LocateSystem.IsInit = true;

                tbRenderOpt.TabPages[0].Text = LocateSystem.CreateLocateString("22161220");
                tbRenderOpt.TabPages[1].Text = LocateSystem.CreateLocateString("22161226");
                tbRenderOpt.TabPages[2].Text = LocateSystem.CreateLocateString("22161224");

                lblRenderSys.Text = LocateSystem.CreateLocateString("22161221");
                lblCOO.Text = LocateSystem.CreateLocateString("22161223");
                lblLang.Text = LocateSystem.CreateLocateString("22161225");
                gbRenderOpt.Text = LocateSystem.CreateLocateString("22161222");
            }

            string secName;
            ConfigFile cf = new ConfigFile();

            cf.Load("ogre.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                if (!string.IsNullOrEmpty(secName))
                {
                    cmbSubRenderSys.Items.Add(secName);

                    ConfigFile.SettingsMultiMap settings = seci.Current; ;
                    OgreConfigNode s=new OgreConfigNode();
                    s.settings = settings;
                    s.section = secName;
                    OgreConfigs.Add(s);
                    paramTemp = new NameValuePairList();
                    foreach (KeyValuePair<string,string> pv in settings)
                    {
                        paramTemp[pv.Key] = pv.Value;
                    }
                    pl.Add(paramTemp);
                }
            }

            StringBuilder renderSystem = new StringBuilder(500);
            GetPrivateProfileString("Direct3D9 Rendering Subsystem", "Full Screen", "", renderSystem, 100, @".\ogre.cfg");
            string defaultRS = cfa.getDefaultRS();
            if (!string.IsNullOrEmpty(defaultRS))
            {
                cmbSubRenderSys.SelectedItem = defaultRS;
            }

        }

        private void cmbSubRenderSys_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            InsetSettingsByIndex(cmbSubRenderSys.SelectedIndex);
        }

        private void InsetSettingsByIndex(int index)
        {
            OgreConfig = OgreConfigs[index];
            ConfigFile.SettingsMultiMap p = OgreConfig.settings;
            lstConfigOpt.Items.Clear();
            foreach(KeyValuePair<string,string> ps in p)
            {
                pl[index][ps.Key] = ps.Value;
                lstConfigOpt.Items.Add(ps.Key+":"+ps.Value);
            }
            
        }

        private void lstConfigOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbValueChange.Enabled = true;
            InsertAvaliableValueByIndex(lstConfigOpt.SelectedIndex,OgreConfig.section);
        }

        private void InsertAvaliableValueByIndex(int KeyIndex,string secName)
        {
            cmbValueChange.Items.Clear();
            string[] tempStrs = lstConfigOpt.SelectedItem.ToString().Split(':');
            ConfigOptionMap configOptionMap=r.GetRenderSystemByName(secName).GetConfigOptions();
            OgreConfigNode sn = OgreConfigs[cmbSubRenderSys.SelectedIndex];
            ConfigFile.SettingsMultiMap p = sn.settings;
            KeyValuePair<string, string> pi = p.ElementAt(KeyIndex);
            string Key = pi.Key;
            switch(Key)
            {
                case "Allow NVPerfHUD":
                    foreach(string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "FSAA":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Floating-point mode":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Rendering Device":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Full Screen":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Resource Creation Policy":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "VSync":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "VSync Interval":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Video Mode":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "sRGB Gamma Conversion":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Colour Depth":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Display Frequency":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "RTT Preferred Mode":
                    foreach (string psv in configOptionMap[Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;

            }
            cmbValueChange.SelectedItem = tempStrs[1];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveLanguageSettingsToFIle();
            ApplyChange();
            this.Close();
            
            DemoApp app = new DemoApp();
            app.startDemo();
        }
        private void ApplyChange()
        {
            if (File.Exists("ogre.cfg"))
                File.Delete("ogre.cfg");
            OgreConfigFileAdapter cfa = new OgreConfigFileAdapter("ogre.cfg");
            for (int i = 0; i < OgreConfigs.Count;i++ )
            {
                OgreConfigFileAdapter.saveConfig(OgreConfigs[i],pl[i],cmbSubRenderSys.SelectedItem.ToString());
            }
        }

        private void cmbValueChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValueToConfigLst(cmbValueChange.SelectedIndex,OgreConfigs[cmbSubRenderSys.SelectedIndex].section);
        }
        private void UpdateValueToConfigLst(int ValueIndex, string secName)
        {
            OgreConfigNode sn = OgreConfigs.Where(o => o.section == secName).First();
            ConfigFile.SettingsMultiMap p = sn.settings;
            KeyValuePair<string, string> pi = p.ElementAt(lstConfigOpt.SelectedIndex);
            pl[cmbSubRenderSys.SelectedIndex][pi.Key] = cmbValueChange.SelectedItem.ToString();

            lstConfigOpt.Items.Clear();
            foreach(KeyValuePair<string,string> psb in p)
            {
                lstConfigOpt.Items.Add(psb.Key + ":" + pl[cmbSubRenderSys.SelectedIndex][psb.Key]);
            }
        }
        private LOCATE CovertIndexToLocateInfo(int index)
        {
            switch (index)
            {
                case 0:
                    return LOCATE.en;
                case 1:
                    return LOCATE.cns;
                case 2:
                    return LOCATE.cnt;
                case 3:
                    return LOCATE.de;
                case 4:
                    return LOCATE.fr;
                case 5:
                    return LOCATE.ja;
                default:
                    return LOCATE.en;
            }
        }
        private string CovertLocateInfoStringToReadableString(string locate)
        {
            switch (locate)
            {
                case "en":
                    return "English";
                case "cns":
                    return "Simple Chinese";
                case "cnt":
                    return "Traditional Chinese";
                case "de":
                    return "German";
                case "fr":
                    return "French";
                case "ja":
                    return "Japanese";
                default:
                    return "English";
            }
        }
        private void cmbLanguageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void SaveLanguageSettingsToFIle()
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.CreateText(path);
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    string tmpw;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string tmpr = sr.ReadLine();
                        tmpw = tmpr;
                        sr.Close();
                    }
                    if (CovertLocateInfoStringToReadableString(tmpw) != (string)cmbLanguageSelect.SelectedItem)
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.Begin);
                        sw.Write(CovertIndexToLocateInfo(cmbLanguageSelect.SelectedIndex));
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
