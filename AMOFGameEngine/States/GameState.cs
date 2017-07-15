using System;
using Mogre;
using MMOC;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using AMOFGameEngine.UI;
using AMOFGameEngine.Maps;

namespace AMOFGameEngine.States
{
    public class GameState : AppState
    {
        SceneManager scm;
        CharacterManager characterMgr;
        UIManager uiMgr;
        IntersectionSceneQuery pISQuery;
        CollisionTools collisionMgr;
        Character player;
        MapManager mapMngr;

        public GameState()
        {
            mapMngr = new MapManager();
            scm = null;
        }

        public override void enter(Mods.ModData data = null)
        {
            scm = GameManager.Singleton.mRoot.CreateSceneManager(SceneType.ST_GENERIC);
            m_Camera= scm.CreateCamera("gameCam");
            GameManager.Singleton.mViewport.Camera = m_Camera;
            GameManager.Singleton.mViewport.BackgroundColour = new ColourValue(0,0,0);
            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;
            GameManager.Singleton.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(mRoot_FrameStarted);
            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            pISQuery=scm.CreateIntersectionQuery();
            collisionMgr = new CollisionTools(scm);

            Map defaultScene = new Map("CubeScene.xml", scm);
            defaultScene.create("defaultScene", mapMngr);

            characterMgr = new CharacterManager(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            player = new Character(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse, true);
            player.CharaName = "Player";
            player.CharaMeshName = "Sinbad.mesh";
            player.Create();

            mapMngr.StartMap(mapMngr.FindMapByName("defaultScene"));

            characterMgr.AddCharacterToManageLst(player);
        }

        bool mRoot_FrameStarted(FrameEvent evt)
        {
            //characterMgr.UpdateCharacters(evt.timeSinceLastFrame);

            return true;
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void update(double timeSinceLastFrame)
        {
            
        }

        public override void exit()
        {
            base.exit();
        }
    }
}
