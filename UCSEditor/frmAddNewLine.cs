using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UCSEditor
{
    public partial class frmAddNewLine : Form
    {
        private UCSLine line;
        public UCSLine Line
        {
            get { return line; }
        }
        public frmAddNewLine()
        {
            InitializeComponent();
            line = new UCSLine();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtOriginalText.Text))
            {
                MessageBox.Show("Please input valid data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
