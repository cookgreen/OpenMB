using Mogre;
using Mogre_Procedural.MogreBites;

namespace OpenMB.Widgets
{
	public class PanelTemplateWidget : Widget
	{
		public PanelTemplateWidget(string name, string template, float width = 0, float height = 0, float left = 0, float top = 0)
		{
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate(template, "BorderPanel", name);
			element.MetricsMode = GuiMetricsMode.GMM_RELATIVE;

			if (width == 0 || height == 0)
			{
				element.Width = 1.0f;
				element.Height = 1.0f;
			}
			else if (width > 0 && height > 0)
			{
				element.Width = width;
				element.Height = height;
			}
			element.Top = top;
			element.Left = left;
		}
	}
}