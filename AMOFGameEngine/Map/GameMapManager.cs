﻿using AMOFGameEngine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Map
{
    public class GameMapManager
    {
        private static GameMapManager instance;
        public static GameMapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameMapManager();
                }
                return instance;
            }
        }

        private GameWorld world;
        private IMap currentMap;
        private Queue<IMap> maps;
        public void Load(string name)
        {
            IMap map = new GameMap(name, world.SceneManager);
            maps.Enqueue(map);
            map.LoadMapStarted += Map_LoadMapStarted;
            map.LoadMapFinished += Map_LoadMapFinished;
            map.LoadAsync();
        }

        private void Map_LoadMapFinished()
        {
            currentMap = 
        }

        private void Map_LoadMapStarted()
        {

        }

        public void Initization(GameWorld world)
        {
            this.world = world;
        }

        public void Dispose()
        {
            if (currentMap != null)
            {
                currentMap.Destroy();
            }
            maps.Clear();
        }

        public string GetCurrentMapName()
        {
            if (currentMap != null)
            {
                return currentMap.GetName();
            }
            else
            {
                return string.Empty;
            }
        }

        public GameMap GetCurrentMap()
        {
            return (GameMap)currentMap;
        }
    }
}