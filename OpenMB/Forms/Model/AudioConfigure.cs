using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Forms.Model
{
    public class AudioConfigure : Configure
    {
        private bool isEnableSound;
        private bool isEnableMusic;
        public bool IsEnableSound
        {
            get
            {
                return isEnableSound;
            }
            set
            {
                isEnableSound = value;
                OnPropertyChanged("IsEnableSound");
            }
        }
        public bool IsEnableMusic
        {
            get
            {
                return isEnableMusic;
            }
            set
            {
                isEnableMusic = value;
                OnPropertyChanged("IsEnableMusic");
            }
        }
    }
}
