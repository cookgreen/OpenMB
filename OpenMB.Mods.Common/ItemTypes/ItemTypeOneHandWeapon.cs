using Mogre;
using OpenMB.Game.ItemTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Mods.Common.ItemTypes
{
    public class ItemTypeOneHandWeapon : ItemType
    {
        public override MaterialPtr RenderPreview(Entity itemEnt)
        {
            return base.RenderPreview(itemEnt);
        }
    }
}
