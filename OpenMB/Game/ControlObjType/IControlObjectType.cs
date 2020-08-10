using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
	public interface IControlObjectType
	{
		bool MouseClick(MouseEvent arg, MouseButtonID id);
		bool MouseReleased(MouseEvent arg, MouseButtonID id);
		bool MouseMoved(MouseEvent arg);
		bool KeyPressed(KeyEvent arg);
		bool KeyReleased(KeyEvent arg);
		void Update(float timeSinceLastFrame);
	}
}
