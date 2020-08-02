using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;

namespace OpenMB.UI.Widgets
{
    public class InputBoxWidget : TextWidget
	{
		private BorderPanelOverlayElement inputBoxElement;
		private TextAreaOverlayElement captionTextAreaElement;
		private TextAreaOverlayElement contentTextAreaElement;
		private BorderPanelOverlayElement inputCursorElement;
		private bool isFitToContents;
		private bool isTextMode;
        private string originalText;
		private int init_tick = 120;
		private int tick = 0;

		public override string Text
        {
            get { return originalText; }
            set { originalText = value; }
        }

		public InputBoxWidget(string name, string caption, float width, float boxWidth, string text = null)
		{
			isTextMode = false;
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/InputBox", "BorderPanel", name);
			isFitToContents = false;

			inputBoxElement = (BorderPanelOverlayElement)((OverlayContainer)element).GetChild(name + "/InputBoxText");
			inputBoxElement.Width = width - 10;

			captionTextAreaElement = (TextAreaOverlayElement)((OverlayContainer)element).GetChild(name + "/InputBoxCaption");
			captionTextAreaElement.Width = width - inputBoxElement.Width;

			contentTextAreaElement = (TextAreaOverlayElement)inputBoxElement.GetChild(name + "/InputBoxText/InputBoxContentText");
			contentTextAreaElement.Width = inputBoxElement.Width;

			inputCursorElement = (BorderPanelOverlayElement)inputBoxElement.GetChild(name + "/InputBoxText/InputBoxCursor");
			inputCursorElement.Width = 2;
			inputCursorElement.Hide();

			element.Width = width;
			originalText = string.Empty;

            if (boxWidth > 0)
			{
				if (width <= 0) { isFitToContents = true; }
				inputBoxElement.Width = boxWidth;
				inputBoxElement.Top = 2;
				inputBoxElement.Left = width - boxWidth - 5;
				element.Height = inputBoxElement.Height + 4;
				captionTextAreaElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
				captionTextAreaElement.SetAlignment(TextAreaOverlayElement.Alignment.Left);
				captionTextAreaElement.Left = 12;
				captionTextAreaElement.Top = 9;
			}

			if (!string.IsNullOrEmpty(text))
			{
				contentTextAreaElement.Caption = text;
				originalText = text;
			}

			setCaption(caption);
		}

        public void setCaption(string caption) {
            captionTextAreaElement.Caption = caption;
            if (isFitToContents) {
                element.Width = GetCaptionWidth(caption, ref captionTextAreaElement) + inputBoxElement.Width + 23;
                inputBoxElement.Left = element.Width - inputBoxElement.Width - 5;
            }
        }

		public override void CursorMoved(Vector2 cursorPos)
		{
			if (IsCursorOver(inputBoxElement, cursorPos))
			{
				UIManager.Instance.ChangeCursor("Edit");
			}
			else
			{
				UIManager.Instance.ChangeCursor("Normal");
			}
		}

		public override void CursorPressed(Vector2 cursorPos)
        {
            //Click the text area so that we can input
            if (IsCursorOver(inputBoxElement, cursorPos))
            {
                isTextMode = true;
                inputBoxElement.MaterialName = "SdkTrays/MiniTextBox/Press";
                inputBoxElement.BorderMaterialName = "SdkTrays/MiniTextBox/Press";
				inputCursorElement.Show();
				tick = init_tick;
			}
            else
            {
                isTextMode = false;
                inputBoxElement.MaterialName = "SdkTrays/MiniTextBox";
                inputBoxElement.BorderMaterialName = "SdkTrays/MiniTextBox";
				inputCursorElement.Hide();
				tick = 0;
			}
        }

        public override void KeyPressed(Vector2 mousePos, KeyEvent arg)
        {
			uint text = arg.text;
            if (isTextMode && IsCursorOver(inputBoxElement, mousePos))
            {
                string str = Utilities.Helper.ConvertUintToString(text);

                originalText += str;//original text
                contentTextAreaElement.Caption += str;//cut text
                float textLength = GetCaptionWidth(contentTextAreaElement.Caption, ref contentTextAreaElement);
                if (textLength > inputBoxElement.Width)
                {
                    float offset = textLength - inputBoxElement.Width;
                    contentTextAreaElement.Caption = contentTextAreaElement.Caption.Remove(0, (int)offset);
                }
				calculateInputCursorPosition(str);
			}
        }

		private void calculateInputCursorPosition(string str)
		{
			var totalLeft = inputCursorElement.Left + GetCaptionWidth(str, ref contentTextAreaElement);
			if (totalLeft >= inputBoxElement.Width)
			{
				inputCursorElement.Left = inputBoxElement.Width - 8;
			}
			else
			{
				inputCursorElement.Left = totalLeft;
			}
		}

		public override void Update()
		{
			if (!isTextMode)
			{
				return;
			}
			if (tick == init_tick)
			{
				if (inputCursorElement.IsVisible)
					inputCursorElement.Hide();
				else
					inputCursorElement.Show();
				tick = 0;
			}
			else
			{
				tick++;
			}
		}
	}
}
