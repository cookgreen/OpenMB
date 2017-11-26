using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using AMOFGameEngine.RPG.Objects;

namespace AMOFGameEngine.RPG.Controller
{
    public class CharacterAIController : AIControllerBase
    {
        Character controller;
        public CharacterAIController(Character character)
        {
            controller = character;
        }

        public void Move(Vector3 destPosition)
        {
            ///Animation
            //controller.Info.SetAnimation(0);

            ///Turn around
            Mogre.Vector3 vector = destPosition - controller.Info.Node.Position;
            Mogre.Vector3 faceTo = controller.Info.Node.Orientation * controller.Info.Node.Position;

            float angleCos = faceTo.Normalise() * vector.Normalise();
            Radian r = Mogre.Math.ACos(angleCos);
            controller.Info.Node.Rotate(Mogre.Vector3.UNIT_Y, r);

            ///
        }

        public void Seek(Vector3 targetPosition, Quaternion targetDirection)
        {

        }

        public void Flee()
        {

        }

        public void Attack(Character target)
        {
            ///Run if not in the attack range
            Move(target.Position);

            ///In the range, will attack
            //Attack Animation
            //controller.Info.SetAnimation(1);
        }

        public void Update(float timeSinceLastFrame)
        {
        }
    }
}
