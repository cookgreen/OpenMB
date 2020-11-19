using Mogre;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.ItemTypes
{
	public class PlaceholderItemType : IItemType
	{
		public virtual string Name { get { return "Placeholder"; } }

		public virtual string SpawnAttachBoneName { get { return "Spin"; } }

		public ModData ModData { get; set; }

		public Item Item { get; set; }

        public IItemController ItemController { get; }

        public virtual MaterialPtr RenderPreview(Entity itemEnt)
		{
			return null;
		}

		public virtual void SpawnIntoWorld()
		{

		}

		public virtual void SpawnIntoCharacter(GameWorld world, Character character)
		{

		}

		public virtual void Use(params object[] param)
		{
		}

        public void Equip(Character character)
        {
        }

        public void Spawn()
        {
        }
    }
}
