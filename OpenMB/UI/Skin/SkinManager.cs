using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Skin
{
    public class SkinManager
    {
        private Dictionary<string, SkinFile> availableSkins;
        private SkinFile currentSkin;
        private static SkinManager instance;
        public static SkinManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SkinManager();
                }
                return instance;
            }
        }

        public SkinManager()
		{
            availableSkins = new Dictionary<string, SkinFile>();
        }

        public void LoadSkin(string skinFileName)
        {
            currentSkin = SkinFile.Load(skinFileName);
            availableSkins.Add(currentSkin.Name, currentSkin);
        }

        public void ChangeSkin(string skinName)
		{
            currentSkin = availableSkins[skinName];
		}

        public string GetSkin(string elementName, string childSkinElement, string subSkinName)
		{
            return currentSkin.Rects.Where(o => o.Name == elementName).FirstOrDefault().Elements.Where(o => o.Name == childSkinElement).FirstOrDefault().Elements.Where(o => o.Name == subSkinName).FirstOrDefault().Value;
		}
    }
}
