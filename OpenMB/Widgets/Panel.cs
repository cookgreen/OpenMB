using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Widgets
{
    public class Panel : Widget
    {
        private List<Widget> widgets;

        public Panel(string name, float width = 0, float height = 0, float left = 0, float top = 0)
        {
            widgets = new List<Widget>();
            OverlayManager overlayMgr = OverlayManager.Singleton;
            mElement = overlayMgr.CreateOverlayElement("BorderPanel", name);
            mElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            mElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_CENTER;
            if (width == 0 || height == 0)
            {
                mElement.Width = float.Parse(GameManager.Instance.videoMode["Width"]) - 20;
                mElement.Height = float.Parse(GameManager.Instance.videoMode["Height"]) - 20;
            }
            else if (width > 0 && height > 0)
            {
                mElement.Width = width;
                mElement.Height = height;
            }
        }

        public void AddWidget(Widget widget)
        {
            widgets.Add(widget);
            ((OverlayContainer)mElement).AddChild(widget.getOverlayElement());
        }

        public void RemoveWidget(Widget widget)
        {
            widgets.Remove(widget);
            ((OverlayContainer)mElement).RemoveChild(widget.getOverlayElement().Name);
            Control.nukeOverlayElement(widget.getOverlayElement());
        }

        public void ClearWidget()
        {
            foreach (Widget widget in widgets)
            {
                ((OverlayContainer)mElement).RemoveChild(widget.getOverlayElement().Name);
                Control.nukeOverlayElement(widget.getOverlayElement());
            }
            widgets.Clear();
        }

        public int GetWidgetNum()
        {
            return widgets.Count;
        }

        public Widget GetWidget(int index)
        {
            if (index >= widgets.Count)
            {
                return null;
            }
            else
            {
                return widgets[index];
            }
        }

        public override void Dispose()
        {
            mElement.Dispose();
            Control.nukeOverlayElement(mElement);
        }
    }
}
