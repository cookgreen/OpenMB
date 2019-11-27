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
        protected BorderPanelOverlayElement inputBoxTextElement;
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
			this.isTextMode = false;
			this.element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/InputBox", "BorderPanel", name);
			this.highlightIndex = 0;
			this.displayIndex = 0;
			this.dragOffset = 0.0f;
			this.selectionIndex = -1;
			this.isFitToContents = false;
			this.isCursorOver = false;
			this.isDragging = false;
			this.itemsShown = 0;
			this.textAreaElement = (TextAreaOverlayElement)((OverlayContainer)this.element).GetChild(name + "/InputBoxCaption");
			this.inputBoxTextElement = (BorderPanelOverlayElement)((OverlayContainer)this.element).GetChild(name + "/InputBoxText");
			this.inputBoxTextElement.Width = width - 10;
			this.smallTextAreaElement = (TextAreaOverlayElement)this.inputBoxTextElement.GetChild(name + "/InputBoxText/InputBoxSmallText");
			this.element.Width = width;
			this.itemElements = new List<BorderPanelOverlayElement>();
			this.mText = string.Empty;
			this.isOnlyAcceptNum = onlyAcceptNum;

			if (boxWidth > 0)
			{
				if (width <= 0) { this.isFitToContents = true; }
				this.inputBoxTextElement.Width = boxWidth;
				this.inputBoxTextElement.Top = 2;
				this.inputBoxTextElement.Left = width - boxWidth - 5;
				this.element.Height = this.inputBoxTextElement.Height + 4;
				this.textAreaElement.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
				this.textAreaElement.SetAlignment(TextAreaOverlayElement.Alignment.Left);
				this.textAreaElement.Left = 12;
				this.textAreaElement.Top = 10;
			}

			if (!string.IsNullOrEmpty(text))
			{
				smallTextAreaElement.Caption = text;
				mText = text;
			}

			this.setCaption(caption);
		}

        public void setCaption(string caption) {
            this.textAreaElement.Caption = caption;
            if (this.isFitToContents) {
                this.element.Width = Widget.GetCaptionWidth(caption, ref this.textAreaElement) + this.inputBoxTextElement.Width + 23;
                this.inputBoxTextElement.Left = this.element.Width - this.inputBoxTextElement.Width - 5;
            }
        }

        public override void CursorPressed(Vector2 cursorPos)
        {
            //Click the text area so that we can input

            if (IsCursorOver(inputBoxTextElement, cursorPos))
            {
                isTextMode = true;
                inputBoxTextElement.MaterialName = "SdkTrays/MiniTextBox/Press";
                inputBoxTextElement.BorderMaterialName = "SdkTrays/MiniTextBox/Press";
            }
            else
            {
                isTextMode = false;
                inputBoxTextElement.MaterialName = "SdkTrays/MiniTextBox";
                inputBoxTextElement.BorderMaterialName = "SdkTrays/MiniTextBox";
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
                    float textLength = Widget.GetCaptionWidth(smallTextAreaElement.Caption, ref smallTextAreaElement);
                    float textBoxLength = smallTextAreaElement.Width;
                    if (textLength > inputBoxTextElement.Width)
                    {
                        float offset = textLength - inputBoxTextElement.Width;
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
                        float textLength = Widget.GetCaptionWidth(smallTextAreaElement.Caption, ref smallTextAreaElement);
                        float textBoxLength = smallTextAreaElement.Width;
                        if (textLength > inputBoxTextElement.Width)
                        {
                            float offset = textLength - inputBoxTextElement.Width;
                            smallTextAreaElement.Caption = smallTextAreaElement.Caption.Remove(0, (int)offset);
                        }
                    }
                }
            }
        }
    }
}
