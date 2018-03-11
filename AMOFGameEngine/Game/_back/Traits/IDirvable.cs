using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Game.Objects;

namespace AMOFGameEngine.Game.Traits
{
    /// <summary>
    /// Can drive
    /// </summary>
    public interface IDirvable
    {
        void Drive(Character driver);

        void Stop();

        void Turn();
    }
}
