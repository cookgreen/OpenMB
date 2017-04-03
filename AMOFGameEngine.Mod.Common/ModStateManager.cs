using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Mod.Common
{
    public class ModStateManager : ModStateListener
    {
        public struct state_info
        {
            public String name;
            public ModState state;
        };
         public ModStateManager()
         {
             m_bShutdown = false;
         }
         ~ModStateManager()
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

         public override void manageAppState(String stateName, ModState state)
         {
		        state_info new_state_info;
		        new_state_info.name = stateName;
		        new_state_info.state = state;
		        m_States.Insert(m_States.Count(),new_state_info);
         }

         public override ModState findByName(String stateName)
         {

             foreach (state_info itr in m_States)
	        {
		        if(itr.name==stateName)
			    return itr.state;
	        }
 
	        return null;
         }

         public void start(ModState state)
         {
             changeAppState(state);
 
	        int timeSinceLastFrame = 1;
	        int startTime = 0;
 
	        while(!m_bShutdown)
	        {
		        if(ModContext .Singleton.Window.IsClosed)m_bShutdown = true;
 
		        WindowEventUtilities.MessagePump();

                if (ModContext.Singleton.Window.IsActive)
		        {
                    startTime = (int)ModContext.Singleton.Timer.MicrosecondsCPU;

                    timeSinceLastFrame = (int)ModContext.Singleton.Timer.MillisecondsCPU - startTime;

			        m_ActiveStateStack.Last().update(timeSinceLastFrame);
                    ModContext.Singleton.Mouse.Capture();
                    ModContext.Singleton.Keyboard.Capture();
                    //ModContext.Singleton.UpdateOgre(timeSinceLastFrame);
                    if (ModContext.Singleton.Root != null)
                    {
                        ModContext.Singleton.Root.RenderOneFrame();
                    }
                    
		        }
		        else
		        {
                    System.Threading.Thread.Sleep(1000);
		        }
	        }

            ModContext.Singleton.Log.LogMessage("Mod Quit");
         }
         public override void changeAppState(ModState state)
         {
             if (m_ActiveStateStack.Count!=0)
             {
                 m_ActiveStateStack.Last().exit();
                 m_ActiveStateStack.RemoveAt(m_ActiveStateStack.Count()-1);
             }

             m_ActiveStateStack.Insert(m_ActiveStateStack.Count(),state);
             init(state);
             m_ActiveStateStack.Last().enter();
         }
         public override bool pushAppState(ModState state)
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
         public override void popAllAndPushAppState<T>(ModState state)
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

         protected void init(ModState state)
         {
             ModContext.Singleton.TrayMgr.setListener(state);
             ModContext.Singleton.Window.ResetStatistics();
         }
         protected List<ModState> m_ActiveStateStack = new List<ModState>();
         protected List<state_info> m_States=new List<state_info>();
         protected bool m_bShutdown;
    }
}
