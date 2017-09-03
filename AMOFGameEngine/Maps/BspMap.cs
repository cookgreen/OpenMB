using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Maps
{
    public class BspMap : Map
    {
        private string archive;
        private string map;
        private SceneManager sceneMgr;
        public BspMap(string archiveName,string mapName,SceneManager scm)
        {
            archive = archiveName;
            map = mapName;
            sceneMgr = scm;
        }

        public override void Load()
        {
            

            ResourceGroupManager rgm = ResourceGroupManager.Singleton;
            rgm.LinkWorldGeometryToResourceGroup(rgm.WorldResourceGroupName, map, sceneMgr);
            rgm.InitialiseResourceGroup(rgm.WorldResourceGroupName);
            rgm.LoadResourceGroup(rgm.WorldResourceGroupName, false);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}
