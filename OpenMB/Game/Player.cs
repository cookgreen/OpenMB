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

		public Player(string name, string controllingObjID)
		{
			Name = name;
			ControllingObjID = controllingObjID;
			EngineManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
			EngineManager.Instance.mouse.MousePressed += Mouse_MousePressed;
			EngineManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
			EngineManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
			EngineManager.Instance.keyboard.KeyReleased += Keyboard_KeyReleased;
		}

		private bool Keyboard_KeyReleased(MOIS.KeyEvent arg)
		{
			return true;
		}

		private bool Keyboard_KeyPressed(MOIS.KeyEvent arg)
		{
			return true;
		}

		private bool Mouse_MouseReleased(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			return true;
		}

		private bool Mouse_MousePressed(MOIS.MouseEvent arg, MOIS.MouseButtonID id)
		{
			return true;
		}

		private bool Mouse_MouseMoved(MOIS.MouseEvent arg)
		{
			return true;
		}

		public void Update(float timeSinceLastFrame)
		{
		}
	}
}
