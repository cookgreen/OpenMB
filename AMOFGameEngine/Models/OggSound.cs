using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using NVorbis;

namespace AMOFGameEngine
{
    enum OGGSTATUS
    {
        OGGS_STOP,
        OGGS_PLAYING,
        OGGS_PAUSE
    }

    class OggSound
    {
        private NAudio.Vorbis.VorbisWaveReader _vorbis;
        private NAudio.Wave.WaveOut _waveout;
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
            _waveout = null;
            _vorbis = null;
            _oggstatus = (int)OGGSTATUS.OGGS_STOP;
        }
        public void PlayOgg()
        {
            _vorbis = new NAudio.Vorbis.VorbisWaveReader(_oggfilename);
            _waveout = new NAudio.Wave.WaveOut();
            _waveout.Init(_vorbis);
            _waveout.Play();
            _oggstatus = (int)OGGSTATUS.OGGS_PLAYING;

        }
        public void PauseOgg()
        {
            if (_waveout != null && _vorbis != null)
            {
                _waveout.Pause();
                _oggstatus = (int)OGGSTATUS.OGGS_PAUSE;
            }
        }
        public void StopOgg()
        {
            if(_waveout!=null && _vorbis!=null)
            {
                _waveout.Stop();
                _oggstatus = (int)OGGSTATUS.OGGS_STOP;
            }
        }
    }
}
