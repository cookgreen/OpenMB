using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OpenMB.Mods.XML;
using System.Xml;

namespace OpenMB.Mods
{
	public class XmlObjectLoader
	{
		protected string modPath;
		protected XmlDocument doc;

		public XmlObjectLoader()
        {

        }

		public XmlObjectLoader(string path)
		{
			modPath = path;
		}

		public bool Load<T>(string xmlStr, out T xmlData)
		{
			try
			{
				XmlSerializer xr = new XmlSerializer(typeof(T));
				TextReader textReader = new StringReader(xmlStr);
				xmlData = (T)xr.Deserialize(textReader);
				textReader.Close();
				return true;
			}
			catch (Exception ex)
			{
				EngineManager.Instance.log.LogMessage(ex.ToString(), LogMessage.LogType.Error);
				xmlData = default(T);
				return false;
			}
		}

		public virtual bool Load<T>(out T ModXMLData)
		{
			try
			{
				XmlSerializer xr = new XmlSerializer(typeof(T));
				FileStream stream = new FileStream(modPath, FileMode.Open, FileAccess.Read);
				ModXMLData = (T)xr.Deserialize(stream);
				stream.Close();
				return true;
			}
			catch (Exception ex)
			{
				EngineManager.Instance.log.LogMessage(ex.ToString(), LogMessage.LogType.Error);
				ModXMLData = default(T);
				return false;
			}
		}

		public bool Save<T>(T xmlData)
		{
			try
			{
				XmlSerializer xr = new XmlSerializer(typeof(T));
				if (File.Exists(modPath))
				{
					File.Delete(modPath);
				}
				FileStream stream = new FileStream(modPath, FileMode.OpenOrCreate, FileAccess.Write);
				xr.Serialize(stream, xmlData);
				stream.Close();
				return true;
			}
			catch (Exception ex)
			{
				EngineManager.Instance.log.LogMessage(ex.ToString(), LogMessage.LogType.Error);
				return false;
			}
		}
	}
}
