using MogreFreeSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AMOFGameEngine.Sound
{
    public enum PlayMode
    {
        Loop,//Will play the sound list one by one
        Random//When play one finished, choose a sound randomly in the sound list, and play
    }
    public enum SoundType
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
        private SoundType type;
        private SoundStatus status;
        private List<SoundObject> soundList;
        private int currentIndex;
        private bool disposed;
        private bool? disposing;
        private BackgroundWorker playThread;
        private Random rand;

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
        public SoundType PlayType
        {
            get { return type; }
            set { type = value; }
        }

        public GameSound()
        {
            type = AMOFGameEngine.Sound.SoundType.Empty;
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

            currentIndex = 0;
            status = SoundStatus.Playing;
            if (soundList.Count == 0)
            {
                return;
            }
            playThread.DoWork += ((o, e) => {
                while (true)
                {
                    if (status == SoundStatus.Stopped)
                    {
                        status = SoundStatus.Ready;
                        break;
                    }
                    else
                    {
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
            });
            playThread.RunWorkerAsync();
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
    }
}
