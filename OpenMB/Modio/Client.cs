using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			string url = e.Argument.ToString();
		}

		public void GetModsAsync(int game_id)
		{
			
		}
	}
}
