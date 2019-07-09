using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OpenMB.Forms.Model
{
    public class GameConfigure : Configure
    {
        private bool isEnableEditMode;
        private string currentSelectedLocate;
        private BindingList<string> avaliableLocates;
        public string CurrentSelectedLocate
        {
            get
            {
                return currentSelectedLocate;
            }
            set
            {
                currentSelectedLocate = value;
                OnPropertyChanged("CurrentSelectedLocate");
            }
        }
        public BindingList<string> AvaliableLocates
        {
            get
            {
                return avaliableLocates;
            }
            set
            {
                avaliableLocates = value;
                OnPropertyChanged("AvaliableLocates");
            }
        }

        public bool IsEnableEditMode
        {
            get
            {
                return isEnableEditMode;
            }

            set
            {
                isEnableEditMode = value;
                OnPropertyChanged("IsEnableEditMode");
            }
        }

        public bool IsEnableCheatMode { get; internal set; }

        public GameConfigure()
        {
            avaliableLocates = new BindingList<string>();
        }
    }
}
