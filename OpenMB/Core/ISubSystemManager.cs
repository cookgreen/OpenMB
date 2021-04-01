using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
    public interface ISubSystemManager
    {
        void Update(float timeSinceLastFrame);
    }
}
