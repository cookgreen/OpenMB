using KBFEditor.FileFormat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBFEditor
{
    public partial class frmMain : Form
    {
        private KBF currentFile;
        private string currentFilePath;

        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "K&K Binary Resource File|*.kbf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var stream = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                currentFilePath = dialog.FileName;
                currentFile = new KBF();
                currentFile.Write(stream);
            }
        }
    }
}
