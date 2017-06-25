using Mogre;
using System.Collections.Generic;

namespace Editor
{
    public class MogreConsole
    {
        const int CONSOLE_LINE_LENGTH = 120;
        const int CONSOLE_LINE_COUNT = 25;

        bool mVisible;
        bool mInitialized;
        SceneManager mSceneManager;
        Rectangle2D mRectangle;
        SceneNode mSceneNode;
        OverlayElement mTextbox;
        Overlay mOverlay;

        float mHeight;
        bool mUpdateOverlay;
        int mStartLine;
        List<string> mLines;
        string mPrompt;
        Dictionary<string, CommandDelegate> mCommands;

        // Delegate is used instead of function pointers (void*) in C++
        public delegate void CommandDelegate(List<string> parameters);

        public bool Visible
        {
            set { mVisible = value; }
            get { return mVisible; }
        }

        public MogreConsole()
        {
            mStartLine = 0;
            mLines = new List<string>();
            mInitialized = false;
            mCommands = new Dictionary<string, CommandDelegate>();
        }

        public void InitConsole(ref Root root)
        {
            if (!root.GetSceneManagerIterator().MoveNext())
            {
                OgreException ogreException = new OgreException((int)OgreException.ExceptionCodes.ERR_INTERNAL_ERROR, "No scene manager found!", "init");
            }

            SceneManagerEnumerator.SceneManagerIterator sceneManagerEnumerator = root.GetSceneManagerIterator();
            sceneManagerEnumerator.MoveNext();
            mSceneManager = sceneManagerEnumerator.Current;
            root.FrameStarted += new FrameListener.FrameStartedHandler(this.FrameStarted);

            mHeight = 1;

            // Create background rectangle covering the whole screen
            mRectangle = new Rectangle2D(true);
            mRectangle.SetCorners(-1, 1, 1, 1 - mHeight);
            mRectangle.SetMaterial("console/background");
            mRectangle.RenderQueueGroup = (byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY;
            mRectangle.BoundingBox = new AxisAlignedBox((-100000.0f * Vector3.UNIT_SCALE), (100000.0f * Vector3.UNIT_SCALE));
            mSceneNode = mSceneManager.RootSceneNode.CreateChildSceneNode("#Console");
            mSceneNode.AttachObject(mRectangle);

            mTextbox = OverlayManager.Singleton.CreateOverlayElement("TextArea", "ConsoleText");
            mTextbox.MetricsMode = GuiMetricsMode.GMM_RELATIVE;
            mTextbox.SetPosition(0, 0);
            mTextbox.SetParameter("font_name", "Console");
            mTextbox.SetParameter("colour_top", "1 1 1");
            mTextbox.SetParameter("colour_bottom", "1 1 1");
            mTextbox.SetParameter("char_height", "0.02");

            mOverlay = OverlayManager.Singleton.Create("Console");
            PanelOverlayElement panelOverlayElement = new PanelOverlayElement("Panel");
            panelOverlayElement._addChild(mTextbox);
            mOverlay.Add2D(panelOverlayElement);
            mOverlay.Show();
            LogManager.Singleton.DefaultLog.MessageLogged += this.MessageLoggedHandler;

            mInitialized = true;
        }

        public void ShutdownConsole()
        {
            if (!mInitialized)
                return;
            mRectangle.Dispose(); ;
            mSceneNode.DetachAllObjects();
            mSceneNode.Dispose();
            mTextbox.Dispose();
            mOverlay.Dispose();
        }

        public void MessageLoggedHandler(string message, LogMessageLevel lml, bool maskDebug, string logName)
        {
            PrintMessage(logName + ": " + message);
        }

        public bool OnKeyPressedKC(MOIS.KeyCode arg)
        {
            if (!mVisible)
                return false;

            if (arg == MOIS.KeyCode.KC_RETURN)
            {
                // Split the parameter list
                string str = mPrompt;
                List<string> parameters = new List<string>();
                string param = string.Empty;
                for (int c = 0; c < mPrompt.Length; c++)
                {
                    if (str[c] == ' ')
                    {
                        if (param.Length > 0)
                            parameters.Add(param);
                        param = string.Empty;
                    }
                    else
                        param += str[c];
                }
                if (param.Length > 0)
                    parameters.Add(param);

                // Try to execute the command - Invoke delegate
                mCommands[mPrompt].Invoke(parameters);

                PrintMessage(mPrompt);
                mPrompt = string.Empty;
            }
            if (arg == MOIS.KeyCode.KC_BACK)
                mPrompt = mPrompt.Substring(0, mPrompt.Length - 2);

            if (arg == MOIS.KeyCode.KC_PGUP)
            {
                if (mStartLine > 0)
                    mStartLine--;
            }
            if (arg == MOIS.KeyCode.KC_PGDOWN)
            {
                if (mStartLine < mLines.Count)
                    mStartLine++;
            }
            else
            {
                string legalChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890+!\"#%&/()=?[]\\*-_.:,; ";
                for (int c = 0; c < legalChars.Length; c++)
                {
                    if (legalChars[c] == 'a')
                    {
                        mPrompt += 'a';
                        break;
                    }
                }
            }
            mUpdateOverlay = true;
            return mUpdateOverlay;
        }


        public bool OnKeyPressed(MOIS.KeyEvent arg)
        {
            if (!mVisible)
                return false;

            if (arg.key == MOIS.KeyCode.KC_RETURN)
            {
                // Split the parameter list
                string str = mPrompt;
                List<string> parameters = new List<string>();
                string param = string.Empty;
                for (int c = 0; c < mPrompt.Length; c++)
                {
                    if (str[c] == ' ')
                    {
                        if (param.Length > 0)
                            parameters.Add(param);
                        param = string.Empty;
                    }
                    else
                        param += str[c];
                }
                if (param.Length > 0)
                    parameters.Add(param);

                // Try to execute the command - Invoke delegate
                mCommands[mPrompt].Invoke(parameters);

                PrintMessage(mPrompt);
                mPrompt = string.Empty;
            }
            if (arg.key == MOIS.KeyCode.KC_BACK)
                mPrompt = mPrompt.Substring(0, mPrompt.Length - 2);

            if (arg.key == MOIS.KeyCode.KC_PGUP)
            {
                if (mStartLine > 0)
                    mStartLine--;
            }
            if (arg.key == MOIS.KeyCode.KC_PGDOWN)
            {
                if (mStartLine < mLines.Count)
                    mStartLine++;
            }
            else
            {
                string legalChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890+!\"#%&/()=?[]\\*-_.:,; ";
                for (int c = 0; c < legalChars.Length; c++)
                {
                    if (legalChars[c] == arg.text)
                    {
                        mPrompt += arg.text;
                        break;
                    }
                }
            }
            mUpdateOverlay = true;
            return mUpdateOverlay;
        }

        public bool FrameStarted(FrameEvent evt)
        {
            if (mVisible && mHeight < 1)
            {
                mHeight += evt.timeSinceLastFrame * 2;
                mTextbox.Show();
                if (mHeight >= 1)
                {
                    mHeight = 1;
                }
            }
            else if (!mVisible && mHeight > 0)
            {
                mHeight -= evt.timeSinceLastFrame * 2;
                if (mHeight <= 0)
                {
                    mHeight = 0;
                    mTextbox.Hide();
                }
            }

            mTextbox.SetPosition(0, (float)((mHeight - 1) * 0.5));
            mRectangle.SetCorners(-1, 1 + mHeight, 1, 1 - mHeight);

            if (mUpdateOverlay)
            {
                string text = string.Empty;
                List<string>.Enumerator i = mLines.GetEnumerator();
                List<string>.Enumerator start = mLines.GetEnumerator();
                List<string>.Enumerator end = mLines.GetEnumerator();

                // Make sure is in range
                if (mStartLine > mLines.Count)
                    mStartLine = mLines.Count - 1;

                for (int c = 0; c < mStartLine; c++)
                    start.MoveNext();
                end = start;
                for (int c = 0; c < CONSOLE_LINE_COUNT; c++)
                {
                    if (end.MoveNext() == false)
                        break;
                    end.MoveNext();
                }
                while (i.MoveNext())
                    text += i.Current + "\n";

                // Add the prompt
                text += "> " + mPrompt;

                mTextbox.Caption = text;
                mUpdateOverlay = false;
            }
            return true;
        }

        public void PrintMessage(string text)
        {
            // Subdivide it into lines
            string str = text;
            int len = text.Length;
            string line = string.Empty;
            for (int c = 0; c < len; c++)
            {
                if (str[c] == '\n' || line.Length >= CONSOLE_LINE_LENGTH)
                {
                    mLines.Add(line);
                    line = "";
                }
                if (str[c] != '\n')
                    line += str[c];
            }
            if (line.Length > 0)
                mLines.Add(line);
            if (mLines.Count > CONSOLE_LINE_COUNT)
                mStartLine = mLines.Count - CONSOLE_LINE_COUNT;
            else
                mStartLine = 0;
            mUpdateOverlay = true;
        }

        public bool FrameEnded(FrameEvent evt)
        {
            return true;
        }

        public void AddCommand(string command, CommandDelegate function)
        {
            mCommands.Add(command, function);
        }

        public void RemoveCommand(string command)
        {
            mCommands.Remove(command);
        }
    }
}