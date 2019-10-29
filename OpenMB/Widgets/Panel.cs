using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Widgets
{
	public enum ValueType
	{
		Abosulte,
		Percent
	}

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
					return ((float)100 / (float)panel.Rows.Count) / (float)100;//Relative
				}
				return -1;
			}
		}
		public PanelRow(Panel panel)
		{
			this.panel = panel;
		}
	}

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
					return Width;
				}
				else if (Type == ValueType.Percent)
				{
					return ((float)100 / (float)panel.Cols.Count) / (float)100;//Relative
				}
				return -1;
			}
		}
		public PanelColumn(Panel panel)
		{
			this.panel = panel;
		}
	}

    public class Panel : Widget
    {
		private List<PanelRow> rows;
		private List<PanelColumn> cols;
        private List<Widget> widgets;

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

		public Panel(string name, float width = 0, float height = 0, float left = 0, float top = 0)
        {
            widgets = new List<Widget>();
            OverlayManager overlayMgr = OverlayManager.Singleton;
			mElement = OverlayManager.Singleton.CreateOverlayElementFromTemplate("EditorPanel", "BorderPanel", name);
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
			cols.Add(new PanelColumn(this) { Type = ValueType.Percent, Width = 100 });
			rows.Add(new PanelRow(this) { Type = ValueType.Percent, Height = 100});
        }

		public void AddRow(ValueType type, float height)
		{
			rows.Add(new PanelRow(this) { Type = type, Height = height });
			if (type == ValueType.Percent)
			{
				for (int i = 0; i < rows.Count; i++)
				{
					if (rows[i].Type == ValueType.Percent)
					{
						rows[i].Height = 100 / rows.Count;
					}
				}
			}
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
		public void AddWidget(Widget widget)
		{
			AddWidget(1, 1, widget);
		}
		public void AddWidget(int rowNum, int colNum, Widget widget)
		{
			widget.Col = colNum;
			widget.Row = rowNum;
			widgets.Add(widget);

			var c = cols[colNum - 1];
			if (c.Type == ValueType.Percent)
			{
				widget.Width = cols[colNum - 1].AbosulteWidth;
			}

			((OverlayContainer)mElement).AddChild(widget.getOverlayElement());
			
			if (rowNum != 1 || colNum != 1)
			{
				float totalLeft = 0;
				float totalTop = 0;

				for (int i = 0; i < colNum - 1; i++)
				{
					totalLeft += cols[i].AbosulteWidth;
				}
				for (int i = 0; i < rowNum - 1; i++)
				{
					totalTop += rows[i].AbosulteHeight;
				}

				widget.Left += totalLeft;
				widget.Top += totalTop;
			}
		}

		public void RemoveWidget(Widget widget)
        {
            widgets.Remove(widget);
            ((OverlayContainer)mElement).RemoveChild(widget.getOverlayElement().Name);
            Control.nukeOverlayElement(widget.getOverlayElement());
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

        public int GetWidgetNum()
        {
            return widgets.Count;
        }

        public Widget GetWidget(int index)
        {
            if (index >= widgets.Count)
            {
                return null;
            }
            else
            {
                return widgets[index];
            }
        }

        public override void Dispose()
        {
            mElement.Dispose();
            Control.nukeOverlayElement(mElement);
        }
    }
}
