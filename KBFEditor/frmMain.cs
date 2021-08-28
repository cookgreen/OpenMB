using KBFEditor.FileFormat;
using KBFEditor.Loader;
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
        private KBFLoader loader;
        private KBF currentFile;
        private string originalText;

        public frmMain()
        {
            InitializeComponent();
            originalText = Text;
            loader = new KBFLoader();
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "K&K Binary Resource File|*.kbf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var stream = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                currentFile = new KBF(stream.Name);
                currentFile.Write(stream);
                stream.Close();

                Text = originalText + " - " + dialog.FileName;

                mnuImportMesh.Enabled = true;
                mnuImportMaterialScript.Enabled = true;
                mnuImportTexture.Enabled = true;
                mnuSaveFile.Enabled = true;
            }
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "K&K Binary Resource File|*.kbf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var stream = new FileStream(dialog.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                currentFile = loader.Read(stream);
                stream.Close();

                ReadFileContents();
            }
        }

        private void mnuImportMesh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ogre Mesh|*.mesh";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(dialog.FileName);

                if (!entryTypeTabControl.TabPages.ContainsKey("TabMesh"))
                {
                    entryTypeTabControl.TabPages.Add("TabMesh","Mesh");
                    var tabPage = entryTypeTabControl.TabPages["TabMesh"];
                    ListBox listBox = new ListBox();
                    tabPage.Controls.Clear();
                    tabPage.Controls.Add(listBox);
                    listBox.Dock = DockStyle.Fill;
                }
                TabPage meshTab = entryTypeTabControl.TabPages["TabMesh"];
                ((ListBox)meshTab.Controls[0]).Items.Add(dialog.SafeFileName);


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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ogre Skeleton File|*.skeleton";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = File.ReadAllBytes(dialog.FileName);
                KBFEntry entry = new KBFEntry(dialog.SafeFileName, "skeleton", bytes);
                currentFile.AddSkeletonEntry(entry);
            }
        }

        private void mnuSaveFile_Click(object sender, EventArgs e)
        {
            loader.Write(currentFile);
        }

        private void ReadFileContents()
        {
            if (currentFile != null)
            {
                entryTypeTabControl.TabPages.Clear();

                if (currentFile.MeshEntries.Count > 0)
                {
                    TabPage meshEntryTabPage = new TabPage();
                    meshEntryTabPage.Text = "Mesh";

                    ListBox meshListBox = new ListBox();
                    meshListBox.Dock = DockStyle.Fill;
                    meshListBox.SelectedIndexChanged += MeshListBox_SelectedIndexChanged;
                    meshEntryTabPage.Controls.Add(meshListBox);

                    foreach (var meshEntry in currentFile.MeshEntries)
                    {
                        meshListBox.Items.Add(meshEntry.Name);
                    }

                    entryTypeTabControl.TabPages.Add(meshEntryTabPage);
                }

                if (currentFile.MatEntries.Count > 0)
                {
                    TabPage matEntryTabPage = new TabPage();
                    matEntryTabPage.Text = "Material";

                    ListBox matListBox = new ListBox();
                    matListBox.Dock = DockStyle.Fill;
                    matListBox.SelectedIndexChanged += MatListBox_SelectedIndexChanged;
                    matEntryTabPage.Controls.Add(matListBox);

                    foreach (var matEntry in currentFile.MatEntries)
                    {
                        matListBox.Items.Add(matEntry.Name);
                    }

                    entryTypeTabControl.TabPages.Add(matEntryTabPage);
                }
            }
        }

        private void MatListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MeshListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Open Ogre Control and load the mesh file
        }
    }
}
