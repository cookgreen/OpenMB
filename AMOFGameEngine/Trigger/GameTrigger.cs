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
        private int waveNum;
        private GameWorld world;
        public GameTrigger(GameWorld world)
        {
            this.world = world;
            waveNum = 1;
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
            if (waveNum < 5)
            {

            }
            else if (waveNum < 10)
            {

            }
            else if (waveNum < 15)
            {

            }
            else
            {

            }
            waveNum++;
        }
    }
}
