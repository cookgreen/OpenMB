using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    enum GameObjectQueryFlags
    {
        AIMESH_VERTEX = 1 << 0,
        AIMESH_LINE = 2 << 0,
        MOVEABLEOBJECT = 3 << 0,
        TERRAIN = 4 << 0
    }
}
