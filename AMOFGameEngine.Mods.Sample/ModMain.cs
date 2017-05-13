using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods.Common;

namespace AMOFGameEngine.Mods.Sample
{
    class ModMain : IMod
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run

        Root root;
        RenderWindow win;
        Mouse mouse;
        Keyboard keyboard;
        SceneManager scm;
        Viewport vp;
        SdkTrayManager trayMgr;

        public bool SetupMod(Root root,RenderWindow win,SdkTrayManager trayMgr, Mouse mouse, Keyboard keyboard)
        {
            this.root=root;
            this.win = win;
            this.mouse = mouse;
            this.keyboard = keyboard;
            try
            {
                LocateResources();
                CreateSceneMgr();
                SetupView();

                return true;
            }
            catch 
            {
                return false;
            }
        }

        void LocateResources()
        {

        }

        void CreateSceneMgr()
        {
            scm = root.CreateSceneManager(SceneType.ST_GENERIC);
        }

        void SetupView()
        {
            win.RemoveAllViewports();
            vp = win.AddViewport(null);
        }

        public void StartMod()
        {
            SampleScene scene = new SampleScene(scm, vp,trayMgr, mouse, keyboard);
            scene.ModStateChangedEvent += new EventHandler<ModEventArgs>(scene_ModStateChangedEvent);
            scene.Enter();
            //scene.Update();
        }

        void scene_ModStateChangedEvent(object sender, ModEventArgs e)
        {
            if (ModStateChangedEvent != null)
            {
                ModStateChangedEvent(sender, e);
            }
        }

        public void StopMod()
        {
            throw new NotImplementedException();
        }
    }
}
