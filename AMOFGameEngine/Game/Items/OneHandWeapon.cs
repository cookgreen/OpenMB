using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class OneHandWeapon : Item
    {
        public OneHandWeapon(Camera cam, Scene physicsScene, int id, int ownerID = -1) : base(cam, physicsScene, id, ownerID)
        {
        }
    }
}