using System;
using System.Collections.Generic;
using Mogre;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Network;

namespace AMOFGameEngine.States
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
            m_Data = e;
            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "MenuSceneMgr");
            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;
            m_Camera = m_SceneMgr.CreateCamera("multiplayerCam");
            GameManager.Instance.mViewport.Camera = m_Camera;
            m_Camera.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;
            GameManager.Instance.mViewport.OverlaysEnabled = true;

            GameManager.Instance.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Instance.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);

            BuildGameListUI();
        }

        #region UI

        private void BuildGameListUI()
        {
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnJoin", "Join",50);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnHost", "Host", 50);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnExit", "Exit", 50);
        }
        void HostGameUI()
        {
        }

        private void BuildEscapeMenu()
        {
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "choose_side", "Choose Side", 200f);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "choose_chara", "Choose Character", 200f);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_CENTER, "exit_multiplayer", "Exit", 200f);
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
                    GameManager.Instance.mTrayMgr.destroyAllWidgets();
                    this.serverpanel = GameManager.Instance.mTrayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400f, this.serverState);
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
                GameManager.Instance.mTrayMgr.destroyAllWidgets();
                serverpanel=GameManager.Instance.mTrayMgr.createParamsPanel(TrayLocation.TL_CENTER, "serverpanel", 400, serverState);
                ServerStartDelegate server = new ServerStartDelegate(ServerStart);
                server.Invoke();
            }
            else if (button.getName() == "btnExit")
            {
                changeAppState(findByName("MainMenu"), m_Data);
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
            if (m_SceneMgr != null)
            {
                m_SceneMgr.DestroyCamera(m_Camera);
                GameManager.Instance.mRoot.DestroySceneManager(m_SceneMgr);
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
