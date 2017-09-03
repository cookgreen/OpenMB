using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;
using Mogre;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.Maps
{
    public class SceneMap : Map
    {
        private DotSceneLoader loader;
        private string mapName;
        private SceneManager scm;
        public SceneMap(string mapName,SceneManager scm)
        {
            this.mapName = mapName;
            this.scm = scm;
        }

        public override void Load()
        {
            loader = new DotSceneLoader();
            loader.ParseDotScene(mapName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, scm);
        }

        public override void Unload()
        {
            foreach (var item in loader.StaticObjects)
            {
                scm.DestroySceneNode(item);
            }
            foreach (var item in loader.DynamicObjects)
            {
                scm.DestroySceneNode(item);
            }
        }

        public List<string> GetStaticObjs()
        {
            return loader.StaticObjects;
        }

        public List<string> GetDynamicObjs()
        {
            return loader.DynamicObjects;
        }
    }
}
