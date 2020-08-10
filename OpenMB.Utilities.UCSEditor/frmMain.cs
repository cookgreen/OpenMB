using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMB.Utilities.LocateFileEditor
{
	public partial class frmMain : Form
	{
		UCSFile ucs = null;
		Dictionary<string, string> data = new Dictionary<string, string>();
		ListViewItem lvi = null;

		public frmMain()
		{
			InitializeComponent();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ofd.Title = "Open";
			ofd.Filter = "";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				ucs = new UCSFile(ofd.FileName);
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
			sfd.Title = "Save As";
			if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
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
			lsvLocateInfo.Items.Add(new ListViewItem());
		}

		private void deleteThisLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lsvLocateInfo.Items.Remove(lsvLocateInfo.SelectedItems[0]);
			deleteThisLineToolStripMenuItem.Enabled = false;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			splitter1.SplitPosition = this.Width / 2;
			lsvLocateInfo.Columns[0].Width = this.Width / 2;
			lsvLocateInfo.Columns[1].Width = this.Width / 2;
		}

		private void txtLocalizedText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (lvi != null)
				{
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

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void aboutUCSEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmAbout about = new frmAbout();
			about.ShowDialog();
		}
	}
}
