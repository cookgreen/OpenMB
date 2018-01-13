using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.Output
{
    public class OutputManager
    {
        private OverlayContainer container;
        private Overlay o;
        private StringVector buffer;
        private List<OverlayElement> textElements;
        private float alphaSinceLastFrame;

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
                alphaSinceLastFrame = textElements[i].Colour.a;
                if (alphaSinceLastFrame > 0.0f)
                {
                    alphaSinceLastFrame -= 0.05f;
                    ColourValue cv = new ColourValue(
                           textElements[i].Colour.r,
                           textElements[i].Colour.g,
                           textElements[i].Colour.b,
                           alphaSinceLastFrame);
                    textElements[i].Colour = cv;
                }
                else if (alphaSinceLastFrame < 0.0f)
                {
                    ColourValue cv = new ColourValue(
                        textElements[i].Colour.r,
                        textElements[i].Colour.g,
                        textElements[i].Colour.b,
                        0);
                    textElements[i].Colour = cv;
                }
            }
        }
    }
}
