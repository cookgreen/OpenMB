using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game.Objects;

namespace AMOFGameEngine.Game.Traits
{
    /// <summary>
    /// Can mount
    /// </summary>
    public interface IMountable
    {
        void Mount(Character rider);

        void Unmount();
    }
}
