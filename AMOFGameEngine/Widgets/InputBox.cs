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

        public string Text
        {
            get { return mSmallTextArea.Caption; }
            set { mSmallTextArea.Caption = value; }
        }

        public InputBox(String name, string caption, float width,float boxWidth)
        {
            this.isTextMode = false;
            this.mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/InputBox", "BorderPanel", name);
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
            }
            else
            {
                isTextMode = false;
            }
        }

        public override void _keyPressed(uint text)
        {
            if (isTextMode)
            {
                string str = new string(System.Text.Encoding.Default.GetChars(BitConverter.GetBytes(text)));
                mSmallTextArea.Caption += str;
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
