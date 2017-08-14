using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Maps
{
    public class MapManager
    {
        Dictionary<string, Map> activeMaps;
        List<Map> maps;
        bool isEnd;

        public MapManager()
        {
            maps = new List<Map>();
            activeMaps = new Dictionary<string, Map>();
            isEnd = false;
        }

        public bool AddMap(string name,Map map)
        {
            if (!activeMaps.ContainsKey(name))
            {
                activeMaps.Add(name, map);
                return true;
            }
            else
            {
                LogManager.Singleton.LogMessage("[Engine Error]: The map with same name has already existed!");
                return false;
            }
        }

        public void ChangeMap(Map map)
        {
            if (map != null)
            {
                if (maps.Count != 0)
                {
                    maps.Last().exit();
                }

                maps.Add(map);

                //GameManager.Singleton.mTrayMgr.setListener(map);

                maps.Last().enter();
            }
        }

        public void StartMap(Map map)
        {
            ChangeMap(map);

            int timeSinceLastFrame = 1;
            int startTime = 0;
            while (!isEnd)
            {
                startTime = (int)GameManager.Singleton.mTimer.MicrosecondsCPU;

                WindowEventUtilities.MessagePump();

                maps.Last().update(timeSinceLastFrame * 1.0 / 1000);

                GameManager.Singleton.mMouse.Capture();
                GameManager.Singleton.mKeyboard.Capture();
                GameManager.Singleton.mRoot.RenderOneFrame();

                timeSinceLastFrame = (int)GameManager.Singleton.mTimer.MillisecondsCPU - startTime;
            }

            maps.Last().exit();
        }
        public void EndMap()
        {
            isEnd = true;
        }
    }
}
