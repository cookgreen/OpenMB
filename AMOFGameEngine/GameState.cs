using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using RMOgre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine
{
    enum QueryFlags
    {
        OGRE_HEAD_MASK = 1 << 0,
        CUBE_MASK = 1 << 1
    };
    class GameState : AppState
    {
        public GameState()
        {
            m_MoveSpeed = 0.1f;
            m_RotateSpeed = 0.3f;

            m_bLMouseDown = false;
            m_bRMouseDown = false;
            m_bQuit = false;
            m_bSettingsMode = false;

            m_pDetailsPanel = null;
        }

        public override void enter()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Entering GameState...");
            AdvancedMogreFramework.LastStateName = "GameState";
            m_SceneMgr=AdvancedMogreFramework.Singleton.m_Root.CreateSceneManager(SceneType.ST_GENERIC, "GameSceneMgr");
            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_SceneMgr.AmbientLight=cvAmbineLight;//(Ogre::ColourValue(0.7f, 0.7f, 0.7f));
 
            Ray r=new Ray();
            m_RSQ = m_SceneMgr.CreateRayQuery(r);
            m_RSQ.QueryMask=1<<0;
 
            m_Camera = m_SceneMgr.CreateCamera("GameCamera");
            Mogre.Vector3 vectCameraPostion=new Mogre.Vector3(5,60,60);
            m_Camera.Position=vectCameraPostion;
            Mogre.Vector3 vectorCameraLookAt=new Mogre.Vector3(5,20,0);
            m_Camera.LookAt(vectorCameraLookAt);
            m_Camera.NearClipDistance=5;
 
            m_Camera.AspectRatio=AdvancedMogreFramework.Singleton.m_Viewport.ActualWidth / AdvancedMogreFramework.Singleton.m_Viewport.ActualHeight;

            AdvancedMogreFramework.Singleton.m_Viewport.Camera=m_Camera;
            m_CurrentObject = null;

 
            buildGUI();
 
            createScene();
        }
        public void createScene()
        {
            Mogre.Vector3 vectLightPos=new Mogre.Vector3(75,75,75);
            m_SceneMgr.CreateLight("Light").Position = vectLightPos;//(75, 75, 75);

            DotSceneLoader pDotSceneLoader = new DotSceneLoader();
            pDotSceneLoader.ParseDotScene("CubeScene.xml", "General", m_SceneMgr, m_SceneMgr.RootSceneNode);
            pDotSceneLoader=null;

            m_SceneMgr.GetEntity("Cube01").QueryFlags = 1 << 1;
            m_SceneMgr.GetEntity("Cube02").QueryFlags=1<<1;//(CUBE_MASK);
            m_SceneMgr.GetEntity("Cube03").QueryFlags=1<<1;//(CUBE_MASK);

            m_pOgreHeadEntity = m_SceneMgr.CreateEntity("Cube", "ogrehead.mesh");
            m_pOgreHeadEntity.QueryFlags=1<<0;
            m_pOgreHeadNode = m_SceneMgr.RootSceneNode.CreateChildSceneNode("CubeNode");
            m_pOgreHeadNode.AttachObject(m_pOgreHeadEntity);
            Mogre.Vector3 vectOgreHeadNodePos = new Mogre.Vector3(0,0,-25);
            m_pOgreHeadNode.Position = vectOgreHeadNodePos;// (Vector3(0, 0, -25));

            m_pOgreHeadMat = m_pOgreHeadEntity.GetSubEntity(1).GetMaterial();
            m_pOgreHeadMatHigh = m_pOgreHeadMat.Clone("OgreHeadMatHigh");
            ColourValue cvAmbinet = new Mogre.ColourValue(1, 0, 0);
            m_pOgreHeadMatHigh.GetTechnique(0).GetPass(0).Ambient = cvAmbinet;
            ColourValue cvDiffuse = new Mogre.ColourValue(1, 0, 0,0);
            m_pOgreHeadMatHigh.GetTechnique(0).GetPass(0).Diffuse = cvDiffuse;
        }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Leaving GameState...");

            if(m_SceneMgr!=null)
                m_SceneMgr.DestroyCamera(m_Camera);
                m_SceneMgr.DestroyQuery(m_RSQ);
                AdvancedMogreFramework.Singleton.m_Root.DestroySceneManager(m_SceneMgr);
        }
        public override bool pause()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Pausing GameState...");
 
            return true;
        }
        public override void resume()
        {
            AdvancedMogreFramework.Singleton.m_Log.LogMessage("Resuming GameState...");
 
            buildGUI();

            AdvancedMogreFramework.Singleton.m_Viewport.Camera=m_Camera;
            m_bQuit = false;
        }
 
	    public void moveCamera()
        {
                if (AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                    m_Camera.MoveRelative(m_TranslateVector);
                m_Camera.MoveRelative(m_TranslateVector / 10);
        }
        public void getInput()
        {
            if(m_bSettingsMode == false)
            {
                Angle angleCameraRoll;
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_A))
                    m_TranslateVector.x = -10;
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_D))
                    m_TranslateVector.x = 10;
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_W))
                    m_TranslateVector.z = -10;
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_S))
                    m_TranslateVector.z = 10;
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_Q))
                    m_TranslateVector.y = -10;
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_E))
                    m_TranslateVector.y = 10;
 
        //camera roll
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_Z))
                    m_Camera.Roll(angleCameraRoll=new Angle(-10));
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_X))
                    m_Camera.Roll(angleCameraRoll=new Angle(10));
 
        //reset roll
                if (AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_C))
                    m_Camera.Roll(-(m_Camera.RealOrientation.Roll));
            }
        }
        public void buildGUI()
        {
            AdvancedMogreFramework.Singleton.m_TrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            AdvancedMogreFramework.Singleton.m_TrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            AdvancedMogreFramework.Singleton.m_TrayMgr.createLabel(TrayLocation.TL_TOP, "GameLbl", Models.LocateSystem.CreateLocateString("11161225"), 250);
            AdvancedMogreFramework.Singleton.m_TrayMgr.showCursor();
 
            List<string> items=new List<string>();
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161226"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161227"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161228"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161229"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161230"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161231"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161232"));
            items.Insert(items.Count, Models.LocateSystem.CreateLocateString("11161233"));

            m_pDetailsPanel = AdvancedMogreFramework.Singleton.m_TrayMgr.createParamsPanel(TrayLocation.TL_TOPLEFT, "DetailsPanel", 200, items.ToArray());
            m_pDetailsPanel.show();

            string infoText = Models.LocateSystem.CreateLocateString("11161234");
            infoText.Insert(infoText.Length, Models.LocateSystem.CreateLocateString("11161235"));
            infoText.Insert(infoText.Length,Models.LocateSystem.CreateLocateString("11161236"));
            AdvancedMogreFramework.Singleton.m_TrayMgr.createTextBox(TrayLocation.TL_RIGHT, "InfoPanel", infoText, 300, 220);
 
            StringVector chatModes=new StringVector();
            chatModes.Insert(chatModes.Count, Models.LocateSystem.CreateLocateString("11161237"));
            chatModes.Insert(chatModes.Count, Models.LocateSystem.CreateLocateString("11161238"));
            chatModes.Insert(chatModes.Count, Models.LocateSystem.CreateLocateString("11161239"));
            AdvancedMogreFramework.Singleton.m_TrayMgr.createLongSelectMenu(TrayLocation.TL_TOPRIGHT, "ChatModeSelMenu", Models.LocateSystem.CreateLocateString("11161240"), 200, 3, chatModes);

            AdvancedMogreFramework.Singleton.m_Mouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_Mouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_Mouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_Keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(m_bSettingsMode == true)
            {
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_S))
                {
                    SelectMenu pMenu = (SelectMenu)AdvancedMogreFramework.Singleton.m_TrayMgr.getWidget("ChatModeSelMenu");
                    if(pMenu.getSelectionIndex() + 1 < (int)pMenu.getNumItems())
                        pMenu.selectItem((uint)pMenu.getSelectionIndex() + 1);
                }
 
                if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_W))
                {
                    SelectMenu pMenu = (SelectMenu)AdvancedMogreFramework.Singleton.m_TrayMgr.getWidget("ChatModeSelMenu");
                    if(pMenu.getSelectionIndex() - 1 >= 0)
                        pMenu.selectItem((uint)pMenu.getSelectionIndex() - 1);
                }
             }
 
            if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_ESCAPE))
            {
                pushAppState(findByName("PauseState"));
                return true;
            }
 
            if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_I))
            {
                if(m_pDetailsPanel.getTrayLocation() == TrayLocation.TL_NONE)
                {
                    AdvancedMogreFramework.Singleton.m_TrayMgr.moveWidgetToTray(m_pDetailsPanel, TrayLocation.TL_TOPLEFT, 0);
                    m_pDetailsPanel.show();
                }
                else
                {
                    AdvancedMogreFramework.Singleton.m_TrayMgr.removeWidgetFromTray(m_pDetailsPanel);
                    m_pDetailsPanel.hide();
                }
            }
 
            if(AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_TAB))
            {
                m_bSettingsMode = !m_bSettingsMode;
                return true;
            }
 
            if(m_bSettingsMode && AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_RETURN) ||
                AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_NUMPADENTER))
            {
            }
 
            if(!m_bSettingsMode || (m_bSettingsMode && !AdvancedMogreFramework.Singleton.m_Keyboard.IsKeyDown(KeyCode.KC_O)))
                AdvancedMogreFramework.Singleton.keyPressed(keyEventRef);
 
                return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            AdvancedMogreFramework.Singleton.keyPressed(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseMove(evt)) return true;
 
            if(m_bRMouseDown)
            {
                Degree deCameraYaw = new Degree(evt.state.X.rel * -0.1f);
                m_Camera.Yaw(deCameraYaw);
                Degree deCameraPitch = new Degree(evt.state.Y.rel * -0.1f);
                m_Camera.Pitch(deCameraPitch);
            }
 
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseDown(evt, id)) return true;
 
            if(id == MouseButtonID.MB_Left)
            {
                onLeftPressed(evt);
                m_bLMouseDown = true;
            }
            else if (id == MouseButtonID.MB_Right)
            {
                m_bRMouseDown = true;
            }
 
            return true;
        }
	    public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_TrayMgr.injectMouseUp(evt, id)) return true;
 
            if(id == MouseButtonID.MB_Left)
            {
                m_bLMouseDown = false;
            }
            else if(id == MouseButtonID.MB_Right)
            {
                m_bRMouseDown = false;
            }
 
            return true;
        }

        public void onLeftPressed(MouseEvent evt)
        {
            if(m_CurrentObject!=null)
            {
                m_CurrentObject.ShowBoundingBox=false;
                m_CurrentEntity.GetSubEntity(1).SetMaterial(m_pOgreHeadMat);
            }
 
            Ray mouseRay = m_Camera.GetCameraToViewportRay(AdvancedMogreFramework.Singleton.m_Mouse.MouseState.X.abs / (float)evt.state.width,
            AdvancedMogreFramework.Singleton.m_Mouse.MouseState.Y.abs / (float)evt.state.height);
            m_RSQ.Ray=mouseRay;
            //m_pRSQ.SortByDistance=true;
 
            RaySceneQueryResult result = m_RSQ.Execute();
 
            foreach(RaySceneQueryResultEntry itr in result)
            {
                if(itr.movable!=null)
                {
                    AdvancedMogreFramework.Singleton.m_Log.LogMessage("MovableName: " + itr.movable.Name);
                    m_CurrentObject = m_SceneMgr.GetEntity(itr.movable.Name).ParentSceneNode;
                    AdvancedMogreFramework.Singleton.m_Log.LogMessage("ObjName " + m_CurrentObject.Name);
                    m_CurrentObject.ShowBoundingBox=true;
                    m_CurrentEntity = m_SceneMgr.GetEntity(itr.movable.Name);
                    m_CurrentEntity.GetSubEntity(1).SetMaterial(m_pOgreHeadMatHigh);
                    break;
                }
            }
        }
        public override void itemSelected(SelectMenu menu)
        {
            switch(menu.getSelectionIndex())
            {
            case 0:
                m_Camera.PolygonMode=(PolygonMode.PM_SOLID);break;
            case 1:
                m_Camera.PolygonMode=(PolygonMode.PM_WIREFRAME);break;
            case 2:
                m_Camera.PolygonMode=(PolygonMode.PM_POINTS);break;
    }
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            if (AdvancedMogreFramework.Singleton.m_TrayMgr != null)
            {
                AdvancedMogreFramework.Singleton.m_TrayMgr.frameRenderingQueued(m_FrameEvent);
            }
 
            if(m_bQuit == true)
            {
                popAppState();
                return;
            }
            if (AdvancedMogreFramework.Singleton.m_TrayMgr != null)
            {
                if (!AdvancedMogreFramework.Singleton.m_TrayMgr.isDialogVisible())
                {
                    if (m_pDetailsPanel.isVisible())
                    {
                        m_pDetailsPanel.setParamValue(0, m_Camera.DerivedPosition.x.ToString());
                        m_pDetailsPanel.setParamValue(1, m_Camera.DerivedPosition.y.ToString());
                        m_pDetailsPanel.setParamValue(2, m_Camera.DerivedPosition.z.ToString());
                        m_pDetailsPanel.setParamValue(3, m_Camera.DerivedOrientation.w.ToString());
                        m_pDetailsPanel.setParamValue(4, m_Camera.DerivedOrientation.x.ToString());
                        m_pDetailsPanel.setParamValue(5, m_Camera.DerivedOrientation.y.ToString());
                        m_pDetailsPanel.setParamValue(6, m_Camera.DerivedOrientation.z.ToString());
                        if (m_bSettingsMode)
                            m_pDetailsPanel.setParamValue(7, "Buffered Input");
                        else
                            m_pDetailsPanel.setParamValue(7, "Un-Buffered Input");
                    }
                }
            }
 
            m_MoveScale = m_MoveSpeed   * (float)timeSinceLastFrame;
            m_RotScale  = m_RotateSpeed * (float)timeSinceLastFrame;
 
            m_TranslateVector = Mogre.Vector3.ZERO;
 
            getInput();
            moveCamera();
        }
        SceneNode m_pOgreHeadNode;
        Entity m_pOgreHeadEntity;
        MaterialPtr m_pOgreHeadMat;
        MaterialPtr m_pOgreHeadMatHigh;

        ParamsPanel m_pDetailsPanel;
        bool m_bQuit;

        Mogre.Vector3 m_TranslateVector;
        float m_MoveSpeed;
        Degree m_RotateSpeed;
        float m_MoveScale;
        Degree m_RotScale;

        RaySceneQuery m_RSQ;
        SceneNode m_CurrentObject;
        Entity m_CurrentEntity;
        bool m_bRMouseDown,m_bLMouseDown;
        bool m_bSettingsMode;
    }
}
