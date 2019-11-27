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
	/// Scrollable text box widget
	/// </summary>
	public class TextBox : Widget
	{

		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public TextBox(string name, string caption, float width, float height)
		{
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/TextBox", "BorderPanel", name);
			element.Width = (width);
			element.Height = (height);
			Mogre.OverlayContainer container = (Mogre.OverlayContainer)element;
			mTextArea = (Mogre.TextAreaOverlayElement)container.GetChild(Name + "/TextBoxText");
			mCaptionBar = (Mogre.BorderPanelOverlayElement)container.GetChild(Name + "/TextBoxCaptionBar");
			mCaptionBar.Width = (width - 4f);
			mCaptionTextArea = (Mogre.TextAreaOverlayElement)mCaptionBar.GetChild(mCaptionBar.Name + "/TextBoxCaption");
			setCaption(caption);
			mScrollTrack = (Mogre.BorderPanelOverlayElement)container.GetChild(Name + "/TextBoxScrollTrack");
			mScrollHandle = (Mogre.PanelOverlayElement)mScrollTrack.GetChild(mScrollTrack.Name + "/TextBoxScrollHandle");
			mScrollHandle.Hide();
			mDragging = false;
			mScrollPercentage = 0f;
			mStartingLine = 0;
			mPadding = 15f;
			mText = "";

#if OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
			mCaptionTextArea.setCharHeight(mCaptionTextArea.CharHeight - 3);
#endif
			refitContents();
		}

		public void setPadding(float padding)
		{
			mPadding = padding;
			refitContents();
		}

		public float getPadding()
		{
			return mPadding;
		}

		public string getCaption()
		{
			return mCaptionTextArea.Caption;
		}

		public void setCaption(string caption)
		{
			mCaptionTextArea.Caption = (caption);
		}

		public string getText()
		{
			return mText;
		}

		//        -----------------------------------------------------------------------------
		//		| Sets text box content. Most of this method is for wordwrap.
		//		-----------------------------------------------------------------------------
		public void setText(string text)
		{
			mText = text;
			mLines.Clear();

			//Mogre.Font font = (Mogre.Font)Mogre.FontManager.Singleton.getByName(mTextArea.FontName).getPointer();
			Mogre.FontPtr font = null;
			if (Mogre.FontManager.Singleton.ResourceExists(mTextArea.FontName))
			{
				font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(mTextArea.FontName);
				if (!font.IsLoaded)
				{
					font.Load();
				}
			}
			else
			{
				OGRE_EXCEPT("this font:", mTextArea.FontName, "is not exist");
			}

			string current = DisplayStringToString(text);
			bool firstWord = true;
			int lastSpace = 0;
			int lineBegin = 0;
			float lineWidth = 0;
			float rightBoundary = element.Width - 2 * mPadding + mScrollTrack.Left + 10f;

			for (int i = 0; i < current.Length; i++)
			{
				if (current[i] == ' ')
				{
					if (mTextArea.SpaceWidth != 0)
						lineWidth += mTextArea.SpaceWidth;
					else
						lineWidth += font.GetGlyphAspectRatio(' ') * mTextArea.CharHeight;
					firstWord = false;
					lastSpace = i;
				}
				else if (current[i] == '\n')
				{
					firstWord = true;
					lineWidth = 0;
					mLines.Add(current.Substring(lineBegin, i - lineBegin));
					lineBegin = i + 1;
				}
				else
				{
					// use glyph information to calculate line width
					lineWidth += font.GetGlyphAspectRatio(current[i]) * mTextArea.CharHeight;
					if (lineWidth > rightBoundary)
					{
						if (firstWord)
						{
							current.Insert(i, "\n");
							i = i - 1;
						}
						else
						{
							//current[lastSpace] = '\n';
							//i = lastSpace - 1;

							char[] str = current.ToCharArray();
							str[lastSpace] = '\n';
							current = new String(str).ToString();
							i = lastSpace - 1;
						}
					}
				}
			}

			mLines.Add(current.Substring(lineBegin));

			uint maxLines = getHeightInLines();

			if (mLines.Count > maxLines) // if too much text, filter based on scroll percentage
			{
				mScrollHandle.Show();
				filterLines();
			}
			else // otherwise just show all the text
			{
				mTextArea.Caption = (current);
				mScrollHandle.Hide();
				mScrollPercentage = 0f;
				mScrollHandle.Top = (0f);
			}
		}

		//        -----------------------------------------------------------------------------
		//		| Sets text box content horizontal alignment.
		//		-----------------------------------------------------------------------------
		public void setTextAlignment(Mogre.TextAreaOverlayElement.Alignment ta)
		{
			if (ta == Mogre.TextAreaOverlayElement.Alignment.Left)
				mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_LEFT);
			else if (ta == Mogre.TextAreaOverlayElement.Alignment.Center)
				mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
			else
				mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
			refitContents();
		}

		public void clearText()
		{
			setText("");
		}

		public void appendText(string text)
		{
			setText(getText() + text);
		}

		//        -----------------------------------------------------------------------------
		//		| Makes adjustments based on new padding, size, or alignment info.
		//		-----------------------------------------------------------------------------
		public void refitContents()
		{
			mScrollTrack.Height = (element.Height - mCaptionBar.Height - 20f);
			mScrollTrack.Top = (mCaptionBar.Height + 10f);

			mTextArea.Top = (mCaptionBar.Height + mPadding - 5f);
			if (mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_RIGHT)
				mTextArea.Left = (-mPadding + mScrollTrack.Left);
			else if (mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_LEFT)
				mTextArea.Left = (mPadding);
			else
				mTextArea.Left = (mScrollTrack.Left / 2f);

			setText(getText());
		}

		//        -----------------------------------------------------------------------------
		//		| Sets how far scrolled down the text is as a percentage.
		//		-----------------------------------------------------------------------------
		public void setScrollPercentage(float percentage)
		{
			mScrollPercentage = UIMathHelper.clamp<float>(percentage, 0f, 1f); //Mogre.Math.Clamp<float>(percentage, 0, 1);
			mScrollHandle.Top = ((int)(percentage * (mScrollTrack.Height - mScrollHandle.Height)));
			filterLines();
		}

		//        -----------------------------------------------------------------------------
		//		| Gets how far scrolled down the text is as a percentage.
		//		-----------------------------------------------------------------------------
		public float getScrollPercentage()
		{
			return mScrollPercentage;
		}

		//        -----------------------------------------------------------------------------
		//		| Gets how many lines of text can fit in this window.
		//		-----------------------------------------------------------------------------
		public uint getHeightInLines()
		{
			return (uint)((element.Height - 2f * mPadding - mCaptionBar.Height + 5f) / mTextArea.CharHeight);
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (!mScrollHandle.IsVisible) // don't care about clicks if text not scrollable
				return;

			Mogre.Vector2 co = Widget.CursorOffset(mScrollHandle, cursorPos);

			if (co.SquaredLength <= 81f)
			{
				mDragging = true;
				mDragOffset = co.y;
			}
			else if (Widget.IsCursorOver(mScrollTrack, cursorPos))
			{
				float newTop = mScrollHandle.Top + co.y;
				float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
				mScrollHandle.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

				// update text area contents based on new scroll percentage
				mScrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
				filterLines();
			}
		}

		public override void CursorReleased(Mogre.Vector2 cursorPos)
		{
			mDragging = false;
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			if (mDragging)
			{
				Mogre.Vector2 co = Widget.CursorOffset(mScrollHandle, cursorPos);
				float newTop = mScrollHandle.Top + co.y - mDragOffset;
				float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
				mScrollHandle.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

				// update text area contents based on new scroll percentage
				mScrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
				filterLines();
			}
		}

		public override void FocusLost()
		{
			mDragging = false; // stop dragging if cursor was lost
		}


		//        -----------------------------------------------------------------------------
		//		| Decides which lines to show.
		//		-----------------------------------------------------------------------------
		protected void filterLines()
		{
			string shown = "";
			uint maxLines = getHeightInLines();
			uint newStart = (uint)(mScrollPercentage * (mLines.Count - maxLines) + 0.5f);

			mStartingLine = newStart;

			for (int i = 0; i < maxLines; i++)
			{
				shown += mLines[(int)mStartingLine + i] + "\n";
			}

			mTextArea.Caption = (shown); // show just the filtered lines
		}

		protected Mogre.TextAreaOverlayElement mTextArea;
		protected Mogre.BorderPanelOverlayElement mCaptionBar;
		protected Mogre.TextAreaOverlayElement mCaptionTextArea;
		protected Mogre.BorderPanelOverlayElement mScrollTrack;
		protected Mogre.PanelOverlayElement mScrollHandle;
		protected string mText = "";
		protected StringVector mLines = new StringVector();
		protected float mPadding = 0f;
		protected bool mDragging;
		protected float mScrollPercentage = 0f;
		protected float mDragOffset = 0f;
		protected uint mStartingLine;
	}
}
