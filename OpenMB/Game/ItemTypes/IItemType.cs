using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.ItemTypes
{
    public interface IItemType
    {
        string Name { get; }

        string AttachBoneName { get; }

        void Use(params object[] param);
    }
}
