using Mogre;
using Mogre.PhysX;

namespace AMOFGameEngine.Game
{
    public class OneHandWeapon : Item
    {
        public OneHandWeapon(int id, string desc, string meshName, GameWorld world) : 
            base(id, desc,meshName, ItemType.IT_ONE_HAND_WEAPON, ItemHaveAttachOption.IHAO_BACK_FROM_LEFT_TO_RIGHT, ItemUseAttachOption.IAO_LEFT_HAND, world)
        {
        }
    }
}