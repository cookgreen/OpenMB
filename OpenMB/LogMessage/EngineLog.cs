using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenMB.Core;

namespace OpenMB.LogMessage
{
    public class EngineLog : IDisposable
    {
        private int logId;
        private string logName;
        private Stream logStream;
        private StreamWriter sw;

        public Stream Stream
        {
            get { return logStream; }
        }
        public string Name
        {
            get { return logName; }
        }

        public EngineLog(string fileName,int id)
        {
            logId = id;
            logName = fileName;
            if(File.Exists(logName))
            {
                File.Delete(logName);
            }
            logStream = new FileStream(logName, FileMode.OpenOrCreate, FileAccess.Write);
            sw = new StreamWriter(logStream);
            InitLogHeader();
        }

        public void InitLogHeader()
        {
            string strokeLine = "-----------------------------" + Environment.NewLine;
            strokeLine += "     AMGE Version " + GameVersion.Current + Environment.NewLine;
            strokeLine += "-----------------------------" + Environment.NewLine;
            sw.WriteLine(strokeLine);
        }

        public void LogMessage(string message, LogType type = LogType.Infomation)
        {
            string logType = string.Empty;
            switch (type)
            {
                case LogType.Infomation:
                    logType = "[Info]";
                    break;
                case LogType.Warning:
                    logType = "[Warning]";
                    break;
                case LogType.Error:
                    logType = "[Error]";
                    break;
            }
            string logMessage = string.Format("{0} - {1}: {2}", DateTime.Now, logType, message);
            sw.WriteLine(logMessage);
            sw.Flush();
        }

        public void Dispose()
        {
            sw.Close();
        }
    }
}
