using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public class ModXmlDefineModelType : IModModelType
    {
        public string Name
        {
            get
            {
                return "XmlDefine";
            }
        }

        public object Process(ModData data, params object[] param)
        {
            string modelID = param[0].ToString();
            var findedModels = data.Models.Where(o => o.ID == modelID);
            if (findedModels.Count() > 0)
            {
                var findedModel = findedModels.ElementAt(0);
                string modelMesh = findedModel.Mesh;
                string modelMaterial = findedModel.Material;
                return new object[] { modelMesh, modelMaterial };
            }
            else
            {
                return null;
            }
        }
    }
}
