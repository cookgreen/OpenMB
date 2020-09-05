using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public class ScriptValueStorageUnit
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }

        public ScriptValueStorageUnit(string name, object value, string type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
    public class ScriptValueStorage
    {
        private List<ScriptValueStorageUnit> storage;
        public ScriptValueStorageUnit this[string name]
        {
            get
            {
                var findedStorageUnit = storage.Where(o => o.Name == name);
                if (findedStorageUnit.Count() == 1)
                {
                    return findedStorageUnit.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public ScriptValueStorage()
        {
            storage = new List<ScriptValueStorageUnit>();
        }

        public void Append(string name, object value)
        {
            if (storage.Where(o => o.Name == name).Count() == 0)
            {
                ScriptValueStorageUnit storageUnit = new ScriptValueStorageUnit(
                    name, 
                    value, 
                    value.GetType().FullName);
                storage.Add(storageUnit);
            }
        }

        public void Remove(string name)
        {
            var storageUnit = storage.Where(o => o.Name == name).FirstOrDefault();
            if (storageUnit != null)
            {
                storage.Remove(storageUnit);
            }
        }
    }
}
