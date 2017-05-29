using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Data;

namespace AMOFGameEngine.States
{
    public class AppStateManager : AppStateListener
    {
        protected List<AppState> m_ActiveStateStack = new List<AppState>();
        protected List<state_info> m_States = new List<state_info>();
        protected bool m_bShutdown;
         public struct state_info
        {
            public String name;
            public AppState state;
        };
         public AppStateManager()
         {
             m_bShutdown = false;
             GameManager.Singleton.mModMgr .ModStateChangedAction += new Action<ModEventArgs>(ModManager_ModStateChanged);
         }
          ~AppStateManager()
         {
             state_info si;

             while (m_ActiveStateStack.Count!=0)
             {
                 m_ActiveStateStack.Last().exit();
                 m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count()-1);
             }

             while (m_States.Count!=0)
             {
                 si = m_States.Last();
                 si.state.destroy();
                 m_States.RemoveAt(m_States.Count()-1);
             }
         }

          public override void manageAppState(String stateName, AppState state)
         {
		        state_info new_state_info;
		        new_state_info.name = stateName;
		        new_state_info.state = state;
		        m_States.Insert(m_States.Count(),new_state_info);
         }

         public override AppState findByName(String stateName)
         {

             foreach (state_info itr in m_States)
	        {
		        if(itr.name==stateName)
			    return itr.state;
	        }
 
	        return null;
         }

         public void start(AppState state)
         {
             changeAppState(state);
 
	        int timeSinceLastFrame = 1;
	        int startTime = 0;
 
	        while(!m_bShutdown)
	        {
		        if(GameManager.Singleton.mRenderWnd.IsClosed)m_bShutdown = true;
 
		        WindowEventUtilities.MessagePump();

                if (GameManager.Singleton.mRenderWnd.IsActive)
		        {
                    startTime = (int)GameManager.Singleton.mTimer.MicrosecondsCPU;

                    m_ActiveStateStack.Last().update(timeSinceLastFrame * 1.0 / 1000);
                    GameManager.Singleton.mKeyboard.Capture();
                    GameManager.Singleton.mMouse.Capture();
                    GameManager.Singleton.UpdateOgre(timeSinceLastFrame * 1.0 / 1000);
                    if (GameManager.Singleton.mRoot != null)
                    {
                        GameManager.Singleton.mRoot.RenderOneFrame();
                    }
                    timeSinceLastFrame = (int)GameManager.Singleton.mTimer.MillisecondsCPU - startTime;
                    
		        }
		        else
		        {
                    System.Threading.Thread.Sleep(1000);
		        }
	        }
            GameManager.Singleton.mLocateMgr.SaveLocateFile();
            GameManager.Singleton.mLog.LogMessage("Game Quit");
         }
         public override void changeAppState(AppState state,AppStateArgs e=null)
         {
             if (state != null)
             {
                 if (m_ActiveStateStack.Count != 0)
                 {
                     m_ActiveStateStack.Last().exit();
                     m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count() - 1);
                 }

                 m_ActiveStateStack.Insert(m_ActiveStateStack.Count(), state);
                 init(state);
                 m_ActiveStateStack.Last().enter(e);
             }
         }
         public override bool pushAppState(AppState state)
         {
             if (m_ActiveStateStack.Count!=0)
             {
                 if (!m_ActiveStateStack.Last().pause())
                     return false;
             }

             m_ActiveStateStack.Insert(m_ActiveStateStack.Count(),state);
             init(state);
             m_ActiveStateStack.Last().enter();

             return true;
         }
         public override void popAppState()
         {
             if (m_ActiveStateStack.Count != 0)
             {
                 m_ActiveStateStack.Last().exit();
                 m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count()-1);
             }

             if (m_ActiveStateStack.Count != 0)
             {
                 init(m_ActiveStateStack.Last());
                 m_ActiveStateStack.Last().resume();
             }
             else
                 shutdown();
         }
         public override void popAllAndPushAppState<T>(AppState state)
        {
            while (m_ActiveStateStack.Count != 0)
            {
                m_ActiveStateStack.Last().exit();
                m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count()-1);
            }

            pushAppState(state);
        }
         public override void pauseAppState()
         {
             if (m_ActiveStateStack.Count != 0)
             {
                 m_ActiveStateStack.Last().pause();
             }

             if (m_ActiveStateStack.Count() > 2)
             {
                 init(m_ActiveStateStack.ElementAt(m_ActiveStateStack.Count() - 2));
                 m_ActiveStateStack.ElementAt(m_ActiveStateStack.Count() - 2).resume();
             }
         }
         public override void shutdown()
         {
             m_bShutdown = true;
         }

         protected void init(AppState state)
         {
             GameManager.Singleton.mTrayMgr.setListener(state);
             GameManager.Singleton.mRenderWnd.ResetStatistics();
         }

         public void ModManager_ModStateChanged(ModEventArgs e)
         {
             if (e.modState == ModState.Stop)
             {
                 changeAppState(findByName("MainMenu"), new AppStateArgs()
                 {
                     modName = e.modName,
                     modIndex = e.modIndex
                 });
             }
             else if (e.modState == ModState.Run)
             {
                 changeAppState(findByName("MainMenu"), new AppStateArgs() { 
                     modName=e.modName,
                     modIndex=e.modIndex
                 });
             }
         }
    }
}
