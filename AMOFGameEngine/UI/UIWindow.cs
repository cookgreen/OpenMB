using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.UI
{
    public class UIWindiw : SdkTrayListener
    {
        protected SdkTrayManager trayMgr;
        protected SceneManager scm;
        protected Camera cam;
        protected bool shutdown;
        UIManager parent;

        public UIWindiw(Camera cam)
        {
            this.trayMgr = GameManager.Singleton.mTrayMgr;
            this.cam = cam;
        }

        public void create(string winName,UIManager uiMgr)
        {
            parent = uiMgr;
            uiMgr.ManageWindow(winName, this);
        }

        public virtual void enter()
        {

        }

        public virtual void close()
        {
            trayMgr.destroyAllWidgets();
        }

        public virtual void update(double timeSinceLastFrame)
        {

        }

        protected void Shutdown() { parent.Shutdown(); }
    }
}
