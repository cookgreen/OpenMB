using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;

namespace AMOFGameEngine.Widgets
{
    public class InputBox : Widget
    {
        protected BorderPanelOverlayElement mInputBoxText;
        protected TextAreaOverlayElement mTextArea;
        protected TextAreaOverlayElement mSmallTextArea;
        protected BorderPanelOverlayElement mScrollTrack;
        protected PanelOverlayElement mScrollHandle;
        protected List<BorderPanelOverlayElement> mItemElements;
        protected uint mMaxItemsShown;
        protected uint mItemsShown;
        protected bool mCursorOver;
        protected bool mFitToContents;
        protected bool mDragging;
        protected List<string> mItems;
        protected int mSelectionIndex;
        protected int mHighlightIndex;
        protected int mDisplayIndex;
        protected float mDragOffset;
        protected bool isTextMode;
        private string mText;
        private bool bOnlyAcceptNum;

        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }

        public InputBox(String name, string caption, float width, float boxWidth,string text=null, bool onlyAcceptNum=false)
        {
            this.isTextMode = false;
            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/InputBox", "BorderPanel", name);
            this.mHighlightIndex = 0;
            this.mDisplayIndex = 0;
            this.mDragOffset = 0.0f;
            this.mSelectionIndex = -1;
            this.mFitToContents = false;
            this.mCursorOver = false;
            this.mDragging = false;
            this.mItemsShown = 0;
            this.mTextArea = (TextAreaOverlayElement)((OverlayContainer)this.mElement).GetChild(name + "/InputBoxCaption");
            this.mInputBoxText = (BorderPanelOverlayElement)((OverlayContainer)this.mElement).GetChild(name + "/InputBoxText");
            this.mInputBoxText.Width = width - 10;
            this.mSmallTextArea = (TextAreaOverlayElement)this.mInputBoxText.GetChild(name + "/InputBoxText/InputBoxSmallText");
            this.mElement.Width = width;
            this.mItemElements = new List<BorderPanelOverlayElement>();
            this.mText = string.Empty;
            this.bOnlyAcceptNum = onlyAcceptNum;

            if (boxWidth > 0)
            {
                if (width <= 0) { this.mFitToContents = true; }
                this.mInputBoxText.Width = boxWidth;
                this.mInputBoxText.Top = 2;
                this.mInputBoxText.Left = width - boxWidth - 5;
                this.mElement.Height = this.mInputBoxText.Height + 4;
                this.mTextArea.HorizontalAlignment = GuiHorizontalAlignment.GHA_LEFT;
                this.mTextArea.SetAlignment(TextAreaOverlayElement.Alignment.Left);
                this.mTextArea.Left = 12;
                this.mTextArea.Top = 10;
            }

            if (!string.IsNullOrEmpty(text))
            {
                mSmallTextArea.Caption = text;
            }

            this.setCaption(caption);
        }

        public void setCaption(string caption) {
            this.mTextArea.Caption = caption;
            if (this.mFitToContents) {
                this.mElement.Width = Widget.getCaptionWidth(caption, ref this.mTextArea) + this.mInputBoxText.Width + 23;
                this.mInputBoxText.Left = this.mElement.Width - this.mInputBoxText.Width - 5;
            }
        }

        public override void _cursorPressed(Vector2 cursorPos)
        {
            //Click the text area so that we can input

            if (isCursorOver(mInputBoxText, cursorPos))
            {
                isTextMode = true;
                mInputBoxText.MaterialName = "SdkTrays/MiniTextBox/Press";
                mInputBoxText.BorderMaterialName = "SdkTrays/MiniTextBox/Press";
            }
            else
            {
                isTextMode = false;
                mInputBoxText.MaterialName = "SdkTrays/MiniTextBox";
                mInputBoxText.BorderMaterialName = "SdkTrays/MiniTextBox";
            }
        }

        public override void _keyPressed(uint text)
        {
            if (isTextMode)
            {
                string str = GameTrayHelper.ConvertUintToString(text);
                if (!bOnlyAcceptNum)
                {
                    mText += str;//original text
                    mSmallTextArea.Caption += str;//cut text
                    float textLength = Widget.getCaptionWidth(mSmallTextArea.Caption, ref mSmallTextArea);
                    float textBoxLength = mSmallTextArea.Width;
                    if (textLength > mInputBoxText.Width)
                    {
                        float offset = textLength - mInputBoxText.Width;
                        mSmallTextArea.Caption = mSmallTextArea.Caption.Remove(0, (int)offset);
                    }
                }
                else
                {
                    int result;
                    if (int.TryParse(str, out result))
                    {
                        mText += str;//original text
                        mSmallTextArea.Caption += str;//cut text
                        float textLength = Widget.getCaptionWidth(mSmallTextArea.Caption, ref mSmallTextArea);
                        float textBoxLength = mSmallTextArea.Width;
                        if (textLength > mInputBoxText.Width)
                        {
                            float offset = textLength - mInputBoxText.Width;
                            mSmallTextArea.Caption = mSmallTextArea.Caption.Remove(0, (int)offset);
                        }
                    }
                }
            }
        }

        public override void _keyPressed(KeyCode key)
        {
            if (isTextMode)
            {
                if (key == KeyCode.KC_DELETE)
                {
                    if (mSmallTextArea.Caption.Length > 0)
                    {
                        mSmallTextArea.Caption = mSmallTextArea.Caption.Remove(mSmallTextArea.Caption.Length - 1);
                    }
                }
            }
        }
    }
}
