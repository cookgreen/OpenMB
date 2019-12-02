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
	public class StaticMultiLineTextBoxWidget : Widget
	{
		protected TextAreaOverlayElement textArea;
		protected BorderPanelOverlayElement captionBar;
		protected TextAreaOverlayElement captionTextArea;
		protected BorderPanelOverlayElement scrollTrack;
		protected PanelOverlayElement scrollHandle;
		protected string originalText = "";
		protected StringVector lines = new StringVector();
		protected float contentPadding = 0f;
		protected bool dragging;
		protected float scrollPercentage = 0f;
		protected float dragOffset = 0f;
		protected uint startingLine;

		public StaticMultiLineTextBoxWidget(string name, string caption, float width, float height)
		{
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/TextBox", "BorderPanel", name);
			element.Width = (width);
			element.Height = (height);
			OverlayContainer container = (OverlayContainer)element;
			textArea = (TextAreaOverlayElement)container.GetChild(Name + "/TextBoxText");
			captionBar = (BorderPanelOverlayElement)container.GetChild(Name + "/TextBoxCaptionBar");
			captionBar.Width = (width - 4f);
			captionTextArea = (TextAreaOverlayElement)captionBar.GetChild(captionBar.Name + "/TextBoxCaption");
			setCaption(caption);
			scrollTrack = (BorderPanelOverlayElement)container.GetChild(Name + "/TextBoxScrollTrack");
			scrollHandle = (PanelOverlayElement)scrollTrack.GetChild(scrollTrack.Name + "/TextBoxScrollHandle");
			scrollHandle.Hide();
			dragging = false;
			scrollPercentage = 0f;
			startingLine = 0;
			contentPadding = 15f;
			originalText = "";

			refitContents();
		}

		public void setPadding(float padding)
		{
			this.contentPadding = padding;
			refitContents();
		}

		public float getPadding()
		{
			return contentPadding;
		}

		public string getCaption()
		{
			return captionTextArea.Caption;
		}

		public void setCaption(string caption)
		{
			captionTextArea.Caption = (caption);
		}

		public string getText()
		{
			return originalText;
		}

		/// <summary>
		/// Sets text box content. Most of this method is for wordwrap.
		/// </summary>
		/// <param name="text"></param>
		public void setText(string text)
		{
			originalText = text;
			lines.Clear();

			//Mogre.Font font = (Mogre.Font)Mogre.FontManager.Singleton.getByName(mTextArea.FontName).getPointer();
			FontPtr font = null;
			if (FontManager.Singleton.ResourceExists(textArea.FontName))
			{
				font = (FontPtr)FontManager.Singleton.GetByName(textArea.FontName);
				if (!font.IsLoaded)
				{
					font.Load();
				}
			}
			else
			{
				OGRE_EXCEPT("this font:", textArea.FontName, "is not exist");
			}

			string current = DisplayStringToString(text);
			bool firstWord = true;
			int lastSpace = 0;
			int lineBegin = 0;
			float lineWidth = 0;
			float rightBoundary = element.Width - 2 * contentPadding + scrollTrack.Left + 10f;

			for (int i = 0; i < current.Length; i++)
			{
				if (current[i] == ' ')
				{
					if (textArea.SpaceWidth != 0)
						lineWidth += textArea.SpaceWidth;
					else
						lineWidth += font.GetGlyphAspectRatio(' ') * textArea.CharHeight;
					firstWord = false;
					lastSpace = i;
				}
				else if (current[i] == '\n')
				{
					firstWord = true;
					lineWidth = 0;
					lines.Add(current.Substring(lineBegin, i - lineBegin));
					lineBegin = i + 1;
				}
				else
				{
					// use glyph information to calculate line width
					lineWidth += font.GetGlyphAspectRatio(current[i]) * textArea.CharHeight;
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
							current = new string(str).ToString();
							i = lastSpace - 1;
						}
					}
				}
			}

			lines.Add(current.Substring(lineBegin));

			uint maxLines = getHeightInLines();

			if (lines.Count > maxLines) // if too much text, filter based on scroll percentage
			{
				scrollHandle.Show();
				filterLines();
			}
			else // otherwise just show all the text
			{
				textArea.Caption = (current);
				scrollHandle.Hide();
				scrollPercentage = 0f;
				scrollHandle.Top = (0f);
			}
		}

		/// <summary>
		/// Sets text box content horizontal alignment.
		/// </summary>
		/// <param name="alignment"></param>
		public void setTextAlignment(Mogre.TextAreaOverlayElement.Alignment alignment)
		{
			if (alignment == Mogre.TextAreaOverlayElement.Alignment.Left)
				textArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_LEFT);
			else if (alignment == Mogre.TextAreaOverlayElement.Alignment.Center)
				textArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
			else
				textArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
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

		/// <summary>
		/// Makes adjustments based on new padding, size, or alignment info.
		/// </summary>
		public void refitContents()
		{
			scrollTrack.Height = (element.Height - captionBar.Height - 20f);
			scrollTrack.Top = (captionBar.Height + 10f);

			textArea.Top = (captionBar.Height + contentPadding - 5f);
			if (textArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_RIGHT)
				textArea.Left = (-contentPadding + scrollTrack.Left);
			else if (textArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_LEFT)
				textArea.Left = (contentPadding);
			else
				textArea.Left = (scrollTrack.Left / 2f);

			setText(getText());
		}

		/// <summary>
		/// Sets how far scrolled down the text is as a percentage.
		/// </summary>
		/// <param name="percentage"></param>
		public void setScrollPercentage(float percentage)
		{
			scrollPercentage = UIMathHelper.clamp<float>(percentage, 0f, 1f); //Mogre.Math.Clamp<float>(percentage, 0, 1);
			scrollHandle.Top = ((int)(percentage * (scrollTrack.Height - scrollHandle.Height)));
			filterLines();
		}

		/// <summary>
		/// Gets how far scrolled down the text is as a percentage.
		/// </summary>
		/// <returns></returns>
		public float getScrollPercentage()
		{
			return scrollPercentage;
		}

		/// <summary>
		/// Gets how many lines of text can fit in this window.
		/// </summary>
		/// <returns></returns>
		public uint getHeightInLines()
		{
			return (uint)((element.Height - 2f * contentPadding - captionBar.Height + 5f) / textArea.CharHeight);
		}

		public override void CursorPressed(Vector2 cursorPos)
		{
			if (!scrollHandle.IsVisible) // don't care about clicks if text not scrollable
				return;

			Vector2 co = CursorOffset(scrollHandle, cursorPos);

			if (co.SquaredLength <= 81f)
			{
				dragging = true;
				dragOffset = co.y;
			}
			else if (IsCursorOver(scrollTrack, cursorPos))
			{
				float newTop = scrollHandle.Top + co.y;
				float lowerBoundary = scrollTrack.Height - scrollHandle.Height;
				scrollHandle.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

				// update text area contents based on new scroll percentage
				scrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
				filterLines();
			}
		}

		public override void CursorReleased(Mogre.Vector2 cursorPos)
		{
			dragging = false;
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			if (dragging)
			{
				Vector2 co = Widget.CursorOffset(scrollHandle, cursorPos);
				float newTop = scrollHandle.Top + co.y - dragOffset;
				float lowerBoundary = scrollTrack.Height - scrollHandle.Height;
				scrollHandle.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

				// update text area contents based on new scroll percentage
				scrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
				filterLines();
			}
		}

		public override void FocusLost()
		{
			dragging = false; // stop dragging if cursor was lost
		}

		/// <summary>
		/// Decides which lines to show.
		/// </summary>
		protected void filterLines()
		{
			string shown = "";
			uint maxLines = getHeightInLines();
			uint newStart = (uint)(scrollPercentage * (lines.Count - maxLines) + 0.5f);

			startingLine = newStart;

			for (int i = 0; i < maxLines; i++)
			{
				shown += lines[(int)startingLine + i] + "\n";
			}

			textArea.Caption = (shown); // show just the filtered lines
		}
	}
}
