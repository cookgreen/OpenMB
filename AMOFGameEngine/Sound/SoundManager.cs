using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Sound
{
    public class SoundManager : IDisposable
    {
        private List<GameSound> soundLst;
        private GameSound currentSound;
        private bool disposed;

        public GameSound CurrentSound
        {
            get { return currentSound; }
            set { currentSound = value; }
        }

        public static SoundManager Singleton
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

       

        public void Init()
        {
            GameSound sound1 = new GameSound();
            sound1.ID = "MainMenu";
            sound1.AddSound(new OggSound("./Music/vivaldi_winter_allegro.ogg"));
            sound1.SoundType = SoundType.MainMenu;
            GameSound sound2 = new GameSound();
            sound2.ID = "YouReachNewlevel";
            sound2.AddSound(new OggSound(""));

            soundLst.Add(sound1);
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
    }
}
