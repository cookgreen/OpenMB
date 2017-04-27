using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using System.Collections;

namespace AMOFGameEngine.Mods.Common
{
    public class Mod
    {
        protected NameValuePairList modInfo;
        protected Root root;
        protected RenderWindow window;
        protected Keyboard mKeyboard;
        protected Mouse mMouse;
        protected SceneManager scm;
        protected bool bDone;
        protected bool bResourcesLoaded;
        protected bool bContentSetup;
        protected SdkTrayManager mTrayMgr;
        protected LogManager mLog;
        protected Timer mTimer;

        public bool IsDone{ get; set; }
        public NameValuePairList ModInfo
        {
            get { return modInfo; }
            set { modInfo = value; }
        }

        public virtual bool InitMod()
        {
            return true;
        }

        public void SetupMod(RenderWindow win, Keyboard keyboard, Mouse mouse, FileSystemLayer fileSystemLayer)
        {
            root = Root.Singleton;
            window = win;
            mKeyboard = keyboard;
            mMouse = mouse;

            LocateModResource();
            Root.Singleton.CreateSceneManager(SceneType.ST_GENERIC);
            SetupModView();
            LoadModResource();
            bResourcesLoaded = true;
            SetupModContent();
            bContentSetup = true;

            bDone = false;
        }

        public virtual void LocateModResource()
        {

        }
        public virtual void LoadModResource()
        {

        }
        public virtual void SetupModContent()
        {

        }
        public virtual void SetupModView()
        {

        }
        public virtual void CleanupModContent()
        {

        }
        public virtual void UnloadModResource()
        {
            ResourceGroupManager.ResourceManagerIterator resItr =
                ResourceGroupManager.Singleton.GetResourceManagerIterator();

            while (resItr.MoveNext())
            {
                resItr.Current.UnloadUnreferencedResources();
            }
        }

        public virtual void ShutDown()
        {
            if (bContentSetup) CleanupModContent();
            if (scm != null) scm.ClearScene();
            bContentSetup = false;
            if (bResourcesLoaded) UnloadModResource();
            bResourcesLoaded = false;
            if (scm != null) root.DestroySceneManager(scm);
            scm = null;

            bDone = true;
        }

        public virtual void Paused()
        {}

        public virtual void Resume()
        {}

        public virtual void SaveState(NameValuePairList state) { }

        public virtual void RestoreState(NameValuePairList state) { }

        public virtual bool FrameStarted(FrameEvent evt) { return true; }

        public virtual bool FrameRenderingQueue(FrameEvent evt) { return true; }

        public virtual bool FrameEnded(FrameEvent evt) { return true; }

        public virtual void WindowMoved(RenderWindow rw) { }

        public virtual void WindowResized(RenderWindow rw) { }

        public virtual bool WindowClosing(RenderWindow rw) { return true; }

        public virtual void WindowClosed(RenderWindow rw) { }

        public virtual void WindowFocuschanged(RenderWindow rw) { }

        public bool KeyPressed(KeyEvent evt) { return true; }

        public bool KeyReleased(KeyEvent evt) { return true; }

        public bool MouseMoved(MouseEvent evt) { return true; }

        public bool MousePressed(MouseEvent evt, MouseButtonID id) { return true; }

        public bool MouseReleased(MouseEvent evt, MouseButtonID id) { return true; }

        public virtual StringVector GetRequiredPlugins()
		{
			return new StringVector();
		}

        public virtual String GetRequiredRenderSystem()
        {
            return "";
        }

        static Mod singleton;
        public static Mod Singleton
        {
            get
            {
                if (singleton == null)
                    singleton = new Mod();
                return singleton;
            }
        }

        public Mod()
        {
            modInfo = new NameValuePairList();
        }
    }
}
