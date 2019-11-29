using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;

namespace OpenMB.Widgets
{
    public class InputBoxWidget : Widget
    {
        protected BorderPanelOverlayElement inputBoxElement;
        protected TextAreaOverlayElement textAreaElement;
        protected TextAreaOverlayElement smallTextAreaElement;
        protected BorderPanelOverlayElement scrollTrackElement;
        protected PanelOverlayElement scrollHandleElement;
        protected List<BorderPanelOverlayElement> itemElements;
        protected uint maxItemsShown;
        protected uint itemsShown;
        protected bool isCursorOver;
        protected bool isFitToContents;
        protected bool isDragging;
        protected List<string> items;
        protected int selectionIndex;
        protected int highlightIndex;
        protected int displayIndex;
        protected float dragOffset;
        protected bool isTextMode;
        private string mText;
        private bool isOnlyAcceptNum;

        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }

		public InputBoxWidget(string name, string caption, float width, float boxWidth, string text = null, bool onlyAcceptNum = false)
		{
			isTextMode = false;
			element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/InputBox", "BorderPanel", name);
			highlightIndex = 0;
			displayIndex = 0;
			dragOffset = 0.0f;
			selectionIndex = -1;
			isFitToContents = false;
			isCursorOver = false;
			isDragging = false;
			itemsShown = 0;
			textAreaElement = (TextAreaOverlayElement)((OverlayContainer)element).GetChild(name + "/InputBoxCaption");
			inputBoxElement = (BorderPanelOverlayElement)((OverlayContainer)element).GetChild(name + "/InputBoxText");
			inputBoxElement.Width = width - 10;
			smallTextAreaElement = (TextAreaOverlayElement)inputBoxElement.GetChild(name + "/InputBoxText/InputBoxSmallText");
			element.Width = width;
			itemElements = new List<BorderPanelOverlayElement>();
			mText = string.Empty;
			isOnlyAcceptNum = onlyAcceptNum;
            textAreaElement.Caption = caption;

            if (boxWidth > 0)
			{
				if (width <= 0) { isFitToContents = true; }
				inputBoxElement.Width = boxWidth;
				inputBoxElement.Top = 2;
				inputBoxElement.Left = width - boxWidth - 5;
				element.Height = this.inputBoxElement.Height + 4;
				textAreaElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
				textAreaElement.SetAlignment(TextAreaOverlayElement.Alignment.Left);
				textAreaElement.Left = 12;
				textAreaElement.Top = 10;
			}

			if (!string.IsNullOrEmpty(text))
			{
				smallTextAreaElement.Caption = text;
				mText = text;
			}

			setCaption(caption);
		}

        public void setCaption(string caption) {
            textAreaElement.Caption = caption;
            if (isFitToContents) {
                element.Width = GetCaptionWidth(caption, ref textAreaElement) + inputBoxElement.Width + 23;
                inputBoxElement.Left = element.Width - inputBoxElement.Width - 5;
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
            }
            else
            {
                isTextMode = false;
                inputBoxElement.MaterialName = "SdkTrays/MiniTextBox";
                inputBoxElement.BorderMaterialName = "SdkTrays/MiniTextBox";
            }
        }

        public override void KeyPressed(uint text)
        {
            if (isTextMode)
            {
                string str = Utilities.Helper.ConvertUintToString(text);
                if (!isOnlyAcceptNum)
                {
                    mText += str;//original text
                    smallTextAreaElement.Caption += str;//cut text
                    float textLength = GetCaptionWidth(smallTextAreaElement.Caption, ref smallTextAreaElement);
                    float textBoxLength = smallTextAreaElement.Width;
                    if (textLength > inputBoxElement.Width)
                    {
                        float offset = textLength - inputBoxElement.Width;
                        smallTextAreaElement.Caption = smallTextAreaElement.Caption.Remove(0, (int)offset);
                    }
                }
                else
                {
                    int result;
                    if (int.TryParse(str, out result))
                    {
                        mText += str;//original text
                        smallTextAreaElement.Caption += str;//cut text
                        float textLength = GetCaptionWidth(smallTextAreaElement.Caption, ref smallTextAreaElement);
                        float textBoxLength = smallTextAreaElement.Width;
                        if (textLength > inputBoxElement.Width)
                        {
                            float offset = textLength - inputBoxElement.Width;
                            smallTextAreaElement.Caption = smallTextAreaElement.Caption.Remove(0, (int)offset);
                        }
                    }
                }
            }
        }
    }
}
