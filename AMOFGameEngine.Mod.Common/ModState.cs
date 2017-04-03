using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.Mod.Common
{
    /// <summary>
    /// Specific Mod State
    /// </summary>
    public class ModState : SdkTrayListener
    {
        public static void create<T>(ModStateListener parent, String name) where T : ModState, new()
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
        public ModState() { }

        protected ModState findByName(String stateName) { return m_pParent.findByName(stateName); }
        protected void changeAppState(ModState state) { m_pParent.changeAppState(state); }
        protected bool pushAppState(ModState state) { return m_pParent.pushAppState(state); }
        protected void popAppState() { m_pParent.popAppState(); }
        protected void shutdown() { m_pParent.shutdown(); }
        protected void popAllAndPushAppState<T>(ModState state) where T : ModState { m_pParent.popAllAndPushAppState<T>(state); }

        protected ModStateListener m_pParent;

        protected Camera m_Camera;
        protected SceneManager m_SceneMgr;
        protected FrameEvent m_FrameEvent;
    }

    /// <summary>
    /// Listener of Mod State to switch Mod State
    /// </summary>
    public class ModStateListener
    {
        public ModStateListener(){}

        public virtual void manageAppState(String stateName, ModState state) { }

        public virtual ModState findByName(String stateName) { return null; }
        public virtual void changeAppState(ModState state) { }
        public virtual bool pushAppState(ModState state) { return false; }
        public virtual void popAppState() { }
        public virtual void pauseAppState() { }
        public virtual void shutdown() { }
        public virtual void popAllAndPushAppState<T>(ModState state) where T : ModState { }
    }
}
