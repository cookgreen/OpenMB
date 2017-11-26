using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mogre;
using MOIS;
using AMOFGameEngine.AI;
using AMOFGameEngine.RPG.Controller;
using AMOFGameEngine.RPG.Data;
using AMOFGameEngine.RPG.Traits;
using AMOFGameEngine.Sound;

namespace AMOFGameEngine.RPG.Objects
{
    //                       Enemy is defeated
    //   --------------------------------------------------
    //   |    Enemy spotted                                |     Can't defeat enemy 
    // Idle ---------------> SpotEnemy ----------------> Attack -----------------> Flee
    //                                  Enemy In range
    internal class CharacterIdleState : State<Character>
    {
        public override void Enter(Character owner, params object[] extraParams)
        {
        }

        public override void Execute(Character owner)
        {
        }

        public override void Exit(Character owner)
        {
        }
    }
    internal class CharacterSpotEnemyState : State<Character>
    {

        public override void Enter(Character owner, params object[] extraParams)
        {
        }

        public override void Execute(Character owner)
        {
        }

        public override void Exit(Character owner)
        {
        }
    }
    internal class CharacterAttackState : State<Character>
    {
        Character target;
        public override void Enter(Character owner, params object[] extraParams)
        {
            target = extraParams[0] as Character;
            target.ChangeState(CharacterUnderAttackState.Instance);
        }

        public override void Execute(Character owner)
        {
            if (target.Info.Alive)
                owner.Attack(target);
            else
                owner.ChangeState(CharacterIdleState.Instance);
        }

        public override void Exit(Character owner)
        {
            target = null;
        }
    }
    internal class CharacterUnderAttackState : State<Character>
    {
        Character attacter;
        public override void Enter(Character owner, params object[] extraParams)
        {
            attacter = extraParams[0] as Character;
        }

        public override void Execute(Character owner)
        {
            if (attacter.Info.Alive)
                owner.Attack(attacter);
            else
                owner.ChangeState(CharacterIdleState.Instance);
        }

        public override void Exit(Character owner)
        {
            attacter = null;
        }
    }
    internal class CharacterFleeState : State<Character>
    {

        public override void Enter(Character owner,params object[] extraParam)
        {
            throw new NotImplementedException();
        }

        public override void Execute(Character owner)
        {
            throw new NotImplementedException();
        }

        public override void Exit(Character owner)
        {
            throw new NotImplementedException();
        }
    }

    public class CharacterInfo : GameObjectInfo
    {
        private string name;
        private string meshName;
        private LevelInfo level;
        private int hitpoint;
        private InventoryInfo inventory;
        private bool alive;

        public bool Alive
        {
            get { return alive; }
        }
        public int HitPoint
        {
            get 
            { 
                return hitpoint; 
            }
            set 
            { 
                if (hitpoint <= 0)
                {
                    alive = false;
                }
                else
                {
                    hitpoint = value;
                }
            }
        }
        public LevelInfo Level
        {
            get { return level; }
            set { level = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string MeshName
        {
            get { return meshName; }
            set { meshName = value; }
        }
        public Camera Cam
        {
            get { return cam; }
            set { cam = value; }
        }
        public Entity Mesh
        {
            get
            {
                return mesh;
            }
        }
        public SceneNode Node
        {
            get
            {
                return node;
            }
        }

        public CharacterInfo(string name, string meshName, Camera cam)
        {
            level = new LevelInfo();
            hitpoint = 100;
            inventory = new InventoryInfo();
        }

        public override void Initization(string name, string meshName, Camera cam)
        {
            this.name = name;
            this.meshName = meshName;
            this.cam = cam;

            mesh = cam.SceneManager.CreateEntity(name, meshName);
            node = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            node.AttachObject(mesh);

            var animNames =new String[]
	    {"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
	    "SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

            animations = new AnimationState[animNames.Length];
            for (int i = 0; i < animNames.Length; i++)
            {
                animations[i] = mesh.GetAnimationState(animNames[i]);
            }
        }


        public override void SetTopAnimation(int animId)
        {

        }
        public override void SetBaseAnimation(int animId)
        {

        }
        public override void Update(float timeSinceLastFrame)
        {
            base.Update(timeSinceLastFrame);
        }
    }

    public class Character : MoveableObject, IAttackable, INodifyStateChanged
    {
        private StateMachine<Character> stateMachine;
        private CharacterController controller;
        private CharacterAIController ai;
        private CharacterInfo info;
        private Item currentWieldItem;
        public event Action<int,int> OnStateChanged;
        public Item CurrentWieldItem
        {
            get { return currentWieldItem; }
            set { currentWieldItem = value; }
        }
        public Side SideInfo
        {
            get;
            set;
        }
        public CharacterInfo Info
        {
            get { return info; }
        }

        public Character(string characterUniqueId, Keyboard keyboard, Mouse mouse)
        {
            uniqueId = Utilities.Helper.GetStringHash(characterUniqueId);
            GameManager.Instance.GameHashMap.Add(characterUniqueId, uniqueId);

            keyboard.KeyPressed += keyboard_KeyPressed;
            keyboard.KeyReleased += keyboard_KeyReleased;
            mouse.MouseMoved += mouse_MouseMoved;
            mouse.MousePressed += mouse_MousePressed;
            mouse.MouseReleased += mouse_MouseReleased;
        }

        public bool Setup(Camera cam, Mods.XML.ModCharacterDfnXML characterDfn, bool controlled = false)
        {
            try
            {
                stateMachine = new StateMachine<Character>(this);
                stateMachine.ChangeState(CharacterIdleState.Instance);
                info = new CharacterInfo(characterDfn.Name, characterDfn.MeshName,cam);
                if (controlled)
                    controller = new CharacterController(info.Name, info.MeshName, cam, true);
                else
                    ai = new CharacterAIController(this);

                return true;
            }
            catch(Exception ex)
            {
                GameManager.Instance.mLog.LogMessage(ex.ToString(), LogMessage.LogType.Error);
                return false;
            }
        }

        public override void Update(float deltaTime)
        {
            stateMachine.Update();

            if (controller != null)
            {
                controller.ControllerUpdate(deltaTime);
            }
        }

        public void Pratol()
        {
            stateMachine.ChangeState(CharacterSpotEnemyState.Instance);
        }

        public void Move(Mogre.Vector3 destPosition)
        {
            ai.Move(destPosition);
        }

        public void UnderAttack(Character attacker)
        {
            stateMachine.ChangeState(CharacterAttackState.Instance);
        }

        public void Flee()
        {
            stateMachine.ChangeState(CharacterFleeState.Instance);
        }

        public void Defence()
        {

        }

        public void UnderControl()
        {
            if (controller == null)
            {
                controller = new CharacterController(info.Name, info.MeshName, info.Cam);
            }
        }

        public void LoseControl()
        {
            controller = null;
        }

        public void StateChanged(int oldState, int newState)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(oldState, newState);
            }
        }

        #region event handler
        bool mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            if (controller != null)
            {
            }
            return true;
        }

        bool mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            if (controller != null)
            {
                controller.InjectMousePressed(arg, id);
            }
            return true;
        }

        bool mouse_MouseMoved(MouseEvent arg)
        {
            if (controller != null)
            {
                controller.InjectMouseMoved(arg);
            }
            return true;
        }

        bool keyboard_KeyReleased(KeyEvent arg)
        {
            if (controller != null)
            {
                controller.InjectKeyReleased(arg);
            }
            return true;
        }

        bool keyboard_KeyPressed(KeyEvent arg)
        {
            if (controller != null)
            {
                controller.InjectKeyPressed(arg);
            }
            return true;
        }
        #endregion

        public void Attack(RPGObject target)
        {
            stateMachine.ChangeState(CharacterAttackState.Instance, target);
            ai.Attack((Character)target);
        }

        public void ChangeState(State<Character> state,params object[] extraParams)
        {
            stateMachine.ChangeState(state, extraParams);
        }
    }
}
