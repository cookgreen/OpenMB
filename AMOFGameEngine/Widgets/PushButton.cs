using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Widgets
{
    public class PushButton : Mogre_Procedural.MogreBites.Widget
    {
        public event Action<object> Clicked;
        private BorderPanelOverlayElement buttonMain;
        private TextAreaOverlayElement buttonText;
        private float top;
        private float left;
        private float width;
        private float height;
        private Vector2[] bounds;
        public PushButton(string name, string caption, float left, float top, float height, float width)
        {
            buttonMain = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/PushButton", "BorderPanel", name + "/Main") as BorderPanelOverlayElement;
            buttonText = buttonMain.GetChild(buttonMain.Name + "/PushButtonCaption") as TextAreaOverlayElement;
            buttonText.Caption = caption;
            buttonMain.Left = left;
            buttonMain.Top = top;
            buttonMain.Width = width;
            buttonMain.Height = height;
            buttonText.Top = (-(buttonText.CharHeight / 2f));
            //buttonMain.Width = (getCaptionWidth(caption, ref buttonText) + buttonMain.Height - 12f);
            bounds = new Vector2[4]
            {
                new Vector2(){x=left,y=top},
                new Vector2(){x=left+width,y=top},
                new Vector2(){x=left,y=top+height},
                new Vector2(){x=left+width,y=top+height}
            };
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        private bool IsInBounds(Vector2 pos)
        {
            if (pos.x >= bounds[0].x && pos.y >= bounds[0].y &&
                pos.x <= bounds[3].x && pos.y <= bounds[3].y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void _cursorReleased(Vector2 cursorPos)
        {
            if (IsInBounds(cursorPos))
            {
                if (Clicked != null)
                {
                    Clicked(this);
                }
            }
        }

        public override void _cursorMoved(Vector2 cursorPos)
        {
            if (IsInBounds(cursorPos))
            {
                buttonMain.MaterialName = "SdkTrays/Button/Over";
            }
        }

        public override void _cursorPressed(Vector2 cursorPos)
        {
            if (IsInBounds(cursorPos))
            {
                if (Clicked != null)
                {
                    Clicked(this);
                }
            }
        }

        public OverlayContainer GetContainer()
        {
            return buttonMain;
        }
    }
}
