using OpenMB.FileFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenMB.Utilties.MapExporter
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnChooseMBMapTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "M&B Map txt file|map.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtMBMapPath.Text = dialog.FileName;
            }
        }

        private void btnChooseMapXml_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Map Xml|*.xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtMapXmlPath.Text = dialog.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMapXmlPath.Text))
            {
                MessageBox.Show("You must choose a valid map xml full path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtMBMapPath.Text))
            {
                MessageBox.Show("You must choose a valid map txt full path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MBWorldMap worldmap = new MBWorldMap();
            worldmap.ParseTxt(txtMBMapPath.Text);
            worldmap.SaveAsXml(txtMapXmlPath.Text);
            MessageBox.Show("Export successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
