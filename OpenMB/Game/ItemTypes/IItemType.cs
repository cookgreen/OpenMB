using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.ItemTypes
{
    public interface IItemType
    {
        string Name { get; }

        void Use(params object[] param);
    }
}
