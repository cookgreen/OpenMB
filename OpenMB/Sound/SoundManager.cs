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
using System.Threading;

namespace OpenMB.Sound
{
    public enum SoundState
    {
        Stopped,
        Playing,
        Paused
    }
    public class SoundManager : IDisposable
    {
        private MogreFreeSL.SoundManager soundEngine;
        private List<GameSound> musicLst;
        private List<SoundEntity> entities;
        private GameSound currentSound;
        private bool disposed;
        private Mods.ModData modData;
        private bool hasSound;
        private bool hasMusic;

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

        public bool HasSound
        {
            get
            {
                return hasSound;
            }
        }

        public bool HasMusic
        {
            get
            {
                return hasMusic;
            }
        }

        static SoundManager instance;

        public SoundManager()
        {
            musicLst = new List<GameSound>();
            currentSound = null;
            entities = new List<SoundEntity>();
        }

        public void InitSystem(bool hasMusic, bool hasSound)
        {
            this.hasMusic = hasMusic;
            this.hasSound = hasSound;
        }
        
        public bool InitSound(Camera cam, Mods.ModData modData)
        {
            try
            {
                this.modData = modData;
                if (!hasMusic && !hasSound)
                {
                    return false;
                }
                soundEngine = MogreFreeSL.SoundManager.Instance;
                soundEngine.InitializeSound(FSL_SOUND_SYSTEM.FSL_SS_DIRECTSOUND, cam);
                var tracks = modData.MusicInfos;
                foreach (var track in tracks)
                {
                    GameSound music = new GameSound();
                    music.AddSound(soundEngine.CreateAmbientSound(findMusicFileByID(track.Id), track.Id, true, false));
                    music.PlayType = track.PlayType;
                    musicLst.Add(music);
                }
                return true;
            }
            catch(Exception ex)
            {
                GameManager.Instance.log.LogMessage(ex.Message, LogMessage.LogType.Error);
                return false;
            }
        }

        public void PlayMusicByID(string musicID)
        {
            if (hasMusic)
            {
                if (currentSound != null)
                {
                    currentSound.Stop();
                }
                for (int i = 0; i < musicLst.Count; i++)
                {
                    if (musicLst[i].Sound[0].Name == musicID)
                    {
                        currentSound = musicLst[i];
                        currentSound.Play();
                    }
                }
            }
        }

        public void PlayMusicByType(SoundType soundType)
        {
            if (hasMusic)
            {
                var ret = musicLst.Where(o => o.PlayType == soundType);
                switch (soundType)
                {
                    case SoundType.MainMenu:
                        ret.ElementAt(0).Play();
                        break;
                    case SoundType.Scene:
                        Thread sceneMusicTh = new Thread(() =>
                        {
                            Random rk = new Random();
                            int idx = rk.Next(0, musicLst.Count);
                            while (true)
                            {
                                if (!ret.ElementAt(idx).Sound.ElementAt(0).IsPlaying())
                                {
                                    ret.ElementAt(idx).Play();
                                }
                                else
                                {
                                    idx = rk.Next(0, musicLst.Count);
                                }
                            }
                        });
                        break;
                }
            }
        }

        public void PlaySoundAtNode(SceneNode node, string soundID)
        {
            if (hasSound)
            {
                SoundEntity entity = null;
                string soundFile = findSoundFileByID(soundID);
                if (!string.IsNullOrEmpty(soundFile))
                {
                    entity = soundEngine.CreateSoundEntity(findSoundFileByID(soundID), node, "", false, false);
                    entity.Play();
                    entities.Add(entity);
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

                if (musicLst != null)
                {
                    musicLst.Clear();
                }
            }
            disposed = true;
        }

        public void Update(float timeSinceLastFrame)
        {
            int idx = 0;
            while (idx < entities.Count)
            {
                if (!entities[idx].IsPlaying() && !entities[idx].IsPaused())
                    entities.Remove(entities[idx]);
                else
                    idx++;
            }
        }

        private string findMusicFileByID(string musicID)
        {
            string musicfile = string.Empty;
            var result = from musicDfn in modData.MusicInfos
                         where musicDfn.Id == musicID
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
                    musicfile = string.Format("{0}//{1}//{2}", Environment.CurrentDirectory, modData.MusicDir, result.First().File);
                }
                else if (result.First().Type == Mods.XML.TrackType.ModuleTrack)
                {
                    musicfile = string.Format("{0}//{1}//{2}", modData.BasicInfo.InstallPath, modData.MusicDir, result.First().File);
                }
                if (File.Exists(musicfile))
                {
                    return musicfile;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private string findSoundFileByID(string soundID)
        {
            string soundFile = string.Empty;
            var result = from soundDfn in modData.SoundInfos
                         where soundDfn.Id == soundID
                         select soundDfn;
            if (result.Count() == 1)
            {
                if (CurrentSound != null)
                {
                    CurrentSound.Stop();
                }
                currentSound = new GameSound();
                soundFile = string.Format("{0}//Music//{1}", modData.BasicInfo.InstallPath, result.First().File);
                if (File.Exists(soundFile))
                {
                    return soundFile;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
