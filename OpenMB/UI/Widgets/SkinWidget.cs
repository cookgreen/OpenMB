using OpenMB.UI.Skin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
    public class SkinWidget : Widget, ISkinable
    {
        public string GetSkin(string skinName, string subSkinName)
		{
			return SkinManager.Instance.GetSkin(GetType().Name.Replace("Widget", ""), skinName, subSkinName);
		}
    }
}
