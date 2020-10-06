using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script
{
    public class ScriptValueStorageUnit
    {
        private readonly int address;
        private string name;
        private object value;
        private string type;
        public string Name { get { return name; } }
        public object Value { get { return value; } }
        public string Type { get { return type; } }

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
            this.name = name;
            this.value = value;
            this.type = type;
        }

        public void ChangeValue(object value)
        {
            this.value = value;
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

        public void ChangeGobalValue(string name, object value)
        {
            var foundStorageUnit = storage.Where(o => o.Name == name);
            if (foundStorageUnit.Count() == 0)
            {
                ScriptValueStorageUnit storageUnit = new ScriptValueStorageUnit(
                    currentAddress,
                    name, 
                    value, 
                    value.GetType().FullName);
                storage.Add(storageUnit);
                currentAddress++;
            }
            else
            {
                var storageUnit = foundStorageUnit.FirstOrDefault();
                storageUnit.ChangeValue(value);
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
