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
	/// Basic slider widget
	/// </summary>
	public class Slider : Widget
	{
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

		public Slider(string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			mDragOffset = 0.0f;
			mValue = 0.0f;
			mMinValue = 0.0f;
			mMaxValue = 0.0f;
			mInterval = 0.0f;
			mDragging = false;
			mFitToContents = false;
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Slider", "BorderPanel", name);
			element.Width = (width);
			Mogre.OverlayContainer c = (Mogre.OverlayContainer)element;
			mTextArea = (Mogre.TextAreaOverlayElement)c.GetChild(Name + "/SliderCaption");
			Mogre.OverlayContainer valueBox = (Mogre.OverlayContainer)c.GetChild(Name + "/SliderValueBox");
			valueBox.Width = (valueBoxWidth);
			valueBox.Left = (-(valueBoxWidth + 5));
			mValueTextArea = (Mogre.TextAreaOverlayElement)valueBox.GetChild(valueBox.Name + "/SliderValueText");
			mTrack = (Mogre.BorderPanelOverlayElement)c.GetChild(Name + "/SliderTrack");
			mHandle = (Mogre.PanelOverlayElement)mTrack.GetChild(mTrack.Name + "/SliderHandle");

			if (trackWidth <= 0f) // tall style
			{
				mTrack.Width = (width - 16f);
			}
			else // long style
			{
				if (width <= 0f)
					mFitToContents = true;
				element.Height = (34f);
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
		public void setRange(float minValue, float maxValue, uint snaps)
		{
			setRange(minValue, maxValue, snaps, true);
		}
		//ORIGINAL LINE: void setRange(Ogre::Real minValue, Ogre::Real maxValue, uint snaps, bool notifyListener = true)
		public void setRange(float minValue, float maxValue, uint snaps, bool notifyListener)
		{
			mMinValue = minValue;
			mMaxValue = maxValue;

			if (snaps <= 1 || mMinValue >= mMaxValue)
			{
				mInterval = 0;
				mHandle.Hide();
				mValue = minValue;
				if (snaps == 1)
					mValueTextArea.Caption = ((mMinValue).ToString());
				else
					mValueTextArea.Caption = ("");
			}
			else
			{
				mHandle.Show();
				mInterval = (maxValue - minValue) / (snaps - 1);
				setValue(minValue, notifyListener);
			}
		}

		public string getValueCaption()
		{
			return mValueTextArea.Caption;
		}

		//        -----------------------------------------------------------------------------
		//		| You can use this method to manually format how the value is displayed.
		//		-----------------------------------------------------------------------------
		public void setValueCaption(string caption)
		{
			mValueTextArea.Caption = (caption);
		}

		public void setValue(float value)
		{
			setValue(value, true);
		}
		//ORIGINAL LINE: void setValue(Ogre::Real value, bool notifyListener = true)
		public void setValue(float @value, bool notifyListener)
		{
			if (mInterval == 0)
				return;

			mValue = SdkTrayMathHelper.clamp<float>(@value, mMinValue, mMaxValue);

			setValueCaption((mValue).ToString());

			if (listener != null && notifyListener)
				listener.sliderMoved(this);

			if (!mDragging)
				mHandle.Left = ((int)((mValue - mMinValue) / (mMaxValue - mMinValue) * (mTrack.Width - mHandle.Width)));
		}

		public float getValue()
		{
			return mValue;
		}

		public string getCaption()
		{
			return mTextArea.Caption;
		}

		public void setCaption(string caption)
		{
			mTextArea.Caption = (caption);

			if (mFitToContents)
				element.Width = (GetCaptionWidth(caption, ref mTextArea) + mValueTextArea.Parent.Width + mTrack.Width + 26f);
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (!mHandle.IsVisible)
				return;

			Mogre.Vector2 co = Widget.CursorOffset(mHandle, cursorPos);

			if (co.SquaredLength <= 81f)
			{
				mDragging = true;
				mDragOffset = co.x;
			}
			else if (Widget.IsCursorOver(mTrack, cursorPos))
			{
				float newLeft = mHandle.Left + co.x;
				float rightBoundary = mTrack.Width - mHandle.Width;

				mHandle.Left = (SdkTrayMathHelper.clamp<int>((int)newLeft, 0, (int)rightBoundary));
				setValue(getSnappedValue(newLeft / rightBoundary));
			}
		}

		public override void CursorReleased(Mogre.Vector2 cursorPos)
		{
			if (mDragging)
			{
				mDragging = false;
				mHandle.Left = ((int)((mValue - mMinValue) / (mMaxValue - mMinValue) * (mTrack.Width - mHandle.Width)));
			}
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			if (mDragging)
			{
				Mogre.Vector2 co = Widget.CursorOffset(mHandle, cursorPos);
				float newLeft = mHandle.Left + co.x - mDragOffset;
				float rightBoundary = mTrack.Width - mHandle.Width;

				mHandle.Left = (SdkTrayMathHelper.clamp<int>((int)newLeft, 0, (int)rightBoundary));
				setValue(getSnappedValue(newLeft / rightBoundary));
			}
		}

		public override void FocusLost()
		{
			mDragging = false;
		}


		protected float getSnappedValue(float percentage)
		{
			percentage = SdkTrayMathHelper.clamp<float>(percentage, 0f, 1f);
			uint whichMarker = (uint)(percentage * (mMaxValue - mMinValue) / mInterval + 0.5f);
			return whichMarker * mInterval + mMinValue;
		}
	}
}
