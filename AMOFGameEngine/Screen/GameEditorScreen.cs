using Mogre;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.Widgets;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Map;

namespace AMOFGameEngine.Screen
{
    public enum EditState
    {
        Free,
        EditAIMeshVertex,
        EditAIMeshLine,
        EditObject,
        EditTerrain
    }
    public class GameEditorScreen : Screen
    {
        private OverlayContainer editorPanel;
        private Button btnSave;
        private Button btnClose;
        private Button btnAIMeshCreateVertex;
        private Button btnAIMeshCreateLine;
        private ListView lsvObjects;
        private Button btnAddObject;
        private GameMapEditor editor;
        private EditState state;
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

        public override void Exit()
        {
            GameManager.Instance.mTrayMgr.hideCursor();
            OverlayContainer.ChildIterator children = editorPanel.GetChildIterator();
            while (children.MoveNext())
            {
                OverlayElement currentElement = children.Current;
                editorPanel.RemoveChild(currentElement.Name);
            }
            GameManager.Instance.mTrayMgr.getTraysLayer().Remove2D(editorPanel);
            Widget.nukeOverlayElement(editorPanel);
        }

        public override void Init(params object[] param)
        {
            editor = param[0] as GameMapEditor;
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            GameManager.Instance.mTrayMgr.showCursor();
        }

        public override void Run()
        {
            state = EditState.Free;

            float top = 0.02f;
            editorPanel = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanel", "BorderPanel", "editorArea") as OverlayContainer;

            var lbGeneral = GameManager.Instance.mTrayMgr.createStaticText(TrayLocation.TL_NONE, "lbGeneral", "General", ColourValue.Black);
            lbGeneral.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            lbGeneral.getOverlayElement().Left = 0.06f;
            lbGeneral.getOverlayElement().Top =  top;
            top = lbGeneral.getOverlayElement().Top + lbGeneral.getOverlayElement().Height;
            editorPanel.AddChild(lbGeneral.getOverlayElement());

            btnSave = GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_NONE, "btnSave", "Save", 150);
            btnSave.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            btnSave.getOverlayElement().Left = 0.06f;
            btnSave.getOverlayElement().Top = 0.02f + top;
            btnSave.OnClick += BtnSave_OnClick;
            top = btnSave.getOverlayElement().Top + btnSave.getOverlayElement().Height;
            editorPanel.AddChild(btnSave.getOverlayElement());

            btnClose = GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_NONE, "btnClose", "Close", 150);
            btnClose.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            btnClose.getOverlayElement().Left = 0.06f;
            btnClose.getOverlayElement().Top = 0.02f +top;
            btnClose.OnClick += BtnClose_OnClick;
            top = btnClose.getOverlayElement().Top + btnClose.getOverlayElement().Height;
            editorPanel.AddChild(btnClose.getOverlayElement());

            var horline = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "horline") as PanelOverlayElement;
            horline.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            horline.Left = 0.01f;
            horline.Width = 0.28f;
            horline.Top = 0.02f + top;
            top = horline.Top + horline.Height;
            editorPanel.AddChild(horline);

            var lbAIMesh = GameManager.Instance.mTrayMgr.createStaticText(TrayLocation.TL_NONE, "lbAIMesh", "AIMesh", ColourValue.Black);
            lbAIMesh.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            lbAIMesh.getOverlayElement().Left = 0.06f;
            lbAIMesh.getOverlayElement().Top = 0.02f + top;
            top = lbAIMesh.getOverlayElement().Top + lbAIMesh.getOverlayElement().Height;
            editorPanel.AddChild(lbAIMesh.getOverlayElement());

            btnAIMeshCreateVertex = GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_NONE, "btnCreateVertex", "Create Vertex", 150);
            btnAIMeshCreateVertex.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            btnAIMeshCreateVertex.getOverlayElement().Left = 0.06f;
            btnAIMeshCreateVertex.getOverlayElement().Top = 0.02f + top;
            btnAIMeshCreateVertex.OnClick += BtnAIMeshCreateVertex_OnClick;
            top = btnAIMeshCreateVertex.getOverlayElement().Top + btnAIMeshCreateVertex.getOverlayElement().Height;
            editorPanel.AddChild(btnAIMeshCreateVertex.getOverlayElement());

            btnAIMeshCreateLine = GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_NONE, "btnCreateLine", "Create Line", 150);
            btnAIMeshCreateLine.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            btnAIMeshCreateLine.getOverlayElement().Left = 0.06f;
            btnAIMeshCreateLine.getOverlayElement().Top = 0.02f + top;
            btnAIMeshCreateLine.OnClick += BtnAIMeshCreateLine_OnClick;
            top = btnAIMeshCreateLine.getOverlayElement().Top + btnAIMeshCreateLine.getOverlayElement().Height;
            editorPanel.AddChild(btnAIMeshCreateLine.getOverlayElement());

            var horline2 = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "horline2") as PanelOverlayElement;
            horline2.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            horline2.Left = 0.01f;
            horline2.Width = 0.28f;
            horline2.Top = 0.02f + top;
            top = horline2.Top + horline2.Height;
            editorPanel.AddChild(horline2);

            var lbObjects = GameManager.Instance.mTrayMgr.createStaticText(TrayLocation.TL_NONE, "lbObjects", "Objects", ColourValue.Black);
            lbObjects.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            lbObjects.getOverlayElement().Left = 0.06f;
            lbObjects.getOverlayElement().Top = 0.02f + top;
            top = lbObjects.getOverlayElement().Top + lbObjects.getOverlayElement().Height;
            editorPanel.AddChild(lbObjects.getOverlayElement());

            lsvObjects = GameManager.Instance.mTrayMgr.createListView(TrayLocation.TL_NONE, "lsvObjects", 0.3f, 0.22f, new List<string>()
            {
                "ObjectName"
            });
            lsvObjects.getOverlayElement().Left = 0.03f;
            lsvObjects.getOverlayElement().Width = 0.24f;
            lsvObjects.getOverlayElement().Height = 0.3f;
            lsvObjects.getOverlayElement().Top = 0.02f + top;
            top = lsvObjects.getOverlayElement().Top + lsvObjects.getOverlayElement().Height;
            editorPanel.AddChild(lsvObjects.getOverlayElement());

            btnAddObject = GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_NONE, "btnAddObject", "Add Object", 100);
            btnAddObject.getOverlayElement().MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            btnAddObject.getOverlayElement().Left = 0.14f;
            btnAddObject.getOverlayElement().Top = 0.02f + top;
            btnAddObject.OnClick += BtnAddObject_OnClick;
            top = btnAddObject.getOverlayElement().Top + btnAddObject.getOverlayElement().Height;
            editorPanel.AddChild(btnAddObject.getOverlayElement());

            GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(editorPanel);
        }

        private void BtnAddObject_OnClick(object obj)
        {
            state = EditState.EditObject;
        }

        private void BtnAIMeshCreateLine_OnClick(object obj)
        {
            state = EditState.EditAIMeshLine;
        }

        private void BtnAIMeshCreateVertex_OnClick(object obj)
        {
            state = EditState.EditAIMeshVertex;
        }

        private void BtnClose_OnClick(object obj)
        {
            OnScreenExit?.Invoke();
        }

        private void BtnSave_OnClick(object obj)
        {
        }

        public override void Update(float timeSinceLastFrame)
        {
        }

        public override void InjectMousePressed(MouseEvent arg, MouseButtonID id)
        {
            base.InjectMousePressed(arg, id);
        }

        public override void InjectMouseMove(MouseEvent arg)
        {
            base.InjectMouseMove(arg);

            Vector2 cursorPos = new Vector2(arg.state.X.abs, arg.state.Y.abs);
            Ray ray = GameManager.Instance.mTrayMgr.getCursorRay(editor.Map.Camera);
            switch (state)
            {
                case EditState.EditAIMeshVertex:
                    
                    break;
                case EditState.Free:
                    var query = editor.Map.SceneManager.CreateRayQuery(ray);
                    query.QueryMask = (uint)GameObjectQueryFlags.AIMESH_VERTEX;
                    RaySceneQueryResult result = query.Execute();
                    foreach(var sResult in result)
                    {
                        if (sResult.movable != null)
                        {
                            //High light the object
                            var ent = editor.Map.SceneManager.GetEntity(sResult.movable.Name);
                            MaterialPtr material = ent.GetSubEntity(1).GetMaterial();
                            ColourValue cv = new ColourValue(1, 1, 0);
                            material.GetTechnique(1).SetAmbient(cv);
                            ent.GetSubEntity(1).SetMaterial(material);
                        }
                    }
                    break;
            }
        }

        public override void InjectMouseReleased(MouseEvent arg, MouseButtonID id)
        {
            base.InjectMouseReleased(arg, id);

            state = EditState.Free;
        }

        public override void Show()
        {
            if(!editorPanel.IsVisible)
            {
                editorPanel.Show();
            }
        }

        public override void Hide()
        {
            if(editorPanel.IsVisible)
            {
                editorPanel.Hide();
            }
        }
    }
}
