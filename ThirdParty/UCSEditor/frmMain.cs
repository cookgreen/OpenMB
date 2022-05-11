using com.google.translate.api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UCSEditor
{
	public partial class frmMain : Form
	{
		private UCSFile ucs = null;
		private Dictionary<string, string> data = new Dictionary<string, string>();
		private ListViewItem lvi = null;
		private GoogleTranslateAPIRequest googleTransApiReq;
		private List<Tuple<UCSLine, ChangeOperation>> pendingChanges;
		private EditorSetting setting;

		public frmMain(EditorSetting setting)
		{
			InitializeComponent();

			this.setting = setting;
			googleTransApiReq = new GoogleTranslateAPIRequest("<unknown>");
            googleTransApiReq.TranslateFinished += GoogleTransApi_TranslateFinished;
			foreach(var kpl in setting.GoogleTranslateAPISetting.TranslateLanguages)
            {
				cmbGoogleTranslationAPILanguages.Items.Add(kpl.DisplayName);
            }
			if (cmbGoogleTranslationAPILanguages.Items.Count > 0)
			{
				cmbGoogleTranslationAPILanguages.SelectedIndex = 0;
			}
		}

        private void GoogleTransApi_TranslateFinished(bool unused, string translatedText, object unused2)
        {
			txtSuggestion.Text = translatedText;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Open";
			dialog.Filter = "UCS Locate File|*.ucs";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				if (pendingChanges != null && pendingChanges.Count > 0)
				{
					saveToolStripMenuItem_Click(null, null);
				}

				Text = "UCSEditor - " + dialog.FileName;

				ucs = new UCSFile(dialog.FileName);
				if (ucs.Process())
				{
					lsvLocateInfo.Items.Clear();

					saveToolStripMenuItem.Enabled = true;
					saveAsToolStripMenuItem.Enabled = true;
					addNewLineToolStripMenuItem.Enabled = true;

					foreach (var kpl in ucs.UCSData)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = kpl.Key;
						lvi.SubItems.Add(kpl.Value);
						lsvLocateInfo.Items.Add(lvi);
					}

					pendingChanges = new List<Tuple<UCSLine, ChangeOperation>>();
				}
			}
		}

		private void lsvLocateInfo_SelectedIndexChanged(object sender, EventArgs e)
		{
			txtKey.Text = "";
			txtLocalizedText.Text = "";
			if (lsvLocateInfo.SelectedItems.Count > 0)
			{
				deleteThisLineToolStripMenuItem.Enabled = true;
				if (lsvLocateInfo.SelectedItems[0].SubItems.Count > 1)
				{
					lvi = lsvLocateInfo.SelectedItems[0];
					txtKey.Text = lsvLocateInfo.SelectedItems[0].Text;
					txtLocalizedText.Text = lsvLocateInfo.SelectedItems[0].SubItems[1].Text;
					googleTransApiReq.TranslateAsync(txtLocalizedText.Text);
					txtSuggestion.Text = "Translating...";
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData();
			ucs.Save(data);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Title = "Save As";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				SaveData();
				ucs.Save(data);
			}
		}

		void SaveData()
		{
			data.Clear();
			foreach (ListViewItem lvi in lsvLocateInfo.Items)
			{
				data.Add(lvi.Text, lvi.SubItems[1].Text);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void addNewLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmAddNewLine addNewLineForm = new frmAddNewLine();
			if (addNewLineForm.ShowDialog() == DialogResult.OK)
			{
				ListViewItem item = new ListViewItem();
				item.Text = addNewLineForm.Line.ID;
				item.SubItems.Add(addNewLineForm.Line.Text);
				lsvLocateInfo.Items.Add(item);

				pendingChanges.Add(new Tuple<UCSLine, ChangeOperation>(addNewLineForm.Line, ChangeOperation.Add));
			}
		}

		private void deleteThisLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete this line?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				UCSLine line = new UCSLine();
				line.ID = lsvLocateInfo.SelectedItems[0].SubItems[0].Text;
				line.Text = lsvLocateInfo.SelectedItems[0].SubItems[1].Text;
				pendingChanges.Add(new Tuple<UCSLine, ChangeOperation>(line, ChangeOperation.Delete));

				lsvLocateInfo.Items.Remove(lsvLocateInfo.SelectedItems[0]);
				deleteThisLineToolStripMenuItem.Enabled = false;
			}
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			lsvLocateInfo.Columns[0].Width = this.Width / 2;
			lsvLocateInfo.Columns[1].Width = this.Width / 2;
		}

		private void txtLocalizedText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (lvi != null)
				{
					UCSLine line = new UCSLine();
					line.ID = txtKey.Text;
					line.Text = txtLocalizedText.Text;
					pendingChanges.Add(new Tuple<UCSLine, ChangeOperation>(line, ChangeOperation.Edit));

					lvi.Text = txtKey.Text;
					lvi.SubItems[1].Text = txtLocalizedText.Text;

					int index = lsvLocateInfo.Items.IndexOf(lvi);
					lsvLocateInfo.Items.RemoveAt(index);
					lsvLocateInfo.Items.Insert(index, lvi);

					txtKey.Text = "";
					txtLocalizedText.Text = "";
				}
			}
		}

		private void txtKey_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (lvi != null)
				{
					UCSLine line = new UCSLine();
					line.ID = txtKey.Text;
					line.Text = txtLocalizedText.Text;
					pendingChanges.Add(new Tuple<UCSLine, ChangeOperation>(line, ChangeOperation.Edit));

					lvi.Text = txtKey.Text;
					lvi.SubItems[1].Text = txtLocalizedText.Text;

					int index = lsvLocateInfo.Items.IndexOf(lvi);
					lsvLocateInfo.Items.RemoveAt(index);
					lsvLocateInfo.Items.Insert(index, lvi);

					txtKey.Text = "";
					txtLocalizedText.Text = "";
				}
			}
		}

		private void aboutUCSEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmAbout about = new frmAbout();
			about.ShowDialog();
		}

        private void cmbGoogleTranslationAPILanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
			googleTransApiReq.DestLangID = setting.GoogleTranslateAPISetting[cmbGoogleTranslationAPILanguages.SelectedItem.ToString()];
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
			if (pendingChanges != null && pendingChanges.Count > 0)
			{
				if (MessageBox.Show("Do you want to save these changes?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					saveAsToolStripMenuItem_Click(null, null);
				}
			}
        }
    }
}
