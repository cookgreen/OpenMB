using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    enum GameObjectQueryFlags
    {
        EDITABLE_SCENE_OBJECT = 1 << 0,
        TERRAIN = 2 << 0
    }
}
