using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using NVorbis;

namespace AMOFGameEngine
{
    class OggSound
    {
        private NAudio.Vorbis.VorbisWaveReader _vorbis;
        private NAudio.Wave.WaveOut _waveout;
        public string OggFileName;
        public OggSound()
        {
            _waveout = null;
            _vorbis = null;
        }
        /*public OggSound Singleton
        {
            get
            {
                if (ogginstance == null)
                {
                    ogginstance = new OggSound();
                }
                return ogginstance;
            }
        }*/
        public void PlayOgg()
        {;
            _vorbis = new NAudio.Vorbis.VorbisWaveReader(OggFileName);
            _waveout = new NAudio.Wave.WaveOut();
            _waveout.Init(_vorbis);
            _waveout.Play();
        }
        public void PauseOgg()
        {
            if (_waveout != null && _vorbis != null)
            {
                _waveout.Pause();
            }
        }
        public void StopOgg()
        {
            if(_waveout!=null && _vorbis!=null)
            {
                _waveout.Stop();
            }
        }
    }
}
