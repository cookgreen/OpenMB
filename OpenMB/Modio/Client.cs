using OpenMB.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Modio
{
	public class Client
	{
		private string apiKey;
		private string accessToken;
		private BackgroundWorker worker;

		public event Action<object> GetResultDataFinished;
		public Client(string apiKey, string accessToken)
		{
			this.apiKey = apiKey;
			this.accessToken = accessToken;
			worker = new BackgroundWorker();
			worker.DoWork += Worker_DoWork;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
			worker.WorkerSupportsCancellation = true;
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			GetResultDataFinished?.Invoke(e.Result);
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			string url = e.Argument.ToString();
			object[] arr = e.Argument as object[];
			try
			{
				System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(arr[2].ToString());
				request.UserAgent = "UserAgent";
				var response = request.GetResponse();
				var stream = response.GetResponseStream();
				string responseJson = null;
				using (StreamReader reader = new StreamReader(stream))
				{
					responseJson = reader.ReadToEnd();
				}
				e.Result = new object[] {
					"finished",
					arr[1],
					responseJson
				};
			}
			catch (Exception ex)
			{
				e.Result = new object[] {
					"error",
					arr[1],
					ex.Message
				};
			}
		}

		public void GetModsAsync(int game_id)
		{
			string token = null;
			if (string.IsNullOrEmpty(apiKey))
			{
				token = accessToken;
			}
			else
			{
				token = apiKey;
			}
			string url = "https://api.mod.io/v1/games/" + game_id.ToString() + "/mods?api_key=" + token;
			worker.RunWorkerAsync(new object[] {
				"ready",
				"get_mods",
				url
			});
		}
	}
}
