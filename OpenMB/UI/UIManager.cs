using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre;
using MOIS;
using OpenMB.Core;
using OpenMB.Mods;
using OpenMB.Mods.XML;
using OpenMB.States;
using OpenMB.UI.Widgets;
using InputContext = MOIS.Mouse;

namespace OpenMB.UI
{
    public class UIManager : IDisposable
	{
		private Stack<UILayer> uiLayers;
		private static UIManager instance;
		protected string Name = ""; // name of this tray system
		protected RenderWindow window; // render window
		protected InputContext inputContext = null;
		protected Overlay cursorLayer; // cursor layer
		protected OverlayContainer cursor; // cursor
		protected UIListener listener; // tray listener
		protected bool cursorWasVisible; // cursor state before showing dialog
		private GameCursor gameCursor;

        public void Append(Widget widget)
        {
			CurrentLayer.Append(widget);
        }

        private ModData modData;

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

        public UILayer CurrentLayer { get { return uiLayers.Peek(); } }
		public UIManager()
        {
			uiLayers = new Stack<UILayer>();
			uiLayers.Push(new UILayer());
		}

		public void Init(string name, Mogre.RenderWindow window, InputContext inputContext, UIListener listener)
		{
			Name = name;
			this.window = window;
			this.inputContext = inputContext;
			this.listener = listener;
			cursorWasVisible = false;

			OverlayManager om = Mogre.OverlayManager.Singleton;
			string nameBase = Name + "/";
			cursorLayer = om.Create(nameBase + "CursorLayer");
			cursorLayer.ZOrder = 1;

			// make backdrop and cursor overlay containers
			cursor = (Mogre.OverlayContainer)om.CreateOverlayElementFromTemplate("SdkTrays/Cursor", "Panel", nameBase + "Cursor");
			cursorLayer.Add2D(cursor);
			cursorLayer.ZOrder = 400;

			ShowCursor();
			gameCursor = new GameCursor();
		}

		public void ChangeCursor(string name)
		{
			gameCursor.ChangeCursor(modData.CursorInfos, name);
		}

		internal Widget GetWidget(UIWidgetLocation trayLoc, uint i)
        {
			return CurrentLayer.GetWidget(trayLoc, i);
        }

        internal void SetListener(UIListener listener)
        {
			CurrentLayer.SetListener(listener);
        }

        public void InitMod(ModData modData)
        {
			this.modData = modData;
			CurrentLayer.InitMod(modData);
        }

        public Overlay GetTraysLayer()
        {
			return CurrentLayer.GetTraysLayer();
		}

		public void ShowCursor(string materialName)
		{
			if (!string.IsNullOrEmpty(materialName))
				GetCursorImage().MaterialName = materialName;

			if (!cursorLayer.IsVisible)
			{
				cursorLayer.Show();
				RefreshCursor();
			}
		}

        internal void FrameRenderingQueued(FrameEvent evt)
        {
			CurrentLayer.FrameRenderingQueued(evt);
        }

        public void RefreshCursor()
		{
			float x = 0f;
			float y = 0f;
			x = inputContext.MouseState.X.abs;
			y = inputContext.MouseState.Y.abs;
			cursor.SetPosition(x, y);
		}

        internal int GetNumWidgets(UIWidgetLocation trayLoc)
        {
			return CurrentLayer.GetNumWidgets(trayLoc);
        }

        internal void DestroyWidget(UIWidgetLocation trayLoc, uint num)
        {
			CurrentLayer.DestroyWidget(trayLoc, num);
        }

        /// <summary>
        /// Displays specified material on cursor, or the last material used if
        ///	none specified. Used to change cursor type.
        /// </summary>
        public void ShowCursor()
		{
			ShowCursor("");
		}

		public void HideCursor()
		{
			cursorLayer.Hide();
			CurrentLayer.HideCursor();
		}

		internal void Update()
        {
        }

        public void AddNewLayer()
		{
			uiLayers.Push(new UILayer());
		}
		public Mogre.Overlay GetCursorLayer()
		{
			return cursorLayer;
		}

		internal void DestroyWidget(Widget widget)
		{
			CurrentLayer.DestroyWidget(widget);
		}

		internal void DestroyWidget(string widgetName)
		{
			CurrentLayer.DestroyWidget(widgetName);
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

        public void DestroyAllWidgets()
        {
			if (uiLayers.Count == 1)
			{
				return;
			}
			CurrentLayer.DestroyAllWidgets();
			uiLayers.Pop();
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
		public Mogre.OverlayContainer GetCursorContainer()
		{
			return cursor;
		}
		public Mogre.OverlayElement GetCursorImage()
		{
			return cursor.GetChild(cursor.Name + "/CursorImage");
		}

		public UIEvent InjectMouseDown(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
		{
			if (!cursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
				return null;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(cursor.Left, cursor.Top);

			return CurrentLayer.InjectMouseDown(evt, id, cursorPos);
		}

		public UIEvent InjectMouseUp(MOIS.MouseEvent evt, MOIS.MouseButtonID id)
		{
			if (!cursorLayer.IsVisible || id != MOIS.MouseButtonID.MB_Left)
				return null;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(cursor.Left, cursor.Top);

			return CurrentLayer.InjectMouseUp(evt, id, cursorPos);
		}

		public UIEvent InjectMouseMove(MouseEvent evt)
		{
			if (!cursorLayer.IsVisible) // don't process if cursor layer is invisible
				return null;

			Mogre.Vector2 cursorPos = new Mogre.Vector2(evt.state.X.abs, evt.state.Y.abs);
			cursor.SetPosition(cursorPos.x, cursorPos.y);

			return CurrentLayer.InjectMouseMove(evt, cursorPos);
		}

		public UIEvent InjectKeyReleased(KeyEvent arg)
		{
			return CurrentLayer.InjectKeyReleased(arg);
		}

		public UIEvent InjectKeyPressed(KeyEvent arg)
		{
			return CurrentLayer.InjectKeyPressed(arg);
		}

		#region Factory Methods
		public Widget CreateWidget(ModData modData, ModUILayoutWidgetDfnXml xmlData)
		{
			return CurrentLayer.CreateWidget(modData, xmlData);
		}
		internal ButtonWidget CreateButton(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CurrentLayer.CreateButton(trayLoc, name, caption);
		}
		internal ButtonWidget CreateButton(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			return CurrentLayer.CreateButton(trayLoc, name, caption, width);
		}
		internal ButtonWidget CreateButton(string name, string caption, float width)
		{
			return CurrentLayer.CreateButton(name, caption, width);
		}
		internal StaticMultiLineTextBoxWidget CreateTextBox(UIWidgetLocation trayLoc, string name, string caption, float width, float height)
		{
			return CurrentLayer.CreateTextBox(trayLoc, name, caption, width, height);
		}
		internal SelectMenuWidget CreateThickSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, uint maxItemsShown)
		{
			return CurrentLayer.CreateThickSelectMenu(trayLoc, name, caption, width, maxItemsShown);
		}

        internal SelectMenuWidget CreateThickSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, uint maxItemsShown, List<string> items)
		{
			return CurrentLayer.CreateThickSelectMenu(trayLoc, name, caption, width, maxItemsShown, items);
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown)
		{
			return CurrentLayer.CreateLongSelectMenu(trayLoc, name, caption, width, boxWidth, maxItemsShown);
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float width, float boxWidth, uint maxItemsShown, List<string> items)
		{
			return CurrentLayer.CreateLongSelectMenu(trayLoc, name, caption, width, boxWidth, maxItemsShown, items);
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown)
		{
			return CurrentLayer.CreateLongSelectMenu(trayLoc, name, caption, boxWidth, maxItemsShown);
		}
		internal SelectMenuWidget CreateLongSelectMenu(UIWidgetLocation trayLoc, string name, string caption, float boxWidth, uint maxItemsShown, List<string> items)
		{
			return CurrentLayer.CreateLongSelectMenu(trayLoc, name, caption, boxWidth, maxItemsShown, items);
		}
		internal LabelWidget CreateLabel(UIWidgetLocation trayLoc, string name, string caption)
		{
			return CurrentLayer.CreateLabel(trayLoc, name, caption);
		}
		internal LabelWidget CreateLabel(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			return CurrentLayer.CreateLabel(trayLoc, name, caption, width);
		}
		internal StaticText CreateStaticText(string name, string text)
		{
			return CurrentLayer.CreateStaticText(name, text);
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
			return CurrentLayer.CreateStaticText(trayLoc, name, caption, width, specificColor, color);
		}
		internal Separator CreateSeparator(UIWidgetLocation trayLoc, string name)
		{
			return CreateSeparator(trayLoc, name, 0f);
		}
		internal Separator CreateSeparator(UIWidgetLocation trayLoc, string name, float width)
		{
			return CurrentLayer.CreateSeparator(trayLoc, name, width);
		}
		internal Slider CreateThickSlider(UIWidgetLocation trayLoc, string name, string caption, float width, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			return CurrentLayer.CreateThickSlider(trayLoc, name, caption, width, valueBoxWidth, minValue, maxValue, snaps);

		}
		internal Slider CreateLongSlider(UIWidgetLocation trayLoc, string name, string caption, float width, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			return CurrentLayer.CreateLongSlider(trayLoc, name, caption, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
		}
		internal Slider CreateLongSlider(UIWidgetLocation trayLoc, string name, string caption, float trackWidth, float valueBoxWidth, float minValue, float maxValue, uint snaps)
		{
			return CreateLongSlider(trayLoc, name, caption, 0, trackWidth, valueBoxWidth, minValue, maxValue, snaps);
		}
		internal ParamsPanelWidget CreateParamsPanel(UIWidgetLocation trayLoc, string name, float width, uint lines)
		{
			return CurrentLayer.CreateParamsPanel(trayLoc, name, width, lines);
		}
		internal ParamsPanelWidget CreateParamsPanel(UIWidgetLocation trayLoc, string name, float width, StringVector paramNames)
		{
			return CurrentLayer.CreateParamsPanel(trayLoc, name, width, paramNames);
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
			return CurrentLayer.CreateCheckBox(trayLoc, name, caption);
		}
		internal CheckBoxWidget CreateCheckBox(UIWidgetLocation trayLoc, string name, string caption, float width)
		{
			return CurrentLayer.CreateCheckBox(trayLoc, name, caption, width);
		}
		internal ProgressBarWidget CreateProgressBar(UIWidgetLocation trayLoc, string name, string caption, float width, float commentBoxWidth)
		{
			return CurrentLayer.CreateProgressBar(trayLoc, name, caption, width, commentBoxWidth);
		}
		internal ListViewWidget CreateListView(UIWidgetLocation trayLoc, string name, float height, float width, List<string> columnNames)
		{
			return CurrentLayer.CreateListView(trayLoc, name, height, width, columnNames);
		}
		internal InputBoxWidget CreateInputBox(UIWidgetLocation trayLocation, string name, string caption, float width, float boxWidth, string text = null)
		{
			return CurrentLayer.CreateInputBox(trayLocation, name, caption, width, boxWidth, text);
		}
		internal PanelWidget CreatePanel(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1)
		{
			return CurrentLayer.CreatePanel(name, width, height, left, top, row, col);
		}
		internal PanelScrollableWidget CreateScrollablePanel(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true)
		{
			return CurrentLayer.CreateScrollablePanel(name, width, height, left, top, row, col, hasBorder);
		}
		internal PanelMaterialWidget CreateMaterialPanel(string name, string texture, float width = 0, float height = 0, float left = 0, float top = 0)
		{
			return CurrentLayer.CreateMaterialPanel(name, texture, width, height, left, top);
		}
		internal PanelTemplateWidget CreateTemplatePanel(string name, string template, int width = 0, int height = 0, int top = 0, int left = 0)
		{
			return CurrentLayer.CreateTemplatePanel(name, template, width, height, top, left);
		}
		#endregion

		internal void clearAllTrays()
		{
			CurrentLayer.clearAllTrays();
		}

		public void Dispose()
		{
			OverlayManager.Singleton.Destroy(cursorLayer);
			Widget.NukeOverlayElement(cursor);
		}
	}
}
