using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public class GameTeam
	{
		private List<GameObject> objects;
		public int ID { get; set; }

		public string SideID { get; set; }

		public List<GameObject> Objects { get { return objects; } }

		public GameTeam(int id, string sideID)
		{
			objects = new List<GameObject>();
			ID = id;
			SideID = sideID;
		}
	}
}
