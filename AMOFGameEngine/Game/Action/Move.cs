using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game.Action
{
    public class Move : Activity
    {
        private Character agent;
        private Queue<Vector3> path;
        private float distance;
        private Vector3 direction;
        private Vector3 destination;

        public Move(Character agent, Vector3 destination)
        {
            this.agent = agent;
            this.destination = destination;
            path = new Queue<Vector3>();

            float a = (destination.z - agent.Position.z) / (destination.x - agent.Position.x);
            float b = destination.z - a * destination.x;

            for (int i = 0; i < Mogre.Math.Abs(destination.x - agent.Position.x) / 5; i++)
            {
                path.Enqueue(new Mogre.Vector3(destination.x + i, 0, a * (destination.x + i) + b));
            }
        }


        private void WalkState(float deltaTime)
        {
            if (direction == Mogre.Vector3.ZERO)
            {
                if (nextLocation())
                {
                    agent.SetAnimation("ANIM_IDLE_TOP", "ANIM_IDLE_BASE", true);
                    State = ActionState.Done;
                }
            }
            else
            {
                float move = agent.MoveInfo.Speed * deltaTime;
                distance -= move;
                if (distance <= 0.0f)
                {
                    agent.SetBodyPos(destination.x, destination.y, destination.z);
                    direction = Vector3.ZERO;
                    if (!nextLocation())
                    {
                        agent.SetAnimation("ANIM_RUN_TOP", "ANIM_RUN_BASE", true);
                    }
                    else
                    {
                        Vector3 src = agent.GetBodyOrientation() * Vector3.UNIT_Z;
                        if ((1.0f + src.DotProduct(direction)) < 0.0001f)
                        {
                            agent.YawBody(new Degree(180));
                        }
                        else
                        {
                            Quaternion quat = src.GetRotationTo(direction);
                            agent.RotateBody(quat);
                        }
                    }
                }
                else
                {
                    agent.TranslateBody(direction * move);
                }
            }
        }

        private bool nextLocation()
        {
            if (path != null)
            {
                if (path.Count == 0)
                    return false;
                destination = path.Dequeue();
                direction = destination - agent.Position;
                distance = direction.Normalise();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(float deltaTime)
        {
            if (State == ActionState.Done || State == ActionState.Cancel)
            {
                return;
            }
            WalkState(deltaTime);
        }
    }
}
