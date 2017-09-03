using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Sound
{
    public enum SoundType
    {
        Empty,
        MainMenu,
        Scene
    }
    public class GameSound : IDisposable
    {
        private string soundID;
        private SoundType st;
        private List<ISound> sound;
        private int currentIndex;
        private bool disposed;

        public string ID
        {
            get { return soundID; }
            set { soundID = value; }
        }
        public List<ISound> Sound
        {
            get { return sound; }
            set { sound = value; }
        }
        public SoundType SoundType
        {
            get { return st; }
            set { st = value; }
        }

        public GameSound()
        {
            st = AMOFGameEngine.Sound.SoundType.Empty;
            sound = new List<ISound>();
        }
        public void AddSound(OggSound s)
        {
            sound.Add(s);
        }
        public void Play()
        {
            if (sound.Count > 0)
            {
                Random rand = new Random();
                int rk=rand.Next(0,sound.Count);
                sound[rk].Play();
                currentIndex = rk;
            }
        }
        public void Stop()
        {
            if (sound.Count > 0)
            {
                sound[currentIndex].Stop();
            }
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
                sound.Clear();
                sound = null;
            }
            disposed = true;
        }
    }
}
