using System;
using Mogre;
using Mogre_Procedural.MogreBites.Addons;
using MMOC;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using AMOFGameEngine.UI;
using AMOFGameEngine.Maps;

namespace AMOFGameEngine.States
{
    public class SinglePlayer : AppState
    {
        private CharacterManager characterMgr;
        private MapManager mapMngr;
        private CollisionTools collisionMgr;
        private SdkCameraMan camMan;
        private Player player;

        public SinglePlayer()
        {
            mapMngr = new MapManager();
        }

        public override void enter(Mods.ModData data = null)
        {
            m_Data = data;
            CreateScene();
            InitGame();

            characterMgr.SpawnPlayer("chara_main_player");
            mapMngr.EnterNewMap(new SceneMap("Cubescene.xml", m_SceneMgr));
            characterMgr.SetSpawnPosition(new Vector3(0, 0,10));
            characterMgr.SpawnCharacter("chara_archer");
            player = characterMgr.GetPlayer();
            

            GameManager.Singleton.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            GameManager.Singleton.UpdateGame(evt.timeSinceLastFrame);

            return true;
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;
        }

        public override void exit()
        {
            GameManager.Singleton.AllGameObjects.Clear();

            base.exit();
        }

        private void CreateScene()
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            m_SceneMgr.AmbientLight = new ColourValue(0.7f, 0.7f, 0.7f);

            m_Camera = m_SceneMgr.CreateCamera("gameCam");
            m_Camera.NearClipDistance = 5;

            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mViewport.Camera = m_Camera;

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            collisionMgr = new CollisionTools(m_SceneMgr);
            m_Camera.FarClipDistance = 50000;

            m_SceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);

            Light light = m_SceneMgr.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;

            camMan = new SdkCameraMan(m_Camera);
            camMan.setStyle(CameraStyle.CS_MANUAL);
            GameManager.Singleton.mTrayMgr.hideCursor();
        }

        private void SetInput()
        {
            GameManager.Singleton.mMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(mMouse_MousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(mMouse_MouseReleased);
            GameManager.Singleton.mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(mKeyboard_KeyPressed);
            GameManager.Singleton.mKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(mKeyboard_KeyReleased);
        }

        bool mKeyboard_KeyReleased(MOIS.KeyEvent arg)
        {

            return true;
        }

        bool mKeyboard_KeyPressed(MOIS.KeyEvent arg)
        {
            return true;
        }

        bool mMouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        bool mMouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
        {
            return true;
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            return true;
        }

        private void InitGame()
        {
            characterMgr = new CharacterManager(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            characterMgr.Init(m_Data.CharacterInfos);
            mapMngr = new MapManager();
        }
    }
}
