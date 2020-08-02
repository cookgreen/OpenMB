using Mogre;
using Mogre_Procedural.MogreBites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.UI.Widgets
{
	/// <summary>
	/// Basic selection menu widget
	/// </summary>
	public class SelectMenuWidget : Widget, IHasSubItems
	{
		protected BorderPanelOverlayElement smallBoxElement;
		protected BorderPanelOverlayElement expandedBoxElement;
		protected TextAreaOverlayElement textAreaElement;
		protected TextAreaOverlayElement smallTextAreaElement;
		protected BorderPanelOverlayElement scrollTrackElement;
		protected PanelOverlayElement scrollHandleElement;
		protected List<BorderPanelOverlayElement> itemElements = new List<Mogre.BorderPanelOverlayElement>();
		protected uint maxItemsShown;
		protected uint itemsShown;
		protected bool isCursorOver;
		protected bool isExpanded;
		protected bool isFitToContents;
		protected bool isDragging;
		protected int selectionIndex;
		protected int highlightIndex;
		protected int displayIndex;
		protected float dragOffset = 0f;
		public event Action<object, int> OnSelectedIndexChanged;
		public List<string> Items { get; private set; }

		public SelectMenuWidget(string name, string caption, float width, float boxWidth, uint maxItemsShown)
		{
			Items = new List<string>();
			highlightIndex = 0;
			displayIndex = 0;
			dragOffset = 0.0f;
			selectionIndex = -1;
			isFitToContents = false;
			isCursorOver = false;
			isExpanded = false;
			isDragging = false;
			this.maxItemsShown = maxItemsShown;
			itemsShown = 0;
			element = (Mogre.BorderPanelOverlayElement)Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenu", "BorderPanel", name);
			textAreaElement = (Mogre.TextAreaOverlayElement)((Mogre.OverlayContainer)element).GetChild(name + "/MenuCaption");
			smallBoxElement = (Mogre.BorderPanelOverlayElement)((Mogre.OverlayContainer)element).GetChild(name + "/MenuSmallBox");
			smallBoxElement.Width = (width - 10);
			smallTextAreaElement = (Mogre.TextAreaOverlayElement)smallBoxElement.GetChild(name + "/MenuSmallBox/MenuSmallText");
			element.Width = (width);

			if (boxWidth > 0f) // long style
			{
				if (width <= 0f)
					isFitToContents = true;
				smallBoxElement.Width = (boxWidth);
				smallBoxElement.Top = (2f);
				smallBoxElement.Left = (width - boxWidth - 5f);
				element.Height = (smallBoxElement.Height + 4f);
				textAreaElement.HorizontalAlignment = (GuiHorizontalAlignment.GHA_LEFT);
				textAreaElement.SetAlignment(Mogre.TextAreaOverlayElement.Alignment.Left);
				textAreaElement.Left = (12f);
				textAreaElement.Top = (10f);
			}

			expandedBoxElement = (Mogre.BorderPanelOverlayElement)((Mogre.OverlayContainer)element).GetChild(name + "/MenuExpandedBox");
			expandedBoxElement.Width = (smallBoxElement.Width + 10);
			expandedBoxElement.Hide();
			scrollTrackElement = (Mogre.BorderPanelOverlayElement)expandedBoxElement.GetChild(expandedBoxElement.Name + "/MenuScrollTrack");
			scrollHandleElement = (Mogre.PanelOverlayElement)scrollTrackElement.GetChild(scrollTrackElement.Name + "/MenuScrollHandle");

			setCaption(caption);
		}

		public bool Expanded()
		{
			return isExpanded;
		}

		public string getCaption()
		{
			return textAreaElement.Caption;
		}

		public void setCaption(string caption)
		{
			textAreaElement.Caption = (caption);
			if (isFitToContents)
			{
				element.Width = (GetCaptionWidth(caption, ref textAreaElement) + smallBoxElement.Width + 23f);
				smallBoxElement.Left = (element.Width - smallBoxElement.Width - 5f);
			}
		}

		public int GetNumItems()
		{
			return Items.Count;
		}

		public void SetItems(List<string> items)
		{
			this.Items = items;
			selectionIndex = -1;

			for (int i = 0; i < itemElements.Count; i++) // destroy all the item elements
			{
				NukeOverlayElement(itemElements[i]);
			}
			itemElements.Clear();

			itemsShown = System.Math.Max((uint)2, System.Math.Min(maxItemsShown, (uint)this.Items.Count));

			for (int i = 0; i < itemsShown; i++) // create all the item elements
			{
				Mogre.BorderPanelOverlayElement e = (Mogre.BorderPanelOverlayElement)Mogre.OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/SelectMenuItem", "BorderPanel", expandedBoxElement.Name + "/Item" + (i + 1).ToString());

				e.Top = (6 + i * (smallBoxElement.Height - 8));
				e.Width = (expandedBoxElement.Width - 32);

				expandedBoxElement.AddChild(e);
				itemElements.Add(e);
			}

			if (items.Count > 0)
				SelectItem(0, false);
			else
				smallTextAreaElement.Caption = string.Empty;
		}

		public void AddItem(string item)
		{
			Items.Add(item);
			SetItems(Items);
		}

		public void RemoveItem(string item)
		{
			int it = -1;

			for (int i = 0; i < Items.Count; i++)
			{
				if (item == Items[i])
				{
					it = i;
					break;
				}
			}

			if (it != -1)
			{
				Items.RemoveAt(it);
				if (Items.Count < itemsShown)
				{
					itemsShown = (uint)Items.Count;
					NukeOverlayElement(itemElements[itemElements.Count - 1]);
					if (itemElements.Count > 0)
						itemElements.RemoveAt(itemElements.Count - 1);//remove the end
				}
			}
			else
			{
				string desc = "Menu \"" + Name + "\" contains no item \"" + item + "\".";
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::removeItem");
			}
		}

		public void RemoveItem(uint index)
		{
			//stringVector.iterator it = new stringVector.iterator();
			int it = -1;
			//uint i = 0;
			//for (it = mItems.begin(); it != mItems.end(); it++)
			for (int j = 0; j < Items.Count; j++)
			{
				if (j == index)
				{
					it = j;
					break;
				}
				//i++;
			}

			//if (it != mItems.end())
			if (it != -1)
			{
				Items.RemoveAt(it);
				if (Items.Count < itemsShown)
				{
					itemsShown = (uint)Items.Count;
					NukeOverlayElement(itemElements[itemElements.Count - 1]);
					itemElements.RemoveAt(itemElements.Count - 1);//remove the end
				}
			}
			else
			{
				string desc = "Menu \"" + Name + "\" contains no item at position " + (index).ToString() + ".";
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::removeItem");
			}
		}

		public void ClearItems()
		{
			Items.Clear();
			selectionIndex = -1;
			smallTextAreaElement.Caption = ("");
		}

		public void SelectItem(uint index)
		{
			SelectItem(index, true);
		}

		public void SelectItem(uint index, bool notifyListener)
		{
			if (index >= Items.Count)
			{
				string desc = "Menu \"" + Name + "\" contains no item at position " + (index).ToString() + ".";
				OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::selectItem");
			}

			selectionIndex = (int)index;
			FitCaptionToArea(Items[(int)index], ref smallTextAreaElement, smallBoxElement.Width - smallTextAreaElement.Left * 2f);

			if (listener != null && notifyListener)
				listener.itemSelected(this);

			OnSelectedIndexChanged?.Invoke(this, (int)index);
		}

		public void SelectItem(string item)
		{
			SelectItem(item, true);
		}

		public void SelectItem(string item, bool notifyListener)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				if (item == Items[i])
				{
					SelectItem((uint)i, notifyListener);
					return;
				}
			}

			string desc = "Menu \"" + Name + "\" contains no item \"" + item + "\".";
			OGRE_EXCEPT("Mogre.Exception.ERR_ITEM_NOT_FOUND", desc, "SelectMenu::selectItem");
		}

		public string getSelectedItem()
		{
			if (selectionIndex == -1)
			{
				return "";
			}
			else
				return Items[selectionIndex];
		}

		public int getSelectionIndex()
		{
			return selectionIndex;
		}

		public override void CursorPressed(Mogre.Vector2 cursorPos)
		{
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

			if (isExpanded)
			{
				if (scrollHandleElement.IsVisible) // check for scrolling
				{
					Mogre.Vector2 co = Widget.CursorOffset(scrollHandleElement, cursorPos);

					if (co.SquaredLength <= 81f)
					{
						isDragging = true;
						dragOffset = co.y;
						return;
					}
					else if (Widget.IsCursorOver(scrollTrackElement, cursorPos))
					{
						float newTop = scrollHandleElement.Top + co.y;
						float lowerBoundary = scrollTrackElement.Height - scrollHandleElement.Height;
						scrollHandleElement.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

						float scrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
						setDisplayIndex((uint)(scrollPercentage * (Items.Count - itemElements.Count) + 0.5f));
						return;
					}
				}

				if (!IsCursorOver(expandedBoxElement, cursorPos, 3f))
					retract();
				else
				{
					float l = itemElements[0]._getDerivedLeft() * om.ViewportWidth + 5f;
					float t = itemElements[0]._getDerivedTop() * om.ViewportHeight + 5f;
					float r = l + itemElements[itemElements.Count - 1].Width - 10f;
					float b = itemElements[itemElements.Count - 1]._getDerivedTop() * om.ViewportHeight + itemElements[itemElements.Count - 1].Height - 5;

					if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b)
					{
						if (highlightIndex != selectionIndex)
							SelectItem((uint)highlightIndex);
						retract();
					}
				}
			}
			else
			{
				if (Items.Count < 2) // don't waste time showing a menu if there's no choice
					return;

				if (IsCursorOver(smallBoxElement, cursorPos, 4f))
				{
					expandedBoxElement.Show();
					smallBoxElement.Hide();

					// calculate how much vertical space we need
					float idealHeight = itemsShown * (smallBoxElement.Height - 8f) + 20f;
					expandedBoxElement.Height = (idealHeight);
					scrollTrackElement.Height = (expandedBoxElement.Height - 20f);

					expandedBoxElement.Left = (smallBoxElement.Left - 4f);

					// if the expanded menu goes down off the screen, make it go up instead
					if (smallBoxElement._getDerivedTop() * om.ViewportHeight + idealHeight > om.ViewportHeight)
					{
						expandedBoxElement.Top = (smallBoxElement.Top + smallBoxElement.Height - idealHeight + 3f);
						// if we're in thick style, hide the caption because it will interfere with the expanded menu
						if (textAreaElement.HorizontalAlignment == GuiHorizontalAlignment.GHA_CENTER)
							textAreaElement.Hide();
					}
					else
						expandedBoxElement.Top = (smallBoxElement.Top + 3f);

					isExpanded = true;
					highlightIndex = selectionIndex;
					setDisplayIndex((uint)highlightIndex);

					if (itemsShown < Items.Count) // update scrollbar position
					{
						scrollHandleElement.Show();
						float lowerBoundary = scrollTrackElement.Height - scrollHandleElement.Height;
						scrollHandleElement.Top = ((int)(displayIndex * lowerBoundary / (Items.Count - itemElements.Count)));
					}
					else
						scrollHandleElement.Hide();
				}
			}
		}

		public override void CursorReleased(Mogre.Vector2 cursorPos)
		{
			isDragging = false;
		}

		public override void CursorMoved(Mogre.Vector2 cursorPos)
		{
			Mogre.OverlayManager om = Mogre.OverlayManager.Singleton;

			if (isExpanded)
			{
				if (isDragging)
				{
					Mogre.Vector2 co = Widget.CursorOffset(scrollHandleElement, cursorPos);
					float newTop = scrollHandleElement.Top + co.y - dragOffset;
					float lowerBoundary = scrollTrackElement.Height - scrollHandleElement.Height;
					scrollHandleElement.Top = (UIMathHelper.clamp<int>((int)newTop, 0, (int)lowerBoundary));

					float scrollPercentage = UIMathHelper.clamp<float>(newTop / lowerBoundary, 0f, 1f);
					int newIndex = (int)(scrollPercentage * (Items.Count - itemElements.Count) + 0.5f);
					if (newIndex != displayIndex)
						setDisplayIndex((uint)newIndex);
					return;
				}

				float l = itemElements[0]._getDerivedLeft() * om.ViewportWidth + 5f;
				float t = itemElements[0]._getDerivedTop() * om.ViewportHeight + 5f;
				float r = l + itemElements[itemElements.Count - 1].Width - 10f;
				float b = itemElements[itemElements.Count - 1]._getDerivedTop() * om.ViewportHeight + itemElements[itemElements.Count - 1].Height - 5f;

				if (cursorPos.x >= l && cursorPos.x <= r && cursorPos.y >= t && cursorPos.y <= b)
				{
					int newIndex = (int)(displayIndex + (cursorPos.y - t) / (b - t) * itemElements.Count);
					if (highlightIndex != newIndex)
					{
						highlightIndex = newIndex;
						setDisplayIndex((uint)displayIndex);
					}
				}
			}
			else
			{
				if (IsCursorOver(smallBoxElement, cursorPos, 4f))
				{
					smallBoxElement.MaterialName = ("SdkTrays/MiniTextBox/Over");
					smallBoxElement.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
					isCursorOver = true;
				}
				else
				{
					if (isCursorOver)
					{
						smallBoxElement.MaterialName = ("SdkTrays/MiniTextBox");
						smallBoxElement.BorderMaterialName = ("SdkTrays/MiniTextBox");
						isCursorOver = false;
					}
				}
			}
		}

		public override void FocusLost()
		{
			if (expandedBoxElement.IsVisible)
				retract();
		}


		//        -----------------------------------------------------------------------------
		//		| Internal method - sets which item goes at the top of the expanded menu.
		//		-----------------------------------------------------------------------------
		protected void setDisplayIndex(uint index)
		{
			index = (uint)System.Math.Min((int)index, (int)(Items.Count - itemElements.Count));
			displayIndex = (int)index;
			Mogre.BorderPanelOverlayElement ie;
			Mogre.TextAreaOverlayElement ta;

			for (int i = 0; i < (int)itemElements.Count; i++)
			{
				ie = itemElements[i];
				ta = (Mogre.TextAreaOverlayElement)ie.GetChild(ie.Name + "/MenuItemText");

				FitCaptionToArea(Items[displayIndex + i], ref ta, ie.Width - 2f * ta.Left);

				if ((displayIndex + i) == highlightIndex)
				{
					ie.MaterialName = ("SdkTrays/MiniTextBox/Over");
					ie.BorderMaterialName = ("SdkTrays/MiniTextBox/Over");
				}
				else
				{
					ie.MaterialName = ("SdkTrays/MiniTextBox");
					ie.BorderMaterialName = ("SdkTrays/MiniTextBox");
				}
			}
		}

		//        -----------------------------------------------------------------------------
		//		| Internal method - cleans up an expanded menu.
		//		-----------------------------------------------------------------------------
		protected void retract()
		{
			isDragging = false;
			isExpanded = false;
			expandedBoxElement.Hide();
			textAreaElement.Show();
			smallBoxElement.Show();
			smallBoxElement.MaterialName = ("SdkTrays/MiniTextBox");
			smallBoxElement.BorderMaterialName = ("SdkTrays/MiniTextBox");
		}
	}
}
