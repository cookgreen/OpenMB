using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Widgets
{
    public class ListViewColumn
    {
        public string ColumnName;
        public float ColumnWidth;
        public float ColumnHeight;
        public BorderPanelOverlayElement ColumnEnity;
    }

    public class ListViewItem
    {
        public float Top;
        public List<OverlayElement> Items;
        public ListViewItem()
        {
            Items = new List<OverlayElement>();
        }
    }

    public class ListView : Mogre_Procedural.MogreBites.Widget
    {
        public event Action<object> SelectionChanged;
        public List<ListViewColumn> Columns
        {
            get
            {
                return columns;
            }
        }
        public List<ListViewItem> Items;
        private string name;
        private OverlayContainer listview;
        private BorderPanelOverlayElement scroll;
        private OverlayElement drag;
        private List<ListViewColumn> columns;
        private List<ListViewItem> items;
        private float top;
        private float left;
        private float width;
        private float height;
        private float maxShowItem;
        private List<OverlayElement> allUsedElements;
        public ListView(string name, float left, float top, float height, float width, List<string> columnNames)
        {
            listview = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/ListView", "BorderPanel", name) as OverlayContainer;
            scroll = listview.GetChild(name+"/ListViewScroll") as BorderPanelOverlayElement;
            drag = scroll.GetChild(name + "/ListViewScroll" + "/ListViewDrag") as OverlayElement;
            this.name = name;
            this.top = top;
            this.height = height;
            this.width = width;
            this.left = left;
            listview.Top = top;
            listview.Left = left;
            listview.Height = height;
            listview.Width = width;
            maxShowItem = width / 0.04f;
            columns = new List<ListViewColumn>();
            items = new List<ListViewItem>();
            allUsedElements = new List<OverlayElement>();
            scroll.Height = height - 0.016f;
            drag.Hide();

            LoadColumns(columnNames);
        }

        public void LoadColumns(List<string> columnNames)
        {
            if (columns != null && columns.Count > 0)
            {
                foreach (var column in columns)
                {
                    GameTrayManager.nukeOverlayElement(column.ColumnEnity);
                }
                columns.Clear();
            }
            top = 0.01f;
            left = 0.01f;
            for (int i = 0; i < columnNames.Count; i++)
            {
                var listViewCol = OverlayManager.Singleton.CreateOverlayElementFromTemplate("ListView/Column", "BorderPanel", "col" + i) as BorderPanelOverlayElement;
                var textArea = listViewCol.GetChild(listViewCol.Name + "/ListViewColumnCaption") as TextAreaOverlayElement;
                textArea.Caption = columnNames[i];
                ListViewColumn lsc = new ListViewColumn();
                lsc.ColumnName = columnNames[i];
                lsc.ColumnEnity = listViewCol;
                columns.Add(lsc);
                listViewCol.Top = top;
                listViewCol.Left = left;
                listViewCol.Width = width / columnNames.Count - 0.005f;
                listViewCol.Show();
                listview.AddChild(listViewCol);
                left = left + listViewCol.Width;

                if (i != columnNames.Count - 1)
                {
                    var verLine = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/VerticalLine", "Panel", "colline" + i) as PanelOverlayElement;
                    verLine.Left = left;
                    verLine.Top = top;
                    verLine.Height = height - 0.018f;
                    verLine.Show();
                    listview.AddChild(verLine);
                    verLine.Dispose();
                    allUsedElements.Add(verLine);
                }
            }
            var horline = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "colhorline") as PanelOverlayElement;
            horline.Left = 0.008f;
            horline.Top = 0.05338f;
            horline.Width = width - 0.026f;
            horline.Show();
            allUsedElements.Add(horline);
            listview.AddChild(horline);
        }

        public void AddItem(List<string> item)
        {
            float left = 0.01f;
            ListViewItem lvi = new ListViewItem();
            if (items.Count == 0)
            {
                lvi.Top = top + 0.042f;
            }
            else
            {
                ListViewItem lastLvi = items.Last();
                lvi.Top = lastLvi.Top + 0.048f;
            }
            for (int i = 0; i < item.Count;i++ )
            {
                PanelOverlayElement ListViewCell = OverlayManager.Singleton.CreateOverlayElementFromTemplate("ListView/ListViewCell", "Panel", "item" + Guid.NewGuid()) as PanelOverlayElement;
                var txtArea = ListViewCell.GetChild(ListViewCell.Name + "/ListViewCellCaption");
                txtArea.Caption = item[i];
                lvi.Items.Add(ListViewCell);
                ListViewCell.Top = lvi.Top;
                ListViewCell.Left = left;
                ListViewCell.Width = width / item.Count - 0.005f;
                var line = OverlayManager.Singleton.CreateOverlayElementFromTemplate("AMGE/UI/HorizalLine", "Panel", "colhorline" + Guid.NewGuid()) as PanelOverlayElement; ;
                line.Left = 0.0f;
                line.Top = 0.0f;
                line.Width = ListViewCell.Width;
                line.Show();
                ListViewCell.AddChild(line);
                left = left + ListViewCell.Width;
                if (items.Count > maxShowItem)
                {
                    ListViewCell.Hide();
                }
                else
                {
                    ListViewCell.Show();
                }

                listview.AddChild(ListViewCell);
            }
            items.Add(lvi);

            

            if (items.Count > maxShowItem)
            {
                drag.Show();
            }
            else
            {
                drag.Hide();
            }
        }

        public OverlayContainer GetContainer()
        {
            return listview;
        }

        public override void Dispose()
        {
            for (int i = 0; i < columns.Count; i++)
            {
                GameTrayManager.nukeOverlayElement(columns[i].ColumnEnity);
            }
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].Items.Count; j++)
                {
                    GameTrayManager.nukeOverlayElement(items[i].Items[j]);
                }
            }
            for (int i = 0; i < allUsedElements.Count; i++)
            {
                OverlayManager.Singleton.DestroyOverlayElement(allUsedElements[i]);
            }
            allUsedElements.Clear();
            columns.Clear();
            items.Clear();
            OverlayManager.Singleton.DestroyOverlayElement(drag);
            OverlayManager.Singleton.DestroyOverlayElement(scroll);
        }

        public override void _cursorMoved(Vector2 cursorPos)
        {
        }

        public override void _cursorPressed(Vector2 cursorPos)
        {
        }

        public override void _cursorReleased(Vector2 cursorPos)
        {
        }
    }
}
