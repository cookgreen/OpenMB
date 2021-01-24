using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBFEditor.FileFormat
{
    /// <summary>
    /// Kingdom & Knight Binary Resource File Format
    /// </summary>
    public class KBF
    {
        private List<KBFEntry> meshEntries;
        private List<KBFEntry> materialEntries;
        private List<KBFEntry> textureEntries;

        public List<KBFEntry> MeshEntries
        {
            get { return meshEntries; }
        }
        public List<KBFEntry> MatEntries
        {
            get { return materialEntries; }
        }
        public List<KBFEntry> TexEntries
        {
            get { return textureEntries; }
        }

        public KBF()
        {
            meshEntries = new List<KBFEntry>();
            materialEntries = new List<KBFEntry>();
            textureEntries = new List<KBFEntry>();
        }

        public void AddMeshEntry(KBFEntry entry)
        {
            meshEntries.Add(entry);
        }

        public void AddMaterialEntry(KBFEntry entry)
        {
            materialEntries.Add(entry);
        }

        public void AddTextureEntry(KBFEntry entry)
        {
            textureEntries.Add(entry);
        }

        public void Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            reader.ReadBytes(8);
            while (reader.PeekChar() != -1)
            {
                byte b = reader.ReadByte();
                byte[] bytes = reader.ReadBytes(b);
                if (Encoding.UTF8.GetString(bytes) == "end")
                {
                    break;
                }
                else
                {
                    string name = Encoding.UTF8.GetString(bytes);

                    b = reader.ReadByte();
                    bytes = reader.ReadBytes(b);
                    string type = Encoding.UTF8.GetString(bytes);

                    b = reader.ReadByte();
                    bytes = reader.ReadBytes(b);

                    KBFEntry entry = new KBFEntry(name, type, bytes);
                    if (type == "mesh")
                    {
                        meshEntries.Add(entry);
                    }
                    else if (type == "material")
                    {
                        materialEntries.Add(entry);
                    }
                    else if (type == "texture")
                    {
                        textureEntries.Add(entry);
                    }
                }
            }
        }

        public void Write(Stream stream)
        {
            WriteHeader(stream);

            if (meshEntries.Count > 0)
            {
                //WriteString("mesh_start", stream);
                foreach (var entry in meshEntries)
                {
                    entry.Write(stream);
                }
                //WriteString("mesh_end", stream);
            }

            if (materialEntries.Count > 0)
            {
                //WriteString("mat_start", stream);
                foreach (var entry in materialEntries)
                {
                    entry.Write(stream);
                }
                //WriteString("mat_end", stream);
            }

            if (textureEntries.Count > 0)
            {
                //WriteString("tex_start", stream);
                foreach (var entry in textureEntries)
                {
                    entry.Write(stream);
                }
                //WriteString("tex_end", stream);
            }


            WriteEnd(stream);
            stream.Close();
        }

        private void WriteString(string str, Stream stream)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteHeader(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] bytes = Encoding.UTF8.GetBytes("KBF v1.0");
            writer.Write(bytes);
        }

        private void WriteEnd(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] bytes = Encoding.UTF8.GetBytes("end");
            writer.Write(bytes);
        }
    }

    public class KBFEntry
    {
        private string name;
        private string type;
        private byte[] data;

        public string Name
        {
            get { return name; }
        }
        public byte[] Data
        {
            get { return data; }
        }

        public KBFEntry(string name, string type, byte[] data)
        {
            this.name = name;
            this.type = type;
            this.data = data;
        }

        public byte[] Read()
        {
            return data;
        }

        public void Write(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);

            byte[] bytes = Encoding.UTF8.GetBytes(name);
            writer.Write(bytes.Length);
            writer.Write(bytes);

            bytes = Encoding.UTF8.GetBytes(type);
            writer.Write(bytes.Length);
            writer.Write(bytes);

            writer.Write(data.Length);
            writer.Write(data);
        }
    }
}
