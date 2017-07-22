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
        CharacterManager characterMgr;
        UIManager uiMgr;
        IntersectionSceneQuery pISQuery;
        CollisionTools collisionMgr;
        Character player;
        MapManager mapMngr;
        SdkCameraMan camMan;


        public GameState()
        {
            mapMngr = new MapManager();
        }

        public override void enter(Mods.ModData data = null)
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            m_Camera = m_SceneMgr.CreateCamera("gameCam");
            m_Camera.NearClipDistance = 5;
            GameManager.Singleton.mRenderWnd.RemoveAllViewports();
            GameManager.Singleton.mViewport= GameManager.Singleton.mRenderWnd.AddViewport(m_Camera);
            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;
            GameManager.Singleton.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            pISQuery = m_SceneMgr.CreateIntersectionQuery();
            collisionMgr = new CollisionTools(m_SceneMgr);

            m_SceneMgr.AmbientLight = new ColourValue(0.6f,0.6f,0.6f);
            m_SceneMgr.SetSkyBox(true, "Examples/SpaceSkyBox");

            Light light = m_SceneMgr.CreateLight();
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Vector3(-10, 40, 20);
            light.SpecularColour = ColourValue.White;

            camMan = new SdkCameraMan(m_Camera);
            camMan.setStyle(CameraStyle.CS_MANUAL);

            Map defaultScene = new Map("CubeScene.xml", m_SceneMgr);
            defaultScene.create("defaultScene", mapMngr);

            characterMgr = new CharacterManager(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            player = new Character(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse, true);
            player.CharaName = "Player";
            player.CharaMeshName = "Sinbad.mesh";
            player.Create();

            //mapMngr.StartMap(mapMngr.FindMapByName("defaultScene"));

            characterMgr.AddCharacterToManageLst(player);
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            characterMgr.UpdateCharacters(evt.timeSinceLastFrame);

            return true;
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            m_FrameEvent.timeSinceLastFrame = (float)timeSinceLastFrame;

            camMan.frameRenderingQueued(m_FrameEvent);
        }

        public override void exit()
        {
            base.exit();
        }
    }
}
