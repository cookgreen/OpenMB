using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AMOFGameEngine.Forms.Model
{
    public class GameConfigure : Configure
    {
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
        public GameConfigure()
        {
            avaliableLocates = new BindingList<string>();
        }
    }
}
