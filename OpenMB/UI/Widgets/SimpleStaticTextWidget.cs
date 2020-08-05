using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	public class SimpleStaticTextWidget : Widget
	{
		protected TextAreaOverlayElement mTextArea;
		protected bool mFitToTray;
		public override float Width { get { return TextWidth; } }
		public override float Height { get { return TextHeight; } }
		public float TextWidth
		{
			get
			{
				return GetCaptionWidth(Text, ref mTextArea);
			}
		}
		public float TextHeight
		{
			get
			{
				return GetCaptionHeight(Text, ref mTextArea);
			}
		}

		public string Text
		{
			get
			{
				return mTextArea.Caption;
			}
			set
			{
				mTextArea.Caption = value;
			}
		}
		public TextAreaOverlayElement TextElement
		{
			get
			{
				return mTextArea;
			}
		}

		public SimpleStaticTextWidget(string name, string caption, float width, bool specificColor, ColourValue color, float fontSize = 100)
		{
			OverlayManager overlayMgr = OverlayManager.Singleton;
			element = overlayMgr.CreateOverlayElement("BorderPanel", name);
			element.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			//element.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
			element.Height = 0.32f;
			mTextArea = overlayMgr.CreateOverlayElement("TextArea", name + "/StaticTextCaption") as TextAreaOverlayElement;
			mTextArea.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
			mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Left);
			mTextArea.Top = 0f;
			mTextArea.FontName = "EngineFont";
			mTextArea.CharHeight = 0.025f * (fontSize / (float)100);
			mTextArea.SpaceWidth = 0.02f;
			if (!specificColor)
			{
				mTextArea.Colour = new ColourValue(0.9f, 1f, 0.7f);
			}
			else
			{
				mTextArea.Colour = color;
			}
			((OverlayContainer)element).AddChild(mTextArea);
			Text = caption;
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
		}

		public bool _isFitToTray()
		{
			return mFitToTray;
		}
	}
}
