using System;
using Mogre;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Widgets;

namespace AMOFGameEngine.States
{
    public class MultiGameState : AppState
    {
        private CheckBox chkHasPasswd;
        private InputBox ibPasswd;
        public MultiGameState()
        {
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
            //base.enter(e);
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

        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            base.update(timeSinceLastFrame);
        }

        public override void exit()
        {
            base.exit();
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

        void HostGameUI()
        {
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_CENTER, "lbHost", "Host Game",300);
            GameManager.Singleton.mTrayMgr.createInputBox(TrayLocation.TL_CENTER, "ibServerName", "Server Name:", 180);
            chkHasPasswd= GameManager.Singleton.mTrayMgr.createCheckBox(TrayLocation.TL_CENTER, "chkHasPass", "Has Password", 300);
            GameManager.Singleton.mTrayMgr.createLongSelectMenu(TrayLocation.TL_CENTER, "smServerMaps", "Server Map:", 190,10);
        }
    }
}
