using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Maps;
using AMOFGameEngine.Network;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.States
{
    public class ServerState : AppState
    {
        private delegate bool ServerStartDelegate();
        private InputBox ibServerName;
        private InputBox ibServerPort;
        private CheckBox chkHasPasswd;
        private InputBox ibPasswd;
        private MapManager mapMnger;
        private GameServer thisServer;
        private Dictionary<string, string> option;
        private StringVector serverState;
        private ParamsPanel serverpanel;
        public ServerState()
        {
            mapMnger = new MapManager();
            option = new Dictionary<string, string>();
            serverState = new StringVector();
        }

        public override void enter(Mods.ModData e = null)
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;
            m_Camera = m_SceneMgr.CreateCamera("multiplayerCam");
            GameManager.Singleton.mViewport.Camera = m_Camera;
            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbMultiplayer", "Multiplayer", 150);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER,"btnHost","Host Game",200);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_CENTER, "btnJoin", "Join Game",200);

            GameManager.Singleton.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
            
            thisServer = new GameServer();
            thisServer.OnEscapePressed += new Action(Server_OnEscapePressed);
        }

        void HostGameUI()
        {
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbHost", "Host Game", 300);
            ibServerName = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER, "ibServerName", "Server Name:", 180, "New Server");
            ibServerPort = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER, "ibServerPort", "Server Port:", 180, "7458",true);
            chkHasPasswd = GameManager.Singleton.mTrayMgr.createCheckBox(TrayLocation.TL_CENTER, "chkHasPass", "Has Password", 300);
            GameManager.Singleton.mTrayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "smServerMaps", "Server Map:", 190, 10);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnOK", "OK");
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnCancel", "Cancel");
        }

        void Server_OnEscapePressed()
        {
            ShowEscapeMenu();
        }

        private void ShowEscapeMenu()
        {
            GameManager.Singleton.mTrayMgr.showOkDialog("xx", "xx");
        }

        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {
            return GameManager.Singleton.mTrayMgr.injectKeyReleased(arg);
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            return GameManager.Singleton.mTrayMgr.injectKeyPressed(arg);
        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "btnJoin")
            {

            }
            else if (button.getName() == "btnHost")
            {
                HostGameUI();
            }
            else if (button.getName() == "btnCancel")
            {
                exit();
                enter();
            }
            else if (button.getName() == "btnOK")
            {
                GameManager.Singleton.mTrayMgr.destroyAllWidgets();
                serverpanel=GameManager.Singleton.mTrayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400, serverState);
                ServerStartDelegate server = new ServerStartDelegate(ServerStart);
                server.Invoke();
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
            if (thisServer.Started)
            {
                thisServer.Update();
                thisServer.GetServerState(ref serverState);
                serverpanel.setAllParamValues(serverState);
            }
        }

        public override void exit()
        {
            m_SceneMgr.DestroyCamera(m_Camera);
            if (m_SceneMgr != null)
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);

        }

        public override void checkBoxToggled(CheckBox box)
        {
           if (box == chkHasPasswd)
           {
               if (box.isChecked())
               {
                   ibPasswd = GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER,"ibPasswd","Password:",200);
               }
               else
               {
                   GameManager.Singleton.mTrayMgr.destroyWidget("ibPasswd");
               }
           }
        }
    }
}
