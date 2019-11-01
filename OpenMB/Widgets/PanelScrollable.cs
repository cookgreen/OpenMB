using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Widgets
{
	public enum ScrollOritentation
	{
		Up,
		Down,
	}

	public class PanelScrollable : Panel, IScrollable
	{
		private List<Widget> visualWidgets;
		private BorderPanelOverlayElement scroll;
		private OverlayElement drag;
		private float initDragTop;

		public event Action Scrolled;

		public PanelScrollable(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1) : base(name, width, height, left, top, row, col)
		{
			visualWidgets = new List<Widget>();
			string scrollName = name + "_Scroll";
			scroll = OverlayManager.Singleton.CreateOverlayElementFromTemplate("ScrollComponet", "BorderPanel", scrollName) as BorderPanelOverlayElement;
			drag = scroll.GetChild(scrollName + "/Drag") as OverlayElement;

			for (int i = 0; i < Rows.Count; i++)
			{
				Rows[i].Type = ValueType.Abosulte;
			}

			scroll.Left = -0.01f;
			scroll.Top = 0;
			AddChildOverlayElement(scroll);
			initDragTop = drag.Top;
		}

		public override void _mouseMoved(MouseEvent mouseEvent)
		{
			if (mouseEvent.state.Z.rel != 0 && widgets.Count != 0)
			{
				float distance = scroll.Height - drag.Height - initDragTop;
				var moveOffset = distance / (float)rows.Count;

				float offset = mouseEvent.state.Z.rel / Mogre.Math.Abs((float)mouseEvent.state.Z.rel);
				if (offset < 0)
				{
					if (drag.Top + drag.Height + initDragTop <= scroll.Height)
					{
						drag.Top += moveOffset;
						setDisplayWidgets(ScrollOritentation.Down);
					}
				}
				else
				{
					if (drag.Top >= initDragTop)
					{
						drag.Top -= moveOffset;
						setDisplayWidgets(ScrollOritentation.Up);
					}
				}
				Scrolled?.Invoke();
			}
		}

		public override void _cursorMoved(Vector2 cursorPos)
		{
		}

		public new void AddRow(ValueType type = ValueType.Abosulte, float height = 0)
		{
		}

		public new void AddWidget(
			int rowNum,
			int colNum,
			Widget widget,
			AlignMode align = AlignMode.Left,
			DockMode dock = DockMode.None)
		{
			widget.Col = colNum;
			widget.Row = rowNum;
			widget.Left += Padding.PaddingLeft;
			widgets.Add(widget);

			var c = cols[colNum - 1];
			var r = rows[rowNum - 1];

			switch (dock)
			{
				case DockMode.Fill:
					widget.Height = r.AbosulteHeight;
					widget.Width = c.AbosulteWidth;
					break;
				case DockMode.FillHeight:
					widget.Height = r.AbosulteHeight;
					break;
				case DockMode.FillWidth:
					widget.Width = c.AbosulteWidth;
					break;
			}

			if (rowNum != 1 || colNum != 1)
			{
				float relativeLeft = 0;
				float relativeTop = 0;

				for (int i = 0; i < colNum - 1; i++)
				{
					relativeLeft += cols[i].AbosulteWidth;
				}
				for (int i = 0; i < rowNum - 1; i++)
				{
					relativeTop += rows[i].AbosulteHeight;
				}

				widget.Left += relativeLeft;
				widget.Top += relativeTop;
			}

			switch (align)
			{
				case AlignMode.Center:
					widget.Left += (c.AbosulteWidth - widget.Width) / 2;
					break;
				case AlignMode.Right:
					break;
			}

			AddChildOverlayElement(widget.getOverlayElement());

			if (widget.Top + widget.Height > Height)
			{
				scroll.Show();
				widget.hide();
			}
			else
			{
				visualWidgets.Add(widget);
			}

			calculateScrollBar();
		}

		private void calculateScrollBar()
		{
			drag.Height = ((float)visualWidgets.Count / (float)widgets.Count) * scroll.Height;
		}

		private void setDisplayWidgets(ScrollOritentation oritentation)
		{
			float dragTopPos = drag.Top - initDragTop;
			float passedRowNum = dragTopPos / scroll.Height * rows.Count;
			foreach (var widget in visualWidgets)
			{
				widget.hide();
			}
			int passedRowNumInt = (int)(System.Math.Round(passedRowNum, MidpointRounding.AwayFromZero));
			int skipNum = passedRowNumInt * cols.Count;
			visualWidgets = widgets.Skip(skipNum).Take(visualWidgets.Count).ToList();

			for (int i = 0; i < visualWidgets.Count; i++)
			{
				visualWidgets[i].show();
				switch (oritentation)
				{
					case ScrollOritentation.Up:
						visualWidgets[i].Top += passedRowNumInt * visualWidgets[i].Height;
						break;
					case ScrollOritentation.Down:
						visualWidgets[i].Top -= passedRowNumInt * visualWidgets[i].Height;
						break;
				}
			}
		}

		public override void AddedToAnotherWidgetFinished()
		{
			scroll.Height = Height - 0.016f;
		}
	}
}
