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
        private List<KBFEntry> entries;
        public List<KBFEntry> Entries
        {
            get { return entries; }
        }

        public KBF()
        {
            entries = new List<KBFEntry>();
        }

        public void AddEntry(KBFEntry entry)
        {
            entries.Add(entry);
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
                    entries.Add(entry);
                }
            }
        }

        public void Write(Stream stream)
        {
            WriteHeader(stream);
            foreach(var entry in entries)
            {
                entry.Write(stream);
            }
            WriteEnd(stream);
            stream.Close();
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
