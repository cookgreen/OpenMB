using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.UI
{
    public class UIManager
    {
        public struct window_info
        {
            public string windowName;
            public UIWindiw window;
        }

        List<UIWindiw> mActiveWindows;
        List<window_info> mWindows;
        bool close;

        public UIManager()
        {
            mActiveWindows = new List<UIWindiw>();
            mWindows = new List<window_info>();
            close = false;
        }

        public void ManageWindow(string winName, UIWindiw window)
        {
            window_info winInfo;
            winInfo.windowName = winName;
            winInfo.window = window;
            mWindows.Add(winInfo);
        }

        public void ShowWindow(UIWindiw window)
        {
            ChangeWindow(window);

            int timeSinceLastFrame = 1;
            int startTime = 0;
            while (!close)
            {
                startTime = (int)GameManager.Singleton.mTimer.MicrosecondsCPU;

                WindowEventUtilities.MessagePump();

                mActiveWindows.Last().update(timeSinceLastFrame * 1.0 / 1000);

                GameManager.Singleton.mMouse.Capture();
                GameManager.Singleton.mKeyboard.Capture();
                GameManager.Singleton.mRoot.RenderOneFrame();

                timeSinceLastFrame = (int)GameManager.Singleton.mTimer.MillisecondsCPU - startTime;
            }
            mActiveWindows.Last().close();
        }

        public void ChangeWindow(UIWindiw win)
        {
            if (win != null)
            {
                if (mActiveWindows.Count != 0)
                {
                    mActiveWindows.Last().close();
                }

                mActiveWindows.Add(win);

                GameManager.Singleton.mTrayMgr.setListener(win);

                mActiveWindows.Last().enter();
            }
        }

        public void Shutdown()
        {
            close = true;
        }
    }
}
