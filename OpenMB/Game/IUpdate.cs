using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public interface IUpdate
    {
        void Update(float deltaTime);
    }
}
