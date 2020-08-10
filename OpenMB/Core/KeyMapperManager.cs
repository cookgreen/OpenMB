using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public enum GameKeyCode
	{
		INVALID,
		FullScreen,
		Screenshot,
		ShowOgreLogo,
		CHA_Attack,
		CHA_Defense,
		CHA_Kick,
		CHA_MoveForward,
		CHA_MoveBackward,
		CHA_MoveLeft,
		CHA_MoveRight,
		CHA_TurnLeft,
		CHA_TurnRight,
		CAMERA_MoveForward,
		CAMERA_MoveBackward,
		CAMERA_MoveLeft,
		CAMERA_MoveRight,
		CAMERA_TurnLeft,
		CAMERA_TurnRight,
	}

	public class KeyCollection
	{
		public List<KeyCode> keyCodes { get; set; }

		public KeyCollection()
		{
			keyCodes = new List<KeyCode>();
		}

		public KeyCode? ToKeyCode()
		{
			if (keyCodes.Count > 0)
			{
				if (keyCodes.Count == 1)
				{
					return keyCodes[0];
				}
				else
				{
					KeyCode? k = null;
					foreach (var kc in keyCodes)
					{
						if (k == null)
						{
							k = kc;
						}
						else
						{
							k |= kc;
						}
					}
					return k.Value;
				}
			}
			else
			{
				return null;
			}
		}

		public static bool operator ==(KeyCollection left, KeyCollection right)
		{
			if ((object)left == null || (object)right == null)
			{
				return false;
			}
			else
			{
				if (left.keyCodes.Count == right.keyCodes.Count)
				{
					for (int i = 0; i < left.keyCodes.Count; i++)
					{
						var k1 = left.keyCodes[i];
						var k2 = right.keyCodes[i];
						if (k1 != k2)
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public static bool operator !=(KeyCollection left, KeyCollection right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return this == (KeyCollection)obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class KeyMapperManager
	{
		private Dictionary<GameKeyCode, KeyCollection> gameKeyMapper;

		private static KeyMapperManager instance;
		public static KeyMapperManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new KeyMapperManager();
				}
				return instance;
			}
		}

		public KeyMapperManager()
		{
			gameKeyMapper = new Dictionary<GameKeyCode, KeyCollection>();
		}

		public void AddKeyMapper(GameKeyCode gkCode, KeyCollection kc)
		{
			if (gameKeyMapper.ContainsValue(kc))
			{
				GameKeyCode neededDelKey = GameKeyCode.INVALID;
				foreach (var kpl in gameKeyMapper)
				{
					if (kpl.Value == kc)
					{
						neededDelKey = kpl.Key;
						break;
					}
				}
				gameKeyMapper.Remove(neededDelKey);
			}

			if (gameKeyMapper.ContainsKey(gkCode))
			{
				gameKeyMapper[gkCode] = kc;
			}
			else
			{
				gameKeyMapper.Add(gkCode, kc);
			}
		}

		public void RemoveKeyMapper(GameKeyCode gkCode)
		{
			gameKeyMapper.Remove(gkCode);
		}

		public KeyCollection GetKeyCollection(GameKeyCode gkCode)
		{
			if (gameKeyMapper.ContainsKey(gkCode))
			{
				var kc = gameKeyMapper[gkCode];
				return kc;
			}
			else
			{
				return null;
			}
		}
	}
}
