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
using OpenMB.UI;
using OpenMB.UI.Widgets;

namespace OpenMB.States
{
    public class MainMenu : AppState
    {
        protected bool m_bQuit;
        private SelectMenuWidget renderMenu;
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
            
            SoundManager.Instance.InitSound(camera);
            SoundManager.Instance.PlayMusicByType(PlayType.MainMenu);

			ScreenManager.Instance.OnExternalEvent += OnExternalEvent;
			ScreenManager.Instance.ChangeScreen("MainMenu", true , modData, sceneMgr);

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
					//changeAppState(findByName("SinglePlayer"), modData);
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
            ScreenManager.Instance.ExitCurrentScreen();
            UIManager.Instance.DestroyAllWidgets();
            GameManager.Instance.root.DestroySceneManager(sceneMgr);

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
			UIManager.Instance.InjectMouseMove(evt);
            return true;
        }
        public bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
			UIManager.Instance.InjectMouseDown(evt, id);
            return true;
        }
        public bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            UIManager.Instance.InjectMouseUp(evt, id);
            return true;
        }

        private void CheckConfigure()
        {
            bool isModified = false;
            Dictionary<string, string> displayOptions = new Dictionary<string, string>();
            ConfigOptionMap options = GameManager.Instance.root.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
            for (uint i = 3; i < UIManager.Instance.GetNumWidgets(renderMenu.GetTrayLocation());i++ )
            {
                SelectMenuWidget optionMenu = (SelectMenuWidget)UIManager.Instance.GetWidget(renderMenu.GetTrayLocation(), i);
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
            UIManager.Instance.DestroyAllWidgets();
            UIManager.Instance.CreateLabel(UIWidgetLocation.TL_CENTER, "lbConfig", "Configure");
            renderMenu = UIManager.Instance.CreateLongSelectMenu(UIWidgetLocation.TL_CENTER, "rendersys", "Render System", 450, 240, 10);
            List<string> rsNames = new List<string>();
            Const_RenderSystemList rsList = GameManager.Instance.root.GetAvailableRenderers();
            for (int i = 0; i < rsList.Count; i++)
            {
                rsNames.Add(rsList[i].Name);
            }
            renderMenu.SetItems(rsNames);
            renderMenu.SelectItem(GameManager.Instance.root.RenderSystem.Name);

            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnApply", "Apply");
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "btnBack", "Back");
        }

        public override void itemSelected(SelectMenuWidget menu)
        {
            if (menu == renderMenu)
            {
                while (UIManager.Instance.GetNumWidgets(renderMenu.GetTrayLocation()) > 2)
                {
                    UIManager.Instance.DestroyWidget(renderMenu.GetTrayLocation(), 2);
                }
                uint i=0;
                ConfigOptionMap options = GameManager.Instance.root.GetRenderSystemByName(renderMenu.getSelectedItem()).GetConfigOptions();
                foreach (var item in options)
                {
                    i++;
                    SelectMenuWidget optionMenu = UIManager.Instance.CreateLongSelectMenu(
                        UIWidgetLocation.TL_CENTER, "ConfigOption" + i.ToString(), item.Key, 450, 240, 10);
                    optionMenu.SetItems(item.Value.possibleValues.ToList());

                    try
                    {
                        optionMenu.SelectItem(item.Value.currentValue);
                    }
                    catch
                    {
                        optionMenu.AddItem(item.Value.currentValue);
                        optionMenu.SelectItem(item.Value.currentValue);
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
            UIManager.Instance.FrameRenderingQueued(frameEvent);
        }
    }
}
