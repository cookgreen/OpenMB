using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mogre;
using MOIS;
using AMOFGameEngine.Sound;

namespace AMOFGameEngine.Game
{
    /// <summary>
    /// 游戏中具体的角色，包含血量，技能
    /// </summary>
    public class Character : GameObject
    {
        //唯一标识
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        //名字
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //控制器
        private CharacterController controller;

        public CharacterController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        //血量
        private int hitpoint;

        public int Hitpoint
        {
            get { return hitpoint; }
            set { hitpoint = value; }
        }

        //武器
        private Item[] weapons;

        public Item[] Weapons
        {
            get { return weapons; }
            set { weapons = value; }
        }

        //穿戴
        private Item[] clothes;

        public Item[] Clothes
        {
            get { return clothes; }
            set { clothes = value; }
        }

        //背包
        private Inventory backpack;

        public Inventory Backpack
        {
            get { return backpack; }
            set { backpack = value; }
        }

        public Character(Camera cam, int id)
        {
            Id = id;//唯一标识
            Name = string.Empty;//默认名字
            Hitpoint = 100;//默认100血
            Weapons = new Item[4];//四种武器
            Clothes = new Item[4];//四件穿戴,0-帽盔,1-衣服,2-鞋子,3-手套
            Backpack = new Inventory(21, this);//21个单位的物品槽
            controller = new CharacterController(cam, id.ToString(), "Sinbad.mesh");//初始化控制器
        }

        public void WearHat(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_HEAD_ARMOUR)
            {
                Clothes[0] = item;
                controller.AttachEntityToChara("head", item.ItemEnt);
            }
        }

        public void WearClothes(Item item)
        {
            if (item != null && item.ItemType == ItemType.IT_BODY_ARMOUR)
            {
                Clothes[1] = item;
                controller.AttachEntityToChara("back", item.ItemEnt);
            }
        }

        public void AddItemToBackpack(Item item)
        {
            Backpack.AddItemToInventory(item);
        }
    }
}
