using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Map;
using OpenMB.Utilities;
using OpenMB.UI;
using System;
using System.Collections.Generic;
using OpenMB.UI.Widgets;

namespace OpenMB.Screen
{
	public enum EditType
	{
		None,
		EditAIMeshMode,
		EditObjectMode,
		EditTerrainMode
	}
	public enum EditState
	{
		Free,
		Add,
		Edit,
		Select
	}
	public enum EditOperation
	{
		None,
		ChangingObjCoordFllowMouse,
		ChangingObjCoord,
		ChangingObjHeight,
		ChangingObjSize,
	}
	public enum EditObjectType
	{
		Scene_Prop,
		AIMesh_Vertex,
		AIMesh_Line,
	}

	public class GameEditorScreen : Screen
	{
		private const float MARGIN_TOP = 0.02f;
		private const float MARGIN_RIGHT = 0.02f;
		private const float MARGIN_BOTTOM = 0.02f;
		private OverlayContainer editorPanel;
		private StaticText lbGeneral;
		private StaticText lbObjects;
		private StaticText lbAIMesh;
		private ButtonWidget btnSave;
		private ButtonWidget btnClose;
		private ButtonWidget btnAIMeshCreateVertex;
		private ButtonWidget btnAIMeshCreateLine;
		private OverlayElement horline;
		private OverlayElement horline2;
		private ListViewWidget lsvObjects;
		private ButtonWidget btnAddObject;
		private GameMapEditor editor;
		private EditType type;
		private EditState state;
		private EditOperation operation;
		private EditObjectType objType;
		private float distance = 20;
		private Entity currentSelectedEnt;
		private Vector2 lastMousePos;
		public override bool IsVisible
		{
			get
			{
				return editorPanel.IsVisible;
			}
		}
		public override string Name
		{
			get
			{
				return "InnerGameEditor";
			}
		}

		public override event Action OnScreenExit;

		public GameEditorScreen()
		{
			editorPanel = null;
		}

		public override void Init(params object[] param)
		{
			editor = param[0] as GameMapEditor;
			currentSelectedEnt = null;
			UIManager.Instance.DestroyAllWidgets();
			UIManager.Instance.ShowCursor();
		}

		public override void Exit()
		{
			UIManager.Instance.HideCursor();
			editor.Dispose();
			OverlayContainer.ChildIterator children = editorPanel.GetChildIterator();
			while (children.MoveNext())
			{
				OverlayElement currentElement = children.Current;
				editorPanel.RemoveChild(currentElement.Name);
			}

			UIManager.Instance.DestroyWidget(btnAddObject);
			UIManager.Instance.DestroyWidget(btnAIMeshCreateLine);
			UIManager.Instance.DestroyWidget(btnAIMeshCreateVertex);
			UIManager.Instance.DestroyWidget(btnClose);
			UIManager.Instance.DestroyWidget(btnSave);
			UIManager.Instance.DestroyWidget(lbAIMesh);
			UIManager.Instance.DestroyWidget(lbGeneral);
			UIManager.Instance.DestroyWidget(lbObjects);
			UIManager.Instance.DestroyWidget(lsvObjects);
			Widget.NukeOverlayElement(horline);
			Widget.NukeOverlayElement(horline2);

			UIManager.Instance.GetTraysLayer().Remove2D(editorPanel);
			Widget.NukeOverlayElement(editorPanel);
		}

		public override void Run()
		{
			type = EditType.EditAIMeshMode;
			state = EditState.Free;
			operation = EditOperation.None;

			float top = MARGIN_TOP;
			editorPanel = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanel", "BorderPanel", "editorArea") as OverlayContainer;

			lbGeneral = UIManager.Instance.CurrentLayer.CreateStaticText(UIWidgetLocation.TL_NONE, "lbGeneral", "General", ColourValue.Black);
			lbGeneral.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			lbGeneral.Left = 0.06f;
			lbGeneral.Top = top;
			top = lbGeneral.Top + lbGeneral.Height;
			editorPanel.AddChild(lbGeneral.OverlayElement);

			btnSave = UIManager.Instance.CurrentLayer.CreateButton(UIWidgetLocation.TL_NONE, "btnSave", "Save", 150);
			btnSave.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnSave.Left = 0.06f;
			btnSave.Top = 0.02f + top;
			btnSave.OnClick += BtnSave_OnClick;
			top = btnSave.Top + btnSave.Height;
			editorPanel.AddChild(btnSave.OverlayElement);

			btnClose = UIManager.Instance.CurrentLayer.CreateButton(UIWidgetLocation.TL_NONE, "btnClose", "Close", 150);
			btnClose.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnClose.Left = 0.06f;
			btnClose.Top = 0.02f + top;
			btnClose.OnClick += BtnClose_OnClick;
			top = btnClose.Top + btnClose.Height;
			editorPanel.AddChild(btnClose.OverlayElement);

			horline = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "horline") as PanelOverlayElement;
			horline.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			horline.Left = 0.01f;
			horline.Width = 0.28f;
			horline.Top = 0.02f + top;
			top = horline.Top + horline.Height;
			editorPanel.AddChild(horline);

			lbAIMesh = UIManager.Instance.CurrentLayer.CreateStaticText(UIWidgetLocation.TL_NONE, "lbAIMesh", "AIMesh", ColourValue.Black);
			lbAIMesh.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			lbAIMesh.Left = 0.06f;
			lbAIMesh.Top = 0.02f + top;
			top = lbAIMesh.Top + lbAIMesh.Height;
			editorPanel.AddChild(lbAIMesh.OverlayElement);

			btnAIMeshCreateVertex = UIManager.Instance.CurrentLayer.CreateButton(UIWidgetLocation.TL_NONE, "btnCreateVertex", "Create Vertex", 150);
			btnAIMeshCreateVertex.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnAIMeshCreateVertex.Left = 0.06f;
			btnAIMeshCreateVertex.Top = 0.02f + top;
			btnAIMeshCreateVertex.OnClick += BtnAIMeshCreateVertex_OnClick;
			top = btnAIMeshCreateVertex.Top + btnAIMeshCreateVertex.Height;
			editorPanel.AddChild(btnAIMeshCreateVertex.OverlayElement);

			btnAIMeshCreateLine = UIManager.Instance.CurrentLayer.CreateButton(UIWidgetLocation.TL_NONE, "btnCreateLine", "Create Line", 150);
			btnAIMeshCreateLine.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnAIMeshCreateLine.Left = 0.06f;
			btnAIMeshCreateLine.Top = 0.02f + top;
			btnAIMeshCreateLine.OnClick += BtnAIMeshCreateLine_OnClick;
			top = btnAIMeshCreateLine.Top + btnAIMeshCreateLine.Height;
			editorPanel.AddChild(btnAIMeshCreateLine.OverlayElement);

			horline2 = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "horline2") as PanelOverlayElement;
			horline2.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
			horline2.Left = 0.01f;
			horline2.Width = 0.28f;
			horline2.Top = 0.02f + top;
			top = horline2.Top + horline2.Height;
			editorPanel.AddChild(horline2);

			lbObjects = UIManager.Instance.CurrentLayer.CreateStaticText(UIWidgetLocation.TL_NONE, "lbObjects", "Objects", ColourValue.Black);
			lbObjects.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			lbObjects.Left = 0.06f;
			lbObjects.Top = 0.02f + top;
			top = lbObjects.Top + lbObjects.Height;
			editorPanel.AddChild(lbObjects.OverlayElement);

			lsvObjects = UIManager.Instance.CurrentLayer.CreateListView(UIWidgetLocation.TL_NONE, "lsvObjects", 0.3f, 0.22f, new List<string>()
			{
				"ObjectName"
			});
			lsvObjects.Left = 0.03f;
			lsvObjects.Width = 0.24f;
			lsvObjects.Height = 0.3f;
			lsvObjects.Top = 0.02f + top;
			top = lsvObjects.Top + lsvObjects.Height;
			editorPanel.AddChild(lsvObjects.OverlayElement);

			btnAddObject = UIManager.Instance.CurrentLayer.CreateButton(UIWidgetLocation.TL_NONE, "btnAddObject", "Add Object", 100);
			btnAddObject.MetricMode = GuiMetricsMode.GMM_RELATIVE;
			btnAddObject.Left = 0.14f;
			btnAddObject.Top = 0.02f + top;
			btnAddObject.OnClick += BtnAddObject_OnClick;
			top = btnAddObject.Top + btnAddObject.Height;
			editorPanel.AddChild(btnAddObject.OverlayElement);

			UIManager.Instance.GetTraysLayer().Add2D(editorPanel);
		}

		private void BtnAddObject_OnClick(object obj)
		{
			CancelLastOperation();
			state = EditState.Add;
			objType = EditObjectType.Scene_Prop;
		}

		private void BtnAIMeshCreateLine_OnClick(object obj)
		{
			CancelLastOperation();
			state = EditState.Add;
			objType = EditObjectType.AIMesh_Line;
		}

		private void BtnAIMeshCreateVertex_OnClick(object obj)
		{
			CancelLastOperation();
			state = EditState.Add;
			objType = EditObjectType.AIMesh_Vertex;
		}

		private void BtnClose_OnClick(object obj)
		{
			if (OnScreenExit != null)
			{
				OnScreenExit();
			}
		}

		private void BtnSave_OnClick(object obj)
		{
		}

		public override void Update(float timeSinceLastFrame)
		{
			editor.Map.CameraHanlder.MoveCamera();
		}

		public override void InjectMousePressed(MouseEvent arg, MouseButtonID id)
		{
			base.InjectMousePressed(arg, id);
			if (id == MouseButtonID.MB_Right)
			{
				if (state == EditState.Free)
				{
					editor.HidePivot();
					return;
				}
				MaterialPtr material = currentSelectedEnt.GetSubEntity(0).GetMaterial();
				material.GetTechnique(0).SetAmbient(0, 0, 0);
				currentSelectedEnt.GetSubEntity(0).SetMaterial(material);

				state = EditState.Free;
				currentSelectedEnt.ParentSceneNode.ShowBoundingBox = false;
				currentSelectedEnt = null;
				editor.HidePivot();
				operation = EditOperation.None;
			}
			else if (id == MouseButtonID.MB_Left)
			{
				Ray ray = UIManager.Instance.GetCursorRay(editor.Map.Camera);
				var query = editor.Map.SceneManager.CreateRayQuery(ray);
				RaySceneQueryResult result = query.Execute();
				foreach (var sResult in result)
				{
					if (sResult.movable != null &&
					   (sResult.movable.Name.StartsWith("SCENE_OBJECT") || sResult.movable.Name.StartsWith("AIMESH")))
					{
						//High light the object
						var ent = editor.Map.SceneManager.GetEntity(sResult.movable.Name);
						MaterialPtr material = ent.GetSubEntity(0).GetMaterial();
						ColourValue cv = new ColourValue(1, 0, 0);
						material.GetTechnique(0).SetAmbient(cv);
						ColourValue cv2 = new ColourValue(1, 0, 0);
						material.GetTechnique(0).SetDiffuse(cv2);
						ent.GetSubEntity(0).SetMaterial(material);
						ent.ParentSceneNode.ShowBoundingBox = true;
						currentSelectedEnt = ent;
						state = EditState.Edit;
						Mogre.Vector3 entCenterPos = ent.GetWorldBoundingBox().Center;
						editor.ShowPivotAtPosition(entCenterPos);
					}
				}
			}
		}

		private void CancelLastOperation()
		{
			if (currentSelectedEnt != null)
			{
				currentSelectedEnt.ParentSceneNode.ShowBoundingBox = false;
			}
			editor.HidePivot();
			operation = EditOperation.None;
		}

		public override void InjectMouseMove(MouseEvent arg)
		{
			base.InjectMouseMove(arg);

			Vector2 cursorPos = new Vector2(arg.state.X.abs, arg.state.Y.abs);
			Ray ray = UIManager.Instance.GetCursorRay(editor.Map.Camera);
			switch (state)
			{
				case EditState.Add:
					switch (type)
					{
						case EditType.EditAIMeshMode:
							Mogre.Vector3 pos = Helper.ConvertScreenCoordToWorldCoord(
								lastMousePos,
								editor.Map.Camera,
								GameManager.Instance.renderWindow);
							HandleObjectCreate(pos);
							state = EditState.Edit;
							operation = EditOperation.ChangingObjCoordFllowMouse;
							break;
						case EditType.EditObjectMode:
							break;
						case EditType.EditTerrainMode:
							break;
					}
					break;
				case EditState.Edit:
					switch (type)
					{
						case EditType.EditAIMeshMode:
							Mogre.Vector3 pos = ray.Origin + ray.Direction * distance;
							HandleObjOperationNoResize(pos, cursorPos);
							break;
						case EditType.EditObjectMode:
							break;
						case EditType.EditTerrainMode:
							break;
					}
					break;
				case EditState.Free:
					Degree deCameraYaw = new Degree(arg.state.X.rel * -0.1f);
					editor.Map.Camera.Yaw(deCameraYaw);
					Degree deCameraPitch = new Degree(arg.state.Y.rel * -0.1f);
					editor.Map.Camera.Pitch(deCameraPitch);

					if (currentSelectedEnt != null)
					{
						currentSelectedEnt.ParentSceneNode.ShowBoundingBox = false;
					}
					var query = editor.Map.SceneManager.CreateRayQuery(ray);
					query.QueryMask = 1 << 0;
					RaySceneQueryResult result = query.Execute();
					foreach (var sResult in result)
					{
						if (sResult.movable != null &&
							(sResult.movable.Name.StartsWith("SCENE_OBJECT") || sResult.movable.Name.StartsWith("AIMESH")))
						{
							//High light the object
							var ent = editor.Map.SceneManager.GetEntity(sResult.movable.Name);
							MaterialPtr material = ent.GetSubEntity(0).GetMaterial();
							ColourValue cv = new ColourValue(1, 0, 0);
							material.GetTechnique(0).SetAmbient(cv);
							ColourValue cv2 = new ColourValue(1, 0, 0);
							material.GetTechnique(0).SetDiffuse(cv2);
							ent.GetSubEntity(0).SetMaterial(material);
							ent.ParentSceneNode.ShowBoundingBox = true;
							currentSelectedEnt = ent;
						}
					}
					break;
			}
			lastMousePos = cursorPos;
		}

		public override void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
		{
			base.InjectMouseReleased(arg, id);
		}

		public override void InjectKeyPressed(KeyEvent arg)
		{
			base.InjectKeyPressed(arg);
			switch (arg.key)
			{
				case KeyCode.KC_A:
					editor.Map.CameraHanlder.CameraMoveX(-10);
					break;
				case KeyCode.KC_D:
					editor.Map.CameraHanlder.CameraMoveX(10);
					break;
				case KeyCode.KC_W:
					editor.Map.CameraHanlder.CameraMoveZ(-10);
					break;
				case KeyCode.KC_S:
					editor.Map.CameraHanlder.CameraMoveZ(10);
					break;
				case KeyCode.KC_E:
					editor.Map.CameraHanlder.CameraMoveY(10);
					break;
				case KeyCode.KC_Q:
					editor.Map.CameraHanlder.CameraMoveY(-10);
					break;
			}
			switch (state)
			{
				case EditState.Edit:
					switch (arg.key)
					{
						case KeyCode.KC_G://X-Z Panel Movement
							operation = EditOperation.ChangingObjCoord;
							break;
						case KeyCode.KC_T://Increase/Decrease Hight
							operation = EditOperation.ChangingObjHeight;
							break;
					}
					break;
			}
		}

		public override void InjectKeyReleased(KeyEvent arg)
		{
			base.InjectKeyReleased(arg);
			operation = EditOperation.None;
		}

		public override void Show()
		{
			if (!editorPanel.IsVisible)
			{
				editorPanel.Show();
			}
		}

		public override void Hide()
		{
			if (editorPanel.IsVisible)
			{
				editorPanel.Hide();
				editor.Dispose();
			}
		}

		public override bool CheckEnterScreen(Vector2 mousePos)
		{
			return Widget.IsCursorOver(editorPanel, mousePos);
		}

		private void HandleObjOperation(Vector2 offset)
		{
			switch (operation)
			{
				case EditOperation.ChangingObjCoord:
					break;
				case EditOperation.ChangingObjHeight:
					break;
				case EditOperation.ChangingObjSize:
					break;
			}
		}

		private void HandleObjOperationNoResize(Mogre.Vector3 objNewPos, Mogre.Vector2 cursorPos)
		{
			if (currentSelectedEnt == null)
			{
				return;
			}
			Mogre.Vector3 currentPos = currentSelectedEnt.ParentNode.Position;
			switch (operation)
			{
				case EditOperation.ChangingObjCoordFllowMouse:
					currentSelectedEnt.ParentNode.Position = objNewPos;
					break;
				case EditOperation.ChangingObjCoord:
					float offsetX = cursorPos.x - lastMousePos.x;
					float offsetZ = cursorPos.y - lastMousePos.y;

					Mogre.Vector3 newPosXZ = new Mogre.Vector3(
						currentPos.x - offsetX,
						currentPos.y,
						currentPos.z - offsetZ);
					currentSelectedEnt.ParentNode.Position = newPosXZ;
					break;
				case EditOperation.ChangingObjHeight:
					Mogre.Vector3 newPosY = new Mogre.Vector3(currentPos.x, objNewPos.y, currentPos.z);
					currentSelectedEnt.ParentNode.Position = newPosY;
					break;
			}
		}

		private void HandleObjectCreate(Mogre.Vector3 pos)
		{
			switch (objType)
			{
				case EditObjectType.AIMesh_Line:
					currentSelectedEnt = editor.AddNewAIMeshLine(pos);
					break;
				case EditObjectType.AIMesh_Vertex:
					currentSelectedEnt = editor.AddNewAIMeshVertex(pos);
					break;
				case EditObjectType.Scene_Prop:
					break;
			}
		}
	}
}
