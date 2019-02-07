using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class OneHandWeapon : Item
    {
        public OneHandWeapon(string desc, string meshName, Scene physicsScene, Camera cam) : 
            base(desc,meshName, ItemType.IT_ONE_HAND_WEAPON, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, physicsScene, cam)
        {
        }
    }
}