using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.HitBalloons.ModelTypes
{
    public class ItemDefineModelType : IModModelType
    {
        public string Name
        {
            get
            {
                return "ItemDefine";
            }
        }

        public object Process(ModData data, params object[] param)
        {

            string modelID = param[0].ToString();
            var findedItems = data.ItemInfos.Where(o => o.ID == modelID);
            if (findedItems.Count() > 0)
            {
                var findedItem = findedItems.ElementAt(0);
                string modelMesh = findedItem.MeshName;
                string modelMaterial = findedItem.MaterialName;

                Item itm = ItemFactory.Instance.PreProduce(findedItem);

                return new object[] { modelMesh, modelMaterial, itm };
            }
            else
            {
                return null;
            }
        }
    }
}
