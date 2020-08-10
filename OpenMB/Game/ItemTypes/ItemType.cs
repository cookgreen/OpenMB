using Mogre;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game.ItemTypes
{
	public class ItemType : IItemType
	{
		public virtual string Name { get { return "PlaceHolder"; } }

		public virtual string SpawnAttachBoneName { get { return "Spin"; } }

		public ModData ModData { get; set; }

		public Item Item { get; set; }

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
	}
}
