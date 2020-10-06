using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public class ScriptValueStorageUnit
    {
        private int address;
        public string Name { get; set; }
        public object Value { get; set; }
        public string Type { get; set; }

        public int Address
        {
            get
            {
                return address;
            }
        }

        public ScriptValueStorageUnit(int address, string name, object value, string type)
        {
            this.address = address;
            Name = name;
            Value = value;
            Type = type;
        }
    }
    public class ScriptValueStorage
    {
        private int startAddress = 0x0000;
        private int currentAddress;

        private static ScriptValueStorage instance;
        public static ScriptValueStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScriptValueStorage();
                }
                return instance;
            }
        }

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
            currentAddress = startAddress;
        }

        public void Append(string name, object value)
        {
            if (storage.Where(o => o.Name == name).Count() == 0)
            {
                ScriptValueStorageUnit storageUnit = new ScriptValueStorageUnit(
                    currentAddress,
                    name, 
                    value, 
                    value.GetType().FullName);
                storage.Add(storageUnit);
                currentAddress++;
            }
        }

        public int GetVariableAddress(string name)
        {
            return storage.Where(o => o.Name == name).FirstOrDefault().Address;
        }

        public object GetVariableValue(string name)
        {
            return storage.Where(o => o.Name == name).FirstOrDefault().Value;
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
