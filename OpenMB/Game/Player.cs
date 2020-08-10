using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
	/// <summary>
	/// Controllable Entity
	/// </summary>
	public class Player
	{
		public string Name { get; set; }
		public string ControllingObjID { get; set; }
		public IControlObjectType ControllingObjType { get; set; }

		public Player(string name, string controllingObjID, IControlObjectType controllingObjType)
		{
			Name = name;
			ControllingObjID = controllingObjID;
			ControllingObjType = controllingObjType;
			GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
			GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
			GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
			GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
			GameManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;
		}

		private bool Keyboard_KeyReleased(MOIS.KeyEvent arg)
		{
			return ControllingObjType.KeyReleased(arg);
		}

		private bool Keyboard_KeyPressed(MOIS.KeyEvent arg)
		{
			return ControllingObjType.KeyPressed(arg);
		}

		private bool Mouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			return ControllingObjType.MouseReleased(arg, id);
		}

		private bool Mouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			return ControllingObjType.MouseClick(arg, id);
		}

		private bool Mouse_MouseMoved(MOIS.MouseEvent arg)
		{
			return ControllingObjType.MouseMoved(arg);
		}

		public void Update(float timeSinceLastFrame)
		{
			ControllingObjType.Update(timeSinceLastFrame);
		}
	}
}
