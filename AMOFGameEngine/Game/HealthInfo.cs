using AMOFGameEngine.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public class HealthInfo : IUpdate
    {
        private GameObject owner;
        private int effecterId;
        private int hp;
        private bool displayMessage;
        public int HP
        {
            get
            {
                return hp;
            }
        }
        public bool DisplayMessage
        {
            get
            {
                return displayMessage;
            }

            set
            {
                displayMessage = value;
            }
        }

        public HealthInfo(GameObject owner, int initHP = 100, bool displayMessage = true)
        {
            this.owner = owner;
            hp = initHP;
        }

        public void EffectHealth(int effecterId, int point)
        {
            this.effecterId = effecterId;
            hp += point;
        }

        public virtual void Update(float deltaTime)
        {
            if (hp < 0)
            {
                owner.World.RemoveGameObject(owner);
                if(displayMessage)
                {
                    OutputManager.Instance.DisplayMessage(string.Format("Object with id {0} was killed by Object with id {1}",
                        effecterId, owner.ID));
                }
            }
        }
    }
}
