using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Sound
{
    public interface ISound : IDisposable
    {
        void Play();
        void Stop();
        void Pause();
        void Resume();
    }
}
