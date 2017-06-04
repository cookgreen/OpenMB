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
    public class GameSound : OggSound
    {
        string soundID;

        public string ID
        {
            get { return soundID; }
            set { soundID = value; }
        }
        SoundType st;

        public SoundType SoundType
        {
            get { return st; }
            set { st = value; }
        }

        public GameSound(string soundName)
            : base(soundName)
        {
            st = Sound.SoundType.Empty;
        }

        public GameSound(string soundID,string soundName)
            : base(soundName)
        {
            st = Sound.SoundType.Empty;
            this.soundID = soundID;
        }

        public void Play()
        {
            base.PlayOgg();
            GameManager.Singleton.mSoundMgr.CurrentSound = this;
        }

        public void Stop()
        {
            base.StopOgg();
            GameManager.Singleton.mSoundMgr.CurrentSound = null;
        }
    }
}
