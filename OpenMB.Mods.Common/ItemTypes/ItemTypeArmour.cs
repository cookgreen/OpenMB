using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using OpenMB.Game;
using OpenMB.Game.ItemTypes;

namespace OpenMB.Mods.Common.ItemTypes
{
	//This kind of item can reduce the damage receive
	public class ItemTypeArmour : PlaceholderItemType
	{

		public int Armour { get; set; }

		public override string Name
		{
			get
			{
				return "Armour";
			}
		}

		public override string SpawnAttachBoneName
		{
			get
			{
				return "Spin";
			}
		}

		public override void Use(params object[] param)
		{
			GameWorld world = param[0] as GameWorld;
			int userID = int.Parse(param[1].ToString());
			int damage = int.Parse(param[2].ToString());

			Character character = null;
			if (userID == -1)
			{

			}
			else
			{
				character = world.GetAgentById(userID);
			}

		}

		public override MaterialPtr RenderPreview(Entity itemEnt)
		{
			return null;
		}
	}
}
