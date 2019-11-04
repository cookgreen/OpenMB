using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{
	public class StaticTextRelative : Widget
	{
		protected TextAreaOverlayElement mTextArea;
		protected bool mFitToTray;
		public float TextWidth
		{
			get
			{
				return getCaptionWidth(Text, ref mTextArea);
			}
		}
		public float TextHeight
		{
			get
			{
				return getCaptionHeight(Text, ref mTextArea);
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

		public StaticTextRelative(string name, string caption, float width, bool specificColor, ColourValue color)
		{
			OverlayManager overlayMgr = OverlayManager.Singleton;
			mElement = overlayMgr.CreateOverlayElement("BorderPanel", name);
			mElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			mElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
			mElement.Height = 0.32f;
			mTextArea = overlayMgr.CreateOverlayElement("TextArea", name + "/StaticTextCaption") as TextAreaOverlayElement;
			mTextArea.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
			mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Left);
			mTextArea.Top = 0.01f;
			mTextArea.FontName = "EngineFont";
			mTextArea.CharHeight = 0.03f;
			mTextArea.SpaceWidth = 0.02f;
			if (!specificColor)
			{
				mTextArea.Colour = new ColourValue(0.9f, 1f, 0.7f);
			}
			else
			{
				mTextArea.Colour = color;
			}
			((OverlayContainer)mElement).AddChild(mTextArea);
			Text = caption;
		}

		public override void _cursorPressed(Mogre.Vector2 cursorPos)
		{
		}

		public bool _isFitToTray()
		{
			return mFitToTray;
		}

		public override void AddedToAnotherWidgetFinished(
			AlignMode alignMode,
			float parentWidgetLeft,
			float parentWidgetWidth,
			float parentWidgetTop,
			float parentWidgetHeight
		)
		{
			switch(alignMode)
			{
				case AlignMode.Center:
					mElement.Left = (parentWidgetWidth - getCaptionWidth(mTextArea.Caption, ref mTextArea)) / 2;
					break;
			}
		}
	}
}
