using System;
using Mogre;
using MMOC;
using AMOFGameEngine.Mods;
using AMOFGameEngine.RPG;
using Helper;

namespace AMOFGameEngine.States
{
    public class GameState : AppState
    {
        SceneManager scm;
        CharacterManager characterMgr;
        IntersectionSceneQuery pISQuery;
        CollisionTools collisionMgr;
        Character player;
        DotSceneLoader dotSceneLoader;

        public GameState()
        {
            scm = null;
            dotSceneLoader = new DotSceneLoader();
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

            characterMgr = new CharacterManager(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            player = new Character(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse, true);
            player.CharaName = "Player";
            player.CharaMeshName = "Sinbad.mesh";
            player.Create();

            Character npc1 = new Character(m_Camera, GameManager.Singleton.mKeyboard, GameManager.Singleton.mMouse);
            npc1.CharaName = "Smith";
            npc1.CharaMeshName = "Sinbad.mesh";
            npc1.Create();

            Weapon sword = new Weapon(m_Camera);
            sword.ItemName = "Sword";
            sword.ItemMeshName = "Sword.mesh";
            sword.ItemAttachDir = ItemAttachOption.IAO_LEFT;
            sword.Create();

            player.AddEquipment(sword, true);

            characterMgr.AddCharacterToManageLst(player);
            characterMgr.AddCharacterToManageLst(npc1);
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
            
        }

        public override void exit()
        {
            base.exit();
        }
    }
}
