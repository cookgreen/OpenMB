using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Widgets;

namespace OpenMB.Output
{
    public class OutputManager
    {
        private OverlayContainer container;
        private Overlay o;
        private StringVector buffer;
        private List<OverlayElement> textElements;
        private float alphaSinceLastFrame;
		private int delay = 20;

        private static OutputManager instance;
        public static OutputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OutputManager();
                }
                return instance;
            }
        }

        public OutputManager()
        {
            alphaSinceLastFrame = 1;
            textElements = new List<OverlayElement>();
            container = (OverlayContainer)OverlayManager.Singleton.CreateOverlayElement("BorderPanel", "msgContainer");
            o = OverlayManager.Singleton.Create("msgOverlay");
            o.ZOrder = 254;
            o.Show();

            container.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            container._setPosition(0, 0);
            container.Width = 0.2f;
            container.Height = 0.1f;
            o.Add2D(container);

            buffer = new StringVector();
            instance = this;
        }

        public void DisplayMessage(string message, string color = "0xffffff")
        {
            if (!OverlayManager.Singleton.HasOverlayElement("msgText" + textElements.Count))
            {
                TextAreaOverlayElement textArea = OverlayManager.Singleton.CreateOverlayElement("TextArea", "msgText" + textElements.Count) as TextAreaOverlayElement;
                textArea.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
                textArea.Left = 0.01f;
                textArea.Top = 0.9f;
                textArea.Width = 0.2f;
                textArea.Height = 0.1f;
                textArea.SetParameter("font_name", "EngineFont");
                textArea.SetParameter("char_height", "0.03");
                textArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
                container.AddChild(textArea);
                textArea.Colour = Utilities.Helper.HexToRgb(color.ToString());
                textArea.Caption = message;
                buffer.Add(message);
                textElements.Add(textArea);
                for (int i = 0; i < textElements.IndexOf(textArea); i++)
                {
                    textElements[i].Top -= 0.03f;
                }
            }
        }

        public void Dispose()
        {
            o.Remove2D(container);
            o.Dispose();
            container.Dispose();
            textElements.Clear();
        }

        public void Update(float timeSinceLastFrame)
        {
            for (int i = 0; i < textElements.Count;i++ )
            {
                var itr = textElements[i];
                alphaSinceLastFrame = itr.Colour.a;
                if (alphaSinceLastFrame > 0.0f)
                {
					if (delay > 0)
					{
						delay--;
					}
					else
					{
						alphaSinceLastFrame -= 0.01f;
						delay = 20;
						ColourValue cv = new ColourValue(
							   itr.Colour.r,
							   itr.Colour.g,
							   itr.Colour.b,
							   float.Parse(alphaSinceLastFrame.ToString("0.00")));
						itr.Colour = cv;
					}
                }
                if (alphaSinceLastFrame == 0.0f)
                {
                    OverlayManager.Singleton.DestroyOverlayElement(itr);
                    textElements.Remove(itr);
                }
            }
        }
    }
}
