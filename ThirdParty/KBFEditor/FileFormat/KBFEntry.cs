using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBFEditor.FileFormat
{
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
