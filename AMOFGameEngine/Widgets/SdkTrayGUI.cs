/*
@Author: rains soft
@Modify: Cook Green
-----------------------------------------------------------------------------
This source file is part of mogre-procedural
For the latest info, see http://code.google.com/p/mogre-procedural/
my blog:http://hi.baidu.com/rainssoft
this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/
about ogre:see http://www.ogre3d.org/
Copyright (c) 2013-2020 rains soft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/

namespace Mogre_Procedural.MogreBites
{
    using System;
    using Mogre;
    using System.Collections.Generic;
    using Math = System.Math;
    using InputContext = MOIS.Mouse;
    using AdvancedMogreFramework.Widgets;

    public enum TrayLocation : int // enumerator values for widget tray anchoring locations
    {
        TL_TOPLEFT,
        TL_TOP,
        TL_TOPRIGHT,
        TL_LEFT,
        TL_CENTER,
        TL_RIGHT,
        TL_BOTTOMLEFT,
        TL_BOTTOM,
        TL_BOTTOMRIGHT,
        TL_NONE
    }

    public enum ButtonState : int // enumerator values for button states
    {
        BS_UP,
        BS_OVER,
        BS_DOWN
    }



    //    =============================================================================
    //	| Listener class for responding to tray events.
    //	=============================================================================
    public class SdkTrayListener
    {

        public virtual void Dispose() {
        }
        public virtual void buttonHit(Button button) {
        }
        public virtual void itemSelected(SelectMenu menu) {
        }
        public virtual void labelHit(Label label) {
        }
        public virtual void sliderMoved(Slider slider) {
        }
        public virtual void checkBoxToggled(CheckBox box) {
        }
        public virtual void okDialogClosed(string message) {
        }
        public virtual void yesNoDialogClosed(string question, bool yesHit) {
        }
    }
    public class SdkTrayMathHelper
    {
        public const double PI = Math.PI;

        public const double SQUARED_PI = PI * PI;

        public const double HALF_PI = 0.5 * PI;

        public const double QUARTER_PI = 0.5 * HALF_PI;

        public const double TWO_PI = 2.0 * PI;

        public const double THREE_PI_HALVES = TWO_PI - HALF_PI;

        public const double DEGTORAD = PI / 180.0;

        public const double RADTODEG = 180.0 / PI;

        public static readonly double SQRTOFTWO = Math.Sqrt(2.0);

        public static readonly double HALF_SQRTOFTWO = 0.5 * SQRTOFTWO;

        /**
        * Gets the difference between two angles
        * This value is always positive (0 - 180)
        *
        * @param angle1
        * @param angle2
        * @return the positive angle difference
        */
        public static float getAngleDifference(float angle1, float angle2) {
            return Math.Abs(wrapAngle(angle1 - angle2));
        }

        /**
        * Gets the difference between two radians
        * This value is always positive (0 - PI)
        *
        * @param radian1
        * @param radian2
        * @return the positive radian difference
        */
        public static double getRadianDifference(double radian1, double radian2) {
            return Math.Abs(wrapRadian(radian1 - radian2));
        }

        /**
        * Wraps the angle between -180 and 180 degrees
        *
        * @param angle to wrap
        * @return -180 > angle <= 180
        */
        public static float wrapAngle(float angle) {
            angle %= 360f;
            if (angle <= -180) {
                return angle + 360;
            }
            else if (angle > 180) {
                return angle - 360;
            }
            else {
                return angle;
            }
        }

        /**
        * Wraps a byte between 0 and 256
        *
        * @param value to wrap
        * @return 0 >= byte < 256
        */
        public static byte wrapByte(int value) {
            value %= 256;
            if (value < 0) {
                value += 256;
            }
            return (byte)value;
        }

        /**
        * Wraps the radian between -PI and PI
        *
        * @param radian to wrap
        * @return -PI > radian <= PI
        */
        public static double wrapRadian(double radian) {
            radian %= TWO_PI;
            if (radian <= -PI) {
                return radian + TWO_PI;
            }
            else if (radian > PI) {
                return radian - TWO_PI;
            }
            else {
                return radian;
            }
        }

        /**
        * Rounds a number to the amount of decimals specified
        *
        * @param input to round
        * @param decimals to round to
        * @return the rounded number
        */
        public static double round(double input, int decimals) {
            double p = Math.Pow(10, decimals);
            return Math.Round(input * p) / p;
        }



        /**
        * Calculates the value at x using linear interpolation
        *
        * @param x the X coord of the value to interpolate
        * @param x1 the X coord of q0
        * @param x2 the X coord of q1
        * @param q0 the first known value (x1)
        * @param q1 the second known value (x2)
        * @return the interpolated value
        */
        public static double lerp(double x, double x1, double x2, double q0, double q1) { return ((x2 - x) / (x2 - x1)) * q0 + ((x - x1) / (x2 - x1)) * q1; }


        /**
* Calculates the value at x,y,z using trilinear interpolation
*
* @param x the X coord of the value to interpolate
* @param y the Y coord of the value to interpolate
* @param z the Z coord of the value to interpolate
* @param q000 the first known value (x1, y1, z1)
* @param q001 the second known value (x1, y2, z1)
* @param q010 the third known value (x1, y1, z2)
* @param q011 the fourth known value (x1, y2, z2)
* @param q100 the fifth known value (x2, y1, z1)
* @param q101 the sixth known value (x2, y2, z1)
* @param q110 the seventh known value (x2, y1, z2)
* @param q111 the eighth known value (x2, y2, z2)
* @param x1 the X coord of q000, q001, q010 and q011
* @param x2 the X coord of q100, q101, q110 and q111
* @param y1 the Y coord of q000, q010, q100 and q110
* @param y2 the Y coord of q001, q011, q101 and q111
* @param z1 the Z coord of q000, q001, q100 and q101
* @param z2 the Z coord of q010, q011, q110 and q111
* @return the interpolated value
*/
        public static double triLerp(double x, double y, double z, double q000, double q001,
        double q010, double q011, double q100, double q101, double q110, double q111,
        double x1, double x2, double y1, double y2, double z1, double z2) {
            double q00 = lerp(x, x1, x2, q000, q100);
            double q01 = lerp(x, x1, x2, q010, q110);
            double q10 = lerp(x, x1, x2, q001, q101);
            double q11 = lerp(x, x1, x2, q011, q111);
            double q0 = lerp(y, y1, y2, q00, q10);
            double q1 = lerp(y, y1, y2, q01, q11);
            return lerp(z, z1, z2, q0, q1);
        }


        static public T clamp<T>(T val, T min, T max) where T : IComparable<T> {

            if (val.CompareTo(min) < 0) { return min; }
            else if (val.CompareTo(max) > 0) { return max; }
            else { return val; }
        }

        public static int floor(double x) {
            int y = (int)x;
            if (x < y) {
                return y - 1;
            }
            return y;
        }

        public static int floor(float x) {
            int y = (int)x;
            if (x < y) {
                return y - 1;
            }
            return y;
        }

        /**
        * Gets the maximum byte value from two values
        *
        * @param value1
        * @param value2
        * @return the maximum value
        */
        public static byte max(byte value1, byte value2) {
            return value1 > value2 ? value1 : value2;
        }

        /**
        * Rounds an integer up to the next power of 2.
        *
        * @param x
        * @return the lowest power of 2 greater or equal to x
        */
        public static int roundUpPow2(int x) {
            if (x <= 0) {
                return 1;
            }
            else if (x > 0x40000000) {
                throw new ArgumentException("Rounding " + x + " to the next highest power of two would exceed the int range");
            }
            else {
                x--;
                x |= x >> 1;
                x |= x >> 2;
                x |= x >> 4;
                x |= x >> 8;
                x |= x >> 16;
                x++;
                return x;
            }
        }

        public static bool isInBlock(Mogre.Vector3 origin, Mogre.Vector3 p, int side) {
            return (p.x >= origin.x && p.x < origin.x + side) && (p.y >= origin.y && p.y < origin.y + side) && (p.z >= origin.z - side && p.z < origin.z);
        }
    }
    public class FontDefault
    {
        public const string Default = "FONT.DEFAULT";
        public const string DefaultBold = "FONT.DEFAULT.BOLD";
        public const string Torchlight = "FONT.TORCHLIGHT";
        public const string Consolas = "FONT.CONSOLAS";
        public static void Load(string fontFileName, string fontRef, int size) {
            // Create the font resources
            //ResourceGroupManager.Singleton.AddResourceLocation("Media/fonts", "FileSystem");
            //Load("Default.ttf", Font.Default, 26);
            //Load("DefaultBold.ttf", Font.DefaultBold, 28);
            //Load("Torchlight.ttf", Font.Torchlight, 36);
            //Load("Consolas.ttf", Font.Consolas, 26);

            ResourcePtr font = FontManager.Singleton.Create(fontRef, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            font.SetParameter("type", "truetype");
            font.SetParameter("source", fontFileName);
            font.SetParameter("size", size.ToString());
            font.SetParameter("resolution", "96");
            font.Load();
        }
    }

    //    =============================================================================
    //	| Abstract base class for all widgets.
    //	=============================================================================
    public class Widget : IDisposable
    {

        public Widget() {
            mTrayLoc = TrayLocation.TL_NONE;
            mElement = null;
            mListener = null;
        }
        /// <summary>
        /// dispose this widget
        /// </summary>
        public virtual void Dispose() {
        }

        public void cleanup() {
            if (mElement != null)
                nukeOverlayElement(mElement);
            mElement = null;
        }

        //        -----------------------------------------------------------------------------
        //		| Static utility method to recursively delete an overlay element plus
        //		| all of its children from the system.
        //		-----------------------------------------------------------------------------
        public static void nukeOverlayElement(OverlayElement element) {
            Mogre.OverlayContainer container = element as Mogre.OverlayContainer;
            if (container != null) {
                List<Mogre.OverlayElement> toDelete = new List<Mogre.OverlayElement>();

                Mogre.OverlayContainer.ChildIterator children = container.GetChildIterator();
                while (children.MoveNext()) {
                    toDelete.Add(children.Current);
                }

                for (int i = 0; i < toDelete.Count; i++) {
                    nukeOverlayElement(toDelete[i]);
                }
            }
            if (element != null) {
                Mogre.OverlayContainer parent = element.Parent;
                if (parent != null)
                    parent.RemoveChild(element.Name);
                Mogre.OverlayManager.Singleton.DestroyOverlayElement(element);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Static utility method to check if the cursor is over an overlay element.
        //		-----------------------------------------------------------------------------
        public static bool isCursorOver(Mogre.OverlayElement element, Mogre.Vector2 cursorPos) {
            return isCursorOver(element, cursorPos, 0f);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: static bool isCursorOver(Ogre::OverlayElement* element, const Ogre::Vector2& cursorPos, Ogre::Real voidBorder = 0)
        public static bool isCursorOver(Mogre.OverlayElement element, Mogre.Vector2 cursorPos, float voidBorder) {
            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
            float l = element._getDerivedLeft() * om.ViewportWidth;
            float t = element._getDerivedTop() * om.ViewportHeight;
            float r = l + element.Width;
            float b = t + element.Height;

            return (cursorPos.x >= l + voidBorder && cursorPos.x <= r - voidBorder && cursorPos.y >= t + voidBorder && cursorPos.y <= b - voidBorder);
        }

        //        -----------------------------------------------------------------------------
        //		| Static utility method used to get the cursor's offset from the center
        //		| of an overlay element in pixels.
        //		-----------------------------------------------------------------------------
        public static Mogre.Vector2 cursorOffset(Mogre.OverlayElement element, Mogre.Vector2 cursorPos) {
            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
            return new Mogre.Vector2(cursorPos.x - (element._getDerivedLeft() * om.ViewportWidth + element.Width / 2), cursorPos.y - (element._getDerivedTop() * om.ViewportHeight + element.Height / 2f));
        }
        //public static Vector2 cursorOffset(Mogre.OverlayContainer containerElement,Vector2 cursorPos)
        //{
        //    Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
        //    return new Mogre.Vector2(cursorPos.x - (containerElement._getDerivedLeft() * om.ViewportWidth + containerElement.Width / 2), cursorPos.y - (containerElement._getDerivedTop() * om.ViewportHeight + containerElement.Height / 2f));
        //}
        //        -----------------------------------------------------------------------------
        //		| Static utility method used to get the width of a caption in a text area.
        //		-----------------------------------------------------------------------------
        public static float getCaptionWidth(string caption, ref Mogre.TextAreaOverlayElement area) {
            Mogre.FontPtr font = null;
            if (Mogre.FontManager.Singleton.ResourceExists(area.FontName)) {
                font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(area.FontName);
                if (!font.IsLoaded) {
                    font.Load();
                }
            }
            else {
                OGRE_EXCEPT("this font:", area.FontName, "is not exist");
            }
            //Font font = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);
            string current = DISPLAY_STRING_TO_STRING(caption);
            float lineWidth = 0f;

            for (int i = 0; i < current.Length; i++) {
                // be sure to provide a line width in the text area
                if (current[i] == ' ') {
                    if (area.SpaceWidth != 0)
                        lineWidth += area.SpaceWidth;
                    else
                        lineWidth += font.GetGlyphAspectRatio(' ') * area.CharHeight;
                }
                else if (current[i] == '\n')
                    break;
                // use glyph information to calculate line width
                else
                    lineWidth += font.GetGlyphAspectRatio(current[i]) * area.CharHeight;
            }

            return (uint)lineWidth;
        }

        protected static void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p + "_" + p_2 + "_" + p_3);
        }

        protected static string DISPLAY_STRING_TO_STRING(string caption) {
#if DISPLAY_STRING_TO_STRING_AlternateDefinition1
			string current = (caption.asUTF8_c_str());
#elif DISPLAY_STRING_TO_STRING_AlternateDefinition2
			string current = (caption.asUTF8());
#elif DISPLAY_STRING_TO_STRING_AlternateDefinition3
			string current = (caption);
#endif
            return caption;
        }

        //        -----------------------------------------------------------------------------
        //		| Static utility method to cut off a string to fit in a text area.
        //		-----------------------------------------------------------------------------
        public static void fitCaptionToArea(string caption, ref Mogre.TextAreaOverlayElement area, float maxWidth) {
            Mogre.FontPtr font = null;
            if (Mogre.FontManager.Singleton.ResourceExists(area.FontName)) {
                font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(area.FontName);
                if (!font.IsLoaded) {
                    font.Load();
                }
            }
            else {
                OGRE_EXCEPT("this font:", area.FontName, "is not exist");
            }
            Mogre.FontPtr f = font;
            string s = DISPLAY_STRING_TO_STRING(caption);
            //int nl = s.find('\n');
            //if (nl != string.npos)
            //	s = s.substr(0, nl);
            int nl = s.IndexOf('\n');
            if (nl != -1) s = s.Substring(0, nl);

            float width = 0;

            for (int i = 0; i < s.Length; i++) {
                if (s[i] == ' ' && area.SpaceWidth != 0)
                    width += area.SpaceWidth;
                else
                    width += f.GetGlyphAspectRatio(s[i]) * area.CharHeight;
                if (width > maxWidth) {
                    s = s.Substring(0, i);
                    break;
                }
            }

            area.Caption = (s);
        }

        public Mogre.OverlayElement getOverlayElement() {
            return mElement;
        }

        public string getName() {
            return mElement.Name;
        }

        public TrayLocation getTrayLocation() {
            return mTrayLoc;
        }

        public void hide() {
            mElement.Hide();
        }

        public void show() {
            mElement.Show();
        }

        public bool isVisible() {
            return mElement.IsVisible;
        }

        // callbacks

        public virtual void _cursorPressed(Mogre.Vector2 cursorPos) {
        }
        public virtual void _cursorReleased(Mogre.Vector2 cursorPos) {
        }
        public virtual void _cursorMoved(Mogre.Vector2 cursorPos) {
        }
        public virtual void _focusLost() {
        }

        // internal methods used by SdkTrayManager. do not call directly.

        public void _assignToTray(TrayLocation trayLoc) {
            mTrayLoc = trayLoc;
        }
        public void _assignListener(SdkTrayListener listener) {
            mListener = listener;
        }


        protected Mogre.OverlayElement mElement;
        protected TrayLocation mTrayLoc;
        protected SdkTrayListener mListener;



    }


    //    =============================================================================
    //	| Basic button class.
    //	=============================================================================
    public class Button : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public Button(string name, string caption, float width) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Button", "BorderPanel", name);
            mBP = (Mogre.BorderPanelOverlayElement)mElement;
            mTextArea = (Mogre.TextAreaOverlayElement)mBP.GetChild(mBP.Name + "/ButtonCaption");

#if  OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
#endif
            mTextArea.Top = (-(mTextArea.CharHeight / 2f));

            if (width > 0f) {
                mElement.Width = (width);
                mFitToContents = false;
            }
            else
                mFitToContents = true;

            setCaption(caption);
            mState = ButtonState.BS_UP;
        }

        public override void Dispose() {
            base.Dispose();
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);
            if (mFitToContents)
                mElement.Width = (getCaptionWidth(caption, ref mTextArea) + mElement.Height - 12f);
        }

        public ButtonState getState() {
            return mState;
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            if (isCursorOver(mElement, cursorPos, 4))
                setState(ButtonState.BS_DOWN);
        }

        public override void _cursorReleased(Mogre.Vector2 cursorPos) {
            if (mState == ButtonState.BS_DOWN) {
                setState(ButtonState.BS_OVER);
                if (mListener != null)
                    mListener.buttonHit(this);
            }
        }

        public override void _cursorMoved(Mogre.Vector2 cursorPos) {
            if (isCursorOver(mElement, cursorPos, 4f)) {
                if (mState == ButtonState.BS_UP)
                    setState(ButtonState.BS_OVER);
            }
            else {
                if (mState != ButtonState.BS_UP)
                    setState(ButtonState.BS_UP);
            }
        }

        public override void _focusLost() {
            setState(ButtonState.BS_UP); // reset button if cursor was lost
        }

        //protected:
        protected void setState(ButtonState bs) {
            if (bs == ButtonState.BS_OVER) {
                mBP.BorderMaterialName = ("SdkTrays/Button/Over");
                mBP.MaterialName = ("SdkTrays/Button/Over");
            }
            else if (bs == ButtonState.BS_UP) {
                mBP.BorderMaterialName = ("SdkTrays/Button/Up");
                mBP.MaterialName = ("SdkTrays/Button/Up");
            }
            else {
                mBP.BorderMaterialName = ("SdkTrays/Button/Down");
                mBP.MaterialName = ("SdkTrays/Button/Down");
            }

            mState = bs;
        }

        protected ButtonState mState;
        protected Mogre.BorderPanelOverlayElement mBP;
        protected Mogre.TextAreaOverlayElement mTextArea;
        protected bool mFitToContents;
    }

    //    =============================================================================
    //	| Scrollable text box widget.
    //	=============================================================================
    public class TextBox : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public TextBox(string name, string caption, float width, float height) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/TextBox", "BorderPanel", name);
            mElement.Width = (width);
            mElement.Height = (height);
            Mogre.OverlayContainer container = (Mogre.OverlayContainer)mElement;
            mTextArea = (Mogre.TextAreaOverlayElement)container.GetChild(getName() + "/TextBoxText");
            mCaptionBar = (Mogre.BorderPanelOverlayElement)container.GetChild(getName() + "/TextBoxCaptionBar");
            mCaptionBar.Width = (width - 4f);
            mCaptionTextArea = (Mogre.TextAreaOverlayElement)mCaptionBar.GetChild(mCaptionBar.Name + "/TextBoxCaption");
            setCaption(caption);
            mScrollTrack = (Mogre.BorderPanelOverlayElement)container.GetChild(getName() + "/TextBoxScrollTrack");
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

        public void setPadding(float padding) {
            mPadding = padding;
            refitContents();
        }

        public float getPadding() {
            return mPadding;
        }

        public string getCaption() {
            return mCaptionTextArea.Caption;
        }

        public void setCaption(string caption) {
            mCaptionTextArea.Caption = (caption);
        }

        public string getText() {
            return mText;
        }

        //        -----------------------------------------------------------------------------
        //		| Sets text box content. Most of this method is for wordwrap.
        //		-----------------------------------------------------------------------------
        public void setText(string text) {
            mText = text;
            mLines.Clear();

            //Mogre.Font font = (Mogre.Font)Mogre.FontManager.Singleton.getByName(mTextArea.FontName).getPointer();
            Mogre.FontPtr font = null;
            if (Mogre.FontManager.Singleton.ResourceExists(mTextArea.FontName)) {
                font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(mTextArea.FontName);
                if (!font.IsLoaded) {
                    font.Load();
                }
            }
            else {
                OGRE_EXCEPT("this font:", mTextArea.FontName, "is not exist");
            }

            string current = DISPLAY_STRING_TO_STRING(text);
            bool firstWord = true;
            int lastSpace = 0;
            int lineBegin = 0;
            float lineWidth = 0;
            float rightBoundary = mElement.Width - 2 * mPadding + mScrollTrack.Left + 10f;

            for (int i = 0; i < current.Length; i++) {
                if (current[i] == ' ') {
                    if (mTextArea.SpaceWidth != 0)
                        lineWidth += mTextArea.SpaceWidth;
                    else
                        lineWidth += font.GetGlyphAspectRatio(' ') * mTextArea.CharHeight;
                    firstWord = false;
                    lastSpace = i;
                }
                else if (current[i] == '\n') {
                    firstWord = true;
                    lineWidth = 0;
                    mLines.Add(current.Substring(lineBegin, i - lineBegin));
                    lineBegin = i + 1;
                }
                else {
                    // use glyph information to calculate line width
                    lineWidth += font.GetGlyphAspectRatio(current[i]) * mTextArea.CharHeight;
                    if (lineWidth > rightBoundary) {
                        if (firstWord) {
                            current.Insert(i, "\n");
                            i = i - 1;
                        }
                        else {
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
        public void setTextAlignment(Mogre.TextAreaOverlayElement.Alignment ta) {
            if (ta == Mogre.TextAreaOverlayElement.Alignment.Left)
                mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_LEFT);
            else if (ta == Mogre.TextAreaOverlayElement.Alignment.Center)
                mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
            else
                mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
            refitContents();
        }

        public void clearText() {
            setText("");
        }

        public void appendText(string text) {
            setText(getText() + text);
        }

        //        -----------------------------------------------------------------------------
        //		| Makes adjustments based on new padding, size, or alignment info.
        //		-----------------------------------------------------------------------------
        public void refitContents() {
            mScrollTrack.Height = (mElement.Height - mCaptionBar.Height - 20f);
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
        public void setScrollPercentage(float percentage) {
            mScrollPercentage = SdkTrayMathHelper.clamp<float>(percentage, 0f, 1f); //Mogre.Math.Clamp<float>(percentage, 0, 1);
            mScrollHandle.Top = ((int)(percentage * (mScrollTrack.Height - mScrollHandle.Height)));
            filterLines();
        }

        //        -----------------------------------------------------------------------------
        //		| Gets how far scrolled down the text is as a percentage.
        //		-----------------------------------------------------------------------------
        public float getScrollPercentage() {
            return mScrollPercentage;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets how many lines of text can fit in this window.
        //		-----------------------------------------------------------------------------
        public uint getHeightInLines() {
            return (uint)((mElement.Height - 2f * mPadding - mCaptionBar.Height + 5f) / mTextArea.CharHeight);
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            if (!mScrollHandle.IsVisible) // don't care about clicks if text not scrollable
                return;

            Mogre.Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);

            if (co.SquaredLength <= 81f) {
                mDragging = true;
                mDragOffset = co.y;
            }
            else if (Widget.isCursorOver(mScrollTrack, cursorPos)) {
                float newTop = mScrollHandle.Top + co.y;
                float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
                mScrollHandle.Top = (SdkTrayMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

                // update text area contents based on new scroll percentage
                mScrollPercentage = SdkTrayMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
                filterLines();
            }
        }

        public override void _cursorReleased(Mogre.Vector2 cursorPos) {
            mDragging = false;
        }

        public override void _cursorMoved(Mogre.Vector2 cursorPos) {
            if (mDragging) {
                Mogre.Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);
                float newTop = mScrollHandle.Top + co.y - mDragOffset;
                float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
                mScrollHandle.Top = (SdkTrayMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

                // update text area contents based on new scroll percentage
                mScrollPercentage = SdkTrayMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
                filterLines();
            }
        }

        public override void _focusLost() {
            mDragging = false; // stop dragging if cursor was lost
        }


        //        -----------------------------------------------------------------------------
        //		| Decides which lines to show.
        //		-----------------------------------------------------------------------------
        protected void filterLines() {
            string shown = "";
            uint maxLines = getHeightInLines();
            uint newStart = (uint)(mScrollPercentage * (mLines.Count - maxLines) + 0.5f);

            mStartingLine = newStart;

            for (int i = 0; i < maxLines; i++) {
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

    //    =============================================================================
    //	| Basic selection menu widget.
    //	=============================================================================
    public class SelectMenu : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public SelectMenu(string name, string caption, float width, float boxWidth, uint maxItemsShown) {
            mHighlightIndex = 0;
            mDisplayIndex = 0;
            mDragOffset = 0.0f;
            mSelectionIndex = -1;
            mFitToContents = false;
            mCursorOver = false;
            mExpanded = false;
            mDragging = false;
            mMaxItemsShown = maxItemsShown;
            mItemsShown = 0;
            mElement = (Mogre.BorderPanelOverlayElement)Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenu", "BorderPanel", name);
            mTextArea = (Mogre.TextAreaOverlayElement)((Mogre.OverlayContainer)mElement).GetChild(name + "/MenuCaption");
            mSmallBox = (Mogre.BorderPanelOverlayElement)((Mogre.OverlayContainer)mElement).GetChild(name + "/MenuSmallBox");
            mSmallBox.Width = (width - 10);
            mSmallTextArea = (Mogre.TextAreaOverlayElement)mSmallBox.GetChild(name + "/MenuSmallBox/MenuSmallText");
            mElement.Width = (width);
            //
#if OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
			mSmallTextArea.setCharHeight(mSmallTextArea.CharHeight - 3);
#endif

            if (boxWidth > 0f) // long style
			{
                if (width <= 0f)
                    mFitToContents = true;
                mSmallBox.Width = (boxWidth);
                mSmallBox.Top = (2f);
                mSmallBox.Left = (width - boxWidth - 5f);
                mElement.Height = (mSmallBox.Height + 4f);
                mTextArea.HorizontalAlignment = (GuiHorizontalAlignment.GHA_LEFT);
                mTextArea.SetAlignment(Mogre.TextAreaOverlayElement.Alignment.Left);
                mTextArea.Left = (12f);
                mTextArea.Top = (10f);
            }

            mExpandedBox = (Mogre.BorderPanelOverlayElement)((Mogre.OverlayContainer)mElement).GetChild(name + "/MenuExpandedBox");
            mExpandedBox.Width = (mSmallBox.Width + 10);
            mExpandedBox.Hide();
            mScrollTrack = (Mogre.BorderPanelOverlayElement)mExpandedBox.GetChild(mExpandedBox.Name + "/MenuScrollTrack");
            mScrollHandle = (Mogre.PanelOverlayElement)mScrollTrack.GetChild(mScrollTrack.Name + "/MenuScrollHandle");

            setCaption(caption);
        }

        public bool isExpanded() {
            return mExpanded;
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);
            if (mFitToContents) {
                mElement.Width = (getCaptionWidth(caption, ref mTextArea) + mSmallBox.Width + 23f);
                mSmallBox.Left = (mElement.Width - mSmallBox.Width - 5f);
            }
        }

        public StringVector getItems() {
            return mItems;
        }

        public int getNumItems() {
            return mItems.Count;
        }

        public void setItems(StringVector items) {
            mItems = items;
            mSelectionIndex = -1;

            for (int i = 0; i < mItemElements.Count; i++) // destroy all the item elements
			{
                nukeOverlayElement(mItemElements[i]);
            }
            mItemElements.Clear();

            mItemsShown = System.Math.Max((uint)2, System.Math.Min(mMaxItemsShown, (uint)mItems.Count));

            for (int i = 0; i < mItemsShown; i++) // create all the item elements
			{
                Mogre.BorderPanelOverlayElement e = (Mogre.BorderPanelOverlayElement)Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenuItem", "BorderPanel", mExpandedBox.Name + "/Item" + (i + 1).ToString());

                e.Top = (6 + i * (mSmallBox.Height - 8));
                e.Width = (mExpandedBox.Width - 32);

                mExpandedBox.AddChild(e);
                mItemElements.Add(e);
            }

            if (!items.IsEmpty)
                selectItem(0, false);
            else
                mSmallTextArea.Caption = ("");
        }

        public void addItem(string item) {
            mItems.Add(item);
            setItems(mItems);
        }

        public void removeItem(string item) {
            //StringVector.Iterator it = new StringVector.Iterator();
            int it = -1;
            //for (it = mItems.begin(); it != mItems.end(); it++)
            for (int i = 0; i < mItems.Count; i++) {
                if (item == mItems[i]) {
                    it = i;
                    break;
                }
            }

            //if (it != mItems.end())
            if (it != -1) {
                mItems.Erase(it);
                if (mItems.Count < mItemsShown) {
                    mItemsShown = (uint)mItems.Count;
                    nukeOverlayElement(mItemElements[mItemElements.Count - 1]);
                    if (mItemElements.Count > 0)
                        mItemElements.RemoveAt(mItemElements.Count - 1);//remove the end
                }
            }
            else {
                string desc = "Menu \"" + getName() + "\" contains no item \"" + item + "\".";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::removeItem");
            }
        }

        public void removeItem(uint index) {
            //stringVector.iterator it = new stringVector.iterator();
            int it = -1;
            //uint i = 0;
            //for (it = mItems.begin(); it != mItems.end(); it++)
            for (int j = 0; j < mItems.Count; j++) {
                if (j == index) {
                    it = j;
                    break;
                }
                //i++;
            }

            //if (it != mItems.end())
            if (it != -1) {
                mItems.Erase(it);
                if (mItems.Count < mItemsShown) {
                    mItemsShown = (uint)mItems.Count;
                    nukeOverlayElement(mItemElements[mItemElements.Count - 1]);
                    mItemElements.RemoveAt(mItemElements.Count - 1);//remove the end
                }
            }
            else {
                string desc = "Menu \"" + getName() + "\" contains no item at position " + (index).ToString() + ".";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::removeItem");
            }
        }

        public void clearItems() {
            mItems.Clear();
            mSelectionIndex = -1;
            mSmallTextArea.Caption = ("");
        }

        public void selectItem(uint index) {
            selectItem(index, true);
        }

        public void selectItem(uint index, bool notifyListener) {
            if (index >= mItems.Count) {
                string desc = "Menu \"" + getName() + "\" contains no item at position " + (index).ToString() + ".";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::selectItem");
            }

            mSelectionIndex = (int)index;
            fitCaptionToArea(mItems[(int)index], ref mSmallTextArea, mSmallBox.Width - mSmallTextArea.Left * 2f);

            if (mListener != null && notifyListener)
                mListener.itemSelected(this);
        }

        public void selectItem(string item) {
            selectItem(item, true);
        }

        public void selectItem(string item, bool notifyListener) {
            for (int i = 0; i < mItems.Count; i++) {
                if (item == mItems[i]) {
                    selectItem((uint)i, notifyListener);
                    return;
                }
            }

            string desc = "Menu \"" + getName() + "\" contains no item \"" + item + "\".";
            OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::selectItem");
        }

        public string getSelectedItem() {
            if (mSelectionIndex == -1) {
                string desc = "Menu \"" + getName() + "\" has no item selected.";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::getSelectedItem");
                return "";
            }
            else
                return mItems[mSelectionIndex];
        }

        public int getSelectionIndex() {
            return mSelectionIndex;
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

            if (mExpanded) {
                if (mScrollHandle.IsVisible) // check for scrolling
				{
                    Mogre.Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);

                    if (co.SquaredLength <= 81f) {
                        mDragging = true;
                        mDragOffset = co.y;
                        return;
                    }
                    else if (Widget.isCursorOver(mScrollTrack, cursorPos)) {
                        float newTop = mScrollHandle.Top + co.y;
                        float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
                        mScrollHandle.Top = (SdkTrayMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

                        float scrollPercentage = SdkTrayMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
                        setDisplayIndex((uint)(scrollPercentage * (mItems.Count - mItemElements.Count) + 0.5f));
                        return;
                    }
                }

                if (!isCursorOver(mExpandedBox, cursorPos, 3f))
                    retract();
                else {
                    float l = mItemElements[0]._getDerivedLeft() * om.ViewportWidth + 5f;
                    float t = mItemElements[0]._getDerivedTop() * om.ViewportHeight + 5f;
                    float r = l + mItemElements[mItemElements.Count - 1].Width - 10f;
                    float b = mItemElements[mItemElements.Count - 1]._getDerivedTop() * om.ViewportHeight + mItemElements[mItemElements.Count - 1].Height - 5;

                    if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b) {
                        if (mHighlightIndex != mSelectionIndex)
                            selectItem((uint)mHighlightIndex);
                        retract();
                    }
                }
            }
            else {
                if (mItems.Count < 2) // don't waste time showing a menu if there's no choice
                    return;

                if (isCursorOver(mSmallBox, cursorPos, 4f)) {
                    mExpandedBox.Show();
                    mSmallBox.Hide();

                    // calculate how much vertical space we need
                    float idealHeight = mItemsShown * (mSmallBox.Height - 8f) + 20f;
                    mExpandedBox.Height = (idealHeight);
                    mScrollTrack.Height = (mExpandedBox.Height - 20f);

                    mExpandedBox.Left = (mSmallBox.Left - 4f);

                    // if the expanded menu goes down off the screen, make it go up instead
                    if (mSmallBox._getDerivedTop() * om.ViewportHeight + idealHeight > om.ViewportHeight) {
                        mExpandedBox.Top = (mSmallBox.Top + mSmallBox.Height - idealHeight + 3f);
                        // if we're in thick style, hide the caption because it will interfere with the expanded menu
                        if (mTextArea.HorizontalAlignment == GuiHorizontalAlignment.GHA_CENTER)
                            mTextArea.Hide();
                    }
                    else
                        mExpandedBox.Top = (mSmallBox.Top + 3f);

                    mExpanded = true;
                    mHighlightIndex = mSelectionIndex;
                    setDisplayIndex((uint)mHighlightIndex);

                    if (mItemsShown < mItems.Count) // update scrollbar position
					{
                        mScrollHandle.Show();
                        float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
                        mScrollHandle.Top = ((int)(mDisplayIndex * lowerBoundary / (mItems.Count - mItemElements.Count)));
                    }
                    else
                        mScrollHandle.Hide();
                }
            }
        }

        public override void _cursorReleased(Mogre.Vector2 cursorPos) {
            mDragging = false;
        }

        public override void _cursorMoved(Mogre.Vector2 cursorPos) {
            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

            if (mExpanded) {
                if (mDragging) {
                    Mogre.Vector2 co = Widget.cursorOffset(mScrollHandle, cursorPos);
                    float newTop = mScrollHandle.Top + co.y - mDragOffset;
                    float lowerBoundary = mScrollTrack.Height - mScrollHandle.Height;
                    mScrollHandle.Top = (SdkTrayMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

                    float scrollPercentage = SdkTrayMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
                    int newIndex = (int)(scrollPercentage * (mItems.Count - mItemElements.Count) + 0.5f);
                    if (newIndex != mDisplayIndex)
                        setDisplayIndex((uint)newIndex);
                    return;
                }

                float l = mItemElements[0]._getDerivedLeft() * om.ViewportWidth + 5f;
                float t = mItemElements[0]._getDerivedTop() * om.ViewportHeight + 5f;
                float r = l + mItemElements[mItemElements.Count - 1].Width - 10f;
                float b = mItemElements[mItemElements.Count - 1]._getDerivedTop() * om.ViewportHeight + mItemElements[mItemElements.Count - 1].Height - 5f;

                if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b) {
                    int newIndex = (int)(mDisplayIndex + (cursorPos.y - t) / (b - t) * mItemElements.Count);
                    if (mHighlightIndex != newIndex) {
                        mHighlightIndex = newIndex;
                        setDisplayIndex((uint)mDisplayIndex);
                    }
                }
            }
            else {
                if (isCursorOver(mSmallBox, cursorPos, 4f)) {
                    mSmallBox.MaterialName = ("SdkTrays/MiniTextBox/Over");
                    mSmallBox.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
                    mCursorOver = true;
                }
                else {
                    if (mCursorOver) {
                        mSmallBox.MaterialName = ("SdkTrays/MiniTextBox");
                        mSmallBox.BorderMaterialName = ("SdkTrays/MiniTextBox");
                        mCursorOver = false;
                    }
                }
            }
        }

        public override void _focusLost() {
            if (mExpandedBox.IsVisible)
                retract();
        }


        //        -----------------------------------------------------------------------------
        //		| Internal method - sets which item goes at the top of the expanded menu.
        //		-----------------------------------------------------------------------------
        protected void setDisplayIndex(uint index) {
            index = (uint)System.Math.Min((int)index, (int)(mItems.Count - mItemElements.Count));
            mDisplayIndex = (int)index;
            Mogre.BorderPanelOverlayElement ie;
            Mogre.TextAreaOverlayElement ta;

            for (int i = 0; i < (int)mItemElements.Count; i++) {
                ie = mItemElements[i];
                ta = (Mogre.TextAreaOverlayElement)ie.GetChild(ie.Name + "/MenuItemText");

                fitCaptionToArea(mItems[mDisplayIndex + i], ref ta, ie.Width - 2f * ta.Left);

                if ((mDisplayIndex + i) == mHighlightIndex) {
                    ie.MaterialName = ("SdkTrays/MiniTextBox/Over");
                    ie.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
                }
                else {
                    ie.MaterialName = ("SdkTrays/MiniTextBox");
                    ie.BorderMaterialName = ("SdkTrays/MiniTextBox");
                }
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Internal method - cleans up an expanded menu.
        //		-----------------------------------------------------------------------------
        protected void retract() {
            mDragging = false;
            mExpanded = false;
            mExpandedBox.Hide();
            mTextArea.Show();
            mSmallBox.Show();
            mSmallBox.MaterialName = ("SdkTrays/MiniTextBox");
            mSmallBox.BorderMaterialName = ("SdkTrays/MiniTextBox");
        }

        protected Mogre.BorderPanelOverlayElement mSmallBox;
        protected Mogre.BorderPanelOverlayElement mExpandedBox;
        protected Mogre.TextAreaOverlayElement mTextArea;
        protected Mogre.TextAreaOverlayElement mSmallTextArea;
        protected Mogre.BorderPanelOverlayElement mScrollTrack;
        protected Mogre.PanelOverlayElement mScrollHandle;
        protected List<Mogre.BorderPanelOverlayElement> mItemElements = new List<Mogre.BorderPanelOverlayElement>();
        protected uint mMaxItemsShown;
        protected uint mItemsShown;
        protected bool mCursorOver;
        protected bool mExpanded;
        protected bool mFitToContents;
        protected bool mDragging;
        protected StringVector mItems = new StringVector();
        protected int mSelectionIndex;
        protected int mHighlightIndex;
        protected int mDisplayIndex;
        protected float mDragOffset = 0f;
    }

    //    =============================================================================
    //	| Basic label widget.
    //	=============================================================================
    public class Label : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public Label(string name, string caption, float width) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Label", "BorderPanel", name);
            mTextArea = (Mogre.TextAreaOverlayElement)((Mogre.OverlayContainer)mElement).GetChild(getName() + "/LabelCaption");
            //
#if OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
#endif
            setCaption(caption);
            if (width <= 0f)
                mFitToTray = true;
            else {
                mFitToTray = false;
                mElement.Width = (width);
            }
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            if (mListener != null && isCursorOver(mElement, cursorPos, 3f))
                mListener.labelHit(this);
        }

        public bool _isFitToTray() {
            return mFitToTray;
        }


        protected Mogre.TextAreaOverlayElement mTextArea;
        protected bool mFitToTray;
    }

    //    =============================================================================
    //	| Basic separator widget.
    //	=============================================================================
    public class Separator : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public Separator(string name, float width) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Separator", "Panel", name);
            if (width <= 0)
                mFitToTray = true;
            else {
                mFitToTray = false;
                mElement.Width = (width);
            }
        }

        public bool _isFitToTray() {
            return mFitToTray;
        }


        protected bool mFitToTray;
    }

    //    =============================================================================
    //	| Basic slider widget.
    //	=============================================================================
    public class Slider : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public Slider(string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps) {
            mDragOffset = 0.0f;
            mValue = 0.0f;
            mMinValue = 0.0f;
            mMaxValue = 0.0f;
            mInterval = 0.0f;
            mDragging = false;
            mFitToContents = false;
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Slider", "BorderPanel", name);
            mElement.Width = (width);
            Mogre.OverlayContainer c = (Mogre.OverlayContainer)mElement;
            mTextArea = (Mogre.TextAreaOverlayElement)c.GetChild(getName() + "/SliderCaption");
            Mogre.OverlayContainer valueBox = (Mogre.OverlayContainer)c.GetChild(getName() + "/SliderValueBox");
            valueBox.Width = (valueBoxWidth);
            valueBox.Left = (-(valueBoxWidth + 5));
            mValueTextArea = (Mogre.TextAreaOverlayElement)valueBox.GetChild(valueBox.Name + "/SliderValueText");
            mTrack = (Mogre.BorderPanelOverlayElement)c.GetChild(getName() + "/SliderTrack");
            mHandle = (Mogre.PanelOverlayElement)mTrack.GetChild(mTrack.Name + "/SliderHandle");
            //
#if OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
			mValueTextArea.setCharHeight(mValueTextArea.CharHeight - 3);
#endif

            if (trackWidth <= 0f) // tall style
			{
                mTrack.Width = (width - 16f);
            }
            else // long style
			{
                if (width <= 0f)
                    mFitToContents = true;
                mElement.Height = (34f);
                mTextArea.Top = (10f);
                valueBox.Top = (2f);
                mTrack.Top = (-23f);
                mTrack.Width = (trackWidth);
                mTrack.HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
                mTrack.Left = (-(trackWidth + valueBoxWidth + 5f));
            }

            setCaption(caption);
            setRange(minValue, maxValue, snaps, false);
        }

        //        -----------------------------------------------------------------------------
        //		| Sets the minimum value, maximum value, and the number of snapping points.
        //		-----------------------------------------------------------------------------
        public void setRange(float minValue, float maxValue, uint snaps) {
            setRange(minValue, maxValue, snaps, true);
        }
        //ORIGINAL LINE: void setRange(Ogre::Real minValue, Ogre::Real maxValue, uint snaps, bool notifyListener = true)
        public void setRange(float minValue, float maxValue, uint snaps, bool notifyListener) {
            mMinValue = minValue;
            mMaxValue = maxValue;

            if (snaps <= 1 || mMinValue >= mMaxValue) {
                mInterval = 0;
                mHandle.Hide();
                mValue = minValue;
                if (snaps == 1)
                    mValueTextArea.Caption = ((mMinValue).ToString());
                else
                    mValueTextArea.Caption = ("");
            }
            else {
                mHandle.Show();
                mInterval = (maxValue - minValue) / (snaps - 1);
                setValue(minValue, notifyListener);
            }
        }

        public string getValueCaption() {
            return mValueTextArea.Caption;
        }

        //        -----------------------------------------------------------------------------
        //		| You can use this method to manually format how the value is displayed.
        //		-----------------------------------------------------------------------------
        public void setValueCaption(string caption) {
            mValueTextArea.Caption = (caption);
        }

        public void setValue(float value) {
            setValue(value, true);
        }
        //ORIGINAL LINE: void setValue(Ogre::Real value, bool notifyListener = true)
        public void setValue(float @value, bool notifyListener) {
            if (mInterval == 0)
                return;

            mValue = SdkTrayMathHelper.clamp<float>(@value, mMinValue, mMaxValue);

            setValueCaption((mValue).ToString());

            if (mListener != null && notifyListener)
                mListener.sliderMoved(this);

            if (!mDragging)
                mHandle.Left = ((int)((mValue - mMinValue) / (mMaxValue - mMinValue) * (mTrack.Width - mHandle.Width)));
        }

        public float getValue() {
            return mValue;
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);

            if (mFitToContents)
                mElement.Width = (getCaptionWidth(caption, ref mTextArea) + mValueTextArea.Parent.Width + mTrack.Width + 26f);
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            if (!mHandle.IsVisible)
                return;

            Mogre.Vector2 co = Widget.cursorOffset(mHandle, cursorPos);

            if (co.SquaredLength <= 81f) {
                mDragging = true;
                mDragOffset = co.x;
            }
            else if (Widget.isCursorOver(mTrack, cursorPos)) {
                float newLeft = mHandle.Left + co.x;
                float rightBoundary = mTrack.Width - mHandle.Width;

                mHandle.Left = (SdkTrayMathHelper.clamp<int>((int)newLeft, 0, (int)rightBoundary));
                setValue(getSnappedValue(newLeft / rightBoundary));
            }
        }

        public override void _cursorReleased(Mogre.Vector2 cursorPos) {
            if (mDragging) {
                mDragging = false;
                mHandle.Left = ((int)((mValue - mMinValue) / (mMaxValue - mMinValue) * (mTrack.Width - mHandle.Width)));
            }
        }

        public override void _cursorMoved(Mogre.Vector2 cursorPos) {
            if (mDragging) {
                Mogre.Vector2 co = Widget.cursorOffset(mHandle, cursorPos);
                float newLeft = mHandle.Left + co.x - mDragOffset;
                float rightBoundary = mTrack.Width - mHandle.Width;

                mHandle.Left = (SdkTrayMathHelper.clamp<int>((int)newLeft, 0, (int)rightBoundary));
                setValue(getSnappedValue(newLeft / rightBoundary));
            }
        }

        public override void _focusLost() {
            mDragging = false;
        }


        //        -----------------------------------------------------------------------------
        //		| Internal method - given a percentage (from left to right), gets the
        //		| value of the nearest marker.
        //		-----------------------------------------------------------------------------
        protected float getSnappedValue(float percentage) {
            percentage = SdkTrayMathHelper.clamp<float>(percentage, 0f, 1f);
            uint whichMarker = (uint)(percentage * (mMaxValue - mMinValue) / mInterval + 0.5f);
            return whichMarker * mInterval + mMinValue;
        }

        protected Mogre.TextAreaOverlayElement mTextArea;
        protected Mogre.TextAreaOverlayElement mValueTextArea;
        protected Mogre.BorderPanelOverlayElement mTrack;
        protected Mogre.PanelOverlayElement mHandle;
        protected bool mDragging;
        protected bool mFitToContents;
        protected float mDragOffset = 0f;
        protected float mValue = 0f;
        protected float mMinValue = 0f;
        protected float mMaxValue = 0f;
        protected float mInterval = 0f;
    }

    //    =============================================================================
    //	| Basic parameters panel widget.
    //	=============================================================================
    public class ParamsPanel : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public ParamsPanel(string name, float width, uint lines) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ParamsPanel", "BorderPanel", name);
            Mogre.OverlayContainer c = (Mogre.OverlayContainer)mElement;
            mNamesArea = (Mogre.TextAreaOverlayElement)c.GetChild(getName() + "/ParamsPanelNames");
            mValuesArea = (Mogre.TextAreaOverlayElement)c.GetChild(getName() + "/ParamsPanelValues");
            //
#if OGRE_PLATFORM_APPLE_IOS
			mNamesArea.setCharHeight(mNamesArea.CharHeight - 3);
			mValuesArea.setCharHeight(mValuesArea.CharHeight - 3);
#endif
            mElement.Width = (width);
            mElement.Height = (mNamesArea.Top * 2f + lines * mNamesArea.CharHeight);
        }

        public void setAllParamNames(StringVector paramNames) {
            mNames = paramNames;
            mValues.Clear();
            mValues.Resize(mNames.Count, "");
            mElement.Height = (mNamesArea.Top * 2 + mNames.Count * mNamesArea.CharHeight);
            updateText();
        }

        public StringVector getAllParamNames() {
            return mNames;
        }

        public void setAllParamValues(StringVector paramValues) {
            mValues = paramValues;
            mValues.Resize(mNames.Count, "");
            updateText();
        }

        public void setParamValue(string paramName, string paramValue) {
            for (int i = 0; i < mNames.Count; i++) {
                if (mNames[i] == DISPLAY_STRING_TO_STRING(paramName)) {
                    mValues[i] = DISPLAY_STRING_TO_STRING(paramValue);

                    updateText();
                    return;
                }
            }

            string desc = "ParamsPanel \"" + getName() + "\" has no parameter \"" + DISPLAY_STRING_TO_STRING(paramName) + "\".";
            OGRE_EXCEPT("Ogre::Exception::ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::setParamValue");
        }

        public void setParamValue(uint index, string paramValue) {
            if (index >= mNames.Count) {
                string desc = "ParamsPanel \"" + getName() + "\" has no parameter at position " + (index).ToString() + ".";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::setParamValue");
            }

            mValues[(int)index] = DISPLAY_STRING_TO_STRING(paramValue);
            updateText();
        }

        public string getParamValue(string paramName) {
            for (int i = 0; i < mNames.Count; i++) {
                if (mNames[i] == DISPLAY_STRING_TO_STRING(paramName)) return mValues[i];
            }

            string desc = "ParamsPanel \"" + getName() + "\" has no parameter \"" + DISPLAY_STRING_TO_STRING(paramName) + "\".";
            OGRE_EXCEPT("Ogre::Exception::ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::getParamValue");
            return "";
        }

        public string getParamValue(uint index) {
            if (index >= mNames.Count) {
                string desc = "ParamsPanel \"" + getName() + "\" has no parameter at position " + (index).ToString() + ".";
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "ParamsPanel::getParamValue");
            }

            return mValues[(int)index];
        }

        public StringVector getAllParamValues() {
            return mValues;
        }


        //        -----------------------------------------------------------------------------
        //		| Internal method - updates text areas based on name and value lists.
        //		-----------------------------------------------------------------------------
        protected void updateText() {
            string namesDS = "";
            string valuesDS = "";

            for (int i = 0; i < mNames.Count; i++) {
                namesDS += (mNames[i] + ":\n");
                valuesDS += (mValues[i] + "\n");
            }

            mNamesArea.Caption = (namesDS);
            mValuesArea.Caption = (valuesDS);
        }

        protected Mogre.TextAreaOverlayElement mNamesArea;
        protected Mogre.TextAreaOverlayElement mValuesArea;
        protected StringVector mNames = new StringVector();
        protected StringVector mValues = new StringVector();
    }

    //    =============================================================================
    //	| Basic check box widget.
    //	=============================================================================
    public class CheckBox : Widget
    {
        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public CheckBox(string name, string caption, float width) {
            mCursorOver = false;
            mFitToContents = (width <= 0f);
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/CheckBox", "BorderPanel", name);
            Mogre.OverlayContainer c = (Mogre.OverlayContainer)mElement;
            mTextArea = (Mogre.TextAreaOverlayElement)c.GetChild(getName() + "/CheckBoxCaption");
            mSquare = (Mogre.BorderPanelOverlayElement)c.GetChild(getName() + "/CheckBoxSquare");
            mX = mSquare.GetChild(mSquare.Name + "/CheckBoxX");
            mX.Hide();
            mElement.Width = (width);
            //
#if  OGRE_PLATFORM_APPLE_IOS
			mTextArea.setCharHeight(mTextArea.CharHeight - 3);
#endif
            setCaption(caption);
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);
            if (mFitToContents)
                mElement.Width = (getCaptionWidth(caption, ref mTextArea) + mSquare.Width + 23f);
        }

        public bool isChecked() {
            return mX.IsVisible;
        }

        public void setChecked(bool dochecked) {
            setChecked(dochecked, true);
        }
        //ORIGINAL LINE: void setChecked(bool checked, bool notifyListener = true)
        public void setChecked(bool @checked, bool notifyListener) {
            if (@checked)
                mX.Show();
            else
                mX.Hide();
            if (mListener != null && notifyListener)
                mListener.checkBoxToggled(this);
        }

        public void toggle() {
            toggle(true);
        }
        //ORIGINAL LINE: void toggle(bool notifyListener = true)
        public void toggle(bool notifyListener) {
            setChecked(!isChecked(), notifyListener);
        }

        public override void _cursorPressed(Mogre.Vector2 cursorPos) {
            if (mCursorOver && mListener != null)
                toggle();
        }

        public override void _cursorMoved(Mogre.Vector2 cursorPos) {
            if (isCursorOver(mSquare, cursorPos, 5f)) {
                if (!mCursorOver) {
                    mCursorOver = true;
                    mSquare.MaterialName = ("SdkTrays/MiniTextBox/Over");
                    mSquare.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
                }
            }
            else {
                if (mCursorOver) {
                    mCursorOver = false;
                    mSquare.MaterialName = ("SdkTrays/MiniTextBox");
                    mSquare.BorderMaterialName = ("SdkTrays/MiniTextBox");
                }
            }
        }

        public override void _focusLost() {
            mSquare.MaterialName = ("SdkTrays/MiniTextBox");
            mSquare.BorderMaterialName = ("SdkTrays/MiniTextBox");
            mCursorOver = false;
        }


        protected Mogre.TextAreaOverlayElement mTextArea;
        protected Mogre.BorderPanelOverlayElement mSquare;
        protected Mogre.OverlayElement mX;
        protected bool mFitToContents;
        protected bool mCursorOver;
    }

    //    =============================================================================
    //	| Custom, decorative widget created from a template.
    //	=============================================================================
    public class DecorWidget : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public DecorWidget(string name, string templateName) {
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate(templateName, "", name);
        }
    }

    //    =============================================================================
    //	| Basic progress bar widget.
    //	=============================================================================
    public class ProgressBar : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public ProgressBar(string name, string caption, float width, float commentBoxWidth) {
            mProgress = 0.0f;
            mElement = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/ProgressBar", "BorderPanel", name);
            mElement.Width = (width);
            Mogre.OverlayContainer c = (Mogre.OverlayContainer)mElement;
            mTextArea = (Mogre.TextAreaOverlayElement)c.GetChild(getName() + "/ProgressCaption");
            Mogre.OverlayContainer commentBox = (Mogre.OverlayContainer)c.GetChild(getName() + "/ProgressCommentBox");
            commentBox.Width = (commentBoxWidth);
            commentBox.Left = (-(commentBoxWidth + 5f));
            mCommentTextArea = (Mogre.TextAreaOverlayElement)commentBox.GetChild(commentBox.Name + "/ProgressCommentText");
            mMeter = c.GetChild(getName() + "/ProgressMeter");
            mMeter.Width = (width - 10f);
            mFill = ((Mogre.OverlayContainer)mMeter).GetChild(mMeter.Name + "/ProgressFill");
            setCaption(caption);
        }

        //        -----------------------------------------------------------------------------
        //		| Sets the progress as a percentage.
        //		-----------------------------------------------------------------------------
        public void setProgress(float progress) {
            mProgress = SdkTrayMathHelper.clamp<float>(progress, 0f, 1f);
            mFill.Width = (System.Math.Max((int)mFill.Height, (int)(mProgress * (mMeter.Width - 2 * mFill.Left))));
        }

        //        -----------------------------------------------------------------------------
        //		| Gets the progress as a percentage.
        //		-----------------------------------------------------------------------------
        public float getProgress() {
            return mProgress;
        }

        public string getCaption() {
            return mTextArea.Caption;
        }

        public void setCaption(string caption) {
            mTextArea.Caption = (caption);
        }

        public string getComment() {
            return mCommentTextArea.Caption;
        }

        public void setComment(string comment) {
            mCommentTextArea.Caption = (comment);
        }



        protected Mogre.TextAreaOverlayElement mTextArea;
        protected Mogre.TextAreaOverlayElement mCommentTextArea;
        protected Mogre.OverlayElement mMeter;
        protected Mogre.OverlayElement mFill;
        protected float mProgress = 0f;
    }

    //    =============================================================================
    //	| Main class to manage a cursor, backdrop, trays and widgets.
    //	=============================================================================
    public class SdkTrayManager : SdkTrayListener, IDisposable
    {
        #region resource load event
        void _HookResourceGroupLoadEvent() {
            _UnHookResourceGroupLoadEvent();
            if (_hasHookResLoad) return;
            _hasHookResLoad = true;
            ResourceGroupManager.Singleton.ResourceGroupScriptingStarted += _ResourceGroupScriptingStarted;
            ResourceGroupManager.Singleton.ScriptParseStarted += _ScriptParseStarted;
            ResourceGroupManager.Singleton.ScriptParseEnded += _ScriptParseEnded;
            ResourceGroupManager.Singleton.ResourceGroupLoadStarted += _ResourceGroupLoadStarted;
            ResourceGroupManager.Singleton.ResourceLoadStarted += _ResourceLoadStarted;
            ResourceGroupManager.Singleton.WorldGeometryStageStarted += _WorldGeometryStageStarted;
            ResourceGroupManager.Singleton.WorldGeometryStageEnded += _WorldGeometryStageEnded;              

        }
        bool _hasHookResLoad = false;
        void _UnHookResourceGroupLoadEvent() {
            if (!_hasHookResLoad) return;
            _hasHookResLoad = false;
            ResourceGroupManager.Singleton.WorldGeometryStageEnded -= _WorldGeometryStageEnded;
            ResourceGroupManager.Singleton.WorldGeometryStageStarted -= _WorldGeometryStageStarted;
            ResourceGroupManager.Singleton.ResourceLoadStarted -= _ResourceLoadStarted;
            ResourceGroupManager.Singleton.ResourceGroupLoadStarted -= _ResourceGroupLoadStarted;
            ResourceGroupManager.Singleton.ScriptParseEnded -= _ScriptParseEnded;
            ResourceGroupManager.Singleton.ScriptParseStarted -= _ScriptParseStarted;
            ResourceGroupManager.Singleton.ResourceGroupScriptingStarted -= _ResourceGroupScriptingStarted;              
            
        }
        
        void _ResourceGroupScriptingStarted(string groupName, uint scriptCount) {
            //Debug.Assert(numGroupsInit > 0, "You stated you were not going to init any groups, but you did! Divide by zero would follow...");
            //progressBarInc = progressBarMaxSize * initProportion / (float)scriptCount;
            //progressBarInc /= numGroupsInit;
            //loadingDescriptionElement.Caption = "Parsing scripts...";
            //window.Update();
            resourceGroupScriptingStarted(groupName,(int)scriptCount);
        }

        void _ScriptParseStarted(string scriptName, out bool skipThisScript) {
            //loadingCommentElement.Caption = scriptName;
            //window.Update();
            skipThisScript = false;
            scriptParseStarted(scriptName, skipThisScript);           
        }

        void _ScriptParseEnded(string scriptName, bool skipped) {
            //loadingBarElement.Width += progressBarInc;
            //window.Update();
            scriptParseEnded(scriptName, skipped);
        }

        void _ResourceGroupLoadStarted(string groupName, uint resourceCount) {
            //Debug.Assert(numGroupsLoad > 0, "You stated you were not going to load any groups, but you did! Divide by zero would follow...");
            //progressBarInc = progressBarMaxSize * (1 - initProportion) / (float)resourceCount;
            //progressBarInc /= numGroupsLoad;
            //loadingDescriptionElement.Caption = "Loading resources...";
            //window.Update();
            resourceGroupLoadStarted(groupName, (int)resourceCount);
        }

        void _ResourceLoadStarted(ResourcePtr resource) {
            //loadingCommentElement.Caption = resource.Name;
            //window.Update();
            resourceLoadStarted(resource);
        }

        void _WorldGeometryStageStarted(string description) {
            //loadingCommentElement.Caption = description;
            //window.Update();
            worldGeometryStageStarted(description);
        }

        void _WorldGeometryStageEnded() {
            //loadingBarElement.Width += progressBarInc;
            //window.Update();
            worldGeometryStageEnded();
        }


        public void resourceGroupScriptingStarted(string groupName, int scriptCount) {
            mLoadInc = mGroupInitProportion / scriptCount;
            mLoadBar.setCaption("Parsing...");
            windowUpdate();
        }

        public void scriptParseStarted(string scriptName,  bool skipThisScript) {
            mLoadBar.setComment(scriptName);
            windowUpdate();
        }

        public void scriptParseEnded(string scriptName, bool skipped) {
            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
            windowUpdate();
        }

        public void resourceGroupScriptingEnded(string groupName) {
        }

        public void resourceGroupLoadStarted(string groupName, int resourceCount) {
            mLoadInc = mGroupLoadProportion / resourceCount;
            mLoadBar.setCaption("Loading...");
            windowUpdate();
        }

        public void resourceLoadStarted(Mogre.ResourcePtr resource) {
            mLoadBar.setComment(resource.Name);
            windowUpdate();
        }

        public void resourceLoadEnded() {
            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
            windowUpdate();
        }

        public void worldGeometryStageStarted(string description) {
            mLoadBar.setComment(description);
            windowUpdate();
        }

        public void worldGeometryStageEnded() {
            mLoadBar.setProgress(mLoadBar.getProgress() + mLoadInc);
            windowUpdate();
        }

        public void resourceGroupLoadEnded(string groupName) {
        }
        public void windowUpdate() {

//#if OGRE_PLATFORM != OGRE_PLATFORM_APPLE_IOS && OGRE_PLATFORM != OGRE_PLATFORM_NACL
			mWindow.Update();
//#endif
        }
        #endregion
        #region mouse event
        internal void _UnHookMouseEvent() {
            mInputContext.MousePressed -= (mInputContext_MousePressed);
            mInputContext.MouseReleased -= (mInputContext_MouseReleased);
            mInputContext.MouseMoved -= (mInputContext_MouseMoved);
        }
        internal void _HookMouseEvent() {
            _UnHookMouseEvent();
            mInputContext.MousePressed += (mInputContext_MousePressed);
            mInputContext.MouseReleased += (mInputContext_MouseReleased);
            mInputContext.MouseMoved += (mInputContext_MouseMoved);
        }

        bool mInputContext_MouseMoved(MOIS.MouseEvent arg) {
            this.injectMouseMove(arg);
            return true;
        }

        bool mInputContext_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id) {
            this.injectMouseUp(arg, id);
            return true;
        }

        bool mInputContext_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id) {
            this.injectMouseDown(arg, id);
            return true;
        }

        #endregion
        //        -----------------------------------------------------------------------------
        //		| Creates backdrop, cursor, and trays.
        //		-----------------------------------------------------------------------------
        public SdkTrayManager(string name, Mogre.RenderWindow window, InputContext inputContext)
            : this(name, window, inputContext, null) {
        }

        public SdkTrayManager(string name, Mogre.RenderWindow window, InputContext inputContext, SdkTrayListener listener)
            : base() {
            for (int i = 0; i < mWidgets.Length; i++) {
                mWidgets[i] = new List<Widget>();
            }
            mName = name;
            mWindow = window;
            mInputContext = inputContext;
            mListener = listener;
            mWidgetPadding = 8f;
            mWidgetSpacing = 2f;
            mTrayPadding = 0f;
            mTrayDrag = false;
            mExpandedMenu = null;
            mDialog = null;
            mOk = null;
            mYes = null;
            mNo = null;
            mCursorWasVisible = false;
            mFpsLabel = null;
            mStatsPanel = null;
            mLogo = null;
            mLoadBar = null;
            mGroupInitProportion = 0.0f;
            mGroupLoadProportion = 0.0f;
            mLoadInc = 0.0f;
            mTimer = Mogre.Root.Singleton.Timer;
            mLastStatUpdateTime = 0;

            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

            string nameBase = mName + "/";
            nameBase = nameBase.Replace(' ', '_');
            mBackdropLayer = om.Create(nameBase + "BackdropLayer");
            mTraysLayer = om.Create(nameBase + "WidgetsLayer");
            mPriorityLayer = om.Create(nameBase + "PriorityLayer");
            mCursorLayer = om.Create(nameBase + "CursorLayer");
            mBackdropLayer.ZOrder = (100);
            mTraysLayer.ZOrder = (200);
            mPriorityLayer.ZOrder = (300);
            mCursorLayer.ZOrder = (400);

            // make backdrop and cursor overlay containers
            mCursor = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
            mCursorLayer.Add2D(mCursor);
            mBackdrop = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "Backdrop");
            mBackdropLayer.Add2D(mBackdrop);
            mDialogShade = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "DialogShade");
            mDialogShade.MaterialName = ("SdkTrays/Shade");
            mDialogShade.Hide();
            mPriorityLayer.Add2D(mDialogShade);

            string[] trayNames = { "TopLeft", "Top", "TopRight", "Left", "Center", "Right", "BottomLeft", "Bottom", "BottomRight" };

            for (uint i = 0; i < 9; i++) // make the real trays
			{
                mTrays[i] = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Tray", "BorderPanel", nameBase + trayNames[i] + "Tray");
                mTraysLayer.Add2D(mTrays[i]);

                mTrayWidgetAlign[i] = GuiHorizontalAlignment.GHA_CENTER;

                // align trays based on location
                if (i == (int)TrayLocation.TL_TOP || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_BOTTOM)
                    mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
                if (i == (int)TrayLocation.TL_LEFT || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_RIGHT)
                    mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
                if (i == (int)TrayLocation.TL_TOPRIGHT || i == (int)TrayLocation.TL_RIGHT || i == (int)TrayLocation.TL_BOTTOMRIGHT)
                    mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
                if (i == (int)TrayLocation.TL_BOTTOMLEFT || i == (int)TrayLocation.TL_BOTTOM || i == (int)TrayLocation.TL_BOTTOMRIGHT)
                    mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_BOTTOM);
            }

            // create the null tray for free-floating widgets
            mTrays[9] = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "NullTray");
            mTrayWidgetAlign[9] = GuiHorizontalAlignment.GHA_LEFT;
            mTraysLayer.Add2D(mTrays[9]);
            adjustTrays();

            showTrays();
            showCursor();
        }

        //        -----------------------------------------------------------------------------
        //		| Destroys background, cursor, widgets, and trays.
        //		-----------------------------------------------------------------------------
        public override void Dispose() {
            Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

            destroyAllWidgets();//clean up ok

            for (int i = 0; i < mWidgetDeathRow.Count; i++) // delete widgets queued for destruction
			{
                //delete mWidgetDeathRow[i];
                mWidgetDeathRow[i].Dispose();//?is there need?
                mWidgetDeathRow[i] = null;
            }
            mWidgetDeathRow.Clear();

            om.Destroy(mBackdropLayer);
            om.Destroy(mTraysLayer);
            om.Destroy(mPriorityLayer);
            om.Destroy(mCursorLayer);

            closeDialog();
            hideLoadingBar();

            Widget.nukeOverlayElement(mBackdrop);
            Widget.nukeOverlayElement(mCursor);
            Widget.nukeOverlayElement(mDialogShade);

            for (int i = 0; i < 10; i++) {
                Widget.nukeOverlayElement(mTrays[i]);
            }
            base.Dispose();
        }

        //        -----------------------------------------------------------------------------
        //		| Converts a 2D screen coordinate (in pixels) to a 3D ray into the scene.
        //		-----------------------------------------------------------------------------
        public static Mogre.Ray screenToScene(Mogre.Camera cam, Mogre.Vector2 pt) {
            return cam.GetCameraToViewportRay(pt.x, pt.y);
        }

        //        -----------------------------------------------------------------------------
        //		| Converts a 3D scene position to a 2D screen position (in relative screen size, 0.0-1.0).
        //		-----------------------------------------------------------------------------
        public static Mogre.Vector2 sceneToScreen(Mogre.Camera cam, Mogre.Vector3 pt) {
            Mogre.Vector3 result = cam.ProjectionMatrix * cam.ViewMatrix * pt;
            return new Mogre.Vector2((result.x + 1) / 2, (-result.y + 1) / 2);
        }

        // these methods get the underlying overlays and overlay elements

        public Mogre.OverlayContainer getTrayContainer(TrayLocation trayLoc) {
            return mTrays[(int)trayLoc];
        }
        public Mogre.Overlay getBackdropLayer() {
            return mBackdropLayer;
        }
        public Mogre.Overlay getTraysLayer() {
            return mTraysLayer;
        }
        public Mogre.Overlay getCursorLayer() {
            return mCursorLayer;
        }
        public Mogre.OverlayContainer getBackdropContainer() {
            return mBackdrop;
        }
        public Mogre.OverlayContainer getCursorContainer() {
            return mCursor;
        }
        public Mogre.OverlayElement getCursorImage() {
            return mCursor.GetChild(mCursor.Name + "/CursorImage");
        }

        public void setListener(SdkTrayListener listener) {
            mListener = listener;
        }

        public SdkTrayListener getListener() {
            return mListener;
        }

        public void showAll() {
            showBackdrop();
            showTrays();
            showCursor();
        }

        public void hideAll() {
            hideBackdrop();
            hideTrays();
            hideCursor();
        }

        //        -----------------------------------------------------------------------------
        //		| Displays specified material on backdrop, or the last material used if
        //		| none specified. Good for pause menus like in the browser.
        //		-----------------------------------------------------------------------------
        public void showBackdrop() {
            showBackdrop("");
        }
        public void showBackdrop(string materialName) {
            //if (materialName != Ogre::StringUtil::BLANK)
            if (!string.IsNullOrEmpty(materialName))
                mBackdrop.MaterialName = (materialName);
            mBackdropLayer.Show();
        }

        public void hideBackdrop() {
            mBackdropLayer.Hide();
        }

        //        -----------------------------------------------------------------------------
        //		| Displays specified material on cursor, or the last material used if
        //		| none specified. Used to change cursor type.
        //		-----------------------------------------------------------------------------
        public void showCursor() {
            showCursor("");
        }

        public void showCursor(string materialName) {
            if (!string.IsNullOrEmpty(materialName))
                getCursorImage().MaterialName = (materialName);

            if (!mCursorLayer.IsVisible) {
                mCursorLayer.Show();
                refreshCursor();
            }
        }

        public void hideCursor() {
            mCursorLayer.Hide();
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < mWidgets[i].Count; j++) {
                    mWidgets[i][j]._focusLost();
                }
            }

            setExpandedMenu(null);
        }

        //        -----------------------------------------------------------------------------
        //		| Updates cursor position based on unbuffered mouse state. This is necessary
        //		| because if the tray manager has been cut off from mouse events for a time,
        //		| the cursor position will be out of date.
        //		-----------------------------------------------------------------------------
        public void refreshCursor() {
            float x = 0f;
            float y = 0f;
            x = mInputContext.MouseState.X.abs;
            y = mInputContext.MouseState.Y.abs;
            mCursor.SetPosition(x, y);
        }

        public void showTrays() {
            mTraysLayer.Show();
            mPriorityLayer.Show();
        }

        public void hideTrays() {
            mTraysLayer.Hide();
            mPriorityLayer.Hide();

            // give widgets a chance to reset in case they're in the middle of something
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < mWidgets[i].Count; j++) {
                    mWidgets[i][j]._focusLost();
                }
            }

            setExpandedMenu(null);
        }

        public bool isCursorVisible() {
            return mCursorLayer.IsVisible;
        }
        public bool isBackdropVisible() {
            return mBackdropLayer.IsVisible;
        }
        public bool areTraysVisible() {
            return mTraysLayer.IsVisible;
        }

        //        -----------------------------------------------------------------------------
        //		| Sets horizontal alignment of a tray's contents.
        //		-----------------------------------------------------------------------------
        public void setTrayWidgetAlignment(TrayLocation trayLoc, Mogre.GuiHorizontalAlignment gha) {
            mTrayWidgetAlign[(int)trayLoc] = gha;

            for (int i = 0; i < mWidgets[(int)trayLoc].Count; i++) {
                mWidgets[(int)trayLoc][i].getOverlayElement().HorizontalAlignment = (gha);
            }
        }

        // padding and spacing methods

        public void setWidgetPadding(float padding) {
            mWidgetPadding = System.Math.Max((int)padding, 0);
            adjustTrays();
        }

        public void setWidgetSpacing(float spacing) {
            mWidgetSpacing = System.Math.Max((int)spacing, 0);
            adjustTrays();
        }
        public void setTrayPadding(float padding) {
            mTrayPadding = System.Math.Max((int)padding, 0);
            adjustTrays();
        }


        //ORIGINAL LINE: virtual Ogre::Real getWidgetPadding() const
        public virtual float getWidgetPadding() {
            return mWidgetPadding;
        }

        //ORIGINAL LINE: virtual Ogre::Real getWidgetSpacing() const
        public virtual float getWidgetSpacing() {
            return mWidgetSpacing;
        }

        //ORIGINAL LINE: virtual Ogre::Real getTrayPadding() const
        public virtual float getTrayPadding() {
            return mTrayPadding;
        }

        //        -----------------------------------------------------------------------------
        //		| Fits trays to their contents and snaps them to their anchor locations.
        //		-----------------------------------------------------------------------------
        public virtual void adjustTrays() {
            for (int i = 0; i < 9; i++) // resizes and hides trays if necessary
			{
                float trayWidth = 0;
                float trayHeight = mWidgetPadding;
                List<Mogre.OverlayElement> labelsAndSeps = new List<Mogre.OverlayElement>();

                //if (mWidgets[i].empty()) // hide tray if empty
                if (mWidgets[i].Count == 0) {
                    mTrays[i].Hide();
                    continue;
                }
                else
                    mTrays[i].Show();

                // arrange widgets and calculate final tray size and position
                for (int j = 0; j < mWidgets[i].Count; j++) {
                    Mogre.OverlayElement e = mWidgets[i][j].getOverlayElement();

                    if (j != 0) // don't space first widget
                        trayHeight += mWidgetSpacing;

                    e.VerticalAlignment = (GuiVerticalAlignment.GVA_TOP);
                    e.Top = (trayHeight);

                    switch (e.HorizontalAlignment) {
                        case GuiHorizontalAlignment.GHA_LEFT:
                            e.Left = (mWidgetPadding);
                            break;
                        case GuiHorizontalAlignment.GHA_RIGHT:
                            e.Left = (-(e.Width + mWidgetPadding));
                            break;
                        default:
                            e.Left = (-(e.Width / 2f));
                            break;
                    }

                    // prevents some weird texture filtering problems (just some)
                    e.SetPosition((int)e.Left, (int)e.Top);
                    e.SetDimensions((int)e.Width, (int)e.Height);

                    trayHeight += e.Height;

                    Label l = mWidgets[i][j] as Label;
                    if (l != null && l._isFitToTray()) {
                        labelsAndSeps.Add(e);
                        continue;
                    }
                    Separator s = mWidgets[i][j] as Separator;
                    if (s != null && s._isFitToTray()) {
                        labelsAndSeps.Add(e);
                        continue;
                    }

                    if (e.Width > trayWidth)
                        trayWidth = e.Width;
                }

                // add paddings and resize trays
                mTrays[i].Width = (trayWidth + 2 * mWidgetPadding);
                mTrays[i].Height = (trayHeight + mWidgetPadding);

                for (int j = 0; j < labelsAndSeps.Count; j++) {
                    labelsAndSeps[j].Width = ((int)trayWidth);
                    labelsAndSeps[j].Left = (-(int)(trayWidth / 2));
                }
            }

            for (uint i = 0; i < 9; i++) // snap trays to anchors
			{
                if (i == (int)TrayLocation.TL_TOPLEFT || i == (int)TrayLocation.TL_LEFT || i == (int)TrayLocation.TL_BOTTOMLEFT)
                    mTrays[i].Left = (mTrayPadding);
                if (i == (int)TrayLocation.TL_TOP || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_BOTTOM)
                    mTrays[i].Left = (-mTrays[i].Width / 2);
                if (i == (int)TrayLocation.TL_TOPRIGHT || i == (int)TrayLocation.TL_RIGHT || i == (int)TrayLocation.TL_BOTTOMRIGHT)
                    mTrays[i].Left = (-(mTrays[i].Width + mTrayPadding));

                if (i == (int)TrayLocation.TL_TOPLEFT || i == (int)TrayLocation.TL_TOP || i == (int)TrayLocation.TL_TOPRIGHT)
                    mTrays[i].Top = (mTrayPadding);
                if (i == (int)TrayLocation.TL_LEFT || i == (int)TrayLocation.TL_CENTER || i == (int)TrayLocation.TL_RIGHT)
                    mTrays[i].Top = (-mTrays[i].Height / 2);
                if (i == (int)TrayLocation.TL_BOTTOMLEFT || i == (int)TrayLocation.TL_BOTTOM || i == (int)TrayLocation.TL_BOTTOMRIGHT)
                    mTrays[i].Top = (-mTrays[i].Height - mTrayPadding);

                // prevents some weird texture filtering problems (just some)
                mTrays[i].SetPosition((int)mTrays[i].Left, (int)mTrays[i].Top);
                mTrays[i].SetDimensions((int)mTrays[i].Width, (int)mTrays[i].Height);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Returns a 3D ray into the scene that is directly underneath the cursor.
        //		-----------------------------------------------------------------------------
        public Mogre.Ray getCursorRay(Mogre.Camera cam) {
            return screenToScene(cam, new Mogre.Vector2(mCursor._getLeft(), mCursor._getTop()));
        }

        public Button createButton(TrayLocation trayLoc, string name, string caption) {
            return createButton(trayLoc, name, caption, 0f);
        }

        public Button createButton(TrayLocation trayLoc, string name, string caption, float width) {
            Button b = new Button(name, caption, width);
            moveWidgetToTray(b, trayLoc);
            b._assignListener(mListener);
            return b;
        }

        public TextBox createTextBox(TrayLocation trayLoc, string name, string caption, float width, float height) {
            TextBox tb = new TextBox(name, caption, width, height);
            moveWidgetToTray(tb, trayLoc);
            tb._assignListener(mListener);
            return tb;
        }

        public SelectMenu createThickSelectMenu(TrayLocation trayLoc, string name, string caption, float width, uint maxItemsShown) {
            return createThickSelectMenu(trayLoc, name, caption, width, maxItemsShown, new StringVector());
        }
        public SelectMenu createThickSelectMenu(TrayLocation trayLoc, string name, string caption, float width, uint maxItemsShown, StringVector items) {
            SelectMenu sm = new SelectMenu(name, caption, width, 0f, maxItemsShown);
            moveWidgetToTray(sm, trayLoc);
            sm._assignListener(mListener);
            //if (!items.empty())
            if (!items.IsEmpty)
                sm.setItems(items);
            return sm;
        }

        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown) {
            return createLongSelectMenu(trayLoc, name, caption, width, boxWidth, maxItemsShown, new StringVector());
        }

        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown, StringVector items) {
            SelectMenu sm = new SelectMenu(name, caption, width, boxWidth, maxItemsShown);
            moveWidgetToTray(sm, trayLoc);
            sm._assignListener(mListener);
            //if (!items.empty())
            if (!items.IsEmpty)
                sm.setItems(items);
            return sm;
        }

        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown) {
            return createLongSelectMenu(trayLoc, name, caption, boxWidth, maxItemsShown, new StringVector());
        }

        public SelectMenu createLongSelectMenu(TrayLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown, StringVector items) {
            return createLongSelectMenu(trayLoc, name, caption, 0, boxWidth, maxItemsShown, items);
        }

        public Label createLabel(TrayLocation trayLoc, string name, string caption) {
            return createLabel(trayLoc, name, caption, 0f);
        }

        public Label createLabel(TrayLocation trayLoc, string name, string caption, float width) {
            Label l = new Label(name, caption, width);
            moveWidgetToTray(l, trayLoc);
            l._assignListener(mListener);
            return l;
        }

        public StaticText createStaticText(TrayLocation trayLoc, string name, string caption)
        {
            return createStaticText(trayLoc, name, caption, 0f);
        }
        public StaticText createStaticText(TrayLocation trayLoc, string name, string caption, float width)
        {
            StaticText st = new StaticText(name, caption, width);
            moveWidgetToTray(st, trayLoc);
            st._assignListener(mListener);
            return st;
        }

        public Separator createSeparator(TrayLocation trayLoc, string name) {
            return createSeparator(trayLoc, name, 0f);
        }

        public Separator createSeparator(TrayLocation trayLoc, string name, float width) {
            Separator s = new Separator(name, width);
            moveWidgetToTray(s, trayLoc);
            return s;
        }

        public Slider createThickSlider(TrayLocation trayLoc, string name, string caption, float width, float valueBoxWidth, float minValue, float maxValue, uint snaps) {
            Slider s = new Slider(name, caption, width, 0, valueBoxWidth, minValue, maxValue, snaps);
            moveWidgetToTray(s, trayLoc);
            s._assignListener(mListener);
            return s;
        }

        public Slider createLongSlider(TrayLocation trayLoc, string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps) {
            if (trackWidth <= 0)
                trackWidth = 1;
            Slider s = new Slider(name, caption, width, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
            moveWidgetToTray(s, trayLoc);
            s._assignListener(mListener);
            return s;
        }

        public Slider createLongSlider(TrayLocation trayLoc, string name, string caption, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps) {
            return createLongSlider(trayLoc, name, caption, 0, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
        }

        public ParamsPanel createParamsPanel(TrayLocation trayLoc, string name, float width, uint lines) {
            ParamsPanel pp = new ParamsPanel(name, width, lines);
            moveWidgetToTray(pp, trayLoc);
            return pp;
        }

        public ParamsPanel createParamsPanel(TrayLocation trayLoc, string name, float width, StringVector paramNames) {
            ParamsPanel pp = new ParamsPanel(name, width, (uint)paramNames.Count);
            pp.setAllParamNames(paramNames);
            moveWidgetToTray(pp, trayLoc);
            return pp;
        }
        public ParamsPanel createParamsPanel(TrayLocation trayLoc, string name, float width, string[] paramNames) {
            StringVector sv = new StringVector();
            foreach (var v in paramNames) {
                sv.Add(v);
            }
            return createParamsPanel(trayLoc, name, width, sv);
        }
        public CheckBox createCheckBox(TrayLocation trayLoc, string name, string caption) {
            return createCheckBox(trayLoc, name, caption, 0f);
        }

        public CheckBox createCheckBox(TrayLocation trayLoc, string name, string caption, float width) {
            CheckBox cb = new CheckBox(name, caption, width);
            moveWidgetToTray(cb, trayLoc);
            cb._assignListener(mListener);
            return cb;
        }

        public DecorWidget createDecorWidget(TrayLocation trayLoc, string name, string templateName) {
            DecorWidget dw = new DecorWidget(name, templateName);
            moveWidgetToTray(dw, trayLoc);
            return dw;
        }

        public ProgressBar createProgressBar(TrayLocation trayLoc, string name, string caption, float width, float commentBoxWidth) {
            ProgressBar pb = new ProgressBar(name, caption, width, commentBoxWidth);
            moveWidgetToTray(pb, trayLoc);
            return pb;
        }

        //        -----------------------------------------------------------------------------
        //		| Shows frame statistics widget set in the specified location.
        //		-----------------------------------------------------------------------------
        public void showFrameStats(TrayLocation trayLoc) {
            showFrameStats(trayLoc, -1);
        }
        //ORIGINAL LINE: void showFrameStats(TrayLocation trayLoc, int place = -1)
        public void showFrameStats(TrayLocation trayLoc, int place) {
            if (!areFrameStatsVisible()) {
                StringVector stats = new StringVector();
                stats.Add("Average FPS");
                stats.Add("Best FPS");
                stats.Add("Worst FPS");
                stats.Add("Triangles");
                stats.Add("Batches");

                mFpsLabel = createLabel(TrayLocation.TL_NONE, mName + "/FpsLabel", "FPS:", 180);
                mFpsLabel._assignListener(this);
                mStatsPanel = createParamsPanel(TrayLocation.TL_NONE, mName + "/StatsPanel", 180, stats);
            }

            moveWidgetToTray(mFpsLabel, trayLoc, place);
            moveWidgetToTray(mStatsPanel, trayLoc, locateWidgetInTray(mFpsLabel) + 1);
        }

        //        -----------------------------------------------------------------------------
        //		| Hides frame statistics widget set.
        //		-----------------------------------------------------------------------------
        public void hideFrameStats() {
            if (areFrameStatsVisible()) {
                destroyWidget(mFpsLabel);
                destroyWidget(mStatsPanel);
                //delete mFpsLabel
                //delete mStatsPanel
                //mFpsLabel.Dispose();
                //mStatsPanel.Dispose();
                mFpsLabel = null;
                mStatsPanel = null;
            }
        }

        public bool areFrameStatsVisible() {
            return mFpsLabel != null;
        }

        //        -----------------------------------------------------------------------------
        //		| Toggles visibility of advanced statistics.
        //		-----------------------------------------------------------------------------
        public void toggleAdvancedFrameStats() {
            if (mFpsLabel != null)
                labelHit(mFpsLabel);
        }

        //        -----------------------------------------------------------------------------
        //		| Shows logo in the specified location.
        //		-----------------------------------------------------------------------------
        public void showLogo(TrayLocation trayLoc) {
            showLogo(trayLoc, -1);
        }
        //ORIGINAL LINE: void showLogo(TrayLocation trayLoc, int place = -1)
        public void showLogo(TrayLocation trayLoc, int place) {
            if (!isLogoVisible())
                mLogo = createDecorWidget(TrayLocation.TL_NONE, mName + "/Logo", "SdkTrays/Logo");
            moveWidgetToTray(mLogo, trayLoc, place);
        }

        public void hideLogo() {
            if (isLogoVisible()) {
                destroyWidget(mLogo);
                mLogo.Dispose();
                mLogo = null;
            }
        }

        public bool isLogoVisible() {
            return mLogo != null;
        }

        //        -----------------------------------------------------------------------------
        //		| Shows loading bar. Also takes job settings: the number of resource groups
        //		| to be initialised, the number of resource groups to be loaded, and the
        //		| proportion of the job that will be taken up by initialisation. Usually,
        //		| script parsing takes up most time, so the default value is 0.7.
        //		-----------------------------------------------------------------------------
        public void showLoadingBar(uint numGroupsInit, uint numGroupsLoad) {
            showLoadingBar(numGroupsInit, numGroupsLoad, 0.7f);
        }
        public void showLoadingBar(uint numGroupsInit) {
            showLoadingBar(numGroupsInit, 1, 0.7f);
        }
        public void showLoadingBar() {
            showLoadingBar(1, 1, 0.7f);
        }
        //ORIGINAL LINE: void showLoadingBar(uint numGroupsInit = 1, uint numGroupsLoad = 1, Ogre::Real initProportion = 0.7)
        public void showLoadingBar(uint numGroupsInit, uint numGroupsLoad, float initProportion) {
            if (mDialog != null)
                closeDialog();
            if (mLoadBar != null)
                hideLoadingBar();

            mLoadBar = new ProgressBar(mName + "/LoadingBar", "Loading...", 400, 308);
            Mogre.OverlayElement e = mLoadBar.getOverlayElement();
            mDialogShade.AddChild(e);
            e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
            e.Left = (-(e.Width / 2));
            e.Top = (-(e.Height / 2));

            //Mogre.ResourceGroupManager.Singleton.addResourceGroupListener(this);
            this._HookResourceGroupLoadEvent();

            mCursorWasVisible = isCursorVisible();
            hideCursor();
            mDialogShade.Show();

            // calculate the proportion of job required to init/load one group

            if (numGroupsInit == 0 && numGroupsLoad != 0) {
                mGroupInitProportion = 0;
                mGroupLoadProportion = 1;
            }
            else if (numGroupsLoad == 0 && numGroupsInit != 0) {
                mGroupLoadProportion = 0;
                if (numGroupsInit != 0)
                    mGroupInitProportion = 1;
            }
            else if (numGroupsInit == 0 && numGroupsLoad == 0) {
                mGroupInitProportion = 0;
                mGroupLoadProportion = 0;
            }
            else {
                mGroupInitProportion = initProportion / numGroupsInit;
                mGroupLoadProportion = (1 - initProportion) / numGroupsLoad;
            }
        }

        public void hideLoadingBar() {
            if (mLoadBar != null) {
                mLoadBar.cleanup();
                //delete mLoadBar;
                mLoadBar.Dispose();
                mLoadBar = null;

                //Mogre.ResourceGroupManager.Singleton.removeResourceGroupListener(this);
                this._UnHookResourceGroupLoadEvent();
                if (mCursorWasVisible)
                    showCursor();
                mDialogShade.Hide();
            }
        }

        public bool isLoadingBarVisible() {
            return mLoadBar != null;
        }

        //        -----------------------------------------------------------------------------
        //		| Pops up a message dialog with an OK button.
        //		-----------------------------------------------------------------------------
        public void showOkDialog(string caption, string message) {
            if (mLoadBar != null)
                hideLoadingBar();

            Mogre.OverlayElement e;

            if (mDialog != null) {
                mDialog.setCaption(caption);
                mDialog.setText(message);

                if (mOk != null)
                    return;
                else {
                    mYes.cleanup();
                    mNo.cleanup();
                    //delete mYes;
                    //delete mNo;
                    mYes.Dispose();
                    mNo.Dispose();
                    mYes = null;
                    mNo = null;
                }
            }
            else {
                // give widgets a chance to reset in case they're in the middle of something
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < mWidgets[i].Count; j++) {
                        mWidgets[i][j]._focusLost();
                    }
                }

                mDialogShade.Show();

                mDialog = new TextBox(mName + "/DialogBox", caption, 300f, 208f);
                mDialog.setText(message);
                e = mDialog.getOverlayElement();
                mDialogShade.AddChild(e);
                e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
                e.Left = (-(e.Width / 2f));
                e.Top = (-(e.Height / 2f));

                mCursorWasVisible = isCursorVisible();
                showCursor();
            }

            mOk = new Button(mName + "/OkButton", "OK", 60f);
            mOk._assignListener(this);
            e = mOk.getOverlayElement();
            mDialogShade.AddChild(e);
            e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
            e.Left = (-(e.Width / 2f));
            e.Top = (mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5f);
        }

        //        -----------------------------------------------------------------------------
        //		| Pops up a question dialog with Yes and No buttons.
        //		-----------------------------------------------------------------------------
        public void showYesNoDialog(string caption, string question) {
            if (mLoadBar != null)
                hideLoadingBar();

            Mogre.OverlayElement e;

            if (mDialog != null) {
                mDialog.setCaption(caption);
                mDialog.setText(question);

                if (mOk != null) {
                    mOk.cleanup();
                    //delete mOk;
                    mOk.Dispose();
                    mOk = null;
                }
                else
                    return;
            }
            else {
                // give widgets a chance to reset in case they're in the middle of something
                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < mWidgets[i].Count; j++) {
                        mWidgets[i][j]._focusLost();
                    }
                }

                mDialogShade.Show();

                mDialog = new TextBox(mName + "/DialogBox", caption, 300f, 208f);
                mDialog.setText(question);
                e = mDialog.getOverlayElement();
                mDialogShade.AddChild(e);
                e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
                e.Left = (-(e.Width / 2f));
                e.Top = (-(e.Height / 2f));

                mCursorWasVisible = isCursorVisible();
                showCursor();
            }

            mYes = new Button(mName + "/YesButton", "Yes", 58f);
            mYes._assignListener(this);
            e = mYes.getOverlayElement();
            mDialogShade.AddChild(e);
            e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
            e.Left = (-(e.Width + 2f));
            e.Top = (mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5f);

            mNo = new Button(mName + "/NoButton", "No", 50f);
            mNo._assignListener(this);
            e = mNo.getOverlayElement();
            mDialogShade.AddChild(e);
            e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
            e.Left = (3f);
            e.Top = (mDialog.getOverlayElement().Top + mDialog.getOverlayElement().Height + 5f);
        }

        //        -----------------------------------------------------------------------------
        //		| Hides whatever dialog is currently showing.
        //		-----------------------------------------------------------------------------
        public void closeDialog() {
            if (mDialog != null) {
                if (mOk != null) {
                    mOk.cleanup();
                    //delete mOk;
                    mOk.Dispose();
                    mOk = null;
                }
                else {
                    mYes.cleanup();
                    mNo.cleanup();
                    //delete mYes;
                    //delete mNo;
                    mYes.Dispose();
                    mNo.Dispose();
                    mYes = null;
                    mNo = null;
                }

                mDialogShade.Hide();
                mDialog.cleanup();
                //delete mDialog;
                mDialog.Dispose();
                mDialog = null;

                if (!mCursorWasVisible)
                    hideCursor();
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Determines if any dialog is currently visible.
        //		-----------------------------------------------------------------------------
        public bool isDialogVisible() {
            return mDialog != null;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets a widget from a tray by place.
        //		-----------------------------------------------------------------------------
        public Widget getWidget(TrayLocation trayLoc, uint place) {
            if (place < mWidgets[(int)trayLoc].Count)
                return mWidgets[(int)trayLoc][(int)place];
            return null;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets a widget from a tray by name.
        //		-----------------------------------------------------------------------------
        public Widget getWidget(TrayLocation trayLoc, string name) {
            for (int i = 0; i < mWidgets[(int)trayLoc].Count; i++) {
                if (mWidgets[(int)trayLoc][i].getName() == name)
                    return mWidgets[(int)trayLoc][i];
            }
            return null;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets a widget by name.
        //		-----------------------------------------------------------------------------
        public Widget getWidget(string name) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < mWidgets[i].Count; j++) {
                    if (mWidgets[i][j].getName() == name)
                        return mWidgets[i][j];
                }
            }
            return null;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets the number of widgets in total.
        //		-----------------------------------------------------------------------------
        public uint getNumWidgets() {
            uint total = 0;

            for (int i = 0; i < 10; i++) {
                total += (uint)mWidgets[i].Count;
            }

            return total;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets the number of widgets in a tray.
        //		-----------------------------------------------------------------------------
        public int getNumWidgets(TrayLocation trayLoc) {
            return mWidgets[(int)trayLoc].Count;
        }

        //        -----------------------------------------------------------------------------
        //		| Gets all the widgets of a specific tray.
        //		-----------------------------------------------------------------------------
        //public Mogre.VectorIterator<List<Widget*>> getWidgetIterator(TrayLocation trayLoc)
        public List<Widget> getWidgetIterator(TrayLocation trayLoc) {
            return mWidgets[(int)trayLoc];
            //return Mogre.VectorIterator<List<Widget*>>(mWidgets[(int)trayLoc].begin(), mWidgets[(int)trayLoc].end());
        }

        //        -----------------------------------------------------------------------------
        //		| Gets a widget's position in its tray.
        //		-----------------------------------------------------------------------------
        public int locateWidgetInTray(Widget widget) {
            for (int i = 0; i < mWidgets[(int)widget.getTrayLocation()].Count; i++) {
                if (mWidgets[(int)widget.getTrayLocation()][i] == widget)
                    return i;
            }
            return -1;
        }

        //        -----------------------------------------------------------------------------
        //		| Destroys a widget.
        //		-----------------------------------------------------------------------------
        public void destroyWidget(Widget widget) {
            if (widget == null)
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", "Widget does not exist.", "TrayManager::destroyWidget");

            // in case special widgets are destroyed manually, set them to 0
            if (widget == mLogo)
                mLogo = null;
            else if (widget == mStatsPanel)
                mStatsPanel = null;
            else if (widget == mFpsLabel)
                mFpsLabel = null;

            mTrays[(int)widget.getTrayLocation()].RemoveChild(widget.getName());

            List<Widget> wList = mWidgets[(int)widget.getTrayLocation()];
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
            //wList.erase(std.find(wList.GetEnumerator(), wList.end(), widget));
            for (int j = wList.Count - 1; j >= 0; j--) {
                if (wList[j] == widget) {
                    wList.RemoveAt(j);
                }
            }
            if (widget == mExpandedMenu)
                setExpandedMenu(null);

            widget.cleanup();

            mWidgetDeathRow.Add(widget);

            adjustTrays();
        }

        private void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p + "_" + p_2 + "_" + p_3);
        }

        public void destroyWidget(TrayLocation trayLoc, uint place) {
            destroyWidget(getWidget(trayLoc, place));
        }

        public void destroyWidget(TrayLocation trayLoc, string name) {
            destroyWidget(getWidget(trayLoc, name));
        }

        public void destroyWidget(string name) {
            destroyWidget(getWidget(name));
        }

        //        -----------------------------------------------------------------------------
        //		| Destroys all widgets in a tray.
        //		-----------------------------------------------------------------------------
        public void destroyAllWidgetsInTray(TrayLocation trayLoc) {
            //while (!mWidgets[(int)trayLoc].empty())
            while (mWidgets[(int)trayLoc].Count > 0)
                destroyWidget(mWidgets[(int)trayLoc][0]);
        }

        //        -----------------------------------------------------------------------------
        //		| Destroys all widgets.
        //		-----------------------------------------------------------------------------
        public void destroyAllWidgets() {
            for (uint i = 0; i < 10; i++) // destroy every widget in every tray (including null tray)
			{
                destroyAllWidgetsInTray((TrayLocation)i);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Adds a widget to a specified tray.
        //		-----------------------------------------------------------------------------
        public void moveWidgetToTray(Widget widget, TrayLocation trayLoc) {
            moveWidgetToTray(widget, trayLoc, -1);
        }

        public void moveWidgetToTray(Widget widget, TrayLocation trayLoc, int place) {
            if (widget == null)
                OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", "Widget does not exist.", "TrayManager::moveWidgetToTray");

            // remove widget from old tray
            List<Widget> wList = mWidgets[(int)widget.getTrayLocation()];
            for (int j = wList.Count-1; j >= 0; j--) {
                if (wList[j] == widget) {
                    wList.RemoveAt(j);
                    mTrays[(int)widget.getTrayLocation()].RemoveChild(widget.getName());
                }
            }


            // insert widget into new tray at given position, or at the end if unspecified or invalid
            if (place == -1 || place > (int)mWidgets[(int)trayLoc].Count)
                place = (int)mWidgets[(int)trayLoc].Count;
            mWidgets[(int)trayLoc].Insert(place, widget);
            mTrays[(int)trayLoc].AddChild(widget.getOverlayElement());

            widget.getOverlayElement().HorizontalAlignment = (mTrayWidgetAlign[(int)trayLoc]);

            // adjust trays if necessary
            if (widget.getTrayLocation() != TrayLocation.TL_NONE || trayLoc != TrayLocation.TL_NONE)
                adjustTrays();

            widget._assignToTray(trayLoc);
        }

        public void moveWidgetToTray(string name, TrayLocation trayLoc) {
            moveWidgetToTray(name, trayLoc, -1);
        }

        public void moveWidgetToTray(string name, TrayLocation trayLoc, int place) {
            moveWidgetToTray(getWidget(name), trayLoc, place);
        }

        public void moveWidgetToTray(TrayLocation currentTrayLoc, string name, TrayLocation targetTrayLoc) {
            moveWidgetToTray(currentTrayLoc, name, targetTrayLoc, -1);
        }
        //ORIGINAL LINE: void moveWidgetToTray(TrayLocation currentTrayLoc, const Ogre::String& name, TrayLocation targetTrayLoc, int place = -1)
        public void moveWidgetToTray(TrayLocation currentTrayLoc, string name, TrayLocation targetTrayLoc, int place) {
            moveWidgetToTray(getWidget(currentTrayLoc, name), targetTrayLoc, place);
        }

        public void moveWidgetToTray(TrayLocation currentTrayLoc, uint currentPlace, TrayLocation targetTrayLoc) {
            moveWidgetToTray(currentTrayLoc, currentPlace, targetTrayLoc, -1);
        }
        //ORIGINAL LINE: void moveWidgetToTray(TrayLocation currentTrayLoc, uint currentPlace, TrayLocation targetTrayLoc, int targetPlace = -1)
        public void moveWidgetToTray(TrayLocation currentTrayLoc, uint currentPlace, TrayLocation targetTrayLoc, int targetPlace) {
            moveWidgetToTray(getWidget(currentTrayLoc, currentPlace), targetTrayLoc, targetPlace);
        }

        //        -----------------------------------------------------------------------------
        //		| Removes a widget from its tray. Same as moving it to the null tray.
        //		-----------------------------------------------------------------------------
        public void removeWidgetFromTray(Widget widget) {
            moveWidgetToTray(widget, TrayLocation.TL_NONE);
        }

        public void removeWidgetFromTray(string name) {
            removeWidgetFromTray(getWidget(name));
        }

        public void removeWidgetFromTray(TrayLocation trayLoc, string name) {
            removeWidgetFromTray(getWidget(trayLoc, name));
        }

        public void removeWidgetFromTray(TrayLocation trayLoc, uint place) {
            removeWidgetFromTray(getWidget(trayLoc, place));
        }

        //        -----------------------------------------------------------------------------
        //		| Removes all widgets from a widget tray.
        //		-----------------------------------------------------------------------------
        public void clearTray(TrayLocation trayLoc) {
            if (trayLoc == TrayLocation.TL_NONE) // can't clear the null tray
                return;

            //while (!mWidgets[(int)trayLoc].empty()) // remove every widget from given tray
            while (mWidgets[(int)trayLoc].Count > 0) {
                removeWidgetFromTray(mWidgets[(int)trayLoc][0]);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Removes all widgets from all widget trays.
        //		-----------------------------------------------------------------------------
        public void clearAllTrays() {
            for (uint i = 0; i < 9; i++) {
                clearTray((TrayLocation)i);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Process frame events. Updates frame statistics widget set and deletes
        //		| all widgets queued for destruction.
        //		-----------------------------------------------------------------------------
        public bool frameRenderingQueued(Mogre.FrameEvent evt) {
            for (int i = 0; i < mWidgetDeathRow.Count; i++) {
                //delete mWidgetDeathRow[i];
                mWidgetDeathRow[i] = null;
            }
            mWidgetDeathRow.Clear();


            uint currentTime = mTimer.Milliseconds;
            if (areFrameStatsVisible() && (currentTime - mLastStatUpdateTime > 250)) {
                Mogre.RenderTarget.FrameStats stats = mWindow.GetStatistics();

                mLastStatUpdateTime = currentTime;

                string s = ("FPS: ");
                s += ((int)stats.LastFPS).ToString();

                mFpsLabel.setCaption(s);

                if (mStatsPanel.getOverlayElement().IsVisible) {
                    StringVector values = new StringVector();

                    //StringStream oss = new StringStream();

                    //oss.str("");
                    //oss << std.fixed << std.setprecision(1) << stats.avgFPS;
                    //string str = oss.str();
                    //values.push_back(str);

                    //oss.str("");
                    //oss << std.fixed << std.setprecision(1) << stats.bestFPS;
                    //str = oss.str();
                    //values.push_back(str);

                    //oss.str("");
                    //oss << std.fixed << std.setprecision(1) << stats.worstFPS;
                    //str = oss.str();
                    //values.push_back(str);

                    //str = stringConverter.toString(stats.triangleCount);
                    //values.push_back(str);

                    //str = stringConverter.toString(stats.batchCount);
                    //values.push_back(str);
                    values.Add(stats.AvgFPS.ToString("N", System.Globalization.CultureInfo.InvariantCulture));
                    values.Add(stats.BestFPS.ToString("N", System.Globalization.CultureInfo.InvariantCulture));
                    values.Add(stats.WorstFPS.ToString("N", System.Globalization.CultureInfo.InvariantCulture));
                    values.Add(stats.TriangleCount.ToString("N", System.Globalization.CultureInfo.InvariantCulture));
                    values.Add(stats.BatchCount.ToString("N", System.Globalization.CultureInfo.InvariantCulture));
                    mStatsPanel.setAllParamValues(values);
                }
            }

            return true;
        }





        //        -----------------------------------------------------------------------------
        //		| Toggles visibility of advanced statistics.
        //		-----------------------------------------------------------------------------
        public new void labelHit(Label label) {
            if (mStatsPanel.getOverlayElement().IsVisible) {
                mStatsPanel.getOverlayElement().Hide();
                mFpsLabel.getOverlayElement().Width = (150f);
                removeWidgetFromTray(mStatsPanel);
            }
            else {
                mStatsPanel.getOverlayElement().Show();
                mFpsLabel.getOverlayElement().Width = (180f);
                moveWidgetToTray(mStatsPanel, mFpsLabel.getTrayLocation(), locateWidgetInTray(mFpsLabel) + 1);
            }
        }

        //        -----------------------------------------------------------------------------
        //		| Destroys dialog widgets, notifies listener, and ends high priority session.
        //		-----------------------------------------------------------------------------
        public new void buttonHit(Button button) {
            if (mListener != null) {
                if (button == mOk)
                    mListener.okDialogClosed(mDialog.getText());
                else
                    mListener.yesNoDialogClosed(mDialog.getText(), button == mYes);
            }
            closeDialog();
        }

        //        -----------------------------------------------------------------------------
        //		| Processes mouse button down events. Returns true if the event was
        //		| consumed and should not be passed on to other handlers.
        //		-----------------------------------------------------------------------------

        //#if (OGRE_PLATFORM == OGRE_PLATFORM_APPLE_IOS) || (OGRE_PLATFORM == OGRE_PLATFORM_ANDROID)
        //		public bool injectMouseDown(OIS.MultiTouchEvent evt)
        //#else
        public bool injectMouseDown(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
            //#endif
        {

            //#if (OGRE_PLATFORM != OGRE_PLATFORM_APPLE_IOS) && (OGRE_PLATFORM != OGRE_PLATFORM_ANDROID)
            // only process left button when stuff is visible
            if (!mCursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
                return false;
            //#else
            //            // only process touches when stuff is visible
            //            if (!mCursorLayer.isVisible())
            //                return false;
            //#endif
            Mogre.Vector2 cursorPos = new Mogre.Vector2(mCursor.Left, mCursor.Top);

            mTrayDrag = false;

            if (mExpandedMenu != null) // only check top priority widget until it passes on
			{
                mExpandedMenu._cursorPressed(cursorPos);
                if (!mExpandedMenu.isExpanded())
                    setExpandedMenu(null);
                return true;
            }

            if (mDialog != null) // only check top priority widget until it passes on
			{
                mDialog._cursorPressed(cursorPos);
                if (mOk != null)
                    mOk._cursorPressed(cursorPos);
                else {
                    mYes._cursorPressed(cursorPos);
                    mNo._cursorPressed(cursorPos);
                }
                return true;
            }

            for (uint i = 0; i < 9; i++) // check if mouse is over a non-null tray
			{
                if (mTrays[i].IsVisible && Widget.isCursorOver(mTrays[i], cursorPos, 2f)) {
                    mTrayDrag = true; // initiate a drag that originates in a tray
                    break;
                }
            }

            for (int i = 0; i < mWidgets[9].Count; i++) // check if mouse is over a non-null tray's widgets
			{
                if (mWidgets[9][i].getOverlayElement().IsVisible && Widget.isCursorOver(mWidgets[9][i].getOverlayElement(), cursorPos)) {
                    mTrayDrag = true; // initiate a drag that originates in a tray
                    break;
                }
            }

            if (!mTrayDrag) // don't process if mouse press is not in tray
                return false;

            for (int i = 0; i < 10; i++) {
                if (!mTrays[i].IsVisible)
                    continue;

                for (int j = 0; j < mWidgets[i].Count; j++) {
                    Widget w = mWidgets[i][j];
                    if (!w.getOverlayElement().IsVisible)
                        continue;
                    w._cursorPressed(cursorPos); // send event to widget

                    SelectMenu m = w as SelectMenu;
                    if (m != null && m.isExpanded()) // a menu has begun a top priority session
					{
                        setExpandedMenu(m);
                        return true;
                    }
                }
            }

            return true; // a tray click is not to be handled by another party
        }

        //        -----------------------------------------------------------------------------
        //		| Processes mouse button up events. Returns true if the event was
        //		| consumed and should not be passed on to other handlers.
        //		-----------------------------------------------------------------------------

        //#if (OGRE_PLATFORM == OGRE_PLATFORM_APPLE_IOS) || (OGRE_PLATFORM == OGRE_PLATFORM_ANDROID)
        //        public bool injectMouseUp(OIS.MultiTouchEvent evt)
        //#else
        public bool injectMouseUp(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
            //#endif
        {

            //#if (OGRE_PLATFORM != OGRE_PLATFORM_APPLE_IOS) && (OGRE_PLATFORM != OGRE_PLATFORM_ANDROID)
            // only process left button when stuff is visible
            if (!mCursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
                return false;
            //#else
            //            // only process touches when stuff is visible
            //            if (!mCursorLayer.isVisible())
            //                return false;
            //#endif
            Mogre.Vector2 cursorPos = new Mogre.Vector2(mCursor.Left, mCursor.Top);

            if (mExpandedMenu != null) // only check top priority widget until it passes on
			{
                mExpandedMenu._cursorReleased(cursorPos);
                return true;
            }

            if (mDialog != null) // only check top priority widget until it passes on
			{
                mDialog._cursorReleased(cursorPos);
                if (mOk != null)
                    mOk._cursorReleased(cursorPos);
                else {
                    mYes._cursorReleased(cursorPos);
                    // very important to check if second button still exists, because first button could've closed the popup
                    if (mNo != null)
                        mNo._cursorReleased(cursorPos);
                }
                return true;
            }

            if (!mTrayDrag) // this click did not originate in a tray, so don't process
                return false;

            Widget w = null;

            for (int i = 0; i < 10; i++) {
                if (!mTrays[i].IsVisible)
                    continue;

                for (int j = 0; j < mWidgets[i].Count; j++) {
                    w = mWidgets[i][j];
                    if (!w.getOverlayElement().IsVisible)
                        continue;
                    w._cursorReleased(cursorPos); // send event to widget
                }
            }

            mTrayDrag = false; // stop this drag
            return true; // this click did originate in this tray, so don't pass it on
        }

        //        -----------------------------------------------------------------------------
        //		| Updates cursor position. Returns true if the event was
        //		| consumed and should not be passed on to other handlers.
        //		-----------------------------------------------------------------------------

        //#if (OGRE_PLATFORM == OGRE_PLATFORM_APPLE_IOS) || (OGRE_PLATFORM == OGRE_PLATFORM_ANDROID)
        //        public bool injectMouseMove(OIS.MultiTouchEvent evt)
        //#else
        public bool injectMouseMove(MOIS.MouseEvent evt)
            //#endif
        {
            if (!mCursorLayer.IsVisible) // don't process if cursor layer is invisible
                return false;

            Mogre.Vector2 cursorPos = new Mogre.Vector2(evt.state.X.abs, evt.state.Y.abs);
            mCursor.SetPosition(cursorPos.x, cursorPos.y);

            if (mExpandedMenu != null) // only check top priority widget until it passes on
			{
                mExpandedMenu._cursorMoved(cursorPos);
                return true;
            }

            if (mDialog != null) // only check top priority widget until it passes on
			{
                mDialog._cursorMoved(cursorPos);
                if (mOk != null)
                    mOk._cursorMoved(cursorPos);
                else {
                    mYes._cursorMoved(cursorPos);
                    mNo._cursorMoved(cursorPos);
                }
                return true;
            }

            Widget w = null;

            for (int i = 0; i < 10; i++) {
                if (!mTrays[i].IsVisible)
                    continue;

                for (int j = 0; j < mWidgets[i].Count; j++) {
                    w = mWidgets[i][j];
                    if (!w.getOverlayElement().IsVisible)
                        continue;
                    w._cursorMoved(cursorPos); // send event to widget
                }
            }

            if (mTrayDrag) // don't pass this event on if we're in the middle of a drag
                return true;
            return false;
        }


        //        -----------------------------------------------------------------------------
        //		| Internal method to prioritise / deprioritise expanded menus.
        //		-----------------------------------------------------------------------------
        protected void setExpandedMenu(SelectMenu m) {
            if (mExpandedMenu == null && m != null) {
                Mogre.OverlayContainer c = (Mogre.OverlayContainer)m.getOverlayElement();
                Mogre.OverlayContainer eb = (Mogre.OverlayContainer)c.GetChild(m.getName() + "/MenuExpandedBox");
                eb._update();
                eb.SetPosition((uint)(eb._getDerivedLeft() * Mogre.OverlayManager.Singleton.ViewportWidth), (uint)(eb._getDerivedTop() * Mogre.OverlayManager.Singleton.ViewportHeight));
                c.RemoveChild(eb.Name);
                mPriorityLayer.Add2D(eb);
            }
            else if (mExpandedMenu != null && m == null) {
                Mogre.OverlayContainer eb = mPriorityLayer.GetChild(mExpandedMenu.getName() + "/MenuExpandedBox");
                mPriorityLayer.Remove2D(eb);
                ((Mogre.OverlayContainer)mExpandedMenu.getOverlayElement()).AddChild(eb);
            }

            mExpandedMenu = m;
        }

        protected string mName = ""; // name of this tray system
        protected Mogre.RenderWindow mWindow; // render window
        protected InputContext mInputContext = null;
        protected Mogre.Overlay mBackdropLayer; // backdrop layer
        protected Mogre.Overlay mTraysLayer; // widget layer
        protected Mogre.Overlay mPriorityLayer; // top priority layer
        protected Mogre.Overlay mCursorLayer; // cursor layer
        protected Mogre.OverlayContainer mBackdrop; // backdrop
        protected Mogre.OverlayContainer[] mTrays = new Mogre.OverlayContainer[10]; // widget trays
        protected List<Widget>[] mWidgets = new List<Widget>[10]; // widgets
        protected List<Widget> mWidgetDeathRow = new List<Widget>(); // widget queue for deletion
        protected Mogre.OverlayContainer mCursor; // cursor
        protected SdkTrayListener mListener; // tray listener
        protected float mWidgetPadding = 0f; // widget padding
        protected float mWidgetSpacing = 0f; // widget spacing
        protected float mTrayPadding = 0f; // tray padding
        protected bool mTrayDrag; // a mouse press was initiated on a tray
        protected SelectMenu mExpandedMenu; // top priority expanded menu widget
        protected TextBox mDialog; // top priority dialog widget
        protected Mogre.OverlayContainer mDialogShade; // top priority dialog shade
        protected Button mOk; // top priority OK button
        protected Button mYes; // top priority Yes button
        protected Button mNo; // top priority No button
        protected bool mCursorWasVisible; // cursor state before showing dialog
        protected Label mFpsLabel; // FPS label
        protected ParamsPanel mStatsPanel; // frame stats panel
        protected DecorWidget mLogo; // logo
        protected ProgressBar mLoadBar; // loading bar
        protected float mGroupInitProportion = 0f; // proportion of load job assigned to initialising one resource group
        protected float mGroupLoadProportion = 0f; // proportion of load job assigned to loading one resource group
        protected float mLoadInc = 0f; // loading increment
        protected Mogre.GuiHorizontalAlignment[] mTrayWidgetAlign = new Mogre.GuiHorizontalAlignment[10]; // tray widget alignments
        protected Mogre.Timer mTimer; // Root::getSingleton().getTimer()
        protected uint mLastStatUpdateTime; // The last time the stat text were updated

    }
}

//#endif