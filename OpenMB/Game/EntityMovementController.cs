using Mogre;
using org.critterai.nav;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
    public class EntityMovement
    {
        private Entity movableEntity;
        private Vector3 destPos;
        private List<Vector3> waypoints;

        public event Action MoveFinished;

        public EntityMovement(Entity movableEntity, Vector3 destPos, List<Vector3> waypoints)
        {
            this.movableEntity = movableEntity;
            this.destPos = destPos;
            this.waypoints = waypoints;
        }

        public void Process()
        {
            if (waypoints.Count == 0)
            {
                MoveFinished?.Invoke();
            }
            else
            {
                var waypoint = waypoints.First();

                movableEntity.ParentSceneNode.Position = waypoint;

                waypoints.RemoveAt(0);
            }
        }
    }
    public class EntityMovementController : IUpdate
    {
        private Entity movableEntity;
        private Navmesh navmesh;
        private Queue<EntityMovement> entityMovementActs;

        public EntityMovementController(Entity movableEntity, Navmesh navmesh)
        {
            this.movableEntity = movableEntity;
            this.navmesh = navmesh;
            entityMovementActs = new Queue<EntityMovement>();
        }

        public void MoveTo(Vector3 destPos)
        {
            //Generate a way point list 
            var waypoints = WaypointManager.Instance.GenerateWaypointsBetweenTwoPoints(navmesh, movableEntity.ParentSceneNode.Position, destPos);
            EntityMovement entityMovementAct = new EntityMovement(movableEntity, destPos, waypoints.Select(o=>o.Position).ToList());
            entityMovementAct.MoveFinished += EntityMovementAct_MoveFinished;
            entityMovementActs.Enqueue(entityMovementAct);
        }

        private void EntityMovementAct_MoveFinished()
        {
            entityMovementActs.Dequeue();
        }

        public void Update(float deltaTime)
        {
            entityMovementActs.Peek().Process();
        }
    }
}
