using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.UI
{
    public class GameUI : SdkTrayListener
    {
        public GameUI()
        {
        }

        public virtual void Show() { }

        public virtual void Close() { }
    }
}
