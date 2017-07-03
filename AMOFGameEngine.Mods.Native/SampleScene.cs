using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using AMOFGameEngine.Mods;
using AMOFGameEngine.UI;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.Mods.Sample
{
    public class SampleScene
    {
        protected SceneManager scm;
        protected Camera cam;
        protected Viewport vp;
        protected Mouse mouse;
        protected Keyboard keyboard;
        protected SdkTrayManager trayMgr;
        protected CharacterManager characterMgr;

        public virtual void Enter() { }
        public virtual void Leave() { }
        public virtual void Update(float timeSinceLastFrame) { }
    }
}
