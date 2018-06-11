using AMOFGameEngine.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Trigger
{
    /// <summary>
    /// AI vs. Human Trigger
    /// </summary>
    public class GameTrigger : ITrigger
    {
        private GameWorld world;
        public GameTrigger(GameWorld world)
        {
            this.world = world;
        }
        public int ExecuteTime
        {
            get
            {
                return 0;
            }
        }

        public int FreezeTime
        {
            get
            {
                return 0;
            }
        }

        public bool CheckCondition()
        {
            return true;
        }

        public void Execute()
        {
            var agents = world.Agents;
            var player = agents.Find(o => o.GetControlled());
            for (int i = 0; i < agents.Count; i++)
            {
                if (agents[i].Id != player.Id)
                {
                    //will chase player
                    agents[i].WalkTo(player.Controller.Position);
                }
            }
        }
    }
}
