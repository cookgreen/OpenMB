using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenMB.Modio;
using Newtonsoft.Json.Linq;
using OpenMB.UI;
using System.Windows.Forms;
using OpenMB.UI.Widgets;

namespace OpenMB.Screen
{
	public class ModBrowserScreen : Screen
	{
		private Dictionary<string, Mod> modList;
        private PanelScrollableWidget browserMainPanel;
        private SimpleStaticTextWidget txtMessage;
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
			browserMainPanel = UIManager.Instance.CreateScrollablePanel("modBrowserMainPanel", 0.9f, 0.9f, 0.05f, 0.05f);
			browserMainPanel.Material = "SdkTrays/MiniTray";
			txtMessage = new SimpleStaticTextWidget("message", "Fetching mods...", 0.2f, false, new Mogre.ColourValue());
			browserMainPanel.AddWidget(1, 1, txtMessage, AlignMode.Center, AlignMode.Center, DockMode.Center);
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

					CreateModCard(mod, currentRow, currentCol);

					modList.Add(mod.name_id, mod);

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

		private void CreateModCard(Mod mod, int currentRow, int currentCol)
		{
			PanelWidget modPreviewWidget = new PanelWidget("mod_panel_" + mod.name_id, 0, 0.3f, 0, 0, 2, 1, false);
			modPreviewWidget.ChangeRow(UI.ValueType.Percent, 100);
			modPreviewWidget.ChangeRow(UI.ValueType.Abosulte, 0.3f, 2);
			modPreviewWidget.Material = "SdkTrays/MiniTray";

			browserMainPanel.ChangeRow(UI.ValueType.Abosulte, modPreviewWidget.Height, currentRow);
			browserMainPanel.AddWidget(currentRow, currentCol, modPreviewWidget, AlignMode.Center, AlignMode.Center, DockMode.Fill);

			PanelMaterialWidget pictureWidget = new PanelMaterialWidget("mod_pic_" + mod.name_id, "error.png");
			modPreviewWidget.AddWidget(1, 1, pictureWidget, AlignMode.Center, AlignMode.Center, DockMode.Fill);

			PanelWidget modInfoWidget = new PanelWidget("mod_info_panel_" + mod.name_id, 0, 0, 0, 0, 1, 2, false);
			modInfoWidget.ChangeCol(UI.ValueType.Percent, 80);
			modInfoWidget.ChangeCol(UI.ValueType.Percent, 20, 2);

			modPreviewWidget.AddWidget(2, 1, modInfoWidget, AlignMode.Center, AlignMode.Center, DockMode.Fill);

			SimpleStaticTextWidget modNameWidget = new SimpleStaticTextWidget("mod_text_" + mod.name_id, mod.name, 0.2f, false, new Mogre.ColourValue());
			modInfoWidget.AddWidget(1, 1, modNameWidget, AlignMode.Left, AlignMode.Center);

			SimpleButtonWidget btnModSubscribeWidget = new SimpleButtonWidget("btnModSubscribeWidget_" + mod.name_id, "Subscribe", 0.8f, 0.7f);
			btnModSubscribeWidget.OnClick += (o) =>
			{
				OnScreenEventChanged?.Invoke(btnModSubscribeWidget.Name, null);
			};

			IBackendTask downloadModThumbTask = new DownloadBackendTask(mod.logo.original, "./Media/Engine/Download/"+mod.name_id+"_thumb.png");
			BackendTaskManager.Instance.EnqueueTask(downloadModThumbTask);
			BackendTaskManager.Instance.TaskEnded += (o) =>
			{
				pictureWidget.ChangeTexture(o.ToString());
			};

			modInfoWidget.AddWidget(1, 2, btnModSubscribeWidget, AlignMode.Center, AlignMode.Center);
		}

        public override void Exit()
		{
			base.Exit();

			UIManager.Instance.DestroyAllWidgets();
			OnScreenExit?.Invoke();
		}
    }
}
