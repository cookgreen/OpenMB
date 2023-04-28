using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using com.google.translate.api;

namespace UCSEditor
{
    public partial class frmAddNewLine : Form
    {
        private EditorSetting setting;
        private UCSLine line;
        public UCSLine Line
        {
            get { return line; }
        }

        public frmAddNewLine(EditorSetting setting)
        {
            InitializeComponent();
            this.setting = setting;
            line = new UCSLine();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Please input string ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtOriginalText.Text))
			{
				MessageBox.Show("Please input string content!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

            line.ID = txtID.Text;
            line.Text = txtOriginalText.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPickFromGoogleTranslate_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> languages = new Dictionary<string, string>();
            var tempLanguages = setting.GoogleTranslateAPISetting.TranslateLanguages;
            foreach (var tempLang in tempLanguages)
            {
                languages.Add(tempLang.GoogleTransAPILocate, tempLang.DisplayName);
            }
            frmGoogleTranslate googleTranslateWin = new frmGoogleTranslate(languages, txtOriginalText.Text);
            googleTranslateWin.ShowDialog();
            txtOriginalText.Text = googleTranslateWin.TranslatedText;
        }
    }
}
