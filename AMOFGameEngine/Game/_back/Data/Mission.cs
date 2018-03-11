using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Data
{
    /// <summary>
    /// Mission Information
    /// </summary>
    public abstract class Mission
    {
        /// <summary>
        /// Misson Name
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// Mission Description
        /// </summary>
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// Next Mission if this Mission has finished
        /// </summary>
        private Mission nextMission;

        public Mission NextMission
        {
            get { return nextMission; }
            set { nextMission = value; }
        }
        /// <summary>
        /// Current mission state
        /// </summary>
        MissonState state;

        public MissonState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// Who gives this mission(giver id)
        /// </summary>
        private int giver;

        public int Giver
        {
            get { return giver; }
            set { giver = value; }
        }

        /// <summary>
        /// Who accept this mission(receiver id), normally the player
        /// </summary>
        private int receiver;

        public int Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }

        /// <summary>
        /// Execute when a mission has successed
        /// </summary>
        public virtual void MissionSuccessed()
        {

        }
        /// <summary>
        /// Execute when a mission has failed
        /// </summary>
        public virtual void MissionFailed()
        {

        }
        /// <summary>
        /// Update mission state
        /// </summary>
        public virtual void Update()
        {

        }
    }
}
