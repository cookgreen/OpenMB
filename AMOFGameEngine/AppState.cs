#define T
using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;

namespace AMOFGameEngine
{
    class AppStateListener
    {
	    public AppStateListener(){}
	    ~AppStateListener(){}

        public virtual void manageAppState(String stateName, AppState state) { }

        public virtual AppState findByName(String stateName) { return null; }
        public virtual void changeAppState(AppState state) { }
        public virtual bool pushAppState(AppState state) { return false; }
        public virtual void popAppState() { }
        public virtual void pauseAppState() { }
        public virtual void shutdown() { }
        public virtual void popAllAndPushAppState<T>(AppState state) where T:AppState { }
};
    class AppState :SdkTrayListener
    {
        public static void create<T>(AppStateListener parent, String name) where T : AppState, new()
        {
            T myAppState=new T();				
	        myAppState.m_pParent = parent;					
	        parent.manageAppState(name, myAppState);
        }
 
	    public void destroy()
        {
        }
 
	    public virtual void enter(){}
	    public virtual void exit(){}
	    public virtual bool pause(){return false;}
	    public virtual void resume(){}
        public virtual void update(double timeSinceLastFrame) { }
        public AppState(){}
 
	    protected AppState	findByName(String stateName){return m_pParent.findByName(stateName);}
        protected void changeAppState(AppState state) { m_pParent.changeAppState(state); }
        protected bool pushAppState(AppState state) { return m_pParent.pushAppState(state); }
        protected void popAppState() { m_pParent.popAppState(); }
        protected void shutdown() { m_pParent.shutdown(); }
        protected void popAllAndPushAppState<T>(AppState state) where T:AppState{ m_pParent.popAllAndPushAppState<T>(state); }

        protected AppStateListener m_pParent;

        protected Camera m_pCamera;
        protected SceneManager m_pSceneMgr;
        protected FrameEvent m_FrameEvent;
    }
}