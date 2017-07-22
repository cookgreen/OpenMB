using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.States
{
    class MainMenu : AppState,IDisposable
    {
        int modIndex;
        bool isEnterMod;
        SelectMenu renderMenu;
        public MainMenu()
        {
            modIndex = -1;
            m_bQuit         = false;
            m_FrameEvent    = new FrameEvent();
            isEnterMod = false;
        }
        public override void enter(ModData e=null)
        {
            //if (e != null)
            //{
            //    modIndex = e.modIndex;
            //}

            m_bQuit = false;

            SoundManager.Singleton .PlaySoundByType(SoundType.MainMenu);

            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");

            //GameManager.Singleton.console.InitConsole(ref GameManager.Singleton.mRoot);

            ColourValue cvAmbineLight=new ColourValue(0.7f,0.7f,0.7f);
            m_SceneMgr.AmbientLight=cvAmbineLight;
 
            m_Camera = m_SceneMgr.CreateCamera("MenuCam");
            m_Camera.SetPosition(0,25,-50);
            Mogre.Vector3 vectorCameraLookat=new Mogre.Vector3(0,0,0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance=1;//setNearClipDistance(1);

            GameManager.Singleton.mRenderWnd.RemoveAllViewports();
            GameManager.Singleton.mViewport = GameManager.Singleton.mRenderWnd.AddViewport(null);
            GameManager.Singleton.mViewport.BackgroundColour = new ColourValue(0.5f, 0.5f, 0.5f);

            GameManager.Singleton.mViewport.Camera = m_Camera;

            m_Camera.AspectRatio=GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
            GameManager.Singleton.mTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
            GameManager.Singleton.mTrayMgr.showCursor();

            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", e != null ? LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, e.ModName) : LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "MenuState"), 400);

            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "SinglePlayer", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Single Player"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "LoadGame", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Load Game"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "MultiPlayer", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Multiplayer"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Configure", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Configure"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "ModChooser", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Mods"), 250);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Quit", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Quit"), 250);

            GameManager.Singleton.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            GameManager.Singleton.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);
            createScene();
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            GameManager.Singleton.console.FrameStarted(evt);

            return true;
        }
        public void createScene()
        { }
        public override void exit()
        {
            GameManager.Singleton.mLog.LogMessage("Leaving MenuState...");
 
            m_SceneMgr.DestroyCamera(m_Camera);
            if(m_SceneMgr!=null)
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);

            GameManager.Singleton.mTrayMgr.setListener(null);
            GameManager.Singleton.mTrayMgr.clearAllTrays();
            //GameManager.Singleton.mTrayMgr.destroyAllWidgets();
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(GameManager.Singleton.mKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                m_bQuit = true;
                return true;
            }

            GameManager.Singleton.keyPressed(keyEventRef);
            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            GameManager.Singleton.keyReleased(keyEventRef);
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Singleton.mTrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "Quit")
                m_bQuit = true;
            else if (button.getName() == "LoadGame")
                changeAppState(findByName("GameState"));
            else if (button.getName() == "MultiPlayer")
                changeAppState(findByName("Multiplayer"));
            else if (button.getName() == "SinglePlayer")
                changeAppState(findByName("SinglePlayer"));
            else if (button.getName() == "ModChooser")
                changeAppState(findByName("ModChooser"));
            else if (button.getName() == "Configure")
                Configure();
            else if (button.getName() == "btnBack")
                enter();
        }

        private void Configure()
        {
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbConfig", "Configure");
            renderMenu = GameManager.Singleton.mTrayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "rendersys", "Render System", 450, 240, 10);
            StringVector rsNames = new StringVector();
            Const_RenderSystemList rsList = GameManager.Singleton.mRoot.GetAvailableRenderers();
            for (int i = 0; i < rsList.Count; i++)
            {
                rsNames.Add(rsList[i].Name);
            }
            renderMenu.setItems(rsNames);
            renderMenu.selectItem(GameManager.Singleton.mRoot.RenderSystem.Name);

            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnApply", "Apply");
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnBack", "Back");
        }

        public override void itemSelected(SelectMenu menu)
        {
            if (menu == renderMenu)
            {
                while (GameManager.Singleton.mTrayMgr.getNumWidgets(renderMenu.getTrayLocation()) > 2)
                {
                    GameManager.Singleton.mTrayMgr.destroyWidget(renderMenu.getTrayLocation(), 2);
                }
                uint i=0;
                ConfigOptionMap options = GameManager.Singleton.mRoot.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
                foreach (var item in options)
                {
                    i++;
                    SelectMenu optionMenu = GameManager.Singleton.mTrayMgr.createLongSelectMenu(
                        TrayLocation.TL_CENTER, "ConfigOption" + i.ToString(), item.Key, 450, 240, 10);
                    optionMenu.setItems(item.Value.possibleValues);

                    try
                    {
                        optionMenu.selectItem(item.Value.currentValue);
                    }
                    catch
                    {
                        optionMenu.addItem(item.Value.currentValue);
                        optionMenu.selectItem(item.Value.currentValue);
                    }
                }
            }
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            GameManager.Singleton.mTrayMgr.frameRenderingQueued(m_FrameEvent);

            if(m_bQuit == true)
            {
                shutdown();
                return;
            }
        }

        protected bool m_bQuit;
    }
}
