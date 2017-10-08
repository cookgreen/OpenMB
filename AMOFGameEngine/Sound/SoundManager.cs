using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NVorbis;
using NAudio.Vorbis;

namespace AMOFGameEngine.Sound
{
    public enum SoundState
    {
        Stopped,
        Playing,
        Paused
    }
    public class SoundManager : IDisposable
    {
        private NAudio.Wave.WaveOut soundEngine;
        private List<GameSound> soundLst;
        private GameSound currentSound;
        private bool disposed;

        public GameSound CurrentSound
        {
            get { return currentSound; }
            set { currentSound = value; }
        }

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }
        static SoundManager instance;

        public SoundManager()
        {
            soundLst = new List<GameSound>();
            currentSound = null;
        }

       

        public void SystemInit()
        {
            soundEngine = new NAudio.Wave.WaveOut();
        }

        public void PlaySoundByType(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.MainMenu:
                    var result = from sound in soundLst
                                 where sound.SoundType == SoundType.MainMenu
                                 select sound;
                    if (result.Count() == 1)
                    {
                        if (CurrentSound != null)
                        {
                            CurrentSound.Stop();
                        }
                        GameSound sound = result.First();
                        sound.Play();
                    }
                    break;
                case SoundType.Scene:
                    var result2 = from sound in soundLst
                                 where sound.SoundType == SoundType.Scene
                                 select sound;
                    if (result2.Count() > 0)
                    {
                        if (CurrentSound != null)
                        {
                            CurrentSound.Stop();
                        }
                        int randIndex = new Random().Next(0, result2.Count() - 1);
                        GameSound sound = result2.ElementAt(randIndex);
                        sound.Play();
                    }
                    break;
            }
        }

        public void PlaySoundByID(string soundID)
        {
            var result = from sound in soundLst
                         where sound.ID == soundID
                         select sound;
            if (result.Count() == 1)
            {
                if (CurrentSound != null)
                {
                    CurrentSound.Stop();
                }
                GameSound sound = result.First();
                sound.Play();
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                if (currentSound != null)
                {
                    currentSound.Dispose();
                    currentSound = null;
                }

                if (soundLst != null)
                {
                    soundLst.Clear();
                }
            }
            disposed = true;
        }

        public void Update(float timeSinceLastFrame)
        {
        }
    }
}
