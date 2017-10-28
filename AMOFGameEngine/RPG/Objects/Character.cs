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
    public class CharacterInfo
    {
        private string name;
        private string mesh;
        private LevelInfo level;
        private int hitpoint;
        private InventoryInfo inventory;
        private Camera cam;

        public int HitPoint
        {
            get { return hitpoint; }
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
        public string Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }
        public Camera Cam
        {
            get { return cam; }
            set { cam = value; }
        }

        public CharacterInfo()
        {
            level = new LevelInfo();
            hitpoint = 100;
            inventory = new InventoryInfo();
        }
    }

    public class Character : MoveableObject, IAttackable, INodifyStateChanged
    {
        enum AIState
        {
            IDLE,
            Alarmed,
            Pratol,
            Attack,
            Die,
        }

        private CharacterController controller;
        private CharacterInfo charaDesc;
        private bool alive;
        private bool wielded;
        private bool wieldedR;
        private Item currentWieldItem;
        private Item currentWieldItemR;
        private AIState currentState;
        private Mogre.Vector3 charaOrientation;
        private float angle;
        private int teamId;

        public event Action<int,int> OnStateChanged;
        public Mogre.Vector3 Direction
        {
            get { return charaOrientation ; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
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
        public int Team
        {
            get
            {
                return teamId;
            }
        }

        public Character(string characterUniqueId, Keyboard keyboard, Mouse mouse)
        {
            uniqueId = Utilities.Helper.GetStringHash(characterUniqueId);
            GameManager.Instance.GameHashMap.Add(characterUniqueId, uniqueId);

            currentWieldItem = null;
            currentWieldItemR = null;
            wielded = false;
            wieldedR = false;
            alive = true;
            currentState = AIState.IDLE;
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
                charaDesc = new CharacterInfo();
                charaDesc.Name = characterDfn.Name;
                charaDesc.Mesh = characterDfn.MeshName;
                if (controlled)
                    controller = new CharacterController(charaDesc.Name, charaDesc.Mesh, cam);
                else
                    controller = null;

                return true;
            }
            catch(Exception ex)
            {
                GameManager.Instance.mLog.LogMessage("Engine Error: " + ex.ToString());
                return false;
            }
        }

        public override void Update(float deltaTime)
        {
            if (controller != null)
            {
                controller.ControllerUpdate(deltaTime);
            }
        }

        public void Pratol()
        {
            if (currentState != AIState.Pratol)
            {
                int currentOldState = (int)currentState;
                currentState = AIState.Pratol;
                StateChanged(currentOldState, (int)currentState);
            }
        }

        public void Attack(RPGObject target)
        {
            if (currentState != AIState.Attack)
            {
                int currentOldState = (int)currentState;
                currentState = AIState.Attack;
                StateChanged(currentOldState, (int)currentState);
            }
        }

        public void UnderAttack(Character attacker)
        {
        }

        public void Defence()
        {

        }

        public void UnderControl()
        {
            if (controller == null)
            {
                controller = new CharacterController(charaDesc.Name, charaDesc.Mesh, charaDesc.Cam);
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
    }
}
