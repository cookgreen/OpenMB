using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	/// <summary>
	/// Basic check box widget
	/// </summary>
	public class CheckBoxWidget : Widget
	{
		protected Mogre.TextAreaOverlayElement textAreaElement;
		protected Mogre.BorderPanelOverlayElement squareElement;
		protected Mogre.OverlayElement checkedMarkElement;
		protected bool isFitToContents;
		protected bool isCursorOver;
		public string Text
		{
			get
			{
				return textAreaElement.Caption;
			}
			set
			{
				textAreaElement.Caption = value;
				if (isFitToContents)
					element.Width = (GetCaptionWidth(value, ref textAreaElement) + squareElement.Width + 23f);
			}
		}
		// Do not instantiate any widgets directly. Use SdkTrayManager.
		public CheckBoxWidget(string name, string caption, float width)
		{
			isCursorOver = false;
			isFitToContents = (width <= 0f);
			element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/CheckBox", "BorderPanel", name);
			Mogre.OverlayContainer c = (Mogre.OverlayContainer)element;
			textAreaElement = (Mogre.TextAreaOverlayElement)c.GetChild(Name + "/CheckBoxCaption");
			squareElement = (Mogre.BorderPanelOverlayElement)c.GetChild(Name + "/CheckBoxSquare");
			checkedMarkElement = squareElement.GetChild(squareElement.Name + "/CheckBoxX");
			checkedMarkElement.Hide();
			element.Width = (width);
			Text = caption;
		}

		public bool isChecked()
		{
			return checkedMarkElement.IsVisible;
		}

		public void setChecked(bool dochecked)
		{
			setChecked(dochecked, true);
		}

		public void setChecked(bool @checked, bool notifyListener)
		{
			if (@checked)
				checkedMarkElement.Show();
			else
				checkedMarkElement.Hide();
			if (listener != null && notifyListener)
				listener.checkBoxToggled(this);
		}

		public void Toggle()
		{
			Toggle(true);
		}

		public void Toggle(bool notifyListener)
		{
			setChecked(!isChecked(), notifyListener);
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (isCursorOver && listener != null)
				Toggle();
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			if (IsCursorOver(squareElement, cursorPos, 5f))
			{
				if (!isCursorOver)
				{
					isCursorOver = true;
					squareElement.MaterialName = ("SdkTrays/MiniTextBox/Over");
					squareElement.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
				}
			}
			else
			{
				if (isCursorOver)
				{
					isCursorOver = false;
					squareElement.MaterialName = ("SdkTrays/MiniTextBox");
					squareElement.BorderMaterialName = ("SdkTrays/MiniTextBox");
				}
			}
		}

		public override void FocusLost()
		{
			squareElement.MaterialName = ("SdkTrays/MiniTextBox");
			squareElement.BorderMaterialName = ("SdkTrays/MiniTextBox");
			isCursorOver = false;
		}


	}
}
