using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Script
{
    public class ScriptValueRegister
    {
        private static ScriptValueRegister instance;
        public static ScriptValueRegister Instance
        {
            get
            {
                if(instance==null)
                {
                    instance = new ScriptValueRegister();
                }
                return instance;
            }
        }
        private ScriptLinkTable globalValueTable;
        public ScriptLinkTable GlobalValueTable 
        {
            get
            {
                return globalValueTable;
            }
        }

        public ScriptValueRegister()
        {
            globalValueTable = new ScriptLinkTable();
            for (int i = 0; i < 5; i++)
            {
                ScriptLinkTableNode vect = new ScriptLinkTableNode();
                vect.Name = "vect" + (i + 1);
                ScriptLinkTableNode vectX = new ScriptLinkTableNode();
                ScriptLinkTableNode vectY = new ScriptLinkTableNode();
                ScriptLinkTableNode vectZ = new ScriptLinkTableNode();
                vectX.Name = vect.Name + "_X";
                vectY.Name = vect.Name + "_Y";
                vectZ.Name = vect.Name + "_Z";
                vect.NextNodes.Add(vectX);
                vect.NextNodes.Add(vectY);
                vect.NextNodes.Add(vectZ);
                GlobalValueTable.AddRecord(vect);
            }
        }
    }
}
