using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.Mods;
using AMOFGameEngine.Localization;

namespace AMOFGameEngine.States
{
    public class AppStateManager : AppStateListener, IDisposable
    {
        protected List<AppState> m_ActiveStateStack = new List<AppState>();
        protected List<state_info> m_States = new List<state_info>();
        protected bool m_bShutdown;
        public event Action OnAppStateManagerStarted;
        private bool disposed;

        public struct state_info
        {
            public String name;
            public AppState state;
        };

         public static AppStateManager Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new AppStateManager();
                 }
                 return instance;
             }
         }

        static AppStateManager instance;

         public AppStateManager()
         {
             m_bShutdown = false;
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

             if (OnAppStateManagerStarted != null)
             {
                 OnAppStateManagerStarted();
             }

	         while(!m_bShutdown)
	         {
		         if(GameManager.Instance.mRenderWnd.IsClosed)m_bShutdown = true;
 
		         WindowEventUtilities.MessagePump();

                 if (GameManager.Instance.mRenderWnd.IsActive)
		         {
                     startTime = (int)GameManager.Instance.mTimer.MicrosecondsCPU;

                     m_ActiveStateStack.Last().update(timeSinceLastFrame * 1.0 / 1000);
                     GameManager.Instance.mKeyboard.Capture();
                     GameManager.Instance.mMouse.Capture();
                     GameManager.Instance.UpdateRender(timeSinceLastFrame * 1.0 / 1000);
                     //GameManager.Singleton.UpdateSubSystem(timeSinceLastFrame * 1.0 / 1000);

                     GameManager.Instance.mRoot.RenderOneFrame();

                     timeSinceLastFrame = (int)GameManager.Instance.mTimer.MicrosecondsCPU - startTime;
                     
		         }
		         else
		         {
                     System.Threading.Thread.Sleep(1000);
                }
            }
             //Save locate Info to file before exiting the main game loop
             GameManager.Instance.Exit();
         }
         public override void changeAppState(AppState state,ModData e=null)
         {
             if (state != null)
             {
                 if (m_ActiveStateStack.Count != 0)
                 {
                     m_ActiveStateStack.Last().exit();
                     m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count() - 1);
                 }

                 m_ActiveStateStack.Add(state);
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

             m_ActiveStateStack.Add(state);
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
             GameManager.Instance.mTrayMgr.setListener(state);
             GameManager.Instance.mRenderWnd.ResetStatistics();
         }

         public void Dispose()
         {
             this.Dispose(true);
             GC.SuppressFinalize(this);
         }

         protected virtual void Dispose(bool disposing)
         {
             if (!this.disposed)
             {
                 if (disposing)
                 {
                     while (this.m_ActiveStateStack.Count != 0)
                     {
                         this.m_ActiveStateStack.Last<AppState>().exit();
                         this.m_ActiveStateStack.RemoveAt(this.m_ActiveStateStack.Count<AppState>() - 1);
                     }
                     while (this.m_States.Count != 0)
                     {
                         this.m_States.Last<state_info>().state.destroy();
                         this.m_States.RemoveAt(this.m_States.Count<state_info>() - 1);
                     }
                 }
                 this.disposed = true;
             }
         }
    }
}
