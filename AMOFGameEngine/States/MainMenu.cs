using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Sound;
using AMOFGameEngine.Utilities;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Video;

namespace AMOFGameEngine.States
{
    class MainMenu : AppState
    {
        protected bool m_bQuit;
        private SelectMenu renderMenu;
        public MainMenu()
        {
            m_bQuit         = false;
            m_FrameEvent    = new FrameEvent();
        }
        public override void enter(ModData e=null)
        {
            m_Data = e;
            m_bQuit = false;

            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            m_SceneMgr.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f); ;
 
            m_Camera = m_SceneMgr.CreateCamera("MenuCam");
            m_Camera.FarClipDistance = 5000;
            m_Camera.NearClipDistance = 10;
            m_Camera.SetPosition(320,240,500);
            m_Camera.LookAt(new Mogre.Vector3(320, 240, 0));
            GameManager.Instance.mRenderWnd.RemoveAllViewports();
            GameManager.Instance.mViewport = GameManager.Instance.mRenderWnd.AddViewport(null);
            GameManager.Instance.mViewport.BackgroundColour = new ColourValue(0.5f, 0.5f, 0.5f);
            GameManager.Instance.mViewport.Camera = m_Camera;
            m_Camera.AspectRatio=GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;

            buildMainMenu(e);

            GameManager.Instance.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            GameManager.Instance.mMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            GameManager.Instance.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);
            GameManager.Instance.mKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            GameManager.Instance.mKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            VideoTextureManager.Instance.CreateVideoTexture(m_SceneMgr, 640, 480, Path.Combine(m_Data.BasicInfo.InstallPath, m_Data.BasicInfo.Movie));
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            return true;
        }
        public override void exit()
        {
            if (m_SceneMgr != null)
            {
                m_SceneMgr.DestroyCamera(m_Camera);
                GameManager.Instance.mRoot.DestroySceneManager(m_SceneMgr);
            }
            VideoTextureManager.Instance.DestroyVideoTexture();
            GameManager.Instance.mTrayMgr.clearAllTrays();
            ModManager.Instance.UnloadAllMods();
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(GameManager.Instance.mKeyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
            {
                m_bQuit = true;
                return true;
            }

            return true;
        }
        public bool keyReleased(KeyEvent keyEventRef)
        {
            return true;
        }

        public bool mouseMoved(MouseEvent evt)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.mTrayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "Quit")
            {
                m_bQuit = true;
            }
            else if (button.getName() == "LoadGame")
            {
                changeAppState(findByName("SinglePlayer"));
            }
            else if (button.getName() == "MultiPlayer")
            {
                changeAppState(findByName("Multiplayer"), m_Data);
            }
            else if (button.getName() == "SinglePlayer")
            {
                changeAppState(findByName("SinglePlayer"),m_Data);
            }
            else if (button.getName() == "ModChooser")
            {
                changeAppState(findByName("ModChooser"));
            }
            else if (button.getName() == "Configure")
            {
                Configure();
            }
            else if (button.getName() == "btnBack")
            {
                buildMainMenu(m_Data);
            }
            else if (button.getName() == "btnApply")
            {
                CheckConfigure();
            }
        }

        private void CheckConfigure()
        {
            bool isModified = false;
            Dictionary<string, string> displayOptions = new Dictionary<string, string>();
            ConfigOptionMap options = GameManager.Instance.mRoot.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
            for (uint i = 3; i < GameManager.Instance.mTrayMgr.getNumWidgets(renderMenu.getTrayLocation());i++ )
            {
                SelectMenu optionMenu = (SelectMenu)GameManager.Instance.mTrayMgr.getWidget(renderMenu.getTrayLocation(), i);
                if (optionMenu.getSelectedItem() != options[optionMenu.getCaption()].currentValue)
                    isModified = true;
                displayOptions.Add(optionMenu.getCaption(), optionMenu.getSelectedItem());
            }
            OgreConfigFileAdapter ofa = new OgreConfigFileAdapter("ogre.cfg");
            List<OgreConfigNode> ogrecfgdata = ofa.ReadConfigData();
            OgreConfigNode oneConfig = ogrecfgdata.Where(o => o.Section == renderMenu.getSelectedItem()).FirstOrDefault();
            Dictionary<string, string> fileOptions = oneConfig.Settings;
            if (isModified)
            {
                int indexDeleted = ogrecfgdata.IndexOf(oneConfig);
                ogrecfgdata.RemoveAt(indexDeleted);
                oneConfig.Settings = displayOptions;
                ogrecfgdata.Insert(indexDeleted, oneConfig);
                ofa.SaveConfig(ogrecfgdata);
                m_bQuit = true;

                ReConfigure(renderMenu.getSelectedItem(), displayOptions);
            }
        }

        private void Configure()
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            GameManager.Instance.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbConfig", "Configure");
            renderMenu = GameManager.Instance.mTrayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "rendersys", "Render System", 450, 240, 10);
            StringVector rsNames = new StringVector();
            Const_RenderSystemList rsList = GameManager.Instance.mRoot.GetAvailableRenderers();
            for (int i = 0; i < rsList.Count; i++)
            {
                rsNames.Add(rsList[i].Name);
            }
            renderMenu.setItems(rsNames);
            renderMenu.selectItem(GameManager.Instance.mRoot.RenderSystem.Name);

            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnApply", "Apply");
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnBack", "Back");
        }

        public override void itemSelected(SelectMenu menu)
        {
            if (menu == renderMenu)
            {
                while (GameManager.Instance.mTrayMgr.getNumWidgets(renderMenu.getTrayLocation()) > 2)
                {
                    GameManager.Instance.mTrayMgr.destroyWidget(renderMenu.getTrayLocation(), 2);
                }
                uint i=0;
                ConfigOptionMap options = GameManager.Instance.mRoot.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
                foreach (var item in options)
                {
                    i++;
                    SelectMenu optionMenu = GameManager.Instance.mTrayMgr.createLongSelectMenu(
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
            if (m_bQuit == true)
            {
                shutdown();
                return;
            }

            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            GameManager.Instance.mTrayMgr.frameRenderingQueued(m_FrameEvent);
            VideoTextureManager.Instance.Update((float)timeSinceLastFrame);
        }

        private void buildMainMenu(ModData data)
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            GameManager.Instance.mTrayMgr.showCursor();

            GameManager.Instance.mTrayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", data != null ? LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, data.BasicInfo.Name) : LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "MenuState"), 400);

            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "SinglePlayer", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Single Player"), 200);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "LoadGame", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Load Game"), 200);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "MultiPlayer", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Multiplayer"), 200);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Configure", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Configure"), 200);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "Quit", LocateSystem.Singleton.LOC(LocateFileType.GameQuickString, "Quit"), 200);
        }
    }
}
