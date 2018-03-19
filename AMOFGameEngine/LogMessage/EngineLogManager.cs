using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AMOFGameEngine.LogMessage
{
    public enum LogType
    {
        Infomation,
        Warning,
        Error
    }
    public class EngineLogManager : IDisposable
    {
        private EngineLog log;
        private List<EngineLog> logs;
        private static EngineLogManager instance;
        public static EngineLogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EngineLogManager();
                }
                return instance;
            }
        }

        public EngineLogManager()
        {
            logs = new List<EngineLog>();
            if(!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }
        }

        public EngineLog CreateLog(string name)
        {
            if (log != null)
            {
                log.Dispose();
            }
            log = new EngineLog(name, logs.Count);
            logs.Add(log);
            return log;
        }

        public void LogMessage(string message, LogType type = LogType.Infomation)
        {
            log.LogMessage(message, type);
        }

        public void Dispose()
        {
            foreach (var log in logs)
            {
                log.Dispose();
            }
            logs.Clear();
        }

        public EngineLog GetLog(string name)
        {
            var searchResult = from l in logs
                               where l.Name == name
                               select l;
            return searchResult.FirstOrDefault();
        }

        public void DestroyLog(string name)
        {
            EngineLog log = GetLog(name);
            if (log != null)
            {
                log.Dispose();
                logs.Remove(log);
            }
        }

        public void DestroyLog(EngineLog log)
        {
            if (log != null)
            {
                log.Dispose();
                logs.Remove(log);
            }
        }
    }
}
