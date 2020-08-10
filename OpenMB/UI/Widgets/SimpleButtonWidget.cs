using Mogre;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
    public class SimpleButtonWidget : SkinWidget
	{
		private ButtonState state;
		private BorderPanelOverlayElement borderPanelElement;
        private TextAreaOverlayElement textAreaElement;
		public override event Action<object> OnClick;

		public SimpleButtonWidget(string name, string caption, float width, float height, float left = 0, float top = 0)
        {
			OverlayManager overlayMgr = OverlayManager.Singleton;
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SimpleButton", "BorderPanel", name);
			element.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			element.Top = top;
			element.Left = left;
			element.Height = height;
			element.Width = width;
			borderPanelElement = (BorderPanelOverlayElement)element;
			textAreaElement = overlayMgr.CreateOverlayElement("TextArea", name + "/StaticTextCaption") as TextAreaOverlayElement;
			textAreaElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			textAreaElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;
			textAreaElement.SetAlignment(TextAreaOverlayElement.Alignment.Center);
			textAreaElement.FontName = "EngineFont";
			textAreaElement.CharHeight = 0.025f;
			textAreaElement.SpaceWidth = 0.02f;
			textAreaElement.Colour = ColourValue.Black;
			textAreaElement.Top = height / 45f;
			((OverlayContainer)element).AddChild(textAreaElement);
			textAreaElement.Caption = caption;
		}

        public override void FocusLost()
		{
			SetState(ButtonState.BS_UP);
		}

        public override void MouseMoved(MouseEvent evt)
		{
			Vector2 cursorPos = new Vector2(evt.state.X.abs, evt.state.Y.abs);
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

        public override void CursorPressed(Vector2 cursorPos)
		{
			if (IsCursorOver(element, cursorPos, 4))
			{
				SetState(ButtonState.BS_DOWN);
			}
		}

        public override void CursorReleased(Vector2 cursorPos)
		{
			if (state == ButtonState.BS_DOWN)
			{
				SetState(ButtonState.BS_OVER);
				OnClick?.Invoke(this);
			}
		}

        private void SetState(ButtonState bs)
		{
			if (bs == ButtonState.BS_OVER)
			{
				borderPanelElement.BorderMaterialName = GetSkin("Border", "Over");
				borderPanelElement.MaterialName = GetSkin("Background", "Over");
			}
			else if (bs == ButtonState.BS_UP)
			{
				borderPanelElement.BorderMaterialName = GetSkin("Border", "Up");
				borderPanelElement.MaterialName = GetSkin("Background", "Up");
			}
			else
			{
				borderPanelElement.BorderMaterialName = GetSkin("Border", "Down");
				borderPanelElement.MaterialName = GetSkin("Background", "Down");
			}

			state = bs;
		}
    }
}
