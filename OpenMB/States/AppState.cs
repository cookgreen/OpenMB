using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Mods;
using OpenMB.UI;

namespace OpenMB.States
{
    public class AppStateListener
    {
	    public AppStateListener(){}
	    ~AppStateListener(){}

        public virtual void manageAppState(String stateName, AppState state) { }

        public virtual AppState findByName(String stateName) { return null; }
        public virtual void changeAppState(AppState state, ModData e = null) { }
        public virtual bool pushAppState(AppState state) { return false; }
        public virtual void popAppState() { }
        public virtual void pauseAppState() { }
        public virtual void shutdown() { }
        public virtual void popAllAndPushAppState<T>(AppState state) where T:AppState { }
};
    public class AppState : UIListener
    {
        protected AppStateListener listener;
        protected Camera camera;
        protected SceneManager sceneMgr;
        protected FrameEvent frameEvent;
        protected ModData modData;
        public static void create<T>(string name) where T : AppState, new()
        {
            T myAppState = new T();		
	        myAppState.listener = AppStateManager.Instance;
            AppStateManager.Instance.manageAppState(name, myAppState);
        }
 
	    public void destroy()
        {
        }
	    public virtual void enter(ModData data = null){}
	    public virtual void exit(){}
	    public virtual bool pause(){return false;}
	    public virtual void resume(){}
        public virtual void update(double timeSinceLastFrame) { }
        public AppState(){}

        protected AppState findByName(string stateName) { return listener.findByName(stateName); }
        protected void changeAppState(AppState state,ModData e = null) { listener.changeAppState(state,e); }
        protected bool pushAppState(AppState state) { return listener.pushAppState(state); }
        protected void popAppState() { listener.popAppState(); }
        protected void shutdown() { listener.shutdown(); }
        protected void popAllAndPushAppState<T>(AppState state) where T : AppState { listener.popAllAndPushAppState<T>(state); }

        protected virtual void ReConfigure(string renderName, Dictionary<string, string> displayOptions)
        {
            RenderSystem rs = GameManager.Instance.root.GetRenderSystemByName(renderName);
            foreach (var kpl in displayOptions)
            {
                rs.SetConfigOption(kpl.Key, kpl.Value);
            }
            GameManager.Instance.root.QueueEndRendering();
        }
    }
}