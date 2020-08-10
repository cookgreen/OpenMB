using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenMB.UI.Widgets
{

	/// <summary>
	/// Define a panel row
	/// </summary>
	public class ListViewPanelRow
	{
		public ValueType Type;
		public float Height;
		private SimpleListViewWidget panel;
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

		public ListViewPanelRow(SimpleListViewWidget panel)
		{
			this.panel = panel;
		}
	}

	/// <summary>
	/// Define a panel column
	/// </summary>
	public class ListViewPanelColumn
	{
		public ValueType Type;
		public float Width;
		private SimpleListViewWidget panel;
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
		public ListViewPanelColumn(SimpleListViewWidget panel)
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
	public class SimpleListViewWidget : Widget
	{
		public class ListViewItem
		{
			public List<string> SubItems { get; set; }
			public List<Widget> Widgets { get; set; }
			public ListViewItem()
			{
				SubItems = new List<string>();
				Widgets = new List<Widget>();
			}

			public void Highlight()
			{
				for (int i = 0; i < Widgets.Count; i++)
				{
					Widgets[i].Material = "Engine/Background/Active";
				}
			}

			public void UnHighlight()
			{
				for (int i = 0; i < Widgets.Count; i++)
				{
					Widgets[i].Material = "Engine/Background/Normal";
				}
			}
		}

		protected List<ListViewPanelRow> rows;
		protected List<ListViewPanelColumn> cols;
		protected List<Widget> widgets;
		private List<ListViewItem> items;
		private List<string> columns;
		private PanelWidget header;
		private SimplePanelScrollableWidget content;
		private const float LISTVIEW_ROW_HEIGHT = 0.05f;
		private ListViewItem lastListViewItem;

		public List<ListViewPanelRow> Rows
		{
			get
			{
				return rows;
			}
		}
		public List<ListViewPanelColumn> Cols
		{
			get
			{
				return cols;
			}
		}
		public SimpleListViewWidget(string name, List<string> columns, float width = 0, float height = 0, float left = 0, float top = 0, bool hasBorder = true)
		{
			this.columns = columns;


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

			items = new List<ListViewItem>();
			rows = new List<ListViewPanelRow>();
			cols = new List<ListViewPanelColumn>();
			widgets = new List<Widget>();

			ListViewPanelColumn col = new ListViewPanelColumn(this);
			col.Type = ValueType.Percent;
			col.Width = 100;
			cols.Add(col);

			ListViewPanelRow row = new ListViewPanelRow(this);
			row.Type = ValueType.Abosulte;
			row.Height = LISTVIEW_ROW_HEIGHT;
			rows.Add(row);
			row = new ListViewPanelRow(this);
			row.Type = ValueType.Percent;
			row.Height = 100;
			rows.Add(row);

			header = new PanelWidget("listview_" + name + "_header_" + Guid.NewGuid().ToString(), 0, 0, 0, 0, 1, columns.Count, false);
			content = new SimplePanelScrollableWidget("listview_" + name + "_header_" + Guid.NewGuid().ToString(), 0, 0, 0, 0, LISTVIEW_ROW_HEIGHT, columns.Count, false);
			content.Material = "SdkTrays/MiniTray";

			AddWidget(1, 1, header, AlignMode.Center, AlignMode.Center, DockMode.Fill);
			AddWidget(2, 1, content, AlignMode.Center, AlignMode.Center, DockMode.Fill);

			content.ChangeEachRowHeight(LISTVIEW_ROW_HEIGHT);

			AddColumn();
		}

		private void AddWidget(int rowNum, int colNum, Widget widget, AlignMode hAlign, AlignMode vAlign, DockMode dock, int rowSpan = 0, int colSpan = 0)
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
				case DockMode.Center:
					widget.Width = widget.Width * c.RealWidth;
					//widget.Height = widget.Height * r.AbosulteHeight;
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

		private void AddColumn()
		{
			for (int i = 0; i < columns.Count; i++)
			{
				SimpleStaticTextWidget columnText = new SimpleStaticTextWidget(Guid.NewGuid().ToString(), columns[i], 1, false, ColourValue.Black);
				header.AddWidget(1, i + 1, columnText, AlignMode.Center, AlignMode.Center, DockMode.Fill);
			}
		}

		public void AddItem(ListViewItem listViewItem)
		{
			if (listViewItem.SubItems.Count != columns.Count)
			{
				throw new Exception("Item is not matched with the column!!!");
			}

			int colNum = items.Count + 1;
			for (int i = 0; i < listViewItem.SubItems.Count; i++)
			{
				SimpleStaticTextBackgroundWidget contentText = new SimpleStaticTextBackgroundWidget(Guid.NewGuid().ToString(), listViewItem.SubItems[i], 1, false, ColourValue.Black);
				contentText.Height = LISTVIEW_ROW_HEIGHT;
				contentText.OnClick += (o) =>
				{
					var lvi = items.Where(b => b.Widgets.Where(a => a.Name == contentText.Name).Count() == 1).FirstOrDefault();
					if (lvi != null)
					{
						if (lastListViewItem != null)
						{
							lastListViewItem.UnHighlight();
						}
						lvi.Highlight();
						lastListViewItem = lvi;
					}
				};
				content.AddWidget(colNum, i + 1, contentText, AlignMode.Center, AlignMode.Center, DockMode.Fill, 1, 1);
				listViewItem.Widgets.Add(contentText);
			}

			items.Add(listViewItem);
		}

		public override void CursorPressed(Vector2 cursorPos)
		{
			header.CursorPressed(cursorPos);
			content.CursorPressed(cursorPos);
		}

		public override void CursorReleased(Vector2 cursorPos)
		{
			header.CursorReleased(cursorPos);
			content.CursorReleased(cursorPos);
		}
	}
}
