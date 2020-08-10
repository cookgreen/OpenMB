using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	public enum ScrollOritentation
	{
		Up,
		Down,
	}

	/// <summary>
	/// Scrollable Panel
	/// </summary>
	public class PanelScrollableWidget : PanelWidget, IScrollable
	{
		private List<Widget> visualWidgets;
		private BorderPanelOverlayElement scroll;
		private OverlayElement drag;
		private float initDragTop;
		public event Action Scrolled;
		public float EachRowHeight
		{
			get
			{
				return rows[0].RealHeight;
			}
		}

		public PanelScrollableWidget(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true) : base(name, width, height, left, top, row, col, hasBorder)
		{
			visualWidgets = new List<Widget>();
			string scrollName = name + "_Scroll";
			scroll = OverlayManager.Singleton.CreateOverlayElementFromTemplate("ScrollComponet", "BorderPanel", scrollName) as BorderPanelOverlayElement;
			drag = scroll.GetChild(scrollName + "/Drag") as OverlayElement;

			for (int i = 0; i < Rows.Count; i++)
			{
				Rows[i].Type = ValueType.Auto;
			}

			scroll.Left = -0.01f;
			scroll.Top = 0;
			AddChildOverlayElement(scroll);
			initDragTop = drag.Top;

			scroll.Hide();
			drag.Hide();
		}

		public override void MouseMoved(MouseEvent mouseEvent)
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
			foreach (var widget in visualWidgets)
			{
				widget.MouseMoved(mouseEvent);
			}
		}

		public override void CursorPressed(Vector2 cursorPos)
		{
			foreach (var v in visualWidgets)
			{
				v.CursorPressed(cursorPos);
			}
		}

		public override void FocusLost()
		{
			foreach (var v in visualWidgets)
			{
				v.FocusLost();
			}
		}

		public new void AddRow(ValueType type = ValueType.Abosulte, float height = 0)
		{
			base.AddRow(type, height);
		}

		public override void AddWidget(
			int rowNum,
			int colNum,
			Widget widget,
			AlignMode hAlign = AlignMode.Left,
			AlignMode vAlign = AlignMode.Left,
			DockMode dock = DockMode.None,
			int rowSpan = 1,
			int colSpan = 1,
			bool isAddWidgetToAnother = true)
		{
			if (rowNum > rows.Count)
			{
				int sub = rowNum - rows.Count;
				for (int i = 0; i < sub; i++)
				{
					PanelRow row = new PanelRow(this);
					row.Type = ValueType.Auto;
					rows.Add(row);
				}
			}
			if (colNum > cols.Count)
			{
				int sub = colNum - cols.Count;
				for (int i = 0; i < sub; i++)
				{
					PanelColumn col = new PanelColumn(this);
					col.Type = ValueType.Auto;
					cols.Add(col);
				}
			}

			base.AddWidget(rowNum, colNum, widget, hAlign, vAlign, dock, rowSpan, colSpan);

			if (widget.Top + widget.Height > Height)
			{
				scroll.Show();
				drag.Show();
				widget.Hide();
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
			foreach (var widget in visualWidgets)
			{
				widget.Hide();
			}
			int passedRowNum = (int)(System.Math.Round(dragTopPos / scroll.Height * rows.Count, MidpointRounding.AwayFromZero));
			int skipNum = passedRowNum * cols.Count;
			visualWidgets = widgets.Skip(skipNum).Take(visualWidgets.Count).ToList();

			int curIndex = 0;
			for (int i = 0; i < visualWidgets.Count; i++)
			{
				visualWidgets[i].Top = curIndex * visualWidgets[i].Height;
				if ((i + 1) % cols.Count == 0)
				{
					curIndex++;
				}
				visualWidgets[i].Show();
			}
		}

		public override void AddedToAnotherWidgetFinished(
			AlignMode alignMode,
			float parentWidgetLeft,
			float parentWidgetWidth,
			float parentWidgetTop,
			float parentWidgetHeight)
		{
			scroll.Height = Height - 0.016f;
		}

		public override void RemoveWidget(int rowNum, int colNum)
		{
			var widget = GetWidget(rowNum, colNum);
			if (widget != null)
			{
				visualWidgets.Remove(widget);
			}
			base.RemoveWidget(rowNum, colNum);
		}
	}
}
