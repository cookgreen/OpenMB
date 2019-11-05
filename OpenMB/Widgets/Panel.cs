using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Widgets
{
	public enum DockMode
	{
		Fill,
		FillWidth,
		FillHeight,
		None,
		Center,
	}

	public enum AlignMode
	{
		Center,
		Left,
		Right
	}

	/// <summary>
	/// Value type
	/// </summary>
	public enum ValueType
	{
		Abosulte,
		Percent,
		Auto,
	}

	/// <summary>
	/// Define a panel row
	/// </summary>
	public class PanelRow
	{
		public ValueType Type;
		public float Height;
		private Panel panel;
		public float AbosulteHeight
		{
			get
			{
				if (Type == ValueType.Abosulte)
				{
					return Height;
				}
				else if (Type == ValueType.Percent)
				{
					return ((float)(panel.Height - calculateAllAbsoluteHeights()) / (float)panel.Rows.Where(o => o.Type == ValueType.Percent).Count());//Relative
				}
				else if (Type == ValueType.Auto)
				{
					return Height;
				}
				return -1;
			}
		}

		private float calculateAllAbsoluteHeights()
		{
			float heights = 0;
			foreach(var row in panel.Rows)
			{
				if (row.Type == ValueType.Abosulte)
				{
					heights += row.AbosulteHeight;
				}
			}
			return heights;
		}

		public PanelRow(Panel panel)
		{
			this.panel = panel;
		}
	}

	/// <summary>
	/// Define a panel column
	/// </summary>
	public class PanelColumn
	{
		public ValueType Type;
		public float Width;
		private Panel panel;
		public float AbosulteWidth
		{
			get
			{
				if (Type == ValueType.Abosulte)
				{
					return Width * panel.Width - (panel.Padding.PaddingLeft + panel.Padding.PaddingRight);
				}
				else if (Type == ValueType.Percent)
				{
					return ((float)(panel.Width - (panel.Padding.PaddingLeft + panel.Padding.PaddingRight)) / (float)panel.Cols.Count);//Relative
				}
				else if (Type == ValueType.Auto)
				{
					return Width;
				}
				return -1;
			}
		}
		public PanelColumn(Panel panel)
		{
			this.panel = panel;
		}

		private float calculateAllAbsoluteWidths()
		{
			float widths = 0;
			foreach (var col in panel.Cols)
			{
				if (col.Type == ValueType.Abosulte)
				{
					widths += col.AbosulteWidth;
				}
			}
			return widths;
		}
	}

	/// <summary>
	/// Panel Control
	/// </summary>
    public class Panel : Widget
    {
		protected List<PanelRow> rows;
		protected List<PanelColumn> cols;
		protected List<Widget> widgets;

		public List<PanelRow> Rows
		{
			get
			{
				return rows;
			}
		}
		public List<PanelColumn> Cols
		{
			get
			{
				return cols;
			}
		}

		public Panel(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true)
        {
            widgets = new List<Widget>();
            OverlayManager overlayMgr = OverlayManager.Singleton;
			if(hasBorder)
			{
				mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanel", "BorderPanel", name);
			}
			else
			{
				mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanelNoBorder", "BorderPanel", name);
			}
			mElement.MetricsMode = GuiMetricsMode.GMM_RELATIVE;

			if (width == 0 || height == 0)
            {
				mElement.Width = 1.0f;
				mElement.Height = 1.0f;
            }
            else if (width > 0 && height > 0)
            {
                mElement.Width = width;
                mElement.Height = height;
            }
			mElement.Top = top;
			mElement.Left = left;
			cols = new List<PanelColumn>();
			rows = new List<PanelRow>();
			for (int i = 0; i < row; i++)
			{
				rows.Add(new PanelRow(this) { Type = ValueType.Percent, Height = 100 });
			}
			for (int i = 0; i < col; i++)
			{
				cols.Add(new PanelColumn(this) { Type = ValueType.Percent, Width = 100 });
			}
        }

		public void AddRow(ValueType type, float height = 0)
		{
			var row = new PanelRow(this) { Type = type, Height = height };
			rows.Add(row);
			if (type == ValueType.Percent && height == 0)
			{
				for (int i = 0; i < rows.Count; i++)
				{
					if (rows[i].Type == ValueType.Percent)
					{
						rows[i].Height = 100 / rows.Count;
					}
				}
			}
			else
			{
				row.Height = height;
			}
		}

		public void AddRows(int number, ValueType type, float height = 0)
		{
			for (int i = 0; i < number; i++)
			{
				AddRow(type, height);
			}
		}

		public void ChangeRow(ValueType valueType, float value, int rowNum = 1)
		{
			var row = rows[rowNum - 1];
			row.Type = valueType;
			row.Height = value;
		}

		public void AddCol(ValueType type, float width = 0)
		{
			cols.Add(new PanelColumn(this) { Type = type, Width = width });
			if (type == ValueType.Percent)
			{
				for (int i = 0; i < cols.Count; i++)
				{
					if (cols[i].Type == ValueType.Percent)
					{
						cols[i].Width = 100 / cols.Count;
					}
				}
			}
		}

		public void ChangeCol(ValueType valueType, float value, int colNum = 1)
		{
			var col = cols[colNum - 1];
			col.Type = valueType;
			col.Width = value;
		}

		public void AddWidget(
			int rowNum, 
			int colNum, 
			Widget widget,
			AlignMode align = AlignMode.Left,
			DockMode dock = DockMode.None)
		{
			widget.Col = colNum;
			widget.Row = rowNum;
			widget.Top += Top;
			widget.Left += Left + Padding.PaddingLeft;
			widgets.Add(widget);

			var c = cols[colNum - 1];
			var r = rows[rowNum - 1];

			switch(dock)
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

			float relativeLeft = 0;
			float relativeTop = 0;
			if (rowNum != 1 || colNum != 1)
			{
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
					widget.Top += (r.AbosulteHeight - widget.Height) / 2;
					break;
				case AlignMode.Right:
					break;
			}
			widget.AddedToAnotherWidgetFinished(align, relativeLeft, c.AbosulteWidth, relativeTop, r.AbosulteHeight);
		}

		public void AddWidgetRelative(
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
				case DockMode.Center:
					widget.Width = widget.Width * c.AbosulteWidth;
					//widget.Height = widget.Height * r.AbosulteHeight;
					break;
			}

			if (c.Type == ValueType.Auto)
			{
				c.Width = widget.Width;
			}
			if (r.Type == ValueType.Auto)
			{
				r.Height = widget.Height;
			}

			float relativeLeft = 0;
			float relativeTop = 0;
			if (rowNum != 1 || colNum != 1)
			{
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
			else
			{
				widget.Left = Padding.PaddingLeft;
			}

			switch (align)
			{
				case AlignMode.Center:
					widget.Left += (c.AbosulteWidth - widget.Width) / 2;
					break;
				case AlignMode.Right:
					break;
			}

			((OverlayContainer)mElement).AddChild(widget.getOverlayElement());
			widget.AddedToAnotherWidgetFinished(align, relativeLeft, c.AbosulteWidth, relativeTop, r.AbosulteHeight);
		}

		public void AddWidget(Widget widget)
		{
			AddWidget(1, 1, widget);
		}

		public Widget GetWidget(int rowNum, int colNum)
		{
			var retWidgets = widgets.Where(o => o.Row == rowNum && o.Col == colNum);
			return retWidgets.FirstOrDefault();
		}

		public int GetWidgetNum()
		{
			return widgets.Count;
		}

		public void RemoveWidget(int rowNum, int colNum)
		{
			var widget = GetWidget(rowNum, colNum);
			if (widget != null)
			{
				widgets.Remove(widget);
				((OverlayContainer)mElement).RemoveChild(widget.getOverlayElement().Name);
				Control.nukeOverlayElement(widget.getOverlayElement());
			}
		}

        public void ClearWidget()
        {
            foreach (Widget widget in widgets)
            {
                ((OverlayContainer)mElement).RemoveChild(widget.getOverlayElement().Name);
                Control.nukeOverlayElement(widget.getOverlayElement());
            }
            widgets.Clear();
        }

        public override void Dispose()
        {
            mElement.Dispose();
            Control.nukeOverlayElement(mElement);
        }

		public override void _mouseMoved(MouseEvent evt)
		{
			foreach (var w in widgets)
			{
				w._mouseMoved(evt);
			}
		}

		public override void _cursorPressed(Vector2 cursorPos)
		{
			foreach (var w in widgets)
			{
				if (isCursorOver(w.getOverlayElement(), cursorPos))
				{
					w._cursorPressed(cursorPos);
					break;
				}
			}
		}
		
		public override void _cursorReleased(Vector2 cursorPos)
		{
			foreach (var w in widgets)
			{
				if (isCursorOver(w.getOverlayElement(), cursorPos))
				{
					w._cursorReleased(cursorPos);
					break;
				}
			}
		}

		public override void _focusLost()
		{
			foreach (var w in widgets)
			{
				w._focusLost();
			}
		}
	}
}
