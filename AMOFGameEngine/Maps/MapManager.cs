using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Maps
{
    public class MapManager
    {
        public struct MapInfo
        {
            public string mapName;
            public Map map;
        }

        List<Map> maps;
        List<MapInfo> mapInfos;
        bool isEnd;

        public MapManager()
        {
            maps = new List<Map>();
            mapInfos = new List<MapInfo>();
            isEnd = false;
        }

        public void AddMap(string name,Map map)
        {
            MapInfo mi;
            mi.mapName = name;
            mi.map = map;
            mapInfos.Add(mi);
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

        public Map FindMapByName(string name)
        {
            foreach (var item in mapInfos)
            {
                if (item.mapName == name)
                {
                    return item.map;
                }
            }
            return null;
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
