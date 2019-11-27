using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{

	/// <summary>
	/// Abstract base class for all widgets
	/// </summary>
	public class Widget : IDisposable
	{
		protected OverlayElement element;
		protected UIWidgetLocation trayLoc;
		protected UIListener listener;
		protected Padding padding;
		public string Name
		{
			get
			{
				return element.Name;
			}
		}
		public virtual float DerivedTop
		{
			get
			{
				return element._getDerivedTop();
			}
		}
		public virtual float DerivedLeft
		{
			get
			{
				return element._getDerivedLeft();
			}
		}
		public virtual float Width
		{
			get
			{
				return element.Width;
			}
			set
			{
				element.Width = value;
			}
		}
		public virtual float Height
		{
			get
			{
				return element.Height;
			}
			set
			{
				element.Height = value;
			}
		}
		public float Left
		{
			get
			{
				return element.Left;
			}
			set
			{
				element.Left = value;
			}
		}
		public float Top
		{
			get
			{
				return element.Top;
			}
			set
			{
				element.Top = value;
			}
		}
		public int Row { get; set; }
		public int Col { get; set; }
		public int RowSpan { get; set; }
		public int ColSpan { get; set; }
		public AlignMode HorizontalAlignMode { get; set; }
		public AlignMode VerticalAlignMode { get; set; }
		public DockMode Dock { get; set; }
		public Padding Padding
		{
			get
			{
				return padding;
			}
		}
		public object UserData { get; set; }
		public OverlayElement OverlayElement
		{
			get
			{
				return element;
			}
		}
		public GuiMetricsMode MetricMode
		{
			get
			{
				return element.MetricsMode;
			}
			set
			{
				element.MetricsMode = value;
			}
		}
		public ushort ZOrder
		{
			get
			{
				return element.ZOrder;
			}
		}

		public Widget()
		{
			trayLoc = UIWidgetLocation.TL_NONE;
			element = null;
			listener = null;
			padding = new Padding();
		}
		/// <summary>
		/// dispose this widget
		/// </summary>
		public virtual void Dispose()
		{
		}

		public void Cleanup()
		{
			if (element != null)
			{
				NukeOverlayElement(element);
			}
			element = null;
			Dispose();
		}

		protected void AddChildOverlayElement(OverlayElement overlayElement)
		{
			(element as OverlayContainer).AddChild(overlayElement);
		}

		//        -----------------------------------------------------------------------------
		//		| Static utility method to recursively delete an overlay element plus
		//		| all of its children from the system.
		//		-----------------------------------------------------------------------------
		public static void NukeOverlayElement(OverlayElement element)
		{
			Mogre.OverlayContainer container = element as Mogre.OverlayContainer;
			if (container != null)
			{
				List<Mogre.OverlayElement> toDelete = new List<Mogre.OverlayElement>();

				Mogre.OverlayContainer.ChildIterator children = container.GetChildIterator();
				while (children.MoveNext())
				{
					toDelete.Add(children.Current);
				}

				for (int i = 0; i < toDelete.Count; i++)
				{
					NukeOverlayElement(toDelete[i]);
				}
			}
			if (element != null)
			{
				Mogre.OverlayContainer parent = element.Parent;
				if (parent != null)
					parent.RemoveChild(element.Name);
				Mogre.OverlayManager.Singleton.DestroyOverlayElement(element);
			}
		}

		/// <summary>
		/// Static utility method to check if the cursor is over an overlay element.
		/// </summary>
		/// <returns></returns>
		public static bool IsCursorOver(Mogre.OverlayElement element, Mogre.Vector2 cursorPos)
		{
			return IsCursorOver(element, cursorPos, 0f);
		}

		public static bool IsCursorOver(Mogre.OverlayElement element, Mogre.Vector2 cursorPos, float voidBorder)
		{
			if (element == null)
			{
				return false;
			}
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
			float detrivedLeft = element._getDerivedLeft();
			float detrivedTop = element._getDerivedTop();
			float l = detrivedLeft * om.ViewportWidth;
			float t = detrivedTop * om.ViewportHeight;
			float r = 0;
			float b = 0;
			if (element.MetricsMode == GuiMetricsMode.GMM_RELATIVE)
			{
				r = l + element.Width * om.ViewportWidth;
				b = t + element.Height * om.ViewportHeight;
			}
			else if (element.MetricsMode == GuiMetricsMode.GMM_PIXELS)
			{
				r = l + element.Width;
				b = t + element.Height;
			}

			bool b1 = cursorPos.x >= l + voidBorder;
			bool b2 = cursorPos.x <= r - voidBorder;
			bool b3 = cursorPos.y >= t + voidBorder;
			bool b4 = cursorPos.y <= b - voidBorder;

			return (b1 && b2 && b3 && b4);
		}

		public bool IsCursorOver(Vector2 cursorPos, float voidBorder = 0)
		{
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
			float detrivedLeft = DerivedLeft;
			float detrivedTop = DerivedTop;
			float l = detrivedLeft * om.ViewportWidth;
			float t = detrivedTop * om.ViewportHeight;
			float r = 0;
			float b = 0;
			if (MetricMode == GuiMetricsMode.GMM_RELATIVE)
			{
				r = l + Width * om.ViewportWidth;
				b = t + Height * om.ViewportHeight;
			}
			else if (MetricMode == GuiMetricsMode.GMM_PIXELS)
			{
				r = l + Width;
				b = t + Height;
			}

			bool b1 = cursorPos.x >= l + voidBorder;
			bool b2 = cursorPos.x <= r - voidBorder;
			bool b3 = cursorPos.y >= t + voidBorder;
			bool b4 = cursorPos.y <= b - voidBorder;

			return (b1 && b2 && b3 && b4);
		}

		/// <summary>
		/// Static utility method used to get the cursor's offset from the center of an overlay element in pixels.
		/// </summary>
		public static Mogre.Vector2 CursorOffset(Mogre.OverlayElement element, Mogre.Vector2 cursorPos)
		{
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
			return new Mogre.Vector2(cursorPos.x - (element._getDerivedLeft() * om.ViewportWidth + element.Width / 2), cursorPos.y - (element._getDerivedTop() * om.ViewportHeight + element.Height / 2f));
		}


		//public static Vector2 cursorOffset(Mogre.OverlayContainer containerElement,Vector2 cursorPos)
		//{
		//    Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;
		//    return new Mogre.Vector2(cursorPos.x - (containerElement._getDerivedLeft() * om.ViewportWidth + containerElement.Width / 2), cursorPos.y - (containerElement._getDerivedTop() * om.ViewportHeight + containerElement.Height / 2f));
		//}


		/// <summary>
		/// Static utility method used to get the width of a caption in a text area.
		/// </summary>
		protected static float GetCaptionWidth(string caption, ref TextAreaOverlayElement area)
		{
			Mogre.FontPtr font = null;
			if (Mogre.FontManager.Singleton.ResourceExists(area.FontName))
			{
				font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(area.FontName);
				if (!font.IsLoaded)
				{
					font.Load();
				}
			}
			else
			{
				OGRE_EXCEPT("this font:", area.FontName, "is not exist");
			}
			//Font font = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);
			string current = DisplayStringToString(caption);
			float lineWidth = 0f;

			for (int i = 0; i < current.Length; i++)
			{
				// be sure to provide a line width in the text area
				if (current[i] == ' ')
				{
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

			return lineWidth;
		}

		protected static float GetCaptionHeight(string caption, ref TextAreaOverlayElement area)
		{
			Mogre.FontPtr font = null;
			if (Mogre.FontManager.Singleton.ResourceExists(area.FontName))
			{
				font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(area.FontName);
				if (!font.IsLoaded)
				{
					font.Load();
				}
			}
			else
			{
				OGRE_EXCEPT("this font:", area.FontName, "is not exist");
			}
			//Font font = new Font(ft.Creator, ft.Name, ft.Handle, ft.Group, ft.IsManuallyLoaded);
			string current = DisplayStringToString(caption);
			float lineHeight = 0f;

			for (int i = 0; i < current.Length; i++)
			{
				if (current[i] == '\n')
				{
					var aspectRatio = font.GetGlyphAspectRatio(current[i]);
					if (aspectRatio <= 0)
					{
						lineHeight += aspectRatio * area.CharHeight;
					}
					else
					{
						lineHeight += area.CharHeight;
					}
				}
				else if (i == current.Length - 1)
				{
					lineHeight += area.CharHeight;
				}
			}

			return lineHeight;
		}

		protected static void OGRE_EXCEPT(string p, string p_2, string p_3)
		{
			throw new Exception(p + "_" + p_2 + "_" + p_3);
		}

		protected static string DisplayStringToString(string caption)
		{
			return caption;
		}

		/// <summary>
		/// Static utility method to cut off a string to fit in a text area.
		/// </summary>
		public static void FitCaptionToArea(string caption, ref Mogre.TextAreaOverlayElement area, float maxWidth)
		{
			Mogre.FontPtr font = null;
			if (Mogre.FontManager.Singleton.ResourceExists(area.FontName))
			{
				font = (Mogre.FontPtr)Mogre.FontManager.Singleton.GetByName(area.FontName);
				if (!font.IsLoaded)
				{
					font.Load();
				}
			}
			else
			{
				OGRE_EXCEPT("this font:", area.FontName, "is not exist");
			}
			Mogre.FontPtr f = font;
			string s = DisplayStringToString(caption);
			//int nl = s.find('\n');
			//if (nl != string.npos)
			//	s = s.substr(0, nl);
			int nl = s.IndexOf('\n');
			if (nl != -1) s = s.Substring(0, nl);

			float width = 0;

			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == ' ' && area.SpaceWidth != 0)
					width += area.SpaceWidth;
				else
					width += f.GetGlyphAspectRatio(s[i]) * area.CharHeight;
				if (width > maxWidth)
				{
					s = s.Substring(0, i);
					break;
				}
			}

			area.Caption = (s);
		}

		public UIWidgetLocation GetTrayLocation()
		{
			return trayLoc;
		}

		public void Hide()
		{
			element.Hide();
		}

		public void Show()
		{
			element.Show();
		}

		public bool IsVisible()
		{
			return element.IsVisible;
		}


		public virtual void CursorPressed(Mogre.Vector2 cursorPos)
		{
		}
		public virtual void CursorReleased(Mogre.Vector2 cursorPos)
		{
		}
		public virtual void CursorMoved(Mogre.Vector2 cursorPos)
		{
		}
		public virtual void MouseMoved(MouseEvent evt)
		{
		}
		public virtual void KeyPressed(uint text)
		{
		}
		public virtual void KeyReleased(uint text)
		{
		}
		public virtual void FocusLost()
		{
		}

		// internal methods used by SdkTrayManager. do not call directly.

		public void AssignToTray(UIWidgetLocation trayLoc)
		{
			this.trayLoc = trayLoc;
		}
		public void AssignListener(UIListener listener)
		{
			this.listener = listener;
		}

		/// <summary>
		/// Triggered when added this widget to another finished
		/// </summary>
		public virtual void AddedToAnotherWidgetFinished(
			AlignMode alignMode,
			float parentWidgetLeft,
			float parentWidgetWidth,
			float parentWidgetTop,
			float parentWidgetHeight)
		{
		}

	}
}
