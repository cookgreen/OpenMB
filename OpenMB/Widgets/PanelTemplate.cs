using Mogre;
using Mogre_Procedural.MogreBites;

namespace OpenMB.Widgets
{
	public class PanelTemplate : Widget
	{
		public PanelTemplate(string name, string template, float width = 0, float height = 0, float left = 0, float top = 0)
		{
			mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate(template, "BorderPanel", name);
			mElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;

			if (width == 0 || height == 0)
			{
				mElement.Width = 1.0f;
				mElement.Height = 1.0f;
			}
			else if (width > 0 && height > 0)
			{
				mElement.Width = width;
				mElement.Height = height;
			}
			mElement.Top = top;
			mElement.Left = left;
		}
	}
}