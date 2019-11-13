using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using OpenMB.Localization;
using OpenMB.Sound;
using OpenMB.Utilities;
using OpenMB.Mods;
using OpenMB.Map;
using OpenMB.Screen;

namespace OpenMB.States
{
    public class MainMenu : AppState
    {
        protected bool m_bQuit;
        private SelectMenu renderMenu;
        public MainMenu()
        {
            m_bQuit         = false;
            frameEvent    = new FrameEvent();
        }
        public override void enter(ModData e=null)
        {
            modData = e;
            m_bQuit = false;
            
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            sceneMgr.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f); ;
 
            camera = sceneMgr.CreateCamera("MenuCam");
            camera.FarClipDistance = 5000;
            camera.NearClipDistance = 10;
            camera.SetPosition(320,240,500);
            camera.LookAt(new Mogre.Vector3(320, 240, 0));
            GameManager.Instance.renderWindow.RemoveAllViewports();
            GameManager.Instance.viewport = GameManager.Instance.renderWindow.AddViewport(null);
            GameManager.Instance.viewport.BackgroundColour = new ColourValue(0.5f, 0.5f, 0.5f);
            GameManager.Instance.viewport.Camera = camera;
            camera.AspectRatio=GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
            
            SoundManager.Instance.InitSound(camera, modData);
            SoundManager.Instance.PlayMusicByID("game_title");
            GameMapManager.Instance.Init(modData);

			ScreenManager.Instance.OnExternalEvent += OnExternalEvent;
			ScreenManager.Instance.ChangeScreen("MainMenu", true , modData);

            GameManager.Instance.mouse.MouseMoved += mouseMoved;
            GameManager.Instance.mouse.MousePressed += mousePressed;
            GameManager.Instance.mouse.MouseReleased += mouseReleased;
            GameManager.Instance.keyboard.KeyPressed += keyPressed;
            GameManager.Instance.keyboard.KeyReleased += keyReleased;
            
        }

		private void OnExternalEvent(string widgetName, string value)
		{
			switch (widgetName)
			{
				case "btnQuit":
					m_bQuit = true;
					break;
				case "btnLoadGame":
					changeAppState(findByName("SinglePlayer"), modData);
					break;
				case "btnMultiplayer":
					//changeAppState(findByName("Multiplayer"), m_Data);
					break;
				case "btnSingleplayer":
					changeAppState(findByName("SinglePlayer"), modData);
					break;
				case "btnModChooser":
					changeAppState(findByName("ModChooser"));
					break;
				case "btnConfigre":
					Configure();
					break;
				case "btnCredit":
					changeAppState(findByName("Credit"), modData);
					break;
				case "btnApply":
					CheckConfigure();
					break;
			}
		}
        public override void exit()
        {
            sceneMgr.DestroyCamera(camera);
            GameManager.Instance.trayMgr.destroyAllWidgets();
            GameManager.Instance.root.DestroySceneManager(sceneMgr);
            ModManager.Instance.UnloadAllMods();

            GameManager.Instance.mouse.MouseMoved -= mouseMoved;
            GameManager.Instance.mouse.MousePressed -= mousePressed;
            GameManager.Instance.mouse.MouseReleased -= mouseReleased;
            GameManager.Instance.keyboard.KeyPressed -= keyPressed;
            GameManager.Instance.keyboard.KeyReleased -= keyReleased;
        }

        public bool keyPressed(KeyEvent keyEventRef)
        {
            if(GameManager.Instance.keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
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
            if (GameManager.Instance.trayMgr.injectMouseMove(evt)) return true;
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.trayMgr.injectMouseDown(evt, id)) return true;
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            if (GameManager.Instance.trayMgr.injectMouseUp(evt, id)) return true;
            return true;
        }

        private void CheckConfigure()
        {
            bool isModified = false;
            Dictionary<string, string> displayOptions = new Dictionary<string, string>();
            ConfigOptionMap options = GameManager.Instance.root.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
            for (uint i = 3; i < GameManager.Instance.trayMgr.getNumWidgets(renderMenu.getTrayLocation());i++ )
            {
                SelectMenu optionMenu = (SelectMenu)GameManager.Instance.trayMgr.getWidget(renderMenu.getTrayLocation(), i);
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
            GameManager.Instance.trayMgr.destroyAllWidgets();
            GameManager.Instance.trayMgr.createLabel(TrayLocation.TL_CENTER, "lbConfig", "Configure");
            renderMenu = GameManager.Instance.trayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "rendersys", "Render System", 450, 240, 10);
            StringVector rsNames = new StringVector();
            Const_RenderSystemList rsList = GameManager.Instance.root.GetAvailableRenderers();
            for (int i = 0; i < rsList.Count; i++)
            {
                rsNames.Add(rsList[i].Name);
            }
            renderMenu.setItems(rsNames);
            renderMenu.selectItem(GameManager.Instance.root.RenderSystem.Name);

            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "btnApply", "Apply");
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "btnBack", "Back");
        }

        public override void itemSelected(SelectMenu menu)
        {
            if (menu == renderMenu)
            {
                while (GameManager.Instance.trayMgr.getNumWidgets(renderMenu.getTrayLocation()) > 2)
                {
                    GameManager.Instance.trayMgr.destroyWidget(renderMenu.getTrayLocation(), 2);
                }
                uint i=0;
                ConfigOptionMap options = GameManager.Instance.root.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
                foreach (var item in options)
                {
                    i++;
                    SelectMenu optionMenu = GameManager.Instance.trayMgr.createLongSelectMenu(
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

            frameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
            GameManager.Instance.trayMgr.frameRenderingQueued(frameEvent);
        }

        private void buildMainMenu(ModData data)
        {
            GameManager.Instance.trayMgr.destroyAllWidgets();
            GameManager.Instance.trayMgr.showCursor();

            GameManager.Instance.trayMgr.createLabel(TrayLocation.TL_TOP, "MenuLbl", data != null ? LocateSystem.Instance.LOC(LocateFileType.GameQuickString, data.BasicInfo.Name) : LocateSystem.Instance.LOC(LocateFileType.GameQuickString, "MenuState"), 400);

            if(modData.HasSinglePlayer)
                GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnSingleplayer", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_single_player"), 200);
            if(modData.HasSavedGame)
                GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnLoadGame", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_load"), 200);
            if(modData.HasMultiplater)
                GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnMultiplayer", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_multiplayer"), 200);

            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnConfigure", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_config"), 200);

            if (modData.HasCredit)
                GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnCredit", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_credit"), 200);

            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "btnQuit", LocateSystem.Instance.GetLocalizedString(LocateFileType.GameString, "str_quit"), 200);
        }
    }
}
