using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenMB.Modio;
using Newtonsoft.Json.Linq;

namespace OpenMB.Screen
{
	public class ModBrowserScreen : Screen
	{
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
            Modio.Client client = new Modio.Client(Common.OPENMB_API_KEY, null);
			client.GetModsAsync(Common.OPENMB_MODIO_ID);
            client.GetResultDataFinished += Client_GetResultDataFinished;

			//Create a ui
		}

        private void Client_GetResultDataFinished(object obj)
        {
			object[] arr = obj as object[];
			if (arr[0].ToString() == "finished")
			{
				var retData = JsonConvert.DeserializeObject<ResultData>(arr[2].ToString());
				JArray jarr = retData.data as JArray;
				for (int i = 0; i < jarr.Count; i++)
				{
					JToken token = jarr[i];
					Mod mod = token.ToObject(typeof(Mod)) as Mod;
				}
			}
        }
    }
}
