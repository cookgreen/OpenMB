using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using NVorbis;

namespace AMOFGameEngine.Sound
{
    enum OGGSTATUS
    {
        OGGS_STOP,
        OGGS_PLAYING,
        OGGS_PAUSE
    }

    class OggSound :IDisposable
    {
        private NAudio.Vorbis.VorbisWaveReader vorbis;
        private NAudio.Wave.WaveOut waveout;
        private string _oggfilename;
        public string OggFileName
        {
            get
            {
                return _oggfilename;
            }
            set
            {
                _oggfilename = value;
            }
        }
        private int _oggstatus;
        public int OggStatus
        {
            get
            {
                return _oggstatus;
            }
        }
        public OggSound()
        {
            waveout = null;
            vorbis = null;
            _oggstatus = (int)OGGSTATUS.OGGS_STOP;
        }
        public void PlayOgg()
        {
            vorbis = new NAudio.Vorbis.VorbisWaveReader(_oggfilename);
            waveout = new NAudio.Wave.WaveOut();
            waveout.Init(vorbis);
            waveout.Play();
            _oggstatus = (int)OGGSTATUS.OGGS_PLAYING;

        }
        public void PauseOgg()
        {
            if (waveout != null && vorbis != null)
            {
                waveout.Pause();
                _oggstatus = (int)OGGSTATUS.OGGS_PAUSE;
            }
        }
        public void StopOgg()
        {
            if (waveout != null && vorbis != null)
            {
                waveout.Stop();
                _oggstatus = (int)OGGSTATUS.OGGS_STOP;
            }
        }

        public void Dispose()
        {
            vorbis.Dispose();
            waveout.Dispose();
        }
    }
}
