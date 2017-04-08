using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.Mods.Common
{
    public class ModContext : WindowEventListener
    {
        protected NameValuePairList mLastModState;
        protected Mod mLastMod;
        protected Mod mCurrentSample;         // currently running sample
        protected bool mSamplePaused;             // whether current sample is paused
        protected bool mFirstRun;                 // whether or not this is the first run
        protected bool mLastRun;                  // whether or not this is the final run
        protected String mNextRenderer;     // name of renderer used for next run
        protected Keyboard mKeyboard;
        protected Mouse mMouse;
        protected Root mRoot;
        protected InputManager mInputMgr;
        protected RenderWindow mRenderWnd;

        public ModContext()
        {
            mRoot = null;
            mMouse = null;
            mKeyboard = null;
        }

        public virtual RenderWindow getRenderWindow()
		{
            return mRenderWnd;
		}

		public virtual Mod getCurrentSample()
		{
			return mCurrentSample;
		}

		/*-----------------------------------------------------------------------------
		| Quits the current sample and starts a new one.
		-----------------------------------------------------------------------------*/
		public virtual void runSample(Mod s)
		{
			if (mCurrentSample!=null)
			{
				mCurrentSample.ShutDown();    // quit current sample
				mSamplePaused = false;          // don't pause the next sample
			}

			mRenderWnd.RemoveAllViewports();                  // wipe viewports

			if (s!=null)
			{
                
				// retrieve sample's required plugins and currently installed plugins
				Root.Const_PluginInstanceList ip = mRoot.GetInstalledPlugins();
				StringVector rp = s.GetRequiredPlugins();

				foreach (string j in rp)
				{
					bool found = false;
					// try to find the required plugin in the current installed plugins
					foreach ( IPlugin k in ip )
					{
						if (k.Name == j)
						{
							found = true;
							break;
						}
					}
					if (!found)  // throw an exception if a plugin is not found
					{
						string desc = "Sample requires plugin: "+j;
						string src = "SampleContext::runSample";
                        
						new OgreException((int)OgreException.ExceptionCodes.ERR_NOT_IMPLEMENTED, desc, src);
					}
				}

				// throw an exception if samples requires the use of another renderer
			    String rrs = s.GetRequiredRenderSystem();
				if (!string.IsNullOrEmpty(rrs) && rrs != mRoot.RenderSystem.Name)
				{
					String desc = "Sample only runs with renderer: " + rrs;
					String src = "SampleContext::runSample";
					new OgreException((int)OgreException.ExceptionCodes.ERR_INVALID_STATE, desc, src);
				}

				// test system capabilities against sample requirements
				//s->testCapabilities(mRoot->getRenderSystem()->getCapabilities());

				s.SetupMod(mRenderWnd, mKeyboard, mMouse, null);   // start new sample
			}

			mCurrentSample = s;
		}

		/*-----------------------------------------------------------------------------
		| This function encapsulates the entire lifetime of the context.
		-----------------------------------------------------------------------------*/
		public virtual void go(Mod initialSample = null)
		{
            while (!mLastRun)
			{
				mLastRun = true;  // assume this is our last run

				createRoot();
				if (!oneTimeConfig()) return;

				// if the context was reconfigured, set requested renderer
				if (!mFirstRun) mRoot.RenderSystem=(mRoot.GetRenderSystemByName(mNextRenderer));

				setup();

				// restore the last sample if there was one or, if not, start initial sample
				if (!mFirstRun) recoverLastSample();
				else if (initialSample!=null) runSample(initialSample);

				mRoot.StartRendering();    // start the render loop

				mRoot.SaveConfig();
				shutdown();
				if (mRoot!=null) mRoot.Dispose();
				mFirstRun = false;
			}
		}
        
		public virtual bool isCurrentSamplePaused()
		{
			if (mCurrentSample!=null) return mSamplePaused;
			return false;
		}

		public virtual void pauseCurrentSample()
		{
			if (mCurrentSample!=null && !mSamplePaused)
			{
				mSamplePaused = true;
				mCurrentSample.Paused();
			}
		}

		public virtual void unpauseCurrentSample()
		{
			if (mCurrentSample!=null && mSamplePaused)
			{
				mSamplePaused = false;
				mCurrentSample.Resume();
			}
		}
			
		/*-----------------------------------------------------------------------------
		| Processes frame started events.
		-----------------------------------------------------------------------------*/
		public virtual bool frameStarted(FrameEvent evt)
		{
			captureInputDevices();      // capture input

			// manually call sample callback to ensure correct order
			return (mCurrentSample!=null && !mSamplePaused) ? mCurrentSample.FrameStarted(evt) : true;
		}
			
		/*-----------------------------------------------------------------------------
		| Processes rendering queued events.
		-----------------------------------------------------------------------------*/
		public virtual bool frameRenderingQueued(FrameEvent evt)
		{
			// manually call sample callback to ensure correct order
            return (mCurrentSample != null && !mSamplePaused) ? mCurrentSample.FrameRenderingQueue(evt) : true;
		}
			
		/*-----------------------------------------------------------------------------
		| Processes frame ended events.
		-----------------------------------------------------------------------------*/
		public virtual bool frameEnded(FrameEvent evt)
		{
			// manually call sample callback to ensure correct order
			if (mCurrentSample!=null && !mSamplePaused && !mCurrentSample.FrameEnded(evt)) return false;
			// quit if window was closed
			if (mRenderWnd.IsClosed) return false;
			// go into idle mode if current sample has ended
			if (mCurrentSample!=null && mCurrentSample.IsDone) runSample(null);

			return true;
		}

		/*-----------------------------------------------------------------------------
		| Processes window size change event. Adjusts mouse's region to match that
		| of the window. You could also override this method to prevent resizing.
		-----------------------------------------------------------------------------*/
		public virtual void windowResized(RenderWindow rw)
		{
			// manually call sample callback to ensure correct order
			if (mCurrentSample!=null && !mSamplePaused) mCurrentSample.WindowResized(rw);
		}

		// window event callbacks which manually call their respective sample callbacks to ensure correct order

		public virtual void windowMoved(RenderWindow rw)
		{
			if (mCurrentSample!=null && !mSamplePaused) mCurrentSample.WindowMoved(rw);
		}

		public virtual bool windowClosing(RenderWindow rw)
		{
			if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.WindowClosing(rw);
			return true;
		}

		public virtual void windowClosed(RenderWindow rw)
		{
			if (mCurrentSample!=null && !mSamplePaused) mCurrentSample.WindowClosed(rw);
		}

		public virtual void windowFocusChange(RenderWindow rw)
		{
            if (mCurrentSample != null && !mSamplePaused) mCurrentSample.WindowFocuschanged(rw);
		}

		// keyboard and mouse callbacks which manually call their respective sample callbacks to ensure correct order

		public virtual bool keyPressed(KeyEvent evt)
		{
			if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.KeyPressed(evt);
			return true;
		}

		public virtual bool keyReleased(KeyEvent evt)
		{
			if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.KeyReleased(evt);
			return true;
		}

		public virtual bool mouseMoved(MouseEvent evt)
		{
            if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.MouseMoved(evt);
			return true;
		}
		public virtual bool mousePressed(MouseEvent evt, MouseButtonID id)
		{
			if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.MousePressed(evt, id);
			return true;
		}

		public virtual bool mouseReleased(MouseEvent evt,MouseButtonID id)
		{
			if (mCurrentSample!=null && !mSamplePaused) return mCurrentSample.MouseReleased(evt, id);
			return true;
		}

	

        /*-----------------------------------------------------------------------------
         | Sets up the context after configuration.
         -----------------------------------------------------------------------------*/
		protected virtual void setup()
		{
			createWindow();
			setupInput();
			locateResources();
			loadResources();
            
			TextureManager.Singleton.DefaultNumMipmaps=(5);
            
			// adds context as listener to process context-level (above the sample level) events
			mRoot.FrameEnded+=new FrameListener.FrameEndedHandler(frameEnded);
            mRoot.FrameRenderingQueued+=new FrameListener.FrameRenderingQueuedHandler(frameRenderingQueued);
            mRoot.FrameStarted+=new FrameListener.FrameStartedHandler(frameStarted);
            
			WindowEventUtilities.AddWindowEventListener(mRenderWnd, this);
		}
        
		/*-----------------------------------------------------------------------------
		| Creates the OGRE root.
		-----------------------------------------------------------------------------*/
		public virtual void createRoot()
		{
            String pluginsPath = "";
			mRoot = new Root(pluginsPath, "ogre.cfg", 
				"ogre.log");

		}

		/*-----------------------------------------------------------------------------
		| Configures the startup settings for OGRE. I use the config dialog here,
		| but you can also restore from a config file. Note that this only happens
		| when you start the context, and not when you reset it.
		-----------------------------------------------------------------------------*/
		public virtual bool oneTimeConfig()
		{
			//return mRoot->showConfigDialog();
			 return mRoot.RestoreConfig();
		}

		/*-----------------------------------------------------------------------------
		| Creates the render window to be used for this context. I use an auto-created
		| window here, but you can also create an external window if you wish.
		| Just don't forget to initialise the root.
		-----------------------------------------------------------------------------*/
		public virtual void createWindow()
		{
			mRenderWnd = mRoot.Initialise(true);
		}

		/*-----------------------------------------------------------------------------
		| Sets up OIS input.
		-----------------------------------------------------------------------------*/
		public virtual void setupInput()
		{
			int winHandle = 0;

			mRenderWnd. GetCustomAttribute("WINDOW", out winHandle);

			mInputMgr = InputManager.CreateInputSystem((uint)winHandle);

			createInputDevices();      // create the specific input devices

			windowResized(mRenderWnd);    // do an initial adjustment of mouse area
		}

		/*-----------------------------------------------------------------------------
		| Creates the individual input devices. I only create a keyboard and mouse
		| here because they are the most common, but you can override this method
		| for other modes and devices.
		-----------------------------------------------------------------------------*/
		public virtual void createInputDevices()
		{
			//mMouse.(this);
		}

		/*-----------------------------------------------------------------------------
		| Finds context-wide resource groups. I load paths from a config file here,
		| but you can choose your resource locations however you want.
		-----------------------------------------------------------------------------*/
		public virtual void locateResources()
		{
			// load resource paths from config file
			ConfigFile cf=new ConfigFile();
			cf.Load("resources.cfg",":\t=",true);

			ConfigFile.SectionIterator seci = cf.GetSectionIterator();
			String sec, type, arch;

			// go through all specified resource groups
			while (seci.MoveNext())
			{
				sec = seci.CurrentKey;
				ConfigFile.SettingsMultiMap settings = seci.Current;

				// go through all resource paths
				foreach( KeyValuePair<string,string> kpl in settings )
                {
                    type=kpl.Key;
                    arch=kpl.Value;

                    ResourceGroupManager.Singleton.AddResourceLocation(arch,type,sec);
                }
			}
		}

		/*-----------------------------------------------------------------------------
		| Loads context-wide resource groups. I chose here to simply initialise all
		| groups, but you can fully load specific ones if you wish.
		-----------------------------------------------------------------------------*/
		public virtual void loadResources()
		{
			ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		}

		/*-----------------------------------------------------------------------------
		| Reconfigures the context. Attempts to preserve the current sample state.
		-----------------------------------------------------------------------------*/
		public virtual void reconfigure(String renderer, NameValuePairList options)
		{
			// save current sample state
			mLastSample = mCurrentSample;
			if (mCurrentSample!=null) mCurrentSample.SaveState(mLastSampleState);

			mNextRenderer = renderer;
			RenderSystem rs = mRoot.GetRenderSystemByName(renderer);

			// set all given render system options
            foreach( KeyValuePair<string,string> kpl in options)
            {
                rs.SetConfigOption(kpl.Key,kpl.Value);
            }
			mLastRun = false;             // we want to go again with the new settings
			mRoot.QueueEndRendering();   // break from render loop
		}

		/*-----------------------------------------------------------------------------
		| Recovers the last sample after a reset. You can override in the case that
		| the last sample is destroyed in the process of resetting, and you have to
		| recover it through another means.
		-----------------------------------------------------------------------------*/
		public virtual void recoverLastSample()
		{
			runSample(mLastSample);
			mLastSample.RestoreState(mLastSampleState);
			mLastSample = null;
			mLastSampleState.Clear();
		}

		/*-----------------------------------------------------------------------------
		| Cleans up and shuts down the context.
		-----------------------------------------------------------------------------*/
		public virtual void shutdown()
		{
			if (mCurrentSample!=null)
			{
				mCurrentSample.ShutDown();
				mCurrentSample = null;
			}

			// remove window event listener before shutting down OIS
			WindowEventUtilities.RemoveWindowEventListener(mRenderWnd, this);

			shutdownInput();
		}

		/*-----------------------------------------------------------------------------
		| Destroys OIS input devices and the input manager.
		-----------------------------------------------------------------------------*/
		public virtual void shutdownInput()
		{
			if (mInputMgr!=null)
			{
				mInputMgr.DestroyInputObject(mMouse);

				InputManager.DestroyInputSystem(mInputMgr);
				mInputMgr = null;
			}
		}

		/*-----------------------------------------------------------------------------
		| Captures input device states.
		-----------------------------------------------------------------------------*/
		public virtual void captureInputDevices()
		{
			mKeyboard.Capture();
			mMouse.Capture();
		}

        public Mod mLastSample { get; set; }

        public NameValuePairList mLastSampleState { get; set; }
    }
}
