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
    /// <summary>
    /// Main entry to the Mod
    /// </summary>
    public class ModMain : IMod
    {
        public event EventHandler<ModEventArgs> ModStateChangedEvent;//0-Stop;1-Run

        Root root;
        RenderWindow win;
        Mouse mouse;
        Keyboard keyboard;
        SceneManager scm;
        Viewport vp;
        SdkTrayManager trayMgr;
        NameValuePairList modInfo;

        public bool SetupMod(Root root,RenderWindow win,SdkTrayManager trayMgr, Mouse mouse, Keyboard keyboard)
        {
            this.root=root;
            this.win = win;
            this.mouse = mouse;
            this.keyboard = keyboard;
            this.trayMgr = trayMgr;
            modInfo = new NameValuePairList();
            
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

        /// <summary>
        /// Start Single Player in Mod
        /// </summary>
        public void StartModSP()
        {
            SampleScene scene = new SampleScene(scm, vp,trayMgr, mouse, keyboard);
            scene.ModStateChangedEvent += new EventHandler<ModEventArgs>(scene_ModStateChangedEvent);
            scene.Enter();
            //scene.Update();
        }

        /// <summary>
        /// Start Multiplayer in Mod
        /// </summary>
        public void StartModMP()
        {
            SampleSceneMP sceneMP = new SampleSceneMP(scm, vp, trayMgr, mouse, keyboard);
            sceneMP.ModStateChangedEvent += new EventHandler<ModEventArgs>(sceneMP_ModStateChangedEvent);
            sceneMP.Enter();
        }

        void sceneMP_ModStateChangedEvent(object sender, ModEventArgs e)
        {
            if (ModStateChangedEvent != null)
            {
                ModStateChangedEvent(sender, e);
            }
        }


        /// <summary>
        /// Load Saved In Mod
        /// </summary>
        public void LoadModSavedState()
        {

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
