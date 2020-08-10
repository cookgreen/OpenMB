using Ogre;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenMB.Core
{
	public class BackendTaskManager
	{
		private Queue<IBackendTask> tasks;
		private static BackendTaskManager instance;
		public static BackendTaskManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BackendTaskManager();
				}
				return instance;
			}
		}
		public event Action<object> TaskEnded;

		public BackendTaskManager()
		{
			tasks = new Queue<IBackendTask>();
		}

		public void EnqueueTask(IBackendTask newTask)
		{
			tasks.Enqueue(newTask);
			newTask.TaskEnded += Task_TaskEnded;
		}

		private void Task_TaskEnded(object returnData)
		{
			TaskEnded?.Invoke(returnData);
		}

		public void Update()
		{
			if (tasks.Count == 0)
			{
				return;
			}
			tasks.Dequeue().RunTask();
		}
	}

	public interface IBackendTask
	{
		event Action<object> TaskEnded;
		void RunTask();
	}

	public class DownloadBackendTask : IBackendTask
	{
		private BackgroundWorker worker;
		private string url;
		private string savedFileName;
		public event Action<object> TaskEnded;

		public DownloadBackendTask(string url, string savedFileName)
		{
			worker = new BackgroundWorker();
			worker.DoWork += Worker_DoWork;
			worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
			this.url = url;
			this.savedFileName = savedFileName;
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			string filename = (new DirectoryInfo(savedFileName)).Name;
			TaskEnded?.Invoke(filename);
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs e)
		{
			string url = e.Argument.ToString();
			WebClient client = new WebClient();
			client.DownloadFile(url, savedFileName);
		}

		public void RunTask()
		{
			worker.RunWorkerAsync(url);
		}
	}
}
