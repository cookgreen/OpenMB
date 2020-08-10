using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.UI.Widgets
{

	/// <summary>
	/// Define a panel row
	/// </summary>
	public class PanelRow
	{
		public ValueType Type;
		public float Height;
		private PanelWidget panel;
		public float RealHeight
		{
			get
			{
				if (Type == ValueType.Abosulte)
				{
					var wpt = panel.Parent;
					var het = Height * panel.Height;
					while (wpt != null)
					{
						het *= wpt.Height;
						wpt = wpt.Parent;
					}
					return het;
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
			foreach (var row in panel.Rows)
			{
				if (row.Type == ValueType.Abosulte)
				{
					heights += row.RealHeight;
				}
			}
			return heights;
		}

		public PanelRow(PanelWidget panel)
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
		private PanelWidget panel;
		public float RealWidth
		{
			get
			{
				if (Type == ValueType.Abosulte)
				{
					var wpt = panel.Parent;
					var wid = Width * panel.Width - (panel.Padding.PaddingLeft + panel.Padding.PaddingRight); ;
					while (wpt != null)
					{
						wid *= wpt.Width;
						wpt = wpt.Parent;
					}
					return wid;
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
		public PanelColumn(PanelWidget panel)
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
					widths += col.RealWidth;
				}
			}
			return widths;
		}
	}

	public class PanelCell
	{
		private int row;
		private int col;
		private PanelWidget panel;
		public int Row { get { return row; } }
		public int Col { get { return col; } }
		public PanelCell(int row, int col, PanelWidget panel)
		{
			this.row = row;
			this.col = col;
			this.panel = panel;
		}

		public void AddWidget(Widget widget)
		{

		}
	}

	/// <summary>
	/// Panel Control
	/// </summary>
	public class PanelWidget : Widget, IHasSubWidgets
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

		public PanelWidget(string name, float width = 0, float height = 0, float left = 0, float top = 0, int row = 1, int col = 1, bool hasBorder = true)
		{
			widgets = new List<Widget>();
			OverlayManager overlayMgr = OverlayManager.Singleton;
			if (hasBorder)
			{
				element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanel", "BorderPanel", name);
			}
			else
			{
				element = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanelNoBorder", "BorderPanel", name);
			}
			element.MetricsMode = GuiMetricsMode.GMM_RELATIVE;

			if (width <= 0)
			{
				element.Width = 1.0f;
			}
			else
			{
				element.Width = width;
			}
			if (height <= 0)
			{
				element.Height = 1.0f;
			}
			else
			{
				element.Height = height;
			}

			element.Top = top;
			element.Left = left;
			cols = new List<PanelColumn>();
			rows = new List<PanelRow>();
			initizationRowCol(row, col);

		}

		private void initizationRowCol(int row, int col)
		{
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

		public virtual void AddWidget(int rowNum,
			int colNum,
			Widget widget,
			AlignMode hAlign = AlignMode.Left,
			AlignMode vAlign = AlignMode.Left,
			DockMode dock = DockMode.None,
			int rowSpan = 1,
			int colSpan = 1)
		{
			switch (widget.MetricMode)
			{
				case GuiMetricsMode.GMM_PIXELS:
					AddWidgetPixels(rowNum, colNum, widget, hAlign, vAlign, dock, rowSpan, colSpan);
					break;
				case GuiMetricsMode.GMM_RELATIVE:
					AddWidgetRelative(rowNum, colNum, widget, hAlign, vAlign, dock, rowSpan, colSpan);
					break;
			}
		}

		private void AddWidgetPixels(
			int rowNum,
			int colNum,
			Widget widget,
			AlignMode hAlign,
			AlignMode vAlign,
			DockMode dock,
			int rowSpan,
			int colSpan)
		{
			widget.Col = colNum;
			widget.Row = rowNum;
			widget.Left += Padding.PaddingLeft;
			widgets.Add(widget);

			widget.Parent = this;
			((OverlayContainer)element).AddChild(widget.OverlayElement);

			var c = cols[colNum - 1];
			var r = rows[rowNum - 1];

			switch (dock)
			{
				case DockMode.Fill:
					widget.Height = RelativeToPixels(r.RealHeight, float.Parse(GameManager.Instance.VideoMode["Height"]));
					widget.Width = RelativeToPixels(c.RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"]));
					break;
				case DockMode.FillHeight:
					widget.Height = RelativeToPixels(r.RealHeight, float.Parse(GameManager.Instance.VideoMode["Height"]));
					break;
				case DockMode.FillWidth:
					widget.Width = RelativeToPixels(c.RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"]));
					break;
				case DockMode.Center:
					widget.Width = widget.Width * RelativeToPixels(c.RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"]));
					//widget.Height = widget.Height * r.AbosulteHeight;
					break;
				default:
					widget.Width = RelativeToPixels(c.RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"]));
					widget.Height = RelativeToPixels(r.RealHeight, float.Parse(GameManager.Instance.VideoMode["Height"]));
					break;
			}

			if (c.Type == ValueType.Auto)
			{
				c.Width = PixelsToRelative(widget.Width, RelativeToPixels(c.RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"])));
			}
			if (r.Type == ValueType.Auto)
			{
				r.Height = PixelsToRelative(widget.Height, RelativeToPixels(r.RealHeight, float.Parse(GameManager.Instance.VideoMode["Height"])));
			}

			float relativeLeft = 0;
			float relativeTop = 0;
			for (int i = 0; i < rowNum - 1; i++)
			{
				relativeTop += rows[i].RealHeight;
			}
			for (int i = 0; i < colNum - 1; i++)
			{
				relativeLeft += cols[i].RealWidth;
			}

			widget.Left += relativeLeft;
			widget.Top += RelativeToPixels(relativeTop, float.Parse(GameManager.Instance.VideoMode["Height"]));

			if (rowSpan > 1)
			{
				for (int i = 0; i < rowSpan - 1; i++)
				{
					widget.Height += RelativeToPixels(rows[i].RealHeight, float.Parse(GameManager.Instance.VideoMode["Height"]));
				}
			}
			if (colSpan > 1)
			{
				for (int i = 0; i < colSpan - 1; i++)
				{
					widget.Width += RelativeToPixels(cols[i].RealWidth, float.Parse(GameManager.Instance.VideoMode["Width"]));
				}
			}

			switch (hAlign)
			{
				case AlignMode.Center:
					widget.Left = RelativeToPixels((c.RealWidth - widget.Width) / 2, float.Parse(GameManager.Instance.VideoMode["Width"]));
					break;
				case AlignMode.Right:
					break;
			}

			switch (vAlign)
			{
				case AlignMode.Center:
					widget.Top += RelativeToPixels((r.RealHeight - widget.Height) / 2, float.Parse(GameManager.Instance.VideoMode["Height"]));
					break;
			}
			widget.AddedToAnotherWidgetFinished(hAlign, relativeLeft, c.RealWidth, relativeTop, r.RealHeight);
		}


		private void AddWidgetRelative(
			int rowNum,
			int colNum,
			Widget widget,
			AlignMode hAlign = AlignMode.Left,
			AlignMode vAlign = AlignMode.Left,
			DockMode dock = DockMode.None,
			int rowSpan = 1,
			int colSpan = 1)
		{

			widget.Col = colNum;
			widget.Row = rowNum;
			widgets.Add(widget);

			widget.Parent = this;
			((OverlayContainer)element).AddChild(widget.OverlayElement);

			var c = cols[colNum - 1];
			var r = rows[rowNum - 1];

			switch (dock)
			{
				case DockMode.Fill:
					widget.Height = r.RealHeight - (Padding.PaddingDown + Padding.PaddingTop);
					widget.Width = c.RealWidth - (Padding.PaddingLeft + Padding.PaddingRight);
					widget.Left += Padding.PaddingLeft;
					widget.Top += Padding.PaddingTop;
					break;
				case DockMode.FillHeight:
					widget.Height = r.RealHeight - (Padding.PaddingDown + Padding.PaddingTop);
					widget.Top += Padding.PaddingTop;
					break;
				case DockMode.FillWidth:
					widget.Width = c.RealWidth - (Padding.PaddingLeft + Padding.PaddingRight);
					widget.Left += Padding.PaddingLeft;
					break;
				case DockMode.None:
					widget.Width *= c.RealWidth;
					widget.Height *= r.RealHeight;
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
			for (int i = 0; i < rowNum - 1; i++)
			{
				relativeTop += rows[i].RealHeight;
			}
			for (int i = 0; i < colNum - 1; i++)
			{
				relativeLeft += cols[i].RealWidth;
			}

			widget.Left += relativeLeft;
			widget.Top += relativeTop;

			if (rowSpan > 1)
			{
				for (int i = 0; i < rowSpan - 1; i++)
				{
					widget.Height += rows[i].RealHeight;
				}
			}
			if (colSpan > 1)
			{
				for (int i = 0; i < colSpan - 1; i++)
				{
					widget.Width += cols[i].RealWidth;
				}
			}

			switch (hAlign)
			{
				case AlignMode.Center:
					if (colNum == 1)
					{
						widget.Left = (c.RealWidth - widget.Width) / 2;
					}
					else
					{
						widget.Left += (c.RealWidth - widget.Width) / 2;
					}
					break;
			}

			switch (vAlign)
			{
				case AlignMode.Center:
					if (rowNum == 1)
					{
						widget.Top = (r.RealHeight - widget.Height) / 2;
					}
					else
					{
						widget.Top += (r.RealHeight - widget.Height) / 2;
					}
					break;
			}
			widget.AddedToAnotherWidgetFinished(hAlign, relativeLeft, c.RealWidth, relativeTop, r.RealHeight);
		}

		private float PixelsToRelative(float pixelValue, float referenceValue)
		{
			return pixelValue / referenceValue;
		}

		private float RelativeToPixels(float relativeValue, float referenceValue)
		{
			return relativeValue * referenceValue;
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

		public virtual void RemoveWidget(int rowNum, int colNum)
		{
			var widget = GetWidget(rowNum, colNum);
			if (widget != null)
			{
				widgets.Remove(widget);
				((OverlayContainer)element).RemoveChild(widget.OverlayElement.Name);
				Control.nukeOverlayElement(widget.OverlayElement);
			}
		}

		public override void Dispose()
		{
			foreach (var w in widgets)
			{
				w.Dispose();
			}
			widgets.Clear();
		}

		public override void MouseMoved(MouseEvent evt)
		{
			foreach (var w in widgets)
			{
				w.MouseMoved(evt);
			}
		}

		public override void CursorPressed(Vector2 cursorPos)
		{
			foreach (var w in widgets)
			{
				if (IsCursorOver(w.OverlayElement, cursorPos))
				{
					w.CursorPressed(cursorPos);
					break;
				}
			}
		}

		public override void CursorReleased(Vector2 cursorPos)
		{
			foreach (var w in widgets)
			{
				if (IsCursorOver(w.OverlayElement, cursorPos))
				{
					w.CursorReleased(cursorPos);
					break;
				}
			}
		}

		public override void FocusLost()
		{
			foreach (var w in widgets)
			{
				w.FocusLost();
			}
		}

		public void ChangeTotalCol(int totalColNumber, ValueType valueType = ValueType.Percent)
		{
			initizationRowCol(rows.Count, totalColNumber);
		}

		public void ChangeTotalRow(int totalRowNumber, ValueType valueType = ValueType.Percent)
		{
			initizationRowCol(totalRowNumber, cols.Count);
		}
	}
}
