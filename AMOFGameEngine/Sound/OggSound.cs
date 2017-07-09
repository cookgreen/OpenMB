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

    public class OggSound :IDisposable
    {
        private NAudio.Vorbis.VorbisWaveReader vorbis;
        private NAudio.Wave.WaveOut waveout;
        private string _oggfilename;
        private int _oggstatus;
        protected int OggStatus
        {
            get
            {
                return _oggstatus;
            }
        }
        public OggSound(string oggFileName)
        {
            this._oggfilename = oggFileName;
            waveout = null;
            vorbis = null;
            _oggstatus = (int)OGGSTATUS.OGGS_STOP;
        }
        public void Play()
        {
            vorbis = new NAudio.Vorbis.VorbisWaveReader(_oggfilename);
            waveout = new NAudio.Wave.WaveOut();
            waveout.Init(vorbis);
            waveout.Play();
            _oggstatus = (int)OGGSTATUS.OGGS_PLAYING;

        }
        public void Pause()
        {
            if (waveout != null && vorbis != null)
            {
                waveout.Pause();
                _oggstatus = (int)OGGSTATUS.OGGS_PAUSE;
            }
        }
        public void Stop()
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
