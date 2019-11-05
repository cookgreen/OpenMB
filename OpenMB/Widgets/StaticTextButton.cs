using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{
	/// <summary>
	/// No border button
	/// </summary>
	public class StaticTextButton : Widget
	{
		private ColourValue normalStateColor;
		private ColourValue activeStateColor;
		protected ButtonState mState;
		protected TextAreaOverlayElement mTextArea;
		protected bool mFitToTray;
		public event Action<object> OnClick;
		public override float Width { get { return TextWidth; } }
		public override float Height { get { return TextHeight; } }
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
		public StaticTextButton(string name, string caption, ColourValue normalStateColor, ColourValue activeStateColor, bool specificColor = false)
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
			mTextArea.CharHeight = 0.025f;
			mTextArea.SpaceWidth = 0.02f;
			if (!specificColor)
			{
				normalStateColor = new ColourValue(0.9f, 1f, 0.7f);
			}
			mTextArea.Colour = normalStateColor;
			((OverlayContainer)mElement).AddChild(mTextArea);
			Text = caption;
			_assignListener(GameManager.Instance.trayMgr.Listener);
			this.normalStateColor = normalStateColor;
			this.activeStateColor = activeStateColor;
			mState = ButtonState.BS_UP;
		}

		public ButtonState getState()
		{
			return mState;
		}

		public override void _cursorPressed(Mogre.Vector2 cursorPos)
		{
			if (isCursorOver(cursorPos))
			{
				setState(ButtonState.BS_DOWN);
				if (OnClick != null)
				{
					OnClick(this);
				}
			}
		}

		public override void _cursorReleased(Mogre.Vector2 cursorPos)
		{
			if (mState == ButtonState.BS_DOWN)
			{
				setState(ButtonState.BS_OVER);
			}
		}

		public override void _cursorMoved(Mogre.Vector2 cursorPos)
		{
			if (isCursorOver(cursorPos))
			{
				if (mState == ButtonState.BS_UP)
					setState(ButtonState.BS_OVER);
			}
			else
			{
				if (mState != ButtonState.BS_UP)
					setState(ButtonState.BS_UP);
			}
		}

		public override void _focusLost()
		{
			setState(ButtonState.BS_UP); // reset button if cursor was lost
		}

		protected void setState(ButtonState bs)
		{
			if (bs == ButtonState.BS_OVER)
			{
				mTextArea.Colour = activeStateColor;
			}
			else if (bs == ButtonState.BS_UP)
			{
				mTextArea.Colour = normalStateColor;
			}
			else
			{
				mTextArea.Colour = activeStateColor;
			}
			mState = bs;
		}

		public override void AddedToAnotherWidgetFinished(
			AlignMode alignMode,
			float parentWidgetLeft,
			float parentWidgetWidth,
			float parentWidgetTop,
			float parentWidgetHeight
		)
		{
			switch (alignMode)
			{
				case AlignMode.Center:
					//mElement.Left = (parentWidgetWidth - TextWidth) / 2;
					mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
					mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Center);
					mElement.Left += mElement.Left + TextWidth - parentWidgetWidth / 2;
					break;
			}
		}
	}
}
