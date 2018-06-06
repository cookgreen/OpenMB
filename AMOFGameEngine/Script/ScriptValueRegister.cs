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

        public ScriptLinkTable GlobalValueTable { get;}

        public ScriptValueRegister()
        {
            ScriptLinkTableNode vect1 = new ScriptLinkTableNode();
            ScriptLinkTableNode vect2 = new ScriptLinkTableNode();
            ScriptLinkTableNode vect3 = new ScriptLinkTableNode();
            ScriptLinkTableNode vect4 = new ScriptLinkTableNode();
            ScriptLinkTableNode vect1X = new ScriptLinkTableNode();
            ScriptLinkTableNode vect1Y = new ScriptLinkTableNode();
            ScriptLinkTableNode vect1Z = new ScriptLinkTableNode();
            ScriptLinkTableNode vect2X = new ScriptLinkTableNode();
            ScriptLinkTableNode vect2Y = new ScriptLinkTableNode();
            ScriptLinkTableNode vect2Z = new ScriptLinkTableNode();
            ScriptLinkTableNode vect3X = new ScriptLinkTableNode();
            ScriptLinkTableNode vect3Y = new ScriptLinkTableNode();
            ScriptLinkTableNode vect3Z = new ScriptLinkTableNode();
            ScriptLinkTableNode vect4X = new ScriptLinkTableNode();
            ScriptLinkTableNode vect4Y = new ScriptLinkTableNode();
            ScriptLinkTableNode vect4Z = new ScriptLinkTableNode();
            vect1.Name = "vect1";
            vect2.Name = "vect2";
            vect3.Name = "vect3";
            vect4.Name = "vect4";
            vect1X.Name = "vector1_X";
            vect1Y.Name = "vector1_Y";
            vect1Z.Name = "vector1_Z";
            vect2X.Name = "vector2_X";
            vect2Y.Name = "vector2_Y";
            vect2Z.Name = "vector2_Z";
            vect3X.Name = "vector3_X";
            vect3Y.Name = "vector3_Y";
            vect3Z.Name = "vector3_Z";
            vect4X.Name = "vector4_X";
            vect4Y.Name = "vector4_Y";
            vect4Z.Name = "vector4_Z";
            vect1.NextNodes.Add(vect1X);
            vect1.NextNodes.Add(vect1Y);
            vect1.NextNodes.Add(vect1Z);
            vect2.NextNodes.Add(vect2X);
            vect2.NextNodes.Add(vect2Y);
            vect2.NextNodes.Add(vect2Z);
            vect3.NextNodes.Add(vect3X);
            vect3.NextNodes.Add(vect3Y);
            vect3.NextNodes.Add(vect3Z);
            vect4.NextNodes.Add(vect4X);
            vect4.NextNodes.Add(vect4Y);
            vect4.NextNodes.Add(vect4Z);
            GlobalValueTable.AddRecord(vect1);
            GlobalValueTable.AddRecord(vect2);
            GlobalValueTable.AddRecord(vect3);
            GlobalValueTable.AddRecord(vect4);
        }
    }
}
