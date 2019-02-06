using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// Single Character's brain
    /// </summary>
    public class DecisionSystem
    {
        private Character owner;
        private CharacterState ownerState;
        private List<Character> enemies;
        public DecisionSystem(Character owner)
        {
            this.owner = owner;
            enemies = new List<Character>();
        }

        public void Update(float deltaTime)
        {
            //FSM
            switch (ownerState)
            {
                case CharacterState.Idle://Do nothing
                    break;
                case CharacterState.Seek://Search the enemy
                    break;
                case CharacterState.Attack://Destroy the enemy
                    break;
                case CharacterState.Flee://Retreat!
                    break;
            }
        }

        public void Active()
        {
            ownerState = CharacterState.Seek;
        }

        public void Deactive()
        {
            ownerState = CharacterState.Idle;
        }

        private List<Character> FindAllEneimes()
        {
            return null;
        }

        private Character FindClostestEnemy()
        {
            return null;
        }

        private void FindAllies()
        {

        }

        private void FindClostestAllies()
        {

        }
    }
}
