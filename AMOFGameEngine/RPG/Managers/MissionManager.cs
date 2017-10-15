using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMOFGameEngine.RPG.Object;
using AMOFGameEngine.RPG.Data;

namespace AMOFGameEngine.RPG.Managers
{
    /// <summary>
    /// Character's Mission
    /// </summary>
    public class MissionManager
    {
        private Mission currentMission;
        private List<Mission> allAcceptedMissions;

        public MissionManager()
        {
            allAcceptedMissions = new List<Mission>();
        }

        public void AcceptMission(Mission mission)
        {
            allAcceptedMissions.Add(mission);
        }

        /// <summary>
        /// Start a Mission
        /// </summary>
        public void StartMission()
        {

        }
        /// <summary>
        /// End a Mission
        /// </summary>
        public void EndMission()
        {

        }
        /// <summary>
        /// Mission Successed
        /// </summary>
        public void MissionSuccessed()
        {

        }
        /// <summary>
        /// Mission Failed
        /// </summary>
        public void MissionFailed()
        {

        }

        public void Update()
        {

        }
    }
}
