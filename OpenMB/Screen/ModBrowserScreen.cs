using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenMB.Modio;
using Newtonsoft.Json.Linq;
using OpenMB.Widgets;

namespace OpenMB.Screen
{
	public class ModBrowserScreen : Screen
	{
		private Dictionary<string, Mod> modList;
        private PanelScrollableWidget browserMainPanel;
        private StaticTextRelative txtMessage;
        private const int BROWSER_EACHROW_SHOW_NUMBER = 3;
		private const int BROWSER_PAGE_SHOW_NUMBER = 20;

        public override event Action<string, string> OnScreenEventChanged;
		public override event Action OnScreenExit;

		public override string Name
		{
			get { return "ModBrowser"; }
		}
		public ModBrowserScreen()
		{
		}

		public override void Run()
		{
			modList = new Dictionary<string, Mod>();
            Client client = new Client(Common.OPENMB_API_KEY, null);
			client.GetModsAsync(Common.OPENMB_MODIO_ID);
            client.GetResultDataFinished += Client_GetResultDataFinished;

			//Create a ui
			browserMainPanel = UIManager.Instance.CreateScrollablePanel("modBrowserMainPanel", 0.8f, 0.7f, 0.1f, 0.15f);

			txtMessage = new StaticTextRelative("message", "Fetching mods...", 0.2f, false, new Mogre.ColourValue());
			browserMainPanel.AddWidgetRelative(1, 1, txtMessage, AlignMode.Center, AlignMode.Center, DockMode.Center);
		}

        private void Client_GetResultDataFinished(object obj)
		{
			object[] arr = obj as object[];
			if (arr[0].ToString() == "finished" &&
				arr[1].ToString() == "get_mods")
			{
				browserMainPanel.RemoveWidget(1, 1);

				var retData = JsonConvert.DeserializeObject<ResultData>(arr[2].ToString());
				JArray jarr = retData.data as JArray;

				int rowNumber = BROWSER_PAGE_SHOW_NUMBER / BROWSER_EACHROW_SHOW_NUMBER;
				if (BROWSER_PAGE_SHOW_NUMBER % BROWSER_EACHROW_SHOW_NUMBER != 0)
				{
					rowNumber++;
				}
				browserMainPanel.ChangeTotalCol(BROWSER_EACHROW_SHOW_NUMBER);
				browserMainPanel.ChangeTotalRow(rowNumber);

				int currentRow = 1;
				int currentCol = 1;
				for (int i = 0; i < jarr.Count; i++)
				{
					JToken token = jarr[i];
					Mod mod = token.ToObject(typeof(Mod)) as Mod;

					PanelTemplateWidget modPreviewWidge = new PanelTemplateWidget("mod_" + mod.name_id, "InventorySlot");
					modPreviewWidge.Height = 0.3f;
					modPreviewWidge.Padding.PaddingLeft = 0.01f;
					modPreviewWidge.Padding.PaddingTop = 0.01f;
					modPreviewWidge.Padding.PaddingRight = 0.01f;
					modPreviewWidge.Padding.PaddingDown = 0.01f;
					//browserMainPanel.ChangeRow(Widgets.ValueType.Abosulte, modPreviewWidge.Height, currentRow);
					browserMainPanel.AddWidget(currentRow, currentCol, modPreviewWidge, AlignMode.Center, DockMode.Fill);

					modList.Add(mod.name, mod);

					if ((i + 1) % BROWSER_EACHROW_SHOW_NUMBER == 0)
					{
						currentRow++;
						currentCol = 1;
					}
				}
			}
			else
			{
				txtMessage.Text = "No mods found!";
			}
        }
    }
}
