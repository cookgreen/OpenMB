using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Core
{
	public class InputKeyMouseManager
	{
		private const KeyCode COMBINED_KEY_CODE = KeyCode.KC_LCONTROL;
		private bool combineKey;

		public event Action<KeyCode> SomeKeyPressd;

		public InputKeyMouseManager()
		{
			GameManager.Instance.mouse.MouseMoved += new MouseListener.MouseMovedHandler(MouseMoved);
			GameManager.Instance.mouse.MousePressed += new MouseListener.MousePressedHandler(MousePressed);
			GameManager.Instance.mouse.MouseReleased += new MouseListener.MouseReleasedHandler(MouseReleased);

			GameManager.Instance.keyboard.KeyPressed += new KeyListener.KeyPressedHandler(KeyPressed);
			GameManager.Instance.keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(KeyReleased);
		}

		private bool KeyReleased(KeyEvent arg)
		{
			return true;
		}

		private bool MousePressed(MouseEvent arg, MouseButtonID id)
		{
			return true;
		}

		private bool MouseMoved(MouseEvent arg)
		{
			return true;
		}

		private bool MouseReleased(MouseEvent arg, MouseButtonID id)
		{
			return true;
		}

		private bool KeyPressed(KeyEvent arg)
		{
			PressSomeKey(arg.key);
			return true;
		}

		private void PressSomeKey(KeyCode keyCode)
		{
			if (keyCode == COMBINED_KEY_CODE)
			{
				if (combineKey)
				{
					return;
				}
				combineKey = true;
			}
			else
			{
				if (combineKey)
				{
					SomeKeyPressd?.Invoke(COMBINED_KEY_CODE | keyCode);
					combineKey = false;
				}
				else
				{
					SomeKeyPressd?.Invoke(keyCode);
					combineKey = false;
				}
			}
		}
	}
}
