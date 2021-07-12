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

		public event Action<MouseEvent> MouseHasMoved;
		public event Action<KeyCollection> SomeKeyPressd;

		public InputKeyMouseManager()
		{
			EngineManager.Instance.mouse.MouseMoved += new MouseListener.MouseMovedHandler(MouseMoved);
			EngineManager.Instance.mouse.MousePressed += new MouseListener.MousePressedHandler(MousePressed);
			EngineManager.Instance.mouse.MouseReleased += new MouseListener.MouseReleasedHandler(MouseReleased);

			EngineManager.Instance.keyboard.KeyPressed += new KeyListener.KeyPressedHandler(KeyPressed);
			EngineManager.Instance.keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(KeyReleased);
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
			MouseHasMoved?.Invoke(arg);
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
				KeyCollection keyCollection = new KeyCollection();
				if (combineKey)
				{
					keyCollection.keyCodes.Add(COMBINED_KEY_CODE);
					keyCollection.keyCodes.Add(keyCode);
					SomeKeyPressd?.Invoke(keyCollection);
					combineKey = false;
				}
				else
				{
					keyCollection.keyCodes.Add(keyCode);
					SomeKeyPressd?.Invoke(keyCollection);
					combineKey = false;
				}
			}
		}
	}
}
