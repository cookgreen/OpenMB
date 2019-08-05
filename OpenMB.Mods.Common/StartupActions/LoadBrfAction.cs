using OpenMB.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Mods.Common.StartupActions
{
    class LoadBrfAction : IModSetting
    {
        public string Name
        {
            get
            {
                return "load_brf";
            }
        }

        public string Value
        {
            get;
            set;
        }

        public void Load(ModData currentMod)
        {
            var findedMedia = currentMod.ModMediaData.Where(o => o.MediaName == Value && o.MediaType == XML.ResourceType.Models);
            if (findedMedia.Count() > 0)
            {
                var media = findedMedia.ElementAt(0);
                MBOgreBrf mbBrf = new MBOgreBrf(media.MediaName);
                mbBrf.ReadFromFileSystem(media.FullMediaPath);
                MBOgre.Instance.LoadBrfFile(mbBrf);
            }
        }
    }
}
