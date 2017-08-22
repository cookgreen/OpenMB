using System;
using Mogre;
using MMOC;
using Mogre_Procedural.MogreBites.Addons;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using AMOFGameEngine.UI;
using AMOFGameEngine.Maps;

namespace AMOFGameEngine.States
{
    public class GameState : AppState
    {
        private CharacterManager characterMgr;
        private CollisionTools collisionMgr;
        private Character player;
        private MapManager mapMngr;
        private SdkCameraMan camMan;


        public GameState()
        {
            mapMngr = new MapManager();
        }

        public override void enter(Mods.ModData data = null)
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
            
            characterMgr = new CharacterManager(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            characterMgr.Init(data.CharacterInfos);

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
    }
}
