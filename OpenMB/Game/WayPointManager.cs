using Mogre;
using org.critterai.nav;
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

        public List<Waypoint> GenerateWaypointsBetweenTwoPoints(Navmesh navmesh, Vector3 startPos, Vector3 endPos)
        {
            List<Waypoint> waypoints = new List<Waypoint>();

            NavmeshQuery query;
            var status = NavmeshQuery.Create(navmesh, 1024, out query);
            if (!NavUtil.Failed(status))
            {
                org.critterai.Vector3 navStartPointVect;
                org.critterai.Vector3 navEndPointVect;
                var navStartPointStatus = query.GetNearestPoint(1, new org.critterai.Vector3(startPos.x, startPos.y, startPos.z), out navStartPointVect);
                var navEndPointStatus = query.GetNearestPoint(1, new org.critterai.Vector3(startPos.x, startPos.y, startPos.z), out navEndPointVect);
                if (navStartPointStatus == NavStatus.Sucess && navEndPointStatus == NavStatus.Sucess)
                {
                    NavmeshPoint navStartPoint = new NavmeshPoint(1, new org.critterai.Vector3(startPos.x, startPos.y, startPos.z));
                    NavmeshPoint navEndPoint = new NavmeshPoint(1, new org.critterai.Vector3(endPos.x, endPos.y, endPos.z));

                    uint[] path = new uint[1024];
                    int pathCount;
                    status = query.FindPath(navStartPoint, navEndPoint, new NavmeshQueryFilter(), path, out pathCount);
                    if (!NavUtil.Failed(status))
                    {
                        const int MaxStraightPath = 4; 
                        int wpCount;
                        org.critterai.Vector3[] wpPoints = new org.critterai.Vector3[MaxStraightPath];
                        uint[] wpPath = new uint[MaxStraightPath];

                        WaypointFlag[] wpFlags = new WaypointFlag[MaxStraightPath];
                        status = query.GetStraightPath(navStartPoint.point ,navEndPoint.point
                                                       ,path ,0 ,pathCount ,wpPoints ,wpFlags ,wpPath
                                                       ,out wpCount);
                        if (!NavUtil.Failed(status) && wpCount > 0)
                        {
                            foreach (var wp in wpPoints)
                            {
                                Mogre.Vector3 wayPointPos = new Vector3(wp.x, wp.y, wp.z);
                                waypoints.Add(new Waypoint(wayPointPos, new Vector3()));
                            }
                        }
                    }
                }
            }

            return waypoints;
        }
    }
}
