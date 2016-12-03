using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mogre;

namespace AMOFGameEngine
{
    public partial class ConfigFrm : Form
    {
        Root r=new Root();
        List<Settings> sl = new List<Settings>();
        List<NameValuePairList> pl = new List<NameValuePairList>();
        NameValuePairList paramTemp;
        private Settings s;

        string path = "./language.txt";
        public ConfigFrm()
        {
            InitializeComponent();
        }
        private void ConfigFrm_Load(object sender, EventArgs e)
        {
            Models.LOCATE selectedlocate = Models.LocateSystem.getLanguageFromFile();
            if (selectedlocate != Models.LOCATE.invalid)
            {
                cmbLanguageSelect.SelectedIndex = CovertLocateInfoToIndex(selectedlocate);

                Models.LocateSystem.InitLocateSystem(selectedlocate);// Init Locate System
                Models.LocateSystem.IsInit = true;

                tbRenderOpt.TabPages[0].Text = Models.LocateSystem.CreateLocateString("22161220");
                tbRenderOpt.TabPages[1].Text = Models.LocateSystem.CreateLocateString("22161226");
                tbRenderOpt.TabPages[2].Text = Models.LocateSystem.CreateLocateString("22161224");

                lblRenderSys.Text = Models.LocateSystem.CreateLocateString("22161221");
                lblCOO.Text = Models.LocateSystem.CreateLocateString("22161223");
                lblLang.Text = Models.LocateSystem.CreateLocateString("22161225");
                gbRenderOpt.Text = Models.LocateSystem.CreateLocateString("22161222");
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
                    Settings s;
                    s.settings = settings;
                    s.section = secName;
                    sl.Add(s);
                    paramTemp = new NameValuePairList();
                    foreach (KeyValuePair<string,string> pv in settings)
                    {
                        paramTemp[pv.Key] = pv.Value;
                    }
                    pl.Add(paramTemp);
                }
            }
        }

        private void cmbSubRenderSys_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            InsetSettingsByIndex(cmbSubRenderSys.SelectedIndex);
        }

        private void InsetSettingsByIndex(int index)
        {
            s = sl[index];
            ConfigFile.SettingsMultiMap p = s.settings;
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
            InsertAvaliableValueByIndex(lstConfigOpt.SelectedIndex,s.section);
        }

        private void InsertAvaliableValueByIndex(int KeyIndex,string secName)
        {
            cmbValueChange.Items.Clear();
            ConfigOptionMap com=r.GetRenderSystemByName(secName).GetConfigOptions();
            Settings sn = sl[cmbSubRenderSys.SelectedIndex];
            ConfigFile.SettingsMultiMap p = sn.settings;
            KeyValuePair<string, string> pi = p.ElementAt(KeyIndex);
            string Key = pi.Key;
            switch(Key)
            {
                case "Allow NVPerfHUD":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "FSAA":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Floating-point mode":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Rendering Device":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    cmbValueChange.DataSource = com[pi.Key].possibleValues;
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Full Screen":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                    break;
                case "Resource Creation Policy":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "VSync":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "VSync Interval":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Video Mode":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "sRGB Gamma Conversion":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Colour Depth":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "Display Frequency":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;
                case "RTT Preferred Mode":
                    cmbValueChange.Text = pl[cmbSubRenderSys.SelectedIndex][pi.Key];
                    foreach(string psv in com[pi.Key].possibleValues)
                    {
                        cmbValueChange.Items.Add(psv);
                    }
                    break;

            }
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
            NewConfigFile.filename = "ogre.cfg";
            for (int i = 0; i < sl.Count;i++ )
            {
                NewConfigFile.saveConfig(sl[i],pl[i],cmbSubRenderSys.SelectedItem.ToString());
            }
        }

        private void cmbValueChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateValueToConfigLst(cmbValueChange.SelectedIndex,sl[cmbSubRenderSys.SelectedIndex].section);
        }
        private void UpdateValueToConfigLst(int ValueIndex, string secName)
        {
            Settings sn = sl.Where(o => o.section == secName).First();
            ConfigFile.SettingsMultiMap p = sn.settings;
            KeyValuePair<string, string> pi = p.ElementAt(lstConfigOpt.SelectedIndex);
            pl[cmbSubRenderSys.SelectedIndex][pi.Key] = cmbValueChange.SelectedItem.ToString();

            lstConfigOpt.Items.Clear();
            foreach(KeyValuePair<string,string> psb in p)
            {
                lstConfigOpt.Items.Add(psb.Key + ":" + pl[cmbSubRenderSys.SelectedIndex][psb.Key]);
            }
        }
        private Models.LOCATE CovertIndexToLocateInfo(int index)
        {
            switch (index)
            {
                case 0:
                    return Models.LOCATE.en;
                case 1:
                    return Models.LOCATE.cns;
                case 2:
                    return Models.LOCATE.cnt;
                case 3:
                    return Models.LOCATE.de;
                case 4:
                    return Models.LOCATE.fr;
                case 5:
                    return Models.LOCATE.ja;
                default:
                    return Models.LOCATE.en;
            }
        }
        private int CovertLocateInfoToIndex(Models.LOCATE locate)
        {
            switch (locate)
            {
                case Models.LOCATE.en:
                    return 0;
                case Models.LOCATE.cns:
                    return 1;
                case Models.LOCATE.cnt:
                    return 2;
                case Models.LOCATE.de:
                    return 3;
                case Models.LOCATE.fr:
                    return 4;
                case Models.LOCATE.ja:
                    return 5;
                default:
                    return 0;
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
