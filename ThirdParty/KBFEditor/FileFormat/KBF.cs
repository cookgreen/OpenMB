﻿using System;
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
        private const string KBF_HEADER = "KBF v1.0";

        private string fileName;
        private string fullFileName;
        private List<KBFEntry> meshEntries;
        private List<KBFEntry> materialEntries;
        private List<KBFEntry> textureEntries;
        private List<KBFEntry> skeletonEntries;

        public string FileName
        {
            get { return fileName; }
        }

        public string FullFileName
        {
            get { return fullFileName; }
        }

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
        public List<KBFEntry> SkeletonEntries
        {
            get { return skeletonEntries; }
        }

        public KBF(string fullFileName)
        {
            fileName = Path.GetFileName(fullFileName);
            this.fullFileName = fullFileName;

            meshEntries = new List<KBFEntry>();
            materialEntries = new List<KBFEntry>();
            textureEntries = new List<KBFEntry>();
            skeletonEntries = new List<KBFEntry>();
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

        public void AddSkeletonEntry(KBFEntry entry)
        {
            skeletonEntries.Add(entry);
        }

        public void Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            byte b = reader.ReadByte();
            byte[] bytes = reader.ReadBytes(b);

            string str = Encoding.UTF8.GetString(bytes);
            if (str != KBF_HEADER)
            {
                //Invalid File Format
                throw new InvalidKBFFormat("Invalid KBF File format!");
            }

            while (reader.PeekChar() != -1)
            {
                b = reader.ReadByte();
                bytes = reader.ReadBytes(b);
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
            stream.WriteByte((byte)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteHeader(Stream stream)
        {
            WriteString(KBF_HEADER, stream);
        }

        private void WriteEnd(Stream stream)
        {
            WriteString("end", stream);
        }
    }
}
