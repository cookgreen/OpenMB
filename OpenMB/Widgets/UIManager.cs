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
using InputContext = MOIS.Mouse;
using Math = System.Math;
using Mogre;
using System;
using System.Collections.Generic;
using OpenMB.Mods.XML;
using OpenMB.Mods;
using System.Linq;
using System.Reflection;
using OpenMB.Localization;

namespace OpenMB.Widgets
{
	/// <summary>
	/// Enumerator values for widget tray anchoring locations
	/// </summary>
	public enum UIWidgetLocation : int
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

	/// <summary>
	/// Main class to manage a cursor, backdrop, trays and widgets
	/// </summary>
	public class UIManager : UIListener, IDisposable
	{
		private static UIManager instance;
		public static UIManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new UIManager();
				}
				return instance;
			}
		}
		protected string Name = ""; // name of this tray system
		protected Mogre.RenderWindow window; // render window
		protected InputContext inputContext = null;
		protected Mogre.Overlay backdropLayer; // backdrop layer
		protected Mogre.Overlay traysLayer; // widget layer
		protected Mogre.Overlay priorityLayer; // top priority layer
		protected Mogre.Overlay cursorLayer; // cursor layer
		protected Mogre.OverlayContainer mBackdrop; // backdrop
		protected Mogre.OverlayContainer[] mTrays = new Mogre.OverlayContainer[10]; // widget trays
		protected List<Widget>[] widgets = new List<Widget>[10]; // widgets
		protected List<Widget> widgetDeathRow = new List<Widget>(); // widget queue for deletion
		protected Mogre.OverlayContainer cursor; // cursor
		protected UIListener listener; // tray listener
		protected float widgetPadding = 0f; // widget padding
		protected float widgetSpacing = 0f; // widget spacing
		protected float trayPadding = 0f; // tray padding
		protected bool trayDrag; // a mouse press was initiated on a tray
		protected SelectMenuWidget expandedMenu; // top priority expanded menu widget
		protected TextBox dialog; // top priority dialog widget
		protected Mogre.OverlayContainer dialogShade; // top priority dialog shade
		protected ButtonWidget ok; // top priority OK button
		protected ButtonWidget yes; // top priority Yes button
		protected ButtonWidget no; // top priority No button
		protected bool cursorWasVisible; // cursor state before showing dialog
		protected LabelWidget fpsLabel; // FPS label
		protected ParamsPanelWidget statsPanel; // frame stats panel
		protected DecorWidget logo; // logo
		protected ProgressBarWidget loadBar; // loading bar
		protected float groupInitProportion = 0f; // proportion of load job assigned to initialising one resource group
		protected float groupLoadProportion = 0f; // proportion of load job assigned to loading one resource group
		protected float loadInc = 0f; // loading increment
		protected Mogre.GuiHorizontalAlignment[] trayWidgetAlign = new Mogre.GuiHorizontalAlignment[10]; // tray widget alignments
		protected Mogre.Timer timer; // Root::getSingleton().getTimer()
		protected uint mLastStatUpdateTime; // The last time the stat text were updated
		public UIListener Listener
		{
			get
			{
				return listener;
			}
		}

		public UIManager()
		{
		}

		public void Init(string name, Mogre.RenderWindow window, InputContext inputContext, UIListener listener)
		{
			for (int i = 0; i < widgets.Length; i++)
			{
				widgets[i] = new List<Widget>();
			}
			Name = name;
			this.window = window;
			this.inputContext = inputContext;
			this.listener = listener;
			widgetPadding = 8f;
			widgetSpacing = 2f;
			trayPadding = 0f;
			trayDrag = false;
			expandedMenu = null;
			dialog = null;
			ok = null;
			yes = null;
			no = null;
			cursorWasVisible = false;
			fpsLabel = null;
			statsPanel = null;
			logo = null;
			loadBar = null;
			groupInitProportion = 0.0f;
			groupLoadProportion = 0.0f;
			loadInc = 0.0f;
			timer = Mogre.Root.Singleton.Timer;
			mLastStatUpdateTime = 0;

			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

			string nameBase = Name + "/";
			nameBase = nameBase.Replace(' ', '_');
			backdropLayer = om.Create(nameBase + "BackdropLayer");
			traysLayer = om.Create(nameBase + "WidgetsLayer");
			priorityLayer = om.Create(nameBase + "PriorityLayer");
			cursorLayer = om.Create(nameBase + "CursorLayer");
			backdropLayer.ZOrder = (100);
			traysLayer.ZOrder = (200);
			priorityLayer.ZOrder = (300);
			cursorLayer.ZOrder = (400);

			// make backdrop and cursor overlay containers
			cursor = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
			cursorLayer.Add2D(cursor);
			mBackdrop = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "Backdrop");
			backdropLayer.Add2D(mBackdrop);
			dialogShade = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "DialogShade");
			dialogShade.MaterialName = ("SdkTrays/Shade");
			dialogShade.Hide();
			priorityLayer.Add2D(dialogShade);

			string[] trayNames = { "TopLeft", "Top", "TopRight", "Left", "Center", "Right", "BottomLeft", "Bottom", "BottomRight", "None" };

			for (uint i = 0; i < 9; i++) // make the real trays
			{
				mTrays[i] = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Tray", "BorderPanel", nameBase + trayNames[i] + "Tray");
				traysLayer.Add2D(mTrays[i]);

				trayWidgetAlign[i] = GuiHorizontalAlignment.GHA_CENTER;

				// align trays based on location
				if (i == (int)UIWidgetLocation.TL_TOP || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_BOTTOM)
					mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
				if (i == (int)UIWidgetLocation.TL_LEFT || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_RIGHT)
					mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
				if (i == (int)UIWidgetLocation.TL_TOPRIGHT || i == (int)UIWidgetLocation.TL_RIGHT || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
				if (i == (int)UIWidgetLocation.TL_BOTTOMLEFT || i == (int)UIWidgetLocation.TL_BOTTOM || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_BOTTOM);
			}

			// create the null tray for free-floating widgets
			mTrays[9] = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "NullTray");
			trayWidgetAlign[9] = GuiHorizontalAlignment.GHA_LEFT;
			traysLayer.Add2D(mTrays[9]);
			AdjustTrays();

			ShowTrays();
			ShowCursor();
		}
		/// <summary>
		/// Creates backdrop, cursor, and trays.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="window"></param>
		/// <param name="inputContext"></param>
		public UIManager(string name, Mogre.RenderWindow window, InputContext inputContext)
			: this(name, window, inputContext, null)
		{
		}

		public UIManager(string name, Mogre.RenderWindow window, InputContext inputContext, UIListener listener)
			: base()
		{
			for (int i = 0; i < widgets.Length; i++)
			{
				widgets[i] = new List<Widget>();
			}
			Name = name;
			this.window = window;
			this.inputContext = inputContext;
			this.listener = listener;
			widgetPadding = 8f;
			widgetSpacing = 2f;
			trayPadding = 0f;
			trayDrag = false;
			expandedMenu = null;
			dialog = null;
			ok = null;
			yes = null;
			no = null;
			cursorWasVisible = false;
			fpsLabel = null;
			statsPanel = null;
			logo = null;
			loadBar = null;
			groupInitProportion = 0.0f;
			groupLoadProportion = 0.0f;
			loadInc = 0.0f;
			timer = Mogre.Root.Singleton.Timer;
			mLastStatUpdateTime = 0;

			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

			string nameBase = Name + "/";
			nameBase = nameBase.Replace(' ', '_');
			backdropLayer = om.Create(nameBase + "BackdropLayer");
			traysLayer = om.Create(nameBase + "WidgetsLayer");
			priorityLayer = om.Create(nameBase + "PriorityLayer");
			cursorLayer = om.Create(nameBase + "CursorLayer");
			backdropLayer.ZOrder = (100);
			traysLayer.ZOrder = (200);
			priorityLayer.ZOrder = (300);
			cursorLayer.ZOrder = (400);

			// make backdrop and cursor overlay containers
			cursor = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
			cursorLayer.Add2D(cursor);
			mBackdrop = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "Backdrop");
			backdropLayer.Add2D(mBackdrop);
			dialogShade = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "DialogShade");
			dialogShade.MaterialName = ("SdkTrays/Shade");
			dialogShade.Hide();
			priorityLayer.Add2D(dialogShade);

			string[] trayNames = { "TopLeft", "Top", "TopRight", "Left", "Center", "Right", "BottomLeft", "Bottom", "BottomRight", "None" };

			for (uint i = 0; i < 9; i++) // make the real trays
			{
				mTrays[i] = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Tray", "BorderPanel", nameBase + trayNames[i] + "Tray");
				traysLayer.Add2D(mTrays[i]);

				trayWidgetAlign[i] = GuiHorizontalAlignment.GHA_CENTER;

				// align trays based on location
				if (i == (int)UIWidgetLocation.TL_TOP || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_BOTTOM)
					mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_CENTER);
				if (i == (int)UIWidgetLocation.TL_LEFT || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_RIGHT)
					mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
				if (i == (int)UIWidgetLocation.TL_TOPRIGHT || i == (int)UIWidgetLocation.TL_RIGHT || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].HorizontalAlignment = (GuiHorizontalAlignment.GHA_RIGHT);
				if (i == (int)UIWidgetLocation.TL_BOTTOMLEFT || i == (int)UIWidgetLocation.TL_BOTTOM || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].VerticalAlignment = (GuiVerticalAlignment.GVA_BOTTOM);
			}

			// create the null tray for free-floating widgets
			mTrays[9] = (Mogre.OverlayContainer)om.CreateOverlayElement("Panel", nameBase + "NullTray");
			trayWidgetAlign[9] = GuiHorizontalAlignment.GHA_LEFT;
			traysLayer.Add2D(mTrays[9]);
			AdjustTrays();

			ShowTrays();
			ShowCursor();
		}

		/// <summary>
		/// Destroys background, cursor, widgets, and trays.
		/// </summary>
		public override void Dispose()
		{
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

			DestroyAllWidgets();//clean up ok

			for (int i = 0; i < widgetDeathRow.Count; i++) // delete widgets queued for destruction
			{
				//delete mWidgetDeathRow[i];
				widgetDeathRow[i].Dispose();//?is there need?
				widgetDeathRow[i] = null;
			}
			widgetDeathRow.Clear();

			om.Destroy(backdropLayer);
			om.Destroy(traysLayer);
			om.Destroy(priorityLayer);
			om.Destroy(cursorLayer);

			closeDialog();

			Widget.NukeOverlayElement(mBackdrop);
			Widget.NukeOverlayElement(cursor);
			Widget.NukeOverlayElement(dialogShade);

			for (int i = 0; i < 10; i++)
			{
				Widget.NukeOverlayElement(mTrays[i]);
			}
			base.Dispose();
		}

		/// <summary>
		/// Converts a 2D screen coordinate (in pixels) to a 3D ray into the scene.
		/// </summary>
		/// <param name="cam"></param>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Mogre.Ray ScreenToScene(Mogre.Camera cam, Mogre.Vector2 pt)
		{
			return cam.GetCameraToViewportRay(pt.x, pt.y);
		}

		/// <summary>
		/// Converts a 3D scene position to a 2D screen position (in relative screen size, 0.0-1.0).
		/// </summary>
		/// <param name="cam"></param>
		/// <param name="pt"></param>
		/// <returns></returns>
		public static Mogre.Vector2 SceneToScreen(Mogre.Camera cam, Mogre.Vector3 pt)
		{
			Mogre.Vector3 result = cam.ProjectionMatrix * cam.ViewMatrix * pt;
			return new Mogre.Vector2((result.x + 1) / 2, (-result.y + 1) / 2);
		}

		/// <summary>
		/// these methods get the underlying overlays and overlay elements
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <returns></returns>
		public Mogre.OverlayContainer GetTrayContainer(UIWidgetLocation trayLoc)
		{
			return mTrays[(int)trayLoc];
		}
		public Mogre.Overlay GetBackdropLayer()
		{
			return backdropLayer;
		}
		public Mogre.Overlay GetTraysLayer()
		{
			return traysLayer;
		}
		public Mogre.Overlay GetCursorLayer()
		{
			return cursorLayer;
		}
		public Mogre.OverlayContainer GetBackdropContainer()
		{
			return mBackdrop;
		}
		public Mogre.OverlayContainer GetCursorContainer()
		{
			return cursor;
		}
		public Mogre.OverlayElement GetCursorImage()
		{
			return cursor.GetChild(cursor.Name + "/CursorImage");
		}

		public void SetListener(UIListener listener)
		{
			this.listener = listener;
		}

		public UIListener GetListener()
		{
			return listener;
		}

		public void ShowAll()
		{
			ShowBackdrop();
			ShowTrays();
			ShowCursor();
		}

		public void HideAll()
		{
			HideBackdrop();
			HideTrays();
			HideCursor();
		}

		/// <summary>
		/// Displays specified material on backdrop, or the last material used if
		/// none specified. Good for pause menus like in the browser.
		/// </summary>
		public void ShowBackdrop()
		{
			ShowBackdrop("");
		}
		public void ShowBackdrop(string materialName)
		{
			//if (materialName != Ogre::StringUtil::BLANK)
			if (!string.IsNullOrEmpty(materialName))
				mBackdrop.MaterialName = (materialName);
			backdropLayer.Show();
		}

		public void HideBackdrop()
		{
			backdropLayer.Hide();
		}

		/// <summary>
		/// Displays specified material on cursor, or the last material used if
		///	none specified. Used to change cursor type.
		/// </summary>
		public void ShowCursor()
		{
			ShowCursor("");
		}

		public void ShowCursor(string materialName)
		{
			if (!string.IsNullOrEmpty(materialName))
				GetCursorImage().MaterialName = (materialName);

			if (!cursorLayer.IsVisible)
			{
				cursorLayer.Show();
				RefreshCursor();
			}
		}

		public void HideCursor()
		{
			cursorLayer.Hide();
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < widgets[i].Count; j++)
				{
					widgets[i][j].FocusLost();
				}
			}

			setExpandedMenu(null);
		}

		/// <summary>
		/// Updates cursor position based on unbuffered mouse state. This is necessary
		///	because if the tray manager has been cut off from mouse events for a time,
		///	he cursor position will be out of date.
		/// </summary>
		public void RefreshCursor()
		{
			float x = 0f;
			float y = 0f;
			x = inputContext.MouseState.X.abs;
			y = inputContext.MouseState.Y.abs;
			cursor.SetPosition(x, y);
		}

		public void ShowTrays()
		{
			traysLayer.Show();
			priorityLayer.Show();
		}

		public void HideTrays()
		{
			traysLayer.Hide();
			priorityLayer.Hide();

			// give widgets a chance to reset in case they're in the middle of something
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < widgets[i].Count; j++)
				{
					widgets[i][j].FocusLost();
				}
			}

			setExpandedMenu(null);
		}

		public bool IsCursorVisible()
		{
			return cursorLayer.IsVisible;
		}
		public bool IsBackdropVisible()
		{
			return backdropLayer.IsVisible;
		}
		public bool AreTraysVisible()
		{
			return traysLayer.IsVisible;
		}

		/// <summary>
		/// Sets horizontal alignment of a tray's contents.
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <param name="gha"></param>
		public void SetTrayWidgetAlignment(UIWidgetLocation trayLoc, Mogre.GuiHorizontalAlignment gha)
		{
			trayWidgetAlign[(int)trayLoc] = gha;

			for (int i = 0; i < widgets[(int)trayLoc].Count; i++)
			{
				widgets[(int)trayLoc][i].OverlayElement.HorizontalAlignment = (gha);
			}
		}

		/// <summary>
		/// padding and spacing methods
		/// </summary>
		/// <param name="padding"></param>
		public void SetWidgetPadding(float padding)
		{
			widgetPadding = System.Math.Max((int)padding, 0);
			AdjustTrays();
		}

		public void SetWidgetSpacing(float spacing)
		{
			widgetSpacing = System.Math.Max((int)spacing, 0);
			AdjustTrays();
		}
		public void SetTrayPadding(float padding)
		{
			trayPadding = System.Math.Max((int)padding, 0);
			AdjustTrays();
		}


		public virtual float GetWidgetPadding()
		{
			return widgetPadding;
		}

		public virtual float GetWidgetSpacing()
		{
			return widgetSpacing;
		}

		public virtual float GetTrayPadding()
		{
			return trayPadding;
		}

		/// <summary>
		/// Fits trays to their contents and snaps them to their anchor locations.
		/// </summary>
		public virtual void AdjustTrays()
		{
			for (int i = 0; i < 9; i++) // resizes and hides trays if necessary
			{
				float trayWidth = 0;
				float trayHeight = widgetPadding;
				List<Mogre.OverlayElement> labelsAndSeps = new List<Mogre.OverlayElement>();

				//if (mWidgets[i].empty()) // hide tray if empty
				if (widgets[i].Count == 0)
				{
					mTrays[i].Hide();
					continue;
				}
				else
					mTrays[i].Show();

				// arrange widgets and calculate final tray size and position
				for (int j = 0; j < widgets[i].Count; j++)
				{
					Mogre.OverlayElement e = widgets[i][j].OverlayElement;

					if (j != 0) // don't space first widget
						trayHeight += widgetSpacing;

					e.VerticalAlignment = (GuiVerticalAlignment.GVA_TOP);
					e.Top = (trayHeight);

					switch (e.HorizontalAlignment)
					{
						case GuiHorizontalAlignment.GHA_LEFT:
							e.Left = (widgetPadding);
							break;
						case GuiHorizontalAlignment.GHA_RIGHT:
							e.Left = (-(e.Width + widgetPadding));
							break;
						default:
							e.Left = (-(e.Width / 2f));
							break;
					}

					// prevents some weird texture filtering problems (just some)
					e.SetPosition((int)e.Left, (int)e.Top);
					e.SetDimensions((int)e.Width, (int)e.Height);

					trayHeight += e.Height;

					LabelWidget l = widgets[i][j] as LabelWidget;
					if (l != null && l._isFitToTray())
					{
						labelsAndSeps.Add(e);
						continue;
					}
					Separator s = widgets[i][j] as Separator;
					if (s != null && s._isFitToTray())
					{
						labelsAndSeps.Add(e);
						continue;
					}

					if (e.Width > trayWidth)
						trayWidth = e.Width;
				}

				// add paddings and resize trays
				mTrays[i].Width = (trayWidth + 2 * widgetPadding);
				mTrays[i].Height = (trayHeight + widgetPadding);

				for (int j = 0; j < labelsAndSeps.Count; j++)
				{
					labelsAndSeps[j].Width = ((int)trayWidth);
					labelsAndSeps[j].Left = (-(int)(trayWidth / 2));
				}
			}

			for (uint i = 0; i < 9; i++) // snap trays to anchors
			{
				if (i == (int)UIWidgetLocation.TL_TOPLEFT || i == (int)UIWidgetLocation.TL_LEFT || i == (int)UIWidgetLocation.TL_BOTTOMLEFT)
					mTrays[i].Left = (trayPadding);
				if (i == (int)UIWidgetLocation.TL_TOP || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_BOTTOM)
					mTrays[i].Left = (-mTrays[i].Width / 2);
				if (i == (int)UIWidgetLocation.TL_TOPRIGHT || i == (int)UIWidgetLocation.TL_RIGHT || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].Left = (-(mTrays[i].Width + trayPadding));

				if (i == (int)UIWidgetLocation.TL_TOPLEFT || i == (int)UIWidgetLocation.TL_TOP || i == (int)UIWidgetLocation.TL_TOPRIGHT)
					mTrays[i].Top = (trayPadding);
				if (i == (int)UIWidgetLocation.TL_LEFT || i == (int)UIWidgetLocation.TL_CENTER || i == (int)UIWidgetLocation.TL_RIGHT)
					mTrays[i].Top = (-mTrays[i].Height / 2);
				if (i == (int)UIWidgetLocation.TL_BOTTOMLEFT || i == (int)UIWidgetLocation.TL_BOTTOM || i == (int)UIWidgetLocation.TL_BOTTOMRIGHT)
					mTrays[i].Top = (-mTrays[i].Height - trayPadding);

				// prevents some weird texture filtering problems (just some)
				mTrays[i].SetPosition((int)mTrays[i].Left, (int)mTrays[i].Top);
				mTrays[i].SetDimensions((int)mTrays[i].Width, (int)mTrays[i].Height);
			}
		}

		/// <summary>
		/// Returns a 3D ray into the scene that is directly underneath the cursor.
		/// </summary>
		/// <param name="cam"></param>
		/// <returns></returns>
		public Mogre.Ray GetCursorRay(Mogre.Camera cam)
		{
			return ScreenToScene(cam, new Mogre.Vector2(cursor._getLeft(), cursor._getTop()));
		}

		#region Factory Methods

		public Widget CreateWidget(ModData modData, ModUILayoutWidgetDfnXml xmlData)
		{
			//Find suitable constructor
			Type widgetType = null;
			foreach (var assembly in modData.Assemblies)
			{
				widgetType = assembly.GetTypes().Where(o => o.Name == xmlData.Type + "Widget").FirstOrDefault();
				if (widgetType != null)
				{
					break;
				}
			}

			ConstructorInfo constructorInfo = null;
			if (widgetType != null)
			{
				var flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
				constructorInfo = widgetType.GetConstructors(flags).FirstOrDefault();
			}

			//Prepare the parameters
			if (constructorInfo != null)
			{
				var p = constructorInfo.GetParameters();
				var a = new object[p.Length];
				for (var i = 0; i < p.Length; i++)
				{
					var key = p[i].Name;
					if (xmlData.WidgetParameters.Where(o => o.Name == key).Count() == 0)
						throw new Exception("Invalid Widget Parameter!");
					var widgetParameter = xmlData.WidgetParameters.Where(o => o.Name == key).FirstOrDefault();
                    var paramType = widgetParameter.Type;
                    if (!string.IsNullOrEmpty(widgetParameter.Value) && 
                        (widgetParameter.Value.StartsWith("str_") ||
                        widgetParameter.Value.StartsWith("@")))
                    {
                        string originaContent = "No such Key";
                        var stringInfo = modData.StringInfos.Where(o => o.ID == widgetParameter.Value).FirstOrDefault();
                        if (stringInfo != null)
                        {
                            originaContent = stringInfo.Content;
                        }
                        widgetParameter.Value = LocateSystem.Instance.GetLocalizedString(widgetParameter.Value, originaContent);
                    }
                    switch(paramType)
                    {
                        case "String":
                            a[i] = widgetParameter.Value;
                            break;
                        case "Float":
                            a[i] = float.Parse(widgetParameter.Value);
                            break;
                        case "Double":
                            a[i] = double.Parse(widgetParameter.Value);
                            break;
                        case "Integer":
                            a[i] = int.Parse(widgetParameter.Value);
                            break;
                        case "UnsigedInteger":
                            a[i] = uint.Parse(widgetParameter.Value);
                            break;
                        case "Boolean":
                            a[i] = bool.Parse(widgetParameter.Value);
                            break;
                    }
				}

				//Invoke the constructor and get the widget object
				Widget w = constructorInfo.Invoke(a) as Widget;
                moveWidgetToTray(w, xmlData.TrayLocation);
				return w;
			}

			return null;
		}
		internal ButtonWidget CreateButton(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CreateButton(trayLoc, name, caption, 0f);
		}
		internal ButtonWidget CreateButton(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			ButtonWidget b = new ButtonWidget(name, caption, width);
			moveWidgetToTray(b, trayLoc);
			b.AssignListener(listener);
			return b;
		}
		internal ButtonWidget CreateButton(string name, string caption, float width)
		{
			ButtonWidget b = new ButtonWidget(name, caption, width);
			moveWidgetToTray(b, UIWidgetLocation.TL_NONE);
			b.AssignListener(listener);
			return b;
		}
		internal TextBox CreateTextBox(UIWidgetLocation trayLoc, string name, string caption, float width, float height)
		{
			TextBox tb = new TextBox(name, caption, width, height);
			moveWidgetToTray(tb, trayLoc);
			tb.AssignListener(listener);
			return tb;
		}
		internal SelectMenuWidget CreateThickSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, uint maxItemsShown)
		{
			return CreateThickSelectMenu(trayLoc, name, caption, width, maxItemsShown, new List<string>());
		}
		internal SelectMenuWidget CreateThickSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, uint maxItemsShown, List<string> items)
		{
			SelectMenuWidget sm = new SelectMenuWidget(name, caption, width, 0f, maxItemsShown);
			moveWidgetToTray(sm, trayLoc);
			sm.AssignListener(listener);
			//if (!items.empty())
			if (items.Count > 0)
				sm.SetItems(items);
			return sm;
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown)
		{
			return CreateLongSelectMenu(trayLoc, name, caption, width, boxWidth, maxItemsShown, new List<string>());
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown, List<string> items)
		{
			SelectMenuWidget sm = new SelectMenuWidget(name, caption, width, boxWidth, maxItemsShown);
			moveWidgetToTray(sm, trayLoc);
			sm.AssignListener(listener);
			//if (!items.empty())
			if (items.Count > 0)
				sm.SetItems(items);
			return sm;
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown)
		{
			return CreateLongSelectMenu(trayLoc, name, caption, boxWidth, maxItemsShown, new List<string>());
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown, List<string> items)
		{
			return CreateLongSelectMenu(trayLoc, name, caption, 0, boxWidth, maxItemsShown, items);
		}
		internal LabelWidget CreateLabel(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CreateLabel(trayLoc, name, caption, 0f);
		}
		internal LabelWidget CreateLabel(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			LabelWidget l = new LabelWidget(name, caption, width);
			moveWidgetToTray(l, trayLoc);
			l.AssignListener(listener);
			return l;
		}
		internal StaticText CreateStaticText(string name, string text)
		{
			return CreateStaticText(UIWidgetLocation.TL_NONE, name, text, 0f, false, ColourValue.Black);
		}
		internal StaticText CreateStaticText(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CreateStaticText(trayLoc, name, caption, 0f, false, ColourValue.Black);
		}
		internal StaticText CreateStaticText(UIWidgetLocation trayLoc, string name, string caption, ColourValue color)
		{
			return CreateStaticText(trayLoc, name, caption, 0f, true, color);
		}
		internal StaticText CreateStaticText(UIWidgetLocation trayLoc, string name, string caption, float width, bool specificColor, ColourValue color)
		{
			StaticText st = new StaticText(name, caption, width, specificColor, ColourValue.Black);
			moveWidgetToTray(st, trayLoc);
			st.AssignListener(listener);
			return st;
		}
		internal Separator CreateSeparator(UIWidgetLocation trayLoc, string name)
		{
			return CreateSeparator(trayLoc, name, 0f);
		}
		internal Separator CreateSeparator(UIWidgetLocation trayLoc, string name, float width)
		{
			Separator s = new Separator(name, width);
			moveWidgetToTray(s, trayLoc);
			return s;
		}
		internal Slider CreateThickSlider(UIWidgetLocation trayLoc, string name, string caption, float width, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			Slider s = new Slider(name, caption, width, 0, valueBoxWidth, minValue, maxValue, snaps);
			moveWidgetToTray(s, trayLoc);
			s.AssignListener(listener);
			return s;
		}
		internal Slider CreateLongSlider(UIWidgetLocation trayLoc, string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			if (trackWidth <= 0)
				trackWidth = 1;
			Slider s = new Slider(name, caption, width, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
			moveWidgetToTray(s, trayLoc);
			s.AssignListener(listener);
			return s;
		}
		internal Slider CreateLongSlider(UIWidgetLocation trayLoc, string name, string caption, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			return CreateLongSlider(trayLoc, name, caption, 0, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
		}
		internal ParamsPanelWidget CreateParamsPanel(UIWidgetLocation trayLoc, string name, float width, uint lines)
		{
			ParamsPanelWidget pp = new ParamsPanelWidget(name, width, lines);
			moveWidgetToTray(pp, trayLoc);
			return pp;
		}
		internal ParamsPanelWidget CreateParamsPanel(UIWidgetLocation trayLoc, string name, float width, StringVector paramNames)
		{
			ParamsPanelWidget pp = new ParamsPanelWidget(name, width, (uint)paramNames.Count);
			pp.SetAllParamNames(paramNames);
			moveWidgetToTray(pp, trayLoc);
			return pp;
		}
		internal ParamsPanelWidget CreateParamsPanel(UIWidgetLocation trayLoc, string name, float width, string[] paramNames)
		{
			StringVector sv = new StringVector();
			foreach (var v in paramNames)
			{
				sv.Add(v);
			}
			return CreateParamsPanel(trayLoc, name, width, sv);
		}
		internal CheckBoxWidget CreateCheckBox(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CreateCheckBox(trayLoc, name, caption, 0f);
		}
		internal CheckBoxWidget CreateCheckBox(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			CheckBoxWidget cb = new CheckBoxWidget(name, caption, width);
			moveWidgetToTray(cb, trayLoc);
			cb.AssignListener(listener);
			return cb;
		}
		internal DecorWidget CreateDecorWidget(UIWidgetLocation trayLoc, string name, string templateName)
		{
			DecorWidget dw = new DecorWidget(name, templateName);
			moveWidgetToTray(dw, trayLoc);
			return dw;
		}
		internal ProgressBarWidget CreateProgressBar(UIWidgetLocation trayLoc, string name, string caption, float width, float commentBoxWidth)
		{
			ProgressBarWidget pb = new ProgressBarWidget(name, caption, width, commentBoxWidth);
			moveWidgetToTray(pb, trayLoc);
			return pb;
		}
		internal ListViewWidget CreateListView(UIWidgetLocation trayLoc, string name, float height, float width, List<string> columnNames)
		{
			ListViewWidget lsv = new ListViewWidget(name, -1, -1, height, width, columnNames);
			moveWidgetToTray(lsv, trayLoc);
			lsv.AssignListener(listener);
			return lsv;
		}
		internal InputBoxWidget CreateInputBox(UIWidgetLocation trayLocation, string name, string caption, float width, float boxWidth, string text = null, bool onlyAcceptNum = false)
		{
			InputBoxWidget ib = new InputBoxWidget(name, caption, width, boxWidth, text, onlyAcceptNum);
			moveWidgetToTray(ib, trayLocation);
			ib.Text = text;
			//ib._assignListener(mListener);
			return ib;
		}
		internal PanelWidget CreatePanel(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1)
		{
			PanelWidget panel = new PanelWidget(name, width, height, left, top, row, col);
			moveWidgetToTray(panel, UIWidgetLocation.TL_NONE);
			return panel;
		}
		internal PanelScrollableWidget CreateScrollablePanel(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true)
		{
			PanelScrollableWidget scrollablePanel = new PanelScrollableWidget(name, width, height, left, top, row, col, hasBorder);
			moveWidgetToTray(scrollablePanel, UIWidgetLocation.TL_NONE);
			return scrollablePanel;
		}
		internal PanelMaterialWidget CreateMaterialPanel(string name, string texture, float width = 0, float height = 0, float left = 0, float top = 0)
		{
			PanelMaterialWidget materialPanel = new PanelMaterialWidget(name, texture, width, height, left, top);
			moveWidgetToTray(materialPanel, UIWidgetLocation.TL_NONE);
			return materialPanel;
		}
		internal PanelTemplateWidget CreateTemplatePanel(string name, string template, int width = 0, int height = 0, int top = 0, int left = 0)
		{
			PanelTemplateWidget tmpPanel = new PanelTemplateWidget(name, template, width, height, left, top);
			moveWidgetToTray(tmpPanel, UIWidgetLocation.TL_NONE);
			return tmpPanel;
		}
		#endregion

		/// <summary>
		/// Shows frame statistics widget set in the specified location.
		/// </summary>
		/// <param name="trayLoc"></param>
		public void showFrameStats(UIWidgetLocation trayLoc)
		{
			showFrameStats(trayLoc, -1);
		}

		public void showFrameStats(UIWidgetLocation trayLoc, int place)
		{
			if (!AreFrameStatsVisible())
			{
				StringVector stats = new StringVector();
				stats.Add("Average FPS");
				stats.Add("Best FPS");
				stats.Add("Worst FPS");
				stats.Add("Triangles");
				stats.Add("Batches");

				fpsLabel = CreateLabel(UIWidgetLocation.TL_NONE, Name + "/FpsLabel", "FPS:", 180);
				fpsLabel.AssignListener(this);
				statsPanel = CreateParamsPanel(UIWidgetLocation.TL_NONE, Name + "/StatsPanel", 180, stats);
			}

			moveWidgetToTray(fpsLabel, trayLoc, place);
			moveWidgetToTray(statsPanel, trayLoc, LocateWidgetInTray(fpsLabel) + 1);
		}

		/// <summary>
		/// Hides frame statistics widget set.
		/// </summary>
		public void HideFrameStats()
		{
			if (AreFrameStatsVisible())
			{
				DestroyWidget(fpsLabel);
				DestroyWidget(statsPanel);
				//delete mFpsLabel
				//delete mStatsPanel
				//mFpsLabel.Dispose();
				//mStatsPanel.Dispose();
				fpsLabel = null;
				statsPanel = null;
			}
		}

		public bool AreFrameStatsVisible()
		{
			return fpsLabel != null;
		}

		/// <summary>
		/// Toggles visibility of advanced statistics.
		/// </summary>
		public void ToggleAdvancedFrameStats()
		{
			if (fpsLabel != null)
				LabelHit(fpsLabel);
		}

		/// <summary>
		/// Shows logo in the specified location.
		/// </summary>
		/// <param name="trayLoc"></param>
		public void ShowLogo(UIWidgetLocation trayLoc)
		{
			ShowLogo(trayLoc, -1);
		}

		public void ShowLogo(UIWidgetLocation trayLoc, int place)
		{
			if (!isLogoVisible())
				logo = CreateDecorWidget(UIWidgetLocation.TL_NONE, Name + "/Logo", "SdkTrays/Logo");
			moveWidgetToTray(logo, trayLoc, place);
		}

		public void hideLogo()
		{
			if (isLogoVisible())
			{
				DestroyWidget(logo);
				logo.Dispose();
				logo = null;
			}
		}

		public bool isLogoVisible()
		{
			return logo != null;
		}

		public bool isLoadingBarVisible()
		{
			return loadBar != null;
		}

		/// <summary>
		/// Pops up a message dialog with an OK button.
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="message"></param>
		public void showOkDialog(string caption, string message)
		{
			Mogre.OverlayElement e;

			if (dialog != null)
			{
				dialog.setCaption(caption);
				dialog.setText(message);

				if (ok != null)
					return;
				else
				{
					yes.Cleanup();
					no.Cleanup();
					//delete mYes;
					//delete mNo;
					yes.Dispose();
					no.Dispose();
					yes = null;
					no = null;
				}
			}
			else
			{
				// give widgets a chance to reset in case they're in the middle of something
				for (int i = 0; i < 10; i++)
				{
					for (int j = 0; j < widgets[i].Count; j++)
					{
						widgets[i][j].FocusLost();
					}
				}

				dialogShade.Show();

				dialog = new TextBox(Name + "/DialogBox", caption, 300f, 208f);
				dialog.setText(message);
				e = dialog.OverlayElement;
				dialogShade.AddChild(e);
				e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
				e.Left = (-(e.Width / 2f));
				e.Top = (-(e.Height / 2f));

				cursorWasVisible = IsCursorVisible();
				ShowCursor();
			}

			ok = new ButtonWidget(Name + "/OkButton", "OK", 60f);
			ok.AssignListener(this);
			e = ok.OverlayElement;
			dialogShade.AddChild(e);
			e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
			e.Left = (-(e.Width / 2f));
			e.Top = (dialog.OverlayElement.Top + dialog.OverlayElement.Height + 5f);
		}

		/// <summary>
		/// Pops up a question dialog with Yes and No buttons.
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="question"></param>
		public void showYesNoDialog(string caption, string question)
		{
			Mogre.OverlayElement e;

			if (dialog != null)
			{
				dialog.setCaption(caption);
				dialog.setText(question);

				if (ok != null)
				{
					ok.Cleanup();
					//delete mOk;
					ok.Dispose();
					ok = null;
				}
				else
					return;
			}
			else
			{
				// give widgets a chance to reset in case they're in the middle of something
				for (int i = 0; i < 10; i++)
				{
					for (int j = 0; j < widgets[i].Count; j++)
					{
						widgets[i][j].FocusLost();
					}
				}

				dialogShade.Show();

				dialog = new TextBox(Name + "/DialogBox", caption, 300f, 208f);
				dialog.setText(question);
				e = dialog.OverlayElement;
				dialogShade.AddChild(e);
				e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
				e.Left = (-(e.Width / 2f));
				e.Top = (-(e.Height / 2f));

				cursorWasVisible = IsCursorVisible();
				ShowCursor();
			}

			yes = new ButtonWidget(Name + "/YesButton", "Yes", 58f);
			yes.AssignListener(this);
			e = yes.OverlayElement;
			dialogShade.AddChild(e);
			e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
			e.Left = (-(e.Width + 2f));
			e.Top = (dialog.OverlayElement.Top + dialog.OverlayElement.Height + 5f);

			no = new ButtonWidget(Name + "/NoButton", "No", 50f);
			no.AssignListener(this);
			e = no.OverlayElement;
			dialogShade.AddChild(e);
			e.VerticalAlignment = (GuiVerticalAlignment.GVA_CENTER);
			e.Left = (3f);
			e.Top = (dialog.OverlayElement.Top + dialog.OverlayElement.Height + 5f);
		}

		/// <summary>
		/// Hides whatever dialog is currently showing.
		/// </summary>
		public void closeDialog()
		{
			if (dialog != null)
			{
				if (ok != null)
				{
					ok.Cleanup();
					//delete mOk;
					ok.Dispose();
					ok = null;
				}
				else
				{
					yes.Cleanup();
					no.Cleanup();
					//delete mYes;
					//delete mNo;
					yes.Dispose();
					no.Dispose();
					yes = null;
					no = null;
				}

				dialogShade.Hide();
				dialog.Cleanup();
				//delete mDialog;
				dialog.Dispose();
				dialog = null;

				if (!cursorWasVisible)
					HideCursor();
			}
		}

		/// <summary>
		/// Determines if any dialog is currently visible.
		/// </summary>
		/// <returns></returns>
		public bool IsDialogVisible()
		{
			return dialog != null;
		}

		/// <summary>
		/// Gets a widget from a tray by place.
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <param name="place"></param>
		/// <returns></returns>
		public Widget GetWidget(UIWidgetLocation trayLoc, uint place)
		{
			if (place < widgets[(int)trayLoc].Count)
				return widgets[(int)trayLoc][(int)place];
			return null;
		}

		/// <summary>
		/// Gets a widget from a tray by name.
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public Widget GetWidget(UIWidgetLocation trayLoc, string name)
		{
			for (int i = 0; i < widgets[(int)trayLoc].Count; i++)
			{
				if (widgets[(int)trayLoc][i].Name == name)
					return widgets[(int)trayLoc][i];
			}
			return null;
		}

		/// <summary>
		/// Gets a widget by name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Widget GetWidget(string name)
		{
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < widgets[i].Count; j++)
				{
					if (widgets[i][j].Name == name)
						return widgets[i][j];
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the number of widgets in total.
		/// </summary>
		/// <returns></returns>
		public uint GetNumWidgets()
		{
			uint total = 0;

			for (int i = 0; i < 10; i++)
			{
				total += (uint)widgets[i].Count;
			}

			return total;
		}

		/// <summary>
		/// Gets the number of widgets in a tray.
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <returns></returns>
		public int GetNumWidgets(UIWidgetLocation trayLoc)
		{
			return widgets[(int)trayLoc].Count;
		}

		/// <summary>
		/// Gets all the widgets of a specific tray.
		/// </summary>
		/// <param name="trayLoc"></param>
		/// <returns></returns>
		public List<Widget> GetWidgetIterator(UIWidgetLocation trayLoc)
		{
			return widgets[(int)trayLoc];
			//return Mogre.VectorIterator<List<Widget*>>(mWidgets[(int)trayLoc].begin(), mWidgets[(int)trayLoc].end());
		}

		/// <summary>
		/// Gets a widget's position in its tray.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public int LocateWidgetInTray(Widget widget)
		{
			for (int i = 0; i < widgets[(int)widget.GetTrayLocation()].Count; i++)
			{
				if (widgets[(int)widget.GetTrayLocation()][i] == widget)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Destroys a widget.
		/// </summary>
		/// <param name="widget"></param>
		public void DestroyWidget(Widget widget)
		{
			if (widget == null)
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", "Widget does not exist.", "TrayManager::destroyWidget");

			// in case special widgets are destroyed manually, set them to 0
			if (widget == logo)
				logo = null;
			else if (widget == statsPanel)
				statsPanel = null;
			else if (widget == fpsLabel)
				fpsLabel = null;

			mTrays[(int)widget.GetTrayLocation()].RemoveChild(widget.Name);

			List<Widget> wList = widgets[(int)widget.GetTrayLocation()];
			//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
			//wList.erase(std.find(wList.GetEnumerator(), wList.end(), widget));
			for (int j = wList.Count - 1; j >= 0; j--)
			{
				if (wList[j] == widget)
				{
					wList.RemoveAt(j);
				}
			}
			if (widget == expandedMenu)
				setExpandedMenu(null);

			widget.Cleanup();

			widgetDeathRow.Add(widget);

			AdjustTrays();
		}

		private void OGRE_EXCEPT(string p, string p_2, string p_3)
		{
			throw new Exception(p + "_" + p_2 + "_" + p_3);
		}

		public void DestroyWidget(UIWidgetLocation trayLoc, uint place)
		{
			DestroyWidget(GetWidget(trayLoc, place));
		}

		public void DestroyWidget(UIWidgetLocation trayLoc, string name)
		{
			DestroyWidget(GetWidget(trayLoc, name));
		}

		public void DestroyWidget(string name)
		{
			DestroyWidget(GetWidget(name));
		}

		/// <summary>
		/// Destroys all widgets in a tray.
		/// </summary>
		/// <param name="trayLoc"></param>
		public void DestroyAllWidgetsInTray(UIWidgetLocation trayLoc)
		{
			//while (!mWidgets[(int)trayLoc].empty())
			while (widgets[(int)trayLoc].Count > 0)
				DestroyWidget(widgets[(int)trayLoc][0]);
		}

		/// <summary>
		/// Destroys all widgets.
		/// </summary>
		public void DestroyAllWidgets()
		{
			for (uint i = 0; i < 10; i++) // destroy every widget in every tray (including null tray)
			{
				DestroyAllWidgetsInTray((UIWidgetLocation)i);
			}
		}

		/// <summary>
		/// Adds a widget to a specified tray.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="trayLoc"></param>
		public void moveWidgetToTray(Widget widget, UIWidgetLocation trayLoc)
		{
			moveWidgetToTray(widget, trayLoc, -1);
		}

		public void moveWidgetToTray(Widget widget, UIWidgetLocation trayLoc, int place)
		{
			if (widget == null)
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", "Widget does not exist.", "TrayManager::moveWidgetToTray");

			// remove widget from old tray
			List<Widget> wList = widgets[(int)widget.GetTrayLocation()];
			for (int j = wList.Count - 1; j >= 0; j--)
			{
				if (wList[j] == widget)
				{
					wList.RemoveAt(j);
					mTrays[(int)widget.GetTrayLocation()].RemoveChild(widget.Name);
				}
			}


			// insert widget into new tray at given position, or at the end if unspecified or invalid
			if (place == -1 || place > (int)widgets[(int)trayLoc].Count)
				place = (int)widgets[(int)trayLoc].Count;
			widgets[(int)trayLoc].Insert(place, widget);
			mTrays[(int)trayLoc].AddChild(widget.OverlayElement);

			widget.OverlayElement.HorizontalAlignment = (trayWidgetAlign[(int)trayLoc]);

			// adjust trays if necessary
			if (widget.GetTrayLocation() != UIWidgetLocation.TL_NONE || trayLoc != UIWidgetLocation.TL_NONE)
				AdjustTrays();

			widget.AssignToTray(trayLoc);
		}

		public void moveWidgetToTray(string name, UIWidgetLocation trayLoc)
		{
			moveWidgetToTray(name, trayLoc, -1);
		}

		public void moveWidgetToTray(string name, UIWidgetLocation trayLoc, int place)
		{
			moveWidgetToTray(GetWidget(name), trayLoc, place);
		}

		public void moveWidgetToTray(UIWidgetLocation currentTrayLoc, string name, UIWidgetLocation targetTrayLoc)
		{
			moveWidgetToTray(currentTrayLoc, name, targetTrayLoc, -1);
		}
		//ORIGINAL LINE: void moveWidgetToTray(TrayLocation currentTrayLoc, const Ogre::String& name, TrayLocation targetTrayLoc, int place = -1)
		public void moveWidgetToTray(UIWidgetLocation currentTrayLoc, string name, UIWidgetLocation targetTrayLoc, int place)
		{
			moveWidgetToTray(GetWidget(currentTrayLoc, name), targetTrayLoc, place);
		}

		public void moveWidgetToTray(UIWidgetLocation currentTrayLoc, uint currentPlace, UIWidgetLocation targetTrayLoc)
		{
			moveWidgetToTray(currentTrayLoc, currentPlace, targetTrayLoc, -1);
		}

		public void moveWidgetToTray(UIWidgetLocation currentTrayLoc, uint currentPlace, UIWidgetLocation targetTrayLoc, int targetPlace)
		{
			moveWidgetToTray(GetWidget(currentTrayLoc, currentPlace), targetTrayLoc, targetPlace);
		}

		/// <summary>
		/// Removes a widget from its tray. Same as moving it to the null tray.
		/// </summary>
		/// <param name="widget"></param>
		public void removeWidgetFromTray(Widget widget)
		{
			moveWidgetToTray(widget, UIWidgetLocation.TL_NONE);
		}

		public void removeWidgetFromTray(string name)
		{
			removeWidgetFromTray(GetWidget(name));
		}

		public void removeWidgetFromTray(UIWidgetLocation trayLoc, string name)
		{
			removeWidgetFromTray(GetWidget(trayLoc, name));
		}

		public void removeWidgetFromTray(UIWidgetLocation trayLoc, uint place)
		{
			removeWidgetFromTray(GetWidget(trayLoc, place));
		}

		//        -----------------------------------------------------------------------------
		//		| Removes all widgets from a widget tray.
		//		-----------------------------------------------------------------------------
		public void clearTray(UIWidgetLocation trayLoc)
		{
			if (trayLoc == UIWidgetLocation.TL_NONE) // can't clear the null tray
				return;

			//while (!mWidgets[(int)trayLoc].empty()) // remove every widget from given tray
			while (widgets[(int)trayLoc].Count > 0)
			{
				removeWidgetFromTray(widgets[(int)trayLoc][0]);
			}
		}

		//        -----------------------------------------------------------------------------
		//		| Removes all widgets from all widget trays.
		//		-----------------------------------------------------------------------------
		public void clearAllTrays()
		{
			for (uint i = 0; i < 9; i++)
			{
				clearTray((UIWidgetLocation)i);
			}
		}

		//        -----------------------------------------------------------------------------
		//		| Process frame events. Updates frame statistics widget set and deletes
		//		| all widgets queued for destruction.
		//		-----------------------------------------------------------------------------
		public bool FrameRenderingQueued(Mogre.FrameEvent evt)
		{
			for (int i = 0; i < widgetDeathRow.Count; i++)
			{
				widgetDeathRow[i] = null;
			}
			widgetDeathRow.Clear();


			uint currentTime = timer.Milliseconds;
			if (AreFrameStatsVisible() && (currentTime - mLastStatUpdateTime > 250))
			{
				Mogre.RenderTarget.FrameStats stats = window.GetStatistics();

				mLastStatUpdateTime = currentTime;

				string s = ("FPS: ");
				s += ((int)stats.LastFPS).ToString();

				fpsLabel.setCaption(s);

				if (statsPanel.OverlayElement.IsVisible)
				{
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
					statsPanel.SetAllParamValues(values);
				}
			}

			return true;
		}





		//        -----------------------------------------------------------------------------
		//		| Toggles visibility of advanced statistics.
		//		-----------------------------------------------------------------------------
		public void LabelHit(LabelWidget label)
		{
			if (statsPanel.OverlayElement.IsVisible)
			{
				statsPanel.OverlayElement.Hide();
				fpsLabel.OverlayElement.Width = (150f);
				removeWidgetFromTray(statsPanel);
			}
			else
			{
				statsPanel.OverlayElement.Show();
				fpsLabel.OverlayElement.Width = (180f);
				moveWidgetToTray(statsPanel, fpsLabel.GetTrayLocation(), LocateWidgetInTray(fpsLabel) + 1);
			}
		}

		//        -----------------------------------------------------------------------------
		//		| Destroys dialog widgets, notifies listener, and ends high priority session.
		//		-----------------------------------------------------------------------------
		public void ButtonHit(ButtonWidget button)
		{
			if (listener != null)
			{
				if (button == ok)
					listener.okDialogClosed(dialog.getText());
				else
					listener.yesNoDialogClosed(dialog.getText(), button == yes);
			}
			closeDialog();
		}

		//        -----------------------------------------------------------------------------
		//		| Processes mouse button down events. Returns true if the event was
		//		| consumed and should not be passed on to other handlers.
		//		-----------------------------------------------------------------------------

		public bool InjectMouseDown(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
		{
			if (!cursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
				return false;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(cursor.Left, cursor.Top);

			trayDrag = false;

			if (expandedMenu != null) // only check top priority widget until it passes on
			{
				expandedMenu.CursorPressed(cursorPos);
				if (!expandedMenu.Expanded())
					setExpandedMenu(null);
				return true;
			}

			if (dialog != null) // only check top priority widget until it passes on
			{
				dialog.CursorPressed(cursorPos);
				if (ok != null)
					ok.CursorPressed(cursorPos);
				else
				{
					yes.CursorPressed(cursorPos);
					no.CursorPressed(cursorPos);
				}
				return true;
			}

			for (uint i = 0; i < 9; i++) // check if mouse is over a non-null tray
			{
				if (mTrays[i].IsVisible && Widget.IsCursorOver(mTrays[i], cursorPos, 2f))
				{
					trayDrag = true; // initiate a drag that originates in a tray
					break;
				}
			}

			for (int i = 0; i < widgets[9].Count; i++) // check if mouse is over a non-null tray's widgets
			{
				if (widgets[9][i].OverlayElement.IsVisible && Widget.IsCursorOver(widgets[9][i].OverlayElement, cursorPos))
				{
					trayDrag = true; // initiate a drag that originates in a tray
					break;
				}
			}


			for (int i = 0; i < 10; i++)
			{
				if (!mTrays[i].IsVisible)
					continue;

				for (int j = 0; j < widgets[i].Count; j++)
				{
					Widget w = widgets[i][j];
					if (!w.OverlayElement.IsVisible)
						continue;
					w.CursorPressed(cursorPos); // send event to widget

					SelectMenuWidget m = w as SelectMenuWidget;
					if (m != null && m.Expanded()) // a menu has begun a top priority session
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

		public bool InjectMouseUp(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
		{
			if (!cursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
				return false;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(cursor.Left, cursor.Top);

			if (expandedMenu != null) // only check top priority widget until it passes on
			{
				expandedMenu.CursorReleased(cursorPos);
				return true;
			}

			if (dialog != null) // only check top priority widget until it passes on
			{
				dialog.CursorReleased(cursorPos);
				if (ok != null)
					ok.CursorReleased(cursorPos);
				else
				{
					yes.CursorReleased(cursorPos);
					// very important to check if second button still exists, because first button could've closed the popup
					if (no != null)
						no.CursorReleased(cursorPos);
				}
				return true;
			}

			if (!trayDrag) // this click did not originate in a tray, so don't process
				return false;

			Widget w = null;

			for (int i = 0; i < 10; i++)
			{
				if (!mTrays[i].IsVisible)
					continue;

				for (int j = 0; j < widgets[i].Count; j++)
				{
					w = widgets[i][j];
					if (!w.OverlayElement.IsVisible)
						continue;
					w.CursorReleased(cursorPos); // send event to widget
				}
			}

			trayDrag = false; // stop this drag
			return true; // this click did originate in this tray, so don't pass it on
		}

		//        -----------------------------------------------------------------------------
		//		| Updates cursor position. Returns true if the event was
		//		| consumed and should not be passed on to other handlers.
		//		-----------------------------------------------------------------------------
		public bool InjectMouseMove(MOIS.MouseEvent evt)
		{
			if (!cursorLayer.IsVisible) // don't process if cursor layer is invisible
				return false;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(evt.state.X.abs, evt.state.Y.abs);
			cursor.SetPosition(cursorPos.x, cursorPos.y);

			if (expandedMenu != null) // only check top priority widget until it passes on
			{
				expandedMenu.CursorMoved(cursorPos);
				return true;
			}

			if (dialog != null) // only check top priority widget until it passes on
			{
				dialog.CursorMoved(cursorPos);
				if (ok != null)
					ok.CursorMoved(cursorPos);
				else
				{
					yes.CursorMoved(cursorPos);
					no.CursorMoved(cursorPos);
				}
				return true;
			}

			Widget w = null;

			for (int i = 0; i < 10; i++)
			{
				if (!mTrays[i].IsVisible)
					continue;

				for (int j = 0; j < widgets[i].Count; j++)
				{
					w = widgets[i][j];
					if (!w.OverlayElement.IsVisible)
						continue;
					w.CursorMoved(cursorPos); // send event to widget
					w.MouseMoved(evt); // send event to widget
				}
			}

			if (trayDrag) // don't pass this event on if we're in the middle of a drag
				return true;
			return false;
		}


		//        -----------------------------------------------------------------------------
		//		| Internal method to prioritise / deprioritise expanded menus.
		//		-----------------------------------------------------------------------------
		protected void setExpandedMenu(SelectMenuWidget m)
		{
			if (expandedMenu == null && m != null)
			{
				Mogre.OverlayContainer c = (Mogre.OverlayContainer)m.OverlayElement;
				Mogre.OverlayContainer eb = (Mogre.OverlayContainer)c.GetChild(m.Name + "/MenuExpandedBox");
				eb._update();
				eb.SetPosition((uint)(eb._getDerivedLeft() * Mogre.OverlayManager.Singleton.ViewportWidth), (uint)(eb._getDerivedTop() * Mogre.OverlayManager.Singleton.ViewportHeight));
				c.RemoveChild(eb.Name);
				priorityLayer.Add2D(eb);
			}
			else if (expandedMenu != null && m == null)
			{
				Mogre.OverlayContainer eb = priorityLayer.GetChild(expandedMenu.Name + "/MenuExpandedBox");
				priorityLayer.Remove2D(eb);
				((Mogre.OverlayContainer)expandedMenu.OverlayElement).AddChild(eb);
			}

			expandedMenu = m;
		}

		public void AddOverlayElementToTrayLocation(OverlayElement element, UIWidgetLocation trayLoc)
		{
			mTrays[(int)trayLoc].AddChild(element);
		}

	}

	/// <summary>
	/// Listener class for responding to tray events
	/// </summary>
	public class UIListener
    {

        public virtual void Dispose() {
        }
        public virtual void buttonHit(ButtonWidget button) {
        }
        public virtual void itemSelected(SelectMenuWidget menu) {
        }
        public virtual void labelHit(LabelWidget label) {
        }
        public virtual void sliderMoved(Slider slider) {
        }
        public virtual void checkBoxToggled(CheckBoxWidget box) {
        }
        public virtual void okDialogClosed(string message) {
        }
        public virtual void yesNoDialogClosed(string question, bool yesHit) {
        }
    }
    public class UIMathHelper
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

	public class Padding
	{
		public float PaddingTop = 0;
		public float PaddingLeft = 0;
		public float PaddingDown = 0;
		public float PaddingRight = 0;
	}

	/// <summary>
	/// Custom, decorative widget created from a template
	/// </summary>
	public class DecorWidget : Widget
    {

        // Do not instantiate any widgets directly. Use SdkTrayManager.
        public DecorWidget(string name, string templateName) {
            element = Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate(templateName, "", name);
        }
    }
}