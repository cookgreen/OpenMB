using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using OpenMB.Mods;
using OpenMB.Localization;
using OpenMB.UI;

namespace OpenMB.States
{
	public class AppStateManager : AppStateListener, IDisposable
	{
		protected List<AppState> activeStateStack = new List<AppState>();
		protected List<state_info> states = new List<state_info>();
		protected bool isShutdown;
		public event Action OnAppStateManagerStarted;
		private bool disposed;

		public struct state_info
		{
			public string name;
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
			isShutdown = false;
		}

		public override void manageAppState(String stateName, AppState state)
		{
			state_info new_state_info;
			new_state_info.name = stateName;
			new_state_info.state = state;
			states.Insert(states.Count(), new_state_info);
		}

		public override AppState findByName(String stateName)
		{

			foreach (state_info itr in states)
			{
				if (itr.name == stateName)
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

			while (!isShutdown)
			{
				if (EngineManager.Instance.renderWindow.IsClosed)
				{
					isShutdown = true;
					break;
				}

				WindowEventUtilities.MessagePump();

				if (EngineManager.Instance.renderWindow.IsActive)
				{
					startTime = (int)EngineManager.Instance.timer.MicrosecondsCPU;

					activeStateStack.Last().update(timeSinceLastFrame * 1.0 / 1000);
					EngineManager.Instance.keyboard.Capture();
					EngineManager.Instance.mouse.Capture();
					EngineManager.Instance.Update((float)(timeSinceLastFrame * 1.0 / 1000));

					EngineManager.Instance.root.RenderOneFrame();

					timeSinceLastFrame = (int)EngineManager.Instance.timer.MicrosecondsCPU - startTime;

				}
				else
				{
					System.Threading.Thread.Sleep(1000);
				}
			}
			//Save locate Info to file before exiting the main game loop
			EngineManager.Instance.Exit();
		}
		public override void changeAppState(AppState state, ModData e = null)
		{
			if (state != null)
			{
				if (activeStateStack.Count != 0)
				{
					activeStateStack.Last().exit();
					activeStateStack.RemoveAt(activeStateStack.Count() - 1);
				}

				activeStateStack.Add(state);
				init(state);
				activeStateStack.Last().enter(e);
			}
		}
		public override bool pushAppState(AppState state)
		{
			if (activeStateStack.Count != 0)
			{
				if (!activeStateStack.Last().pause())
					return false;
			}

			activeStateStack.Add(state);
			init(state);
			activeStateStack.Last().enter();

			return true;
		}
		public override void popAppState()
		{
			if (activeStateStack.Count != 0)
			{
				activeStateStack.Last().exit();
				activeStateStack.RemoveAt(activeStateStack.Count() - 1);
			}

			if (activeStateStack.Count != 0)
			{
				init(activeStateStack.Last());
				activeStateStack.Last().resume();
			}
			else
				shutdown();
		}
		public override void popAllAndPushAppState<T>(AppState state)
		{
			while (activeStateStack.Count != 0)
			{
				activeStateStack.Last().exit();
				activeStateStack.RemoveAt(activeStateStack.Count() - 1);
			}

			pushAppState(state);
		}
		public override void pauseAppState()
		{
			if (activeStateStack.Count != 0)
			{
				activeStateStack.Last().pause();
			}

			if (activeStateStack.Count() > 2)
			{
				init(activeStateStack.ElementAt(activeStateStack.Count() - 2));
				activeStateStack.ElementAt(activeStateStack.Count() - 2).resume();
			}
		}
		public override void shutdown()
		{
			isShutdown = true;
		}

		protected void init(AppState state)
		{
			UIManager.Instance.SetListener(state);
			EngineManager.Instance.renderWindow.ResetStatistics();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					while (this.activeStateStack.Count != 0)
					{
						this.activeStateStack.Last<AppState>().exit();
						this.activeStateStack.RemoveAt(this.activeStateStack.Count<AppState>() - 1);
					}
					while (this.states.Count != 0)
					{
						this.states.Last<state_info>().state.destroy();
						this.states.RemoveAt(this.states.Count<state_info>() - 1);
					}
				}
				this.disposed = true;
			}
		}
	}
}
