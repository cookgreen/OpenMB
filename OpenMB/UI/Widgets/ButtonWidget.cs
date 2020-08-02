using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{

	/// <summary>
	/// Enumerator values for button states
	/// </summary>
	public enum ButtonState : int
	{
		BS_UP,
		BS_OVER,
		BS_DOWN
	}

	/// <summary>
	/// Basic Button Class
	/// </summary>
	public class ButtonWidget : Widget
	{
		protected ButtonState state;
		protected Mogre.BorderPanelOverlayElement borderPanelElement;
		protected Mogre.TextAreaOverlayElement textAreaElement;
		protected bool isFitToContents;
		public event Action<object> OnClick;
		public string Text
		{
			get
			{
				return textAreaElement.Caption;
			}
			set
			{
				textAreaElement.Caption = value;
				if (isFitToContents)
					element.Width = (GetCaptionWidth(value, ref textAreaElement) + element.Height - 12f);
			}
		}
		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public ButtonWidget(string name, string caption, float width)
		{
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Button", "BorderPanel", name);
			borderPanelElement = (Mogre.BorderPanelOverlayElement)element;
			textAreaElement = (Mogre.TextAreaOverlayElement)borderPanelElement.GetChild(borderPanelElement.Name + "/ButtonCaption");

			textAreaElement.Top = (-(textAreaElement.CharHeight / 2f));

			if (width > 0f)
			{
				element.Width = (width);
				isFitToContents = false;
			}
			else
				isFitToContents = true;

			Text = caption;
			state = ButtonState.BS_UP;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public ButtonState getState()
		{
			return state;
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (IsCursorOver(element, cursorPos, 4))
			{
				SetState(ButtonState.BS_DOWN);
			}
		}

		public override void CursorReleased(Mogre.Vector2 cursorPos)
		{
			if (state == ButtonState.BS_DOWN)
			{
				SetState(ButtonState.BS_OVER);
				if (listener != null)
					listener.buttonHit(this);
				if (OnClick != null)
				{
					OnClick(this);
				}
			}
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			if (IsCursorOver(element, cursorPos, 4f))
			{
				if (state == ButtonState.BS_UP)
					SetState(ButtonState.BS_OVER);
			}
			else
			{
				if (state != ButtonState.BS_UP)
					SetState(ButtonState.BS_UP);
			}
		}

		public override void FocusLost()
		{
			SetState(ButtonState.BS_UP); // reset button if cursor was lost
		}

		protected void SetState(ButtonState bs)
		{
			if (bs == ButtonState.BS_OVER)
			{
				borderPanelElement.BorderMaterialName = "SdkTrays/Button/Over";
				borderPanelElement.MaterialName = "SdkTrays/Button/Over";
			}
			else if (bs == ButtonState.BS_UP)
			{
				borderPanelElement.BorderMaterialName = "SdkTrays/Button/Up";
				borderPanelElement.MaterialName = "SdkTrays/Button/Up";
			}
			else
			{
				borderPanelElement.BorderMaterialName = "SdkTrays/Button/Down";
				borderPanelElement.MaterialName = "SdkTrays/Button/Down";
			}

			state = bs;
		}
	}
}
