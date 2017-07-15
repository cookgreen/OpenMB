using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;
using Mogre;

namespace AMOFGameEngine.Maps
{
    public class Map
    {
        DotSceneLoader mapLoader;
        SceneManager scm;
        MapManager parent;
        string mapFileName;


        public Map(string mapFileName, SceneManager scm)
        {
            mapLoader = new DotSceneLoader();
            this.mapFileName = mapFileName;
            this.scm = scm;
        }

        public void create(string name, MapManager mapMngr)
        {
            parent = mapMngr;
            parent.AddMap(name, this);
        }

        public void enter()
        {
            mapLoader.ParseDotScene(mapFileName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                scm);
        }

        public void exit()
        {

        }

        public void update(double timeSinceLastFrame)
        {
            
        }
    }
}
