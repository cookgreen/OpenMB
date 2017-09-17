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
        public List<ListViewColumn> Columns
        {
            get
            {
                return columns;
            }
        }
        public List<ListViewItem> Items;
        private Overlay overlay;
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
        public ListView(string name, float left, float top, float height, float width, List<string> columnNames)
        {
            overlay = OverlayManager.Singleton.Create(name + "/Main");
            listview = OverlayManager.Singleton.CreateOverlayElementFromTemplate("ListView", "BorderPanel", name) as OverlayContainer;
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
            scroll.Height = height - 0.016f;
            drag.Hide();

            LoadColumns(columnNames);

            overlay.Add2D(listview);
            overlay.ZOrder = 100;
            overlay.Show();
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
                    var verLine = OverlayManager.Singleton.CreateOverlayElementFromTemplate("Common/VerticalLine", "Panel", "colline" + i) as PanelOverlayElement;
                    verLine.Left = left;
                    verLine.Top = top;
                    verLine.Height = height - 0.018f;
                    verLine.Show();
                    listview.AddChild(verLine);
                }
            }
            var horline = OverlayManager.Singleton.CreateOverlayElementFromTemplate("Common/HorizalLine", "Panel", "colhorline") as PanelOverlayElement;
            horline.Left = 0.008f;
            horline.Top = 0.05338f;
            horline.Width = width - 0.026f;
            horline.Show();
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
                var line = OverlayManager.Singleton.CreateOverlayElementFromTemplate("Common/HorizalLine", "Panel", "colhorline"+Guid.NewGuid()) as PanelOverlayElement; ;
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
