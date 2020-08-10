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
using OpenMB.Mods;
using System.ComponentModel;

namespace OpenMB.Sound
{
	public enum SoundState
	{
		Stopped,
		Playing,
		Paused
	}
	public class MusicSoundManager : IDisposable, IInitializeMod
	{
		private SoundManager soundEngine;
		private List<GameSound> musicLst;
		private List<SoundEntity> entities;
		private GameSound currentSound;
		private bool disposed;
		private Mods.ModData modData;
		private bool hasSound;
		private bool hasMusic;
		private Queue<GameSound> musicPlayQueue;

		public GameSound CurrentSound
		{
			get { return currentSound; }
			set { currentSound = value; }
		}

		public static MusicSoundManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MusicSoundManager();
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

		static MusicSoundManager instance;

		public MusicSoundManager()
		{
			musicLst = new List<GameSound>();
			currentSound = null;
			entities = new List<SoundEntity>();
			musicPlayQueue = new Queue<GameSound>();
		}

		public void InitSystem(bool hasMusic, bool hasSound)
		{
			this.hasMusic = hasMusic;
			this.hasSound = hasSound;
		}

		public bool InitSound(Camera cam)
		{
			try
			{
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
					music.AddSound(soundEngine.CreateAmbientSound(findMusicFileByID(track.ID), track.ID, true, false));
					music.PlayType = track.PlayType;
					musicLst.Add(music);
				}
				return true;
			}
			catch (Exception ex)
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

		public void PlayMusicByType(PlayType playType)
		{
			if (hasMusic)
			{
				var foundedMusicSound = musicLst.Where(o => o.PlayType == playType);
				if (foundedMusicSound.Count() == 0)
				{
					return;
				}

				Random rk = new Random();
				int idx = -1;

				GameSound sound = null;

				switch (playType)
				{
					case PlayType.MainMenu:
						idx = rk.Next(0, foundedMusicSound.Count());
						break;
					case PlayType.Scene:
						idx = rk.Next(0, musicLst.Count);
						break;
				}
				sound = foundedMusicSound.ElementAt(idx);
				musicPlayQueue.Enqueue(sound);
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

		private string findMusicFileByID(string musicID)
		{
			string musicfile = string.Empty;
			var result = from musicDfn in modData.MusicInfos
						 where musicDfn.ID == musicID
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
						 where soundDfn.ID == soundID
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

		public void InitMod(ModData modData)
		{
			this.modData = modData;
		}

		public void Update(float timeSinceLastFrame)
		{
			if (musicPlayQueue.Count == 0)
			{
				return;
			}

			GameSound sound = musicPlayQueue.Peek();

			if (currentSound != null && currentSound.PlayStatus == SoundStatus.Playing)
			{
				return;
			}

			if (currentSound == null || currentSound.ID != sound.ID) //Not the same sound or music
			{
				currentSound = sound;
				currentSound.Play(PlayMode.Random);
				musicPlayQueue.Dequeue();
			}

			currentSound.Update();
		}
	}
}
