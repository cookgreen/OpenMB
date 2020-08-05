using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	public class SimpleStaticTextBackgroundWidget : SimpleStaticTextWidget
	{
		public override event Action<object> OnClick;

		public SimpleStaticTextBackgroundWidget(string name, string caption, float width, bool specificColor, ColourValue color, float fontSize = 100)
			: base(name, caption, width, specificColor, color, fontSize)
		{
			element.MaterialName = "Engine/Background/Normal";
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			if (IsCursorOver(cursorPos))
			{
				SetState(ButtonState.BS_DOWN);
			}
		}

		public override void CursorReleased(Vector2 cursorPos)
		{
			if (state == ButtonState.BS_DOWN)
			{
				SetState(ButtonState.BS_UP);
				OnClick?.Invoke(null);
			}
		}

		private void SetState(ButtonState bs)
		{
			state = bs;
		}
	}
}
