using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Skin
{
    public class SkinManager
    {
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

        public void LoadSkin(string skinFileName)
        {

        }
    }
}
