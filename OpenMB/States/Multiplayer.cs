using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using OpenMB.Mods;
using OpenMB.Network;

namespace OpenMB.States
{
    public class Multiplayer : AppState
    {
        private delegate bool ServerStartDelegate();
        private GameServer thisServer;
        private Dictionary<string, string> option;
        private StringVector serverState;
        private ParamsPanel serverpanel;
        private bool isEscapeMenuOpened;

        public Multiplayer()
        {
            option = new Dictionary<string, string>();
            serverState = new StringVector();
        }

        public override void enter(Mods.ModData e = null)
        {
            modData = e;
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;
            camera = sceneMgr.CreateCamera("multiplayerCam");
            GameManager.Instance.viewport.Camera = camera;
            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;
            GameManager.Instance.viewport.OverlaysEnabled = true;

            GameManager.Instance.keyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.keyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);

            BuildGameListUI();
        }

        #region UI

        private void BuildGameListUI()
        {
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "btnJoin", "Join",50);
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "btnHost", "Host", 50);
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "btnExit", "Exit", 50);
        }
        void HostGameUI()
        {
        }

        private void BuildEscapeMenu()
        {
            GameManager.Instance.trayMgr.destroyAllWidgets();
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "choose_side", "Choose Side", 200f);
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "choose_chara", "Choose Character", 200f);
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_CENTER, "exit_multiplayer", "Exit", 200f);
            this.isEscapeMenuOpened = true;
        }
        #endregion


        void Server_OnEscapePressed()
        {
            ShowEscapeMenu();
        }

        private void ShowEscapeMenu()
        {
        }

        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return true;
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            if (arg.key == MOIS.KeyCode.KC_ESCAPE)
            {
                if (!this.isEscapeMenuOpened)
                {
                    this.BuildEscapeMenu();
                }
                else
                {
                    GameManager.Instance.trayMgr.destroyAllWidgets();
                    this.serverpanel = GameManager.Instance.trayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400f, this.serverState);
                    this.isEscapeMenuOpened = false;
                }
            }
            return true;
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "btnJoin")
            {
            }
            else if (button.getName() == "btnHost")
            {
            }
            else if (button.getName() == "btnCancel")
            {
                exit();
                enter();
            }
            else if (button.getName() == "btnOK")
            {

                thisServer = new GameServer();
                thisServer.OnEscapePressed += new Action(Server_OnEscapePressed);
                GameManager.Instance.trayMgr.destroyAllWidgets();
                serverpanel=GameManager.Instance.trayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400, serverState);
                ServerStartDelegate server = new ServerStartDelegate(ServerStart);
                server.Invoke();
            }
            else if (button.getName() == "btnExit")
            {
                changeAppState(findByName("MainMenu"), modData);
            }
        }

        public bool ServerStart()
        {
            thisServer.Init();
            return thisServer.Go();
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            if (thisServer!=null&&thisServer.Started)
            {
                thisServer.Update();
                thisServer.GetServerState(ref serverState);
                if(!isEscapeMenuOpened)
                    serverpanel.setAllParamValues(serverState);
            }
        }

        public override void exit()
        {
            if (sceneMgr != null)
            {
                sceneMgr.DestroyCamera(camera);
                GameManager.Instance.root.DestroySceneManager(sceneMgr);
            }
            if (thisServer != null)
            {
                thisServer.Exit();
            }
        }

        public override void checkBoxToggled(CheckBox box)
        {
        }
    }
}
