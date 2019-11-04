using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.ItemTypes
{
    public class ItemTypeRideDrive : IItemType
    {
        public string Name {
            get
            {
                return "IT_RIDEDRIVE";
            }
        }

        public string AttachBoneName
        {
            get
            {
                return "Spin";
            }
        }

        public void Use(params object[] param)
        {
        }
    }
}
