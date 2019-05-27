using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class WeaponSystem
    {
        private Item currentWeapon;
        private List<Item> weaponPool;
        private Character user;

        public Item CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
        }

        public List<Item> WeaponPool
        {
            get
            {
                return weaponPool;
            }
        }

        public Character User
        {
            get
            {
                return user;
            }
        }

        public WeaponSystem(Character user, Item currentWeapon)
        {
            this.user = user;
            this.currentWeapon = currentWeapon;
            weaponPool = new List<Item>();
            weaponPool.Add(currentWeapon);
        }

        public void EquipNewWeapon(Item newWeapon)
        {
            weaponPool.Add(newWeapon);
        }

        public void DetachWeapon(Item weapon)
        {
            weaponPool.Remove(weapon);
        }

        public Item GetNextWeaponInCircle()
        {
            int index = weaponPool.IndexOf(currentWeapon);
            if (index == weaponPool.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            return weaponPool[index];
        }

        public bool CheckInRange(Character enemy)
        {
            return (enemy.Position - user.Position).Length <= currentWeapon.Range;
        }

        public void Fire()
        {
        }

        public void Update(float timeSinceLastFrame)
        {
        }
    }
}
