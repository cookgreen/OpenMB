using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using Mogre_Procedural.MogreBites.Addons;

namespace AMOFGameEngine
{
    class SinbadState : AppState
    {
	    ParamsPanel		m_pDetailsPanel;   		// sample details panel
	    bool						m_bQuit;
	    bool						m_pCursorWasVisible;		// was cursor visible before dialog appeared
	    bool						m_pDragLook;              // click and drag to free-look
        public SdkCameraMan	m_pCameraMan;
	    SinbadCharacterController	m_pChara;
	    NameValuePairList		mInfo=new NameValuePairList();    // custom sample info
        public SinbadState()
        {
            m_bQuit = false;
            m_pDetailsPanel = null;
            m_pCamera = null;
            m_pCameraMan = null;
            m_pChara = null;
        }

        public override void enter()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Entering SinbadState...");
 
            m_pSceneMgr = AdvancedMogreFramework.Singleton.m_pRoot.CreateSceneManager(SceneType.ST_GENERIC, "SinbadSceneMgr");

            m_pCamera = m_pSceneMgr.CreateCamera("MainCamera");
	        AdvancedMogreFramework.Singleton.m_pViewport.Camera=m_pCamera;
            m_pCamera.AspectRatio = (float)AdvancedMogreFramework.Singleton.m_pViewport.ActualWidth / (float)AdvancedMogreFramework.Singleton.m_pViewport.ActualHeight;
	        m_pCamera.NearClipDistance=5;

	        m_pCameraMan = new SdkCameraMan(m_pCamera);

            AdvancedMogreFramework.Singleton.m_pMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            AdvancedMogreFramework.Singleton.m_pMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            AdvancedMogreFramework.Singleton.m_pMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            AdvancedMogreFramework.Singleton.m_pKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            AdvancedMogreFramework.Singleton.m_pRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(FrameRenderingQueued);

            buildGUI();
 
            createScene();
        }
        public void createScene()
        {
            // set background and some fog
	        AdvancedMogreFramework.Singleton.m_pViewport.BackgroundColour=new ColourValue(1.0f, 1.0f, 0.8f);
	        m_pSceneMgr.SetFog(FogMode.FOG_LINEAR, new ColourValue(1.0f, 1.0f, 0.8f), 0, 15, 100);

	        // set shadow properties
	        m_pSceneMgr.ShadowTechnique=ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
	        m_pSceneMgr.ShadowColour=new ColourValue(0.5f, 0.5f, 0.5f);
	        m_pSceneMgr.SetShadowTextureSize(1024);
	        m_pSceneMgr.ShadowTextureCount=1;

	        // disable default camera control so the character can do its own
	        m_pCameraMan.setStyle(CameraStyle.CS_MANUAL);
	        // use a small amount of ambient lighting
	        m_pSceneMgr.AmbientLight=new ColourValue(0.3f, 0.3f, 0.3f);

	        // add a bright light above the scene
	        Light light = m_pSceneMgr.CreateLight();
	        light.Type=(Light.LightTypes. LT_POINT);
	        light.Position=new Mogre.Vector3(-10, 40, 20);
	        light.SpecularColour=ColourValue.White;

	        // create a floor mesh resource
	        MeshManager.Singleton.CreatePlane("floor", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
		        new Plane(Mogre.Vector3.UNIT_Y, 0), 100, 100, 10, 10, true, 1, 10, 10, Mogre.Vector3.UNIT_Z);

	        // create a floor entity, give it a material, and place it at the origin
            Entity floor = m_pSceneMgr.CreateEntity("Floor", "floor");
            floor.SetMaterialName("Examples/Rockwall");
	        floor.CastShadows=(false);
            m_pSceneMgr.RootSceneNode.AttachObject(floor);

	        // create our character controller
	        m_pChara = new SinbadCharacterController(m_pCamera);

	        AdvancedMogreFramework.Singleton.m_pTrayMgr.toggleAdvancedFrameStats();

	        StringVector items=new StringVector();
	        items.Insert(items.Count,"Help");
	        ParamsPanel help = AdvancedMogreFramework.Singleton.m_pTrayMgr.createParamsPanel(TrayLocation. TL_TOPLEFT, "HelpMessage", 100, items);
	        help.setParamValue("Help", "H / F1");
        }
        public override void exit()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Leaving SinbadState...");
 
            m_pSceneMgr.DestroyCamera(m_pCamera);
	        if (m_pCameraMan!=null) m_pCameraMan=null;
	
            if(m_pSceneMgr!=null)
                AdvancedMogreFramework.Singleton.m_pRoot.DestroySceneManager(m_pSceneMgr);
        }
        public override bool pause()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Pausing SinbadState...");
            return true;
        }
        public override void resume()
        {
            AdvancedMogreFramework.Singleton.m_pLog.LogMessage("Resuming SinbadState...");
 
            buildGUI();

            AdvancedMogreFramework.Singleton.m_pViewport.Camera=m_pCamera;
            m_bQuit = false;
        }

        void buildGUI()
        {
            AdvancedMogreFramework.Singleton.m_pTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
	        AdvancedMogreFramework.Singleton.m_pTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
	        AdvancedMogreFramework.Singleton.m_pTrayMgr.createLabel(TrayLocation.TL_TOP, "GameLbl", "Game mode", 250);
	        AdvancedMogreFramework.Singleton.m_pTrayMgr.showCursor();


            // create a params panel for displaying sample details
	        StringVector items=new StringVector();
	        items.Insert(items.Count,"cam.pX");
	        items.Insert(items.Count,"cam.pY");
	        items.Insert(items.Count,"cam.pZ");
	        items.Insert(items.Count,"");
	        items.Insert(items.Count,"cam.oW");
	        items.Insert(items.Count,"cam.oX");
	        items.Insert(items.Count,"cam.oY");
	        items.Insert(items.Count,"cam.oZ");
	        items.Insert(items.Count,"");
	        items.Insert(items.Count,"Filtering");
	        items.Insert(items.Count,"Poly Mode");

            m_pDetailsPanel = AdvancedMogreFramework.Singleton.m_pTrayMgr.createParamsPanel(TrayLocation.TL_NONE, "DetailsPanel", 200, items);
	        m_pDetailsPanel.hide();

	        m_pDetailsPanel.setParamValue(9, "Bilinear");
	        m_pDetailsPanel.setParamValue(10, "Solid");
        }

        public bool keyPressed(KeyEvent evt)
        {
            if (!AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible()) m_pChara.injectKeyDown(evt);
	        if(AdvancedMogreFramework.Singleton.m_pKeyboard.IsKeyDown(KeyCode.KC_ESCAPE))
            {
                pushAppState(findByName("PauseState"));
                return true;
            }

	        if (evt.key == KeyCode.KC_H || evt.key == KeyCode.KC_F1)   // toggle visibility of help dialog
			{
				if (!AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible() && mInfo["Help"] != "") AdvancedMogreFramework.Singleton.m_pTrayMgr.showOkDialog("Help", mInfo["Help"]);
				else AdvancedMogreFramework.Singleton.m_pTrayMgr.closeDialog();
			}

			if (AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible()) return true;   // don't process any more keys if dialog is up

			if (evt.key == KeyCode.KC_F)   // toggle visibility of advanced frame stats
			{
				AdvancedMogreFramework.Singleton.m_pTrayMgr.toggleAdvancedFrameStats();
			}
			else if (evt.key == KeyCode.KC_G)   // toggle visibility of even rarer debugging details
			{
				if (m_pDetailsPanel.getTrayLocation() == TrayLocation.TL_NONE)
				{
					AdvancedMogreFramework.Singleton.m_pTrayMgr.moveWidgetToTray(m_pDetailsPanel, TrayLocation.TL_TOPRIGHT, 0);
					m_pDetailsPanel.show();
				}
				else
				{
					AdvancedMogreFramework.Singleton.m_pTrayMgr.removeWidgetFromTray(m_pDetailsPanel);
					m_pDetailsPanel.hide();
				}
			}
			else if (evt.key == KeyCode.KC_T)   // cycle polygon rendering mode
			{
				String newVal;
				TextureFilterOptions tfo;
				uint aniso;

                switch (Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(m_pDetailsPanel.getParamValue(9))))
				{
				case "B":
					newVal = "Trilinear";
					tfo = TextureFilterOptions.TFO_TRILINEAR;
					aniso = 1;
					break;
				case "T":
					newVal = "Anisotropic";
					tfo = TextureFilterOptions.TFO_ANISOTROPIC;
					aniso = 8;
					break;
				case "A":
					newVal = "None";
					tfo = TextureFilterOptions.TFO_NONE;
					aniso = 1;
					break;
				default:
					newVal = "Bilinear";
					tfo = TextureFilterOptions.TFO_BILINEAR;
					aniso = 1;
                    break;
				}

				MaterialManager.Singleton.SetDefaultTextureFiltering(tfo);
				MaterialManager.Singleton.DefaultAnisotropy=aniso;
				m_pDetailsPanel.setParamValue(9, newVal);
			}
			else if (evt.key == KeyCode.KC_R)   // cycle polygon rendering mode
			{
				String newVal;
				PolygonMode pm;

				switch (m_pCamera.PolygonMode)
				{
				case PolygonMode.PM_SOLID:
					newVal = "Wireframe";
					pm = PolygonMode.PM_WIREFRAME;
					break;
				case PolygonMode.PM_WIREFRAME:
					newVal = "Points";
					pm = PolygonMode.PM_POINTS;
					break;
				default:
					newVal = "Solid";
					pm = PolygonMode.PM_SOLID;
                    break;
				}

				m_pCamera.PolygonMode=pm;
				m_pDetailsPanel.setParamValue(10, newVal);
			}
			else if(evt.key == KeyCode.KC_F5)   // refresh all textures
			{
				TextureManager.Singleton.ReloadAll();
			}
			else if (evt.key == KeyCode.KC_SYSRQ)   // take a screenshot
			{
                AdvancedMogreFramework.Singleton.m_pRenderWnd.WriteContentsToTimestampedFile("screenshot", ".png");
			}

			m_pCameraMan.injectKeyDown(evt);
			return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            m_pChara.injectKeyUp(keyEventRef);
	        m_pCameraMan.injectKeyUp(keyEventRef);
            AdvancedMogreFramework.Singleton.keyPressed(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent arg)
        {
            if (!AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible()) m_pChara.injectMouseMove(arg);
            if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseMove(arg)) return true;
	        m_pCameraMan.injectMouseMove(arg);
  
            return true;
        }
        public bool mousePressed(MouseEvent arg, MouseButtonID id)
        {
            // relay input events to character controller
	        if (!AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible()) m_pChara.injectMouseDown(arg, id);
	        if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseDown(arg, id)) return true;
            
	        if (m_pDragLook && id == MouseButtonID.MB_Left)
	        {
		        m_pCameraMan.setStyle(CameraStyle.CS_FREELOOK);
                AdvancedMogreFramework.Singleton.m_pTrayMgr.hideCursor();
	        }

            m_pCameraMan.injectMouseDown(arg, id);
            return true;
        }
        public bool mouseReleased(MouseEvent arg, MouseButtonID id)
        {
            if (AdvancedMogreFramework.Singleton.m_pTrayMgr.injectMouseUp(arg, id)) return true;
            
	        if (m_pDragLook && id == MouseButtonID.MB_Left)
	        {
		        m_pCameraMan.setStyle(CameraStyle.CS_MANUAL);
                AdvancedMogreFramework.Singleton.m_pTrayMgr.showCursor();
	        }

            m_pCameraMan.injectMouseUp(arg, id);

	        return true;
        }

        public override void itemSelected(SelectMenu menu)
        {
            switch(menu.getSelectionIndex())
            {
            case 0:
                m_pCamera.PolygonMode=(PolygonMode.PM_SOLID);break;
            case 1:
                m_pCamera.PolygonMode=(PolygonMode.PM_WIREFRAME);break;
            case 2:
                m_pCamera.PolygonMode=(PolygonMode.PM_POINTS);break;
    }
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (int)timeSinceLastFrame;

	        //m_pChara.addTime((float)timeSinceLastFrame);

            AdvancedMogreFramework.Singleton.m_pTrayMgr.frameRenderingQueued(m_FrameEvent);

            if(m_bQuit == true)
            {
                popAppState();
                return;
            }
 
	        if (!AdvancedMogreFramework.Singleton.m_pTrayMgr.isDialogVisible())
	        {
		        m_pCameraMan.frameRenderingQueued(m_FrameEvent);   // if dialog isn't up, then update the camera

		        if (m_pDetailsPanel.isVisible())   // if details panel is visible, then update its contents
		        {
			        m_pDetailsPanel.setParamValue(0, StringConverter.ToString(m_pCamera.DerivedPosition.x));
                    m_pDetailsPanel.setParamValue(1, StringConverter.ToString(m_pCamera.DerivedPosition.y));
                    m_pDetailsPanel.setParamValue(2, StringConverter.ToString(m_pCamera.DerivedPosition.z));
                    m_pDetailsPanel.setParamValue(4, StringConverter.ToString(m_pCamera.DerivedOrientation.w));
                    m_pDetailsPanel.setParamValue(5, StringConverter.ToString(m_pCamera.DerivedOrientation.x));
                    m_pDetailsPanel.setParamValue(6, StringConverter.ToString(m_pCamera.DerivedOrientation.y));
                    m_pDetailsPanel.setParamValue(7, StringConverter.ToString(m_pCamera.DerivedOrientation.z));
		        }	
	        }
        }
        public bool FrameRenderingQueued(FrameEvent evt)
        {
            // let character update animations and camera
            m_pChara.addTime(evt.timeSinceLastFrame);
            return true;
        }
}
}
