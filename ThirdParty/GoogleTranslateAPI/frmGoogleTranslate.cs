using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.google.translate.api
{
    public partial class frmGoogleTranslate : Form
    {
        private GoogleTranslateAPIRequest req;
        private Dictionary<string, string> translateLanguages;
        private string translatedText;
        public string TranslatedText
        {
            get { return translatedText; }
        }

        public frmGoogleTranslate(Dictionary<string,string> translateLanguages, string originalText = null)
        {
            InitializeComponent();

            req = new GoogleTranslateAPIRequest("<unknown>");
            req.TranslateFinished += TranslateFinished;

            this.translateLanguages = translateLanguages;
            foreach(var language in translateLanguages)
            {
                cmbLanguages.Items.Add(language);
            }
            cmbLanguages.DisplayMember = "Value";

            txtInputText.Text = originalText;
        }

        private void TranslateFinished(bool unused, string translatedText, object userData)
        {
            txtTranslatedText.Text = translatedText;
            this.translatedText = translatedText;
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            req.TranslateAsync(txtInputText.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtTranslatedText.Clear();
        }

        private void cmbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            req.DestLangID = ((KeyValuePair<string, string>)cmbLanguages.SelectedItem).Key;
            btnTranslate.Enabled = true;
        }
    }
}
