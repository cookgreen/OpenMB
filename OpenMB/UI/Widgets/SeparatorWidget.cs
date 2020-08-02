using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{

	/// <summary>
	/// Basic separator widget
	/// </summary>
	public class Separator : Widget
	{

		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public Separator(string name, float width)
		{
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Separator", "Panel", name);
			if (width <= 0)
				mFitToTray = true;
			else
			{
				mFitToTray = false;
				element.Width = (width);
			}
		}

		public bool _isFitToTray()
		{
			return mFitToTray;
		}


		protected bool mFitToTray;
	}
}
