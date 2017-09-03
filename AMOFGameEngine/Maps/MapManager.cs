using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Maps
{
    public class MapManager
    {
        private Stack<Map> MapStack;

        public MapManager()
        {
            MapStack = new Stack<Map>();
        }

        public void EnterNewMap(Map newMap)
        {
            MapStack.Push(newMap);
            if (MapStack.Count > 1)
            {
                MapStack.ElementAt(1).Unload();
            }
            MapStack.Peek().Load();
        }

        public void ReturnLastMap()
        {
            MapStack.Pop();
            MapStack.Peek().Load();
        }
    }
}
