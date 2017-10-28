using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG.Traits
{
    /// <summary>
    /// Can attack
    /// </summary>
    public interface IAttackable
    {
        void Attack(RPGObject target);

        void Defence();
    }
}
