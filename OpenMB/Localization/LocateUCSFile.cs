using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Mogre;

namespace OpenMB.Localization
{
	public enum LocateFileStorageType
	{
		Engine,
		Default
	}
	public class LocateUCSFile : IDisposable
	{
		private const string PATH = @"./Locate/";
		private Dictionary<string, string> ucsKeyValue;
		private string fullPath;
		private LOCATE currentLocate;
		private bool disposed;
		private LocateFileStorageType storageType;

		public LocateUCSFile(string fullPath, LOCATE currentLocate, LocateFileStorageType storageType)
		{
			this.storageType = storageType;
			this.fullPath = fullPath;
			this.currentLocate = currentLocate;
			ucsKeyValue = new Dictionary<string, string>();
			if (storageType == LocateFileStorageType.Engine)
			{
				this.fullPath = string.Format("{0}{1}/{2}", PATH, currentLocate.ToString(), this.fullPath);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			if (disposing)
			{
			}
			disposed = true;
		}

		public void Prepare()
		{
			ucsKeyValue.Clear();
		}

		public bool Process()
		{
			if (File.Exists(fullPath))
			{
				using (StreamReader sr = new StreamReader(fullPath))
				{
					while (sr.Peek() >= 0 && !sr.EndOfStream)
					{
						try
						{
							string line = sr.ReadLine();
							string[] outputTmp = Regex.Split(line, "\t");
							if (!ucsKeyValue.ContainsKey(outputTmp[0]))
							{
								ucsKeyValue.Add(outputTmp[0], outputTmp[1]);
							}
						}
						catch
						{
							continue;
						}
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public string SeekValueByKey(string ID)
		{
			return ucsKeyValue.ContainsKey(ID) ? ucsKeyValue[ID] : string.Empty;
		}

		public string SeekKeyByValue(string value)
		{
			string key = string.Empty;
			if (ucsKeyValue.ContainsValue(value))
			{
				foreach (KeyValuePair<string, string> kpl in ucsKeyValue)
				{
					if (kpl.Value == value)
					{
						key = kpl.Key;
						break;
					}
				}
			}
			return key;
		}

		private bool SeekLocalizedString(string str)
		{
			string localizedKey = null;
			if (ucsKeyValue.ContainsKey(localizedKey))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public string AddNewKeyString(string str)
		{
			if (!SeekLocalizedString(str))
			{
				IEnumerable<string> keys = ucsKeyValue.Keys;
				string lastKey = keys.ElementAt(keys.Count() - 1);
				int Index = int.Parse(lastKey);
				Index = Index + 1;
				ucsKeyValue.Add(Index.ToString(), str);
				return Index.ToString();
			}
			else
			{
				return null;
			}
		}

		public void Save()
		{
			using (StreamWriter sw = new StreamWriter(fullPath))
			{
				foreach (KeyValuePair<string, string> kpl in ucsKeyValue)
				{
					string line = string.Format("{0}\t{1}", kpl.Key, kpl.Value);
					sw.WriteLine(line);
				}
				sw.Flush();
			}
		}

		public string AddQuickString(string str)
		{
			if (!ucsKeyValue.ContainsValue(str))
			{
				string prefix = null;
				prefix = str.Replace(' ', '_').ToLower();
				string key = "qstr_" + prefix;
				if (!ucsKeyValue.ContainsKey(key))
				{
					ucsKeyValue.Add(key, str);
				}
				else
				{
					ucsKeyValue[key] = str;
				}

				return key;
			}
			else
			{
				string key = null;
				foreach (var kpl in ucsKeyValue)
				{
					if (kpl.Value == str)
					{
						key = kpl.Key;
						break;
					}
				}
				return key;
			}
		}
	}
}
