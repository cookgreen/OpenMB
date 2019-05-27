using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
    /// <summary>
    /// Store the Register Variable
    /// </summary>
    public class ScriptLinkTable
    {
        private List<ScriptLinkTableNode> records;
        public ScriptLinkTable()
        {
            records = new List<ScriptLinkTableNode>();
        }

        public void AddRecord(ScriptLinkTableNode record)
        {
            records.Add(record);
        }

        public void RemoveRecord(ScriptLinkTableNode record)
        {
            records.Remove(record);
        }

        public ScriptLinkTableNode GetRecord(string name)
        {
            return records.Find(o => o.Name == name);
        }
    }
}
