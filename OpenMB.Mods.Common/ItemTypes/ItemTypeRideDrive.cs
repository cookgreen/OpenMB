using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mogre;
using OpenMB.Game.ItemTypes;

namespace OpenMB.Mods.Common.ItemTypes
{
	public class ItemTypeRideDrive : PlaceholderItemType
	{
		protected List<string> parameters;

		public override string Name
		{
			get { return "RideDrive"; }
		}

		public override string SpawnAttachBoneName
		{
			get
			{
				return "Spin";
			}
		}

		public ItemTypeRideDrive()
		{
			parameters = new List<string>();
			parameters.Add("Speed");
			parameters.Add("TurnSpeed");
		}

		public override void Use(params object[] param)
		{
		}

		public override MaterialPtr RenderPreview(Entity ent)
		{
			return null;
		}
	}
}
