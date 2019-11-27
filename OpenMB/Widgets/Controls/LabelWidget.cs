using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{

	/// <summary>
	/// Basic label widget
	/// </summary>
	public class LabelWidget : Widget
	{
		protected Mogre.TextAreaOverlayElement textAreaElement;
		protected bool isFitToTray;

		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public LabelWidget(string name, string caption, float width)
		{
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Label", "BorderPanel", name);
			textAreaElement = (Mogre.TextAreaOverlayElement)((Mogre.OverlayContainer)element).GetChild(Name + "/LabelCaption");

			setCaption(caption);
			if (width <= 0f)
				isFitToTray = true;
			else
			{
				isFitToTray = false;
				element.Width = (width);
			}
		}

		public string getCaption()
		{
			return textAreaElement.Caption;
		}

		public void setCaption(string caption)
		{
			textAreaElement.Caption = (caption);
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (listener != null && IsCursorOver(element, cursorPos, 3f))
				listener.labelHit(this);
		}

		public bool _isFitToTray()
		{
			return isFitToTray;
		}

	}
}
