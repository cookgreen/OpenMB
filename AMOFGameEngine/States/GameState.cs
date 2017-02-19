using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using RMOgre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;

namespace AMOFGameEngine
{
    enum QueryFlags
    {
        OGRE_HEAD_MASK = 1 << 0,
        CUBE_MASK = 1 << 1
    };
    class GameState : AppState
    {
        OgreCharacter ogrec;
        ExCamera excamera;
        CharacterListener cl;
        protected bool mForward = false;
        protected bool mBackward = false;
        protected bool mLeft = false;
        protected bool mRight = false;

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
            GameManager.Singleton.mLog.LogMessage("Entering GameState...");
            GameManager.LastStateName = "GameState";
            m_SceneMgr=GameManager.Singleton.mRoot.CreateSceneManager(SceneType.ST_GENERIC, "GameSceneMgr");
                
            m_SceneMgr.DestroyAllCameras();
            GameManager.Singleton.mRenderWnd.RemoveAllViewports();

            ogrec = new OgreCharacter("ogrehead", m_SceneMgr);
            excamera = new ExCamera("ogreheadcam", m_SceneMgr, null);

            excamera.getCamera().NearClipDistance = 5;
            GameManager.Singleton.mViewport = GameManager.Singleton.mRenderWnd.AddViewport(excamera.getCamera());
            excamera.getCamera() .AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            cl = new CharacterListener();
            cl.setCharacter(ogrec);
            cl.setExtendedCamera(excamera);
            cl.mMode = ExCamera.Mode.Fixed;

            buildGUI();
 
            createScene();
        }
        public void createScene()
        {

            Mogre.Vector3 vectLightPos=new Mogre.Vector3(75,75,75);
            m_SceneMgr.CreateLight("Light").Position = vectLightPos;//(75, 75, 75);

            /*DotSceneLoader pDotSceneLoader = new DotSceneLoader();
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
            m_pOgreHeadMatHigh.GetTechnique(0).GetPass(0).Diffuse = cvDiffuse;*/
        }
        public override void exit()
        {
            GameManager.Singleton.mLog.LogMessage("Leaving GameState...");

            if(m_SceneMgr!=null)
                //m_SceneMgr.DestroyCamera(m_Camera);
                m_SceneMgr.DestroyQuery(m_RSQ);
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);
        }
        public override bool pause()
        {
            GameManager.Singleton.mLog.LogMessage("Pausing GameState...");
 
            return true;
        }
        public override void resume()
        {
            GameManager.Singleton.mLog.LogMessage("Resuming GameState...");
 
            buildGUI();

            GameManager.Singleton.mViewport.Camera=m_Camera;
            m_bQuit = false;
        }
 
	    public void moveCamera()
        {
                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_LSHIFT))
                    m_Camera.MoveRelative(m_TranslateVector);
                m_Camera.MoveRelative(m_TranslateVector / 10);
        }
        public void getInput()
        {
            if(m_bSettingsMode == false)
            {
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_A))
                    //m_TranslateVector.x = -10;
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_D))
                   // m_TranslateVector.x = 10;
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_W))
                    //m_TranslateVector.z = -10;
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_S))
                    //m_TranslateVector.z = 10;
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_Q))
                    //m_TranslateVector.y = -10;
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_E))
                    //m_TranslateVector.y = 10;
 
        //camera roll
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_Z))
                    //m_Camera.Roll(angleCameraRoll=new Angle(-10));
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_X))
                    //m_Camera.Roll(angleCameraRoll=new Angle(10));
 
        //reset roll
                if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_C))
                    m_Camera.Roll(-(m_Camera.RealOrientation.Roll));
            }
        }
        public void buildGUI()
        {
            GameManager.Singleton.mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            GameManager.Singleton.mTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_TOP, "GameLbl", LocateSystem.CreateLocateString("11161225"), 250);
            GameManager.Singleton.mTrayMgr.showCursor();
 
            List<string> items=new List<string>();
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161226"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161227"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161228"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161229"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161230"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161231"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161232"));
            items.Insert(items.Count, LocateSystem.CreateLocateString("11161233"));

            m_pDetailsPanel = GameManager.Singleton.mTrayMgr.createParamsPanel(TrayLocation.TL_TOPLEFT, "DetailsPanel", 200, items.ToArray());
            m_pDetailsPanel.show();

            string infoText = LocateSystem.CreateLocateString("11161234");
            infoText.Insert(infoText.Length, LocateSystem.CreateLocateString("11161235"));
            infoText.Insert(infoText.Length,LocateSystem.CreateLocateString("11161236"));
            GameManager.Singleton.mTrayMgr.createTextBox(TrayLocation.TL_RIGHT, "InfoPanel", infoText, 300, 220);
 
            StringVector chatModes=new StringVector();
            chatModes.Insert(chatModes.Count, LocateSystem.CreateLocateString("11161237"));
            chatModes.Insert(chatModes.Count, LocateSystem.CreateLocateString("11161238"));
            chatModes.Insert(chatModes.Count, LocateSystem.CreateLocateString("11161239"));
            GameManager.Singleton.mTrayMgr.createLongSelectMenu(TrayLocation.TL_TOPRIGHT, "ChatModeSelMenu", LocateSystem.CreateLocateString("11161240"), 200, 3, chatModes);

            GameManager.Singleton.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            GameManager.Singleton.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            GameManager.Singleton.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(frameStarted);
        }

        bool frameStarted(FrameEvent evt)
        {
            ogrec.Forward(mForward);
            ogrec.Backward(mBackward);
            ogrec.Left(mLeft);
            ogrec.Right(mRight);

            cl.Update(evt.timeSinceLastFrame);
            return true;
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(m_bSettingsMode == true)
            {
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_S))
                {
                    SelectMenu pMenu = (SelectMenu)GameManager.Singleton.mTrayMgr.getWidget("ChatModeSelMenu");
                    if(pMenu.getSelectionIndex() + 1 < (int)pMenu.getNumItems())
                        pMenu.selectItem((uint)pMenu.getSelectionIndex() + 1);
                }
 
                if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_W))
                {
                    SelectMenu pMenu = (SelectMenu)GameManager.Singleton.mTrayMgr.getWidget("ChatModeSelMenu");
                    if(pMenu.getSelectionIndex() - 1 >= 0)
                        pMenu.selectItem((uint)pMenu.getSelectionIndex() - 1);
                }
             }
 
            if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_ESCAPE))
            {
                pushAppState(findByName("PauseState"));
                return true;
            }
 
            if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_I))
            {
                if(m_pDetailsPanel.getTrayLocation() == TrayLocation.TL_NONE)
                {
                    GameManager.Singleton.mTrayMgr.moveWidgetToTray(m_pDetailsPanel, TrayLocation.TL_TOPLEFT, 0);
                    m_pDetailsPanel.show();
                }
                else
                {
                    GameManager.Singleton.mTrayMgr.removeWidgetFromTray(m_pDetailsPanel);
                    m_pDetailsPanel.hide();
                }
            }
 
            if(GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_TAB))
            {
                m_bSettingsMode = !m_bSettingsMode;
                return true;
            }
 
            if(m_bSettingsMode && GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_RETURN) ||
                GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_NUMPADENTER))
            {
            }


            if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_W))
                mForward = true;
            else if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_D))
                mBackward = true;
            else if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_A))
                mLeft = true;
            else if (GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_S))
                mRight = true;

                if (!m_bSettingsMode || (m_bSettingsMode && !GameManager.Singleton.mKeyboard.IsKeyDown(KeyCode.KC_O)))
                    GameManager.Singleton.keyPressed(keyEventRef);
 
                return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            GameManager.Singleton.keyPressed(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseMove(evt)) return true;
 
            if(m_bRMouseDown)
            {
                Degree deCameraYaw = new Degree(evt.state.X.rel * -0.1f);
                excamera.getCamera() .Yaw(deCameraYaw);
                Degree deCameraPitch = new Degree(evt.state.Y.rel * -0.1f);
                excamera.getCamera().Pitch(deCameraPitch);
            }
 
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseDown(evt, id)) return true;
 
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
            if (GameManager.Singleton.mTrayMgr.injectMouseUp(evt, id)) return true;
 
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
            }
 
            /*Ray mouseRay = m_Camera.GetCameraToViewportRay(AdvancedMogreFramework.Singleton.m_Mouse.MouseState.X.abs / (float)evt.state.width,
            AdvancedMogreFramework.Singleton.m_Mouse.MouseState.Y.abs / (float)evt.state.height);
            m_RSQ.Ray=mouseRay;*/
            //m_pRSQ.SortByDistance=true;
 
            /*RaySceneQueryResult result = m_RSQ.Execute();
 
            foreach(RaySceneQueryResultEntry itr in result)
            {
                if(itr.movable!=null)
                {
                    AdvancedMogreFramework.Singleton.m_Log.LogMessage("MovableName: " + itr.movable.Name);
                    m_CurrentObject = m_SceneMgr.GetEntity(itr.movable.Name).ParentSceneNode;
                    AdvancedMogreFramework.Singleton.m_Log.LogMessage("ObjName " + m_CurrentObject.Name);
                    m_CurrentObject.ShowBoundingBox=true;
                    m_CurrentEntity = m_SceneMgr.GetEntity(itr.movable.Name);
                    break;
                }
            }*/
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
            if (GameManager.Singleton.mTrayMgr != null)
            {
                GameManager.Singleton.mTrayMgr.frameRenderingQueued(m_FrameEvent);
            }
 
            if(m_bQuit == true)
            {
                popAppState();
                return;
            }
            /*if (AdvancedMogreFramework.Singleton.m_TrayMgr != null)
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
            }*/
 
            m_MoveScale = m_MoveSpeed   * (float)timeSinceLastFrame;
            m_RotScale  = m_RotateSpeed * (float)timeSinceLastFrame;
 
            m_TranslateVector = Mogre.Vector3.ZERO;

            /*if (ogrec != null)
            {
                ogrec.update((float)timeSinceLastFrame, AdvancedMogreFramework.Singleton.m_Keyboard);
                if (excamera != null)
                {
                    excamera.update(ogrec.getCameranode()._getDerivedPosition(), ogrec.getSightNode()._getDerivedPosition());
                }
            }*/

            //getInput();
            //moveCamera();
        }

        ParamsPanel m_pDetailsPanel;
        bool m_bQuit;

        Mogre.Vector3 m_TranslateVector;
        float m_MoveSpeed;
        Degree m_RotateSpeed;
        float m_MoveScale;
        Degree m_RotScale;

        RaySceneQuery m_RSQ;
        SceneNode m_CurrentObject;
        bool m_bRMouseDown,m_bLMouseDown;
        bool m_bSettingsMode;
    }
}
