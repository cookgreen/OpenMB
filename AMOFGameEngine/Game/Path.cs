using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Game
{
    public class Path
    {
        private List<Vector3> wayPoints;
        public Path()
        {
            wayPoints = new List<Vector3>();
        }

        public void AddNewWayPoint(Vector3 newWayPoint)
        {
            wayPoints.Add(newWayPoint);
        }

        public void RemoveWayPoint(Vector3 wayPoint)
        {
            wayPoints.Remove(wayPoint);
        }

        public List<Vector3> FollowPath()
        {
            return wayPoints;
        }
    }
}
