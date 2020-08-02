using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;

namespace OpenMB.Widgets
{
    public class ListViewSelectionChangedArgs : EventArgs
    {
        public ListViewItem item;
    }

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
        public string MaterialName
        {
            get
            {
                return Items[0].MaterialName;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].MaterialName = value;
                }
            }
        }
        public ListViewItem()
        {
            Items = new List<OverlayElement>();
        }
    }

    public class ListViewWidget : Widget
    {
        public event Action<object,ListViewSelectionChangedArgs> SelectionChanged;
        public List<ListViewColumn> Columns
        {
            get
            {
                return columns;
            }
        }
        public List<ListViewItem> Items
        {
            get
            {
                return items;
            }
        }
        public ListViewItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
            }
        }
        private string name;
        private OverlayContainer listview;
        private BorderPanelOverlayElement scroll;
        private OverlayElement drag;
        private List<ListViewColumn> columns;
        private List<ListViewItem> items;
        private List<ListViewItem> visibleItems;
        private ListViewItem selectedItem;
        private float top;
        private float left;
        private float width;
        private float height;
        private double maxShowItem;
        private List<OverlayElement> allUsedElements;
        //private bool dragging;
        public ListViewWidget(string name, float left, float top, float height, float width, List<string> columnNames)
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
            scroll.Height = height - 0.016f;
            drag.Hide();

            //remove column's height
            maxShowItem = System.Math.Floor(Convert.ToDouble(float.Parse((height - 0.04f).ToString("0.00")) / 0.045f));
            columns = new List<ListViewColumn>();
            items = new List<ListViewItem>();
            visibleItems = new List<ListViewItem>();
            allUsedElements = new List<OverlayElement>();

            element = listview;

            LoadColumns(columnNames);
        }

        public void LoadColumns(List<string> columnNames)
        {
            if (columns != null && columns.Count > 0)
            {
                foreach (var column in columns)
                {
                    Control.nukeOverlayElement(column.ColumnEnity);
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

        public ListViewItem NewItem()
        {
            ListViewItem item = new ListViewItem();
            return item;
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
                if (items.Count >= maxShowItem)
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

            

            if (items.Count >= maxShowItem)
            {
                drag.Show();
                drag.Height = (float)(maxShowItem * 0.045f * Convert.ToDouble(maxShowItem / items.Count));
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
                Control.nukeOverlayElement(columns[i].ColumnEnity);
            }
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].Items.Count; j++)
                {
                    Control.nukeOverlayElement(items[i].Items[j]);
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

        public void cursorMoved(Vector2 cursorPos)
        {
        }

        public void cursorPressed(Vector2 cursorPos)
        {
            bool found = false;
            int idx = 0;
            if (selectedItem != null)
                selectedItem.MaterialName = "SdkTrays/MiniTray";
            for (int i = 0; i < items.Count; i++)
            {
                for (idx = 0; idx < items[i].Items.Count; idx++)
                {
                    if (Control.isPositionInElement(items[i].Items[idx], cursorPos) && !found)
                    {
                        //we found the which item we click, let's trun its material!
                        selectedItem = items[i];
                        found = true;
                        idx = -1;
                    }
                    else if (idx < items[i].Items.Count && found)
                    {
                        items[i].Items[idx].MaterialName = "AMGE/Engine/Listview/over";
                    }
                    if(idx == items[i].Items.Count - 1 && found)
                    {
                        found = false;
                        if (SelectionChanged != null)
                        {
                            SelectionChanged.Invoke(this, new ListViewSelectionChangedArgs() { item = items[i] });
                        }
                        return;
                    }
                }
            }
        }

        public void cursorReleased(Vector2 cursorPos)
        {
        }

        public void keyPressed(uint text)
        {
        }

        public void keyReleased(uint text)
        {
        }
    }
}
