using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG
{
    public class LevelInfo
    {
        private int currentLevel;
        private int currentXp;
        private int neededXpToUpgradeNextLvl;
        public event Action<bool> NewLevelReached;

        public int CurrentLevel
        {
            get { return currentLevel; }
        }
        public int NeededXpToUpgradeNextLvl
        {
            get { return neededXpToUpgradeNextLvl; }
        }
        public int CurrentXp
        {
            get { return currentXp; }
            set { currentXp = value; }
        }

        public LevelInfo()
        {
            currentLevel = 1;
            currentXp = 0;
            neededXpToUpgradeNextLvl = currentLevel * 200;
        }

        public void UpgradeToNextLevel()
        {
            currentLevel++;
            neededXpToUpgradeNextLvl = currentLevel * 200;
            if (NewLevelReached != null)
            {
                NewLevelReached(true);
            }
        }

        public void AddXp(int value)
        {
            currentXp += currentXp + value;
            if(currentXp>=neededXpToUpgradeNextLvl)
            {
                UpgradeToNextLevel();
            }
        }
    }
}
