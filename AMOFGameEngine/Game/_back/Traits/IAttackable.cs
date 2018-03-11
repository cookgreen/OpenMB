using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Traits
{
    /// <summary>
    /// Can attack
    /// </summary>
    public interface IAttackable
    {
        void Attack(GameObject target);

        void Defence();
    }
}
