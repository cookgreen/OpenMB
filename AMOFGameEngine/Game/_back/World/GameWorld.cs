using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Maps;
using AMOFGameEngine.Game.Objects;

namespace AMOFGameEngine.Game.World
{
    public class GameWorld
    {
        private List<MoveableObject> agents;
        private List<StaticObject> props;
        public List<MoveableObject> Agents
        {
            get
            {
                return agents;
            }
        }
        public List<StaticObject> Props
        {
            get 
            {
                return props;
            }
        }
        public GameWorld(Map map)
        {
            agents = new List<MoveableObject>();
            props = new List<StaticObject>();
        }

        public void Clear()
        {
            agents.Clear();
            props.Clear();
        }
    }
}
