using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.States
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
    public class AppState : SdkTrayListener
    {
        protected AppStateListener m_pParent;
        protected Camera m_Camera;
        protected SceneManager m_SceneMgr;
        protected FrameEvent m_FrameEvent;
        protected ModData m_Data;
        public static void create<T>(string name) where T : AppState, new()
        {
            T myAppState = new T();		
	        myAppState.m_pParent = AppStateManager.Instance;
            AppStateManager.Instance.manageAppState(name, myAppState);
        }
 
	    public void destroy()
        {
        }
	    public virtual void enter(ModData data=null){}
	    public virtual void exit(){}
	    public virtual bool pause(){return false;}
	    public virtual void resume(){}
        public virtual void update(double timeSinceLastFrame) { }
        public AppState(){}

        protected AppState findByName(String stateName) { return m_pParent.findByName(stateName); }
        protected void changeAppState(AppState state,ModData e=null) { m_pParent.changeAppState(state,e); }
        protected bool pushAppState(AppState state) { return m_pParent.pushAppState(state); }
        protected void popAppState() { m_pParent.popAppState(); }
        protected void shutdown() { m_pParent.shutdown(); }
        protected void popAllAndPushAppState<T>(AppState state) where T : AppState { m_pParent.popAllAndPushAppState<T>(state); }

        protected virtual void ReConfigure(string renderName, Dictionary<string, string> displayOptions)
        {
            RenderSystem rs = GameManager.Instance.mRoot.GetRenderSystemByName(renderName);
            foreach (var kpl in displayOptions)
            {
                rs.SetConfigOption(kpl.Key, kpl.Value);
            }
            GameManager.Instance.mRoot.QueueEndRendering();
        }
    }
}