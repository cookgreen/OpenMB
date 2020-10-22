using Mogre;
using OpenMB.Interfaces;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.ItemTypes
{
	public interface IItemType : IRenderPreview
	{
		string Name { get; }

		string SpawnAttachBoneName { get; }

		void Use(params object[] param);

		Item Item { get; set; }

		ModData ModData { get; set; }

		void SpawnIntoWorld();

		void SpawnIntoCharacter(GameWorld world, Character character);
	}
}
