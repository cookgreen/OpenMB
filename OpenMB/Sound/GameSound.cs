using MogreFreeSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenMB.Sound
{
	public enum PlayMode
	{
		Loop,//Will play the sound list one by one
		Random//When play one finished, choose a sound randomly in the sound list, and play
	}

	public enum PlayType
	{
		Empty,
		MainMenu,//Will Play in the main menu
		Scene//Will play in the scene
	}

	public enum SoundStatus
	{
		Ready,
		Playing,
		Stopped
	}

	public class GameSound : IDisposable
	{
		private string soundID;
		private PlayType type;
		private SoundStatus status;
		private List<SoundObject> soundList;
		private int currentIndex;
		private bool disposed;
		private bool? disposing;
		private BackgroundWorker playThread;
		private Random rand;
		private PlayMode mode;

		public string ID
		{
			get { return soundID; }
			set { soundID = value; }
		}
		public List<SoundObject> Sound
		{
			get { return soundList; }
			set { soundList = value; }
		}
		public PlayType PlayType
		{
			get { return type; }
			set { type = value; }
		}

		public SoundStatus PlayStatus
		{
			get { return status; }
		}

		public GameSound()
		{
			type = OpenMB.Sound.PlayType.Empty;
			soundList = new List<SoundObject>();
			playThread = new BackgroundWorker();
			playThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(playThread_RunWorkerCompleted);
			playThread.WorkerSupportsCancellation = true;
			disposing = null;
			rand = new Random();
		}

		void playThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (disposing.HasValue)
			{
				playThread.Dispose();
			}
		}

		public void AddSound(SoundObject s)
		{
			soundList.Add(s);
		}

		public void Play(PlayMode mode = PlayMode.Loop)
		{
			if (status == SoundStatus.Playing)
			{
				status = SoundStatus.Stopped;
			}

			this.mode = mode;

			currentIndex = 0;
			status = SoundStatus.Playing;
		}
		public void Stop()
		{
			status = SoundStatus.Stopped;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			if (disposing)
			{
				this.disposing = disposing;

				if (playThread.IsBusy)
				{
					playThread.CancelAsync();
				}
				else if (!playThread.CancellationPending)
				{
					playThread.Dispose();
				}

				soundList.Clear();
				soundList = null;
			}
			disposed = true;
		}

		public void Update()
		{
			if (soundList.Count == 0)
			{
				return;
			}

			if (!soundList[currentIndex].IsPlaying())
			{
				soundList[currentIndex].Play();
			}
			else
			{
				while (true)//Wait until current sound finished
				{
					if (!soundList[currentIndex].IsPlaying())
					{
						switch (mode)
						{
							case PlayMode.Loop:
								if (currentIndex == soundList.Count - 1)
								{
									currentIndex = 0;
								}
								else
								{
									currentIndex++;
								}
								break;
							case PlayMode.Random:
								int rk = rand.Next(soundList.Count);
								currentIndex = rk;
								break;
						}
					}
				}
			}
		}
	}
}
