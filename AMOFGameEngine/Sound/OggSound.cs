using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using NVorbis;

namespace AMOFGameEngine.Sound
{
    /// <summary>
    /// Specific Class to handle ogg format sound file
    /// </summary>
    public class OggSound : ISound
    {
        private NAudio.Vorbis.VorbisWaveReader oggReader;
        private NAudio.Wave.WaveOut soundEngine;
        private string fileName;
        private SoundState state;
        public OggSound(string oggFileName,NAudio.Wave.WaveOut engine)
        {
            this.fileName = oggFileName;
            soundEngine = engine;
            oggReader = new NAudio.Vorbis.VorbisWaveReader(fileName);
            soundEngine.Init(oggReader);
            state = SoundState.Stopped;
        }
        public void Play()
        {
            soundEngine.Play();
            state = SoundState.Playing;
        }
        public void Pause()
        {
            if (soundEngine != null && oggReader != null)
            {
                soundEngine.Pause();
                state = SoundState.Paused;
            }
        }
        public void Stop()
        {
            if (soundEngine != null && oggReader != null)
            {
                soundEngine.Stop();
                state = SoundState.Stopped;
            }
        }

        public void Resume()
        {
            if (state == SoundState.Paused)
            {
                soundEngine.Resume();
                state = SoundState.Playing;
            }
        }

        public void Dispose()
        {
            oggReader.Dispose();
            soundEngine.Dispose();
        }

    }
}
