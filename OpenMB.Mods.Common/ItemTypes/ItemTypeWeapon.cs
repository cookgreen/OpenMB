using OpenMB.Game.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.ItemTypes
{
    public class ItemTypeWeapon : PlaceholderItemType
    {
        protected List<string> parameters;
        public override List<string> Parameters
        {
            get { return parameters; }
        }

        public ItemTypeWeapon()
        {
            parameters = new List<string>();
            parameters.Add("Range");
            parameters.Add("Damage");
            parameters.Add("ROF");
        }
    }
}
