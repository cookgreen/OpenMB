using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOIS;

namespace OpenMB.Game.ControlObjType
{
	public class ControlObjectTypeSceneProp : IControlObjectType
	{
		private SceneProp propInstance;
		public ControlObjectTypeSceneProp(SceneProp propInstance)
		{
			this.propInstance = propInstance;
		}
		public bool KeyPressed(KeyEvent arg)
		{
			return true;
		}

		public bool KeyReleased(KeyEvent arg)
		{
			return true;
		}

		public bool MouseClick(MouseEvent arg, MouseButtonID id)
		{
			return true;
		}

		public bool MouseMoved(MouseEvent arg)
		{
			return true;
		}

		public bool MouseReleased(MouseEvent arg, MouseButtonID id)
		{
			return true;
		}

		public void Update(float timeSinceLastFrame)
		{
		}
	}
}
