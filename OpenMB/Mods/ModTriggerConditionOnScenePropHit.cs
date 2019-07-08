using OpenMB.Game;
using OpenMB.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods
{
    public class ModTriggerConditionOnScenePropHit : IModTriggerCondition
    {
        public string Name
        {
            get
            {
                return "ti_on_scene_prop_hit";
            }
        }

        public bool CheckCondition(params object[] param)
        {
            string missileScenePropInstanceID = param[0].ToString();
            string scenePropInstanceID = param[1].ToString();
            GameMap map = param[2] as GameMap;
            SceneProp sceneProp = map.GetSceneProp(scenePropInstanceID);
            if (sceneProp == null)
            {
                return false;
            }

            SceneProp missileSceneProp = map.GetSceneProp(missileScenePropInstanceID);
            if (missileSceneProp == null)
            {
                return false;
            }

            return sceneProp.CheckCollide(missileSceneProp);
        }
    }
}
