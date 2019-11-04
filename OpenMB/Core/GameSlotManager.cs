using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public class GameSlotManager
	{
		private Dictionary<string, Dictionary<string, string>> idSlots;

		private static GameSlotManager instance;
		public static GameSlotManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameSlotManager();
				}
				return instance;
			}
		}

		public GameSlotManager()
		{
			idSlots = new Dictionary<string, Dictionary<string, string>>();
		}

		public void SetSlot(string id, string slotID, string slotValue)
		{
			if (idSlots.ContainsKey(id))
			{
				if (idSlots[id].ContainsKey(slotID))
				{
					idSlots[id][slotID] = slotValue;
				}
				else
				{
					idSlots[id].Add(slotID, slotValue);
				}
			}
			else
			{
				idSlots.Add(id, new Dictionary<string, string>());
				idSlots[id].Add(slotID, slotValue);
			}
		}

		public string GetSlot(string id, string slotID)
		{
			if (idSlots.ContainsKey(id) && idSlots[id].ContainsKey(slotID))
			{
				return idSlots[id][slotID];
			}
			else
			{
				return null;
			}
		}

		public bool SlotEqual(string id, string slotID, string value)
		{
			if (idSlots.ContainsKey(id) && idSlots[id].ContainsKey(slotID))
			{
				return idSlots[id][slotID] == value;
			}
			else
			{
				return false;
			}
		}
	}
}
