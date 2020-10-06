using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class Waypoint
    {
        private Vector3 position;
        private Vector3 direction;

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
        }

        public Waypoint(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }
    public class WaypointManager
    {
        private static WaypointManager instance;
        public static WaypointManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WaypointManager();
                }
                return instance;
            }
        }

        public List<Waypoint> GenerateWaypointsBetweenTwoPoints(Vector3 startPos, Vector3 endPos)
        {
            List<Waypoint> waypoints = new List<Waypoint>();



            return waypoints;
        }
    }
}
