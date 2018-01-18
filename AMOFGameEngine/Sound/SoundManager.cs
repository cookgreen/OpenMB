using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NVorbis;
using NAudio.Vorbis;
using Mogre;
using MogreFreeSL;
using System.IO;

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
        private NAudio.Wave.WaveOut musicEngine;
        private MogreFreeSL.SoundManager soundEngine;
        private List<GameSound> soundLst;
        private GameSound currentSound;
        private bool disposed;
        private Mods.ModData modData;
        private bool noSound;
        private bool noMusic;

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
            musicEngine = new NAudio.Wave.WaveOut();
            currentSound = null;
        }

       

        public bool InitSound(Camera cam, Mods.ModData modData)
        {
            this.modData = modData;
            return MogreFreeSL.SoundManager.Instance.InitializeSound(FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND, cam);
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

        public void PlayMusicByID(string soundID)
        {
            if (!noMusic)
            {
                string musicfile = string.Empty;
                var result = from musicDfn in modData.MusicInfos
                             where musicDfn.Id == soundID
                             select musicDfn;
                if (result.Count() == 1)
                {
                    if (CurrentSound != null)
                    {
                        CurrentSound.Stop();
                    }
                    currentSound = new GameSound();
                    if (result.First().Type == Mods.XML.TrackType.EngineTrack)
                    {
                        musicfile = string.Format("{0}//Music//{1}", Environment.CurrentDirectory, result.First().File);
                    }
                    else if (result.First().Type == Mods.XML.TrackType.ModuleTrack)
                    {
                        musicfile = string.Format("{0}//Music//{1}", modData.BasicInfo.InstallPath, result.First().File);
                    }
                    if (File.Exists(musicfile))
                    {
                        currentSound.AddSound(new OggSound(musicfile, musicEngine));
                        currentSound.Play();
                    }
                }
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
