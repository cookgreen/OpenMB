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
        private Stream currentStream;
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
                currentStream = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                currentFilePath = dialog.FileName;
                currentFile = new KBF();
                currentFile.Write(currentStream);

                Text = Text + " - " + dialog.FileName;

                mnuImportMesh.Enabled = true;
                mnuImportMaterialScript.Enabled = true;
                mnuImportTexture.Enabled = true;
                mnuSaveFile.Enabled = true;
            }
        }

        private void mnuImportMesh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ogre Mesh|*.mesh";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(dialog.FileName);
                KBFEntry entry = new KBFEntry(dialog.SafeFileName, "mesh", bytes);
                currentFile.AddMeshEntry(entry);
            }
        }

        private void mnuImportMaterialScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ogre Material|*.material";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(dialog.FileName);
                KBFEntry entry = new KBFEntry(dialog.SafeFileName, "material", bytes);
                currentFile.AddMaterialEntry(entry);
            }
        }

        private void mnuImportTexture_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Texture File|*.jpg;*.png;*.tga;*.dds";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(dialog.FileName);
                KBFEntry entry = new KBFEntry(dialog.SafeFileName, "texture", bytes);
                currentFile.AddTextureEntry(entry);
            }
        }

        private void mnuImportSkeleton_Click(object sender, EventArgs e)
        {

        }

        private void mnuSaveFile_Click(object sender, EventArgs e)
        {
            currentFile.Write(currentStream);
        }
    }
}
