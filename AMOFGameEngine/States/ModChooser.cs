using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.States
{
    using Mods = Dictionary<string, ModManifest>;

    class ModChooser : AppState
    {
        bool isQuit;
        SelectMenu ModChooserMenu;
        TextBox ModDescBox;
        Slider ModSlider;
        StringVector mModNames;
        StringVector mModThumb;
        Mods mMods;
        List<OverlayContainer> mModThumbs;
        float mCarouselPlace;
        string selectedModName;

        public ModChooser()
        {
            isQuit = false;

            mModNames = new StringVector();
            mModThumb = new StringVector();
            mModThumbs = new List<OverlayContainer>();
        }

        public override void enter(ModData e = null)
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "ModChooserSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;

            m_Camera = m_SceneMgr.CreateCamera("ModChooserCam");
            m_Camera.SetPosition(0, 25, -50);
            Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance = 1;

            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mViewport.Camera = m_Camera;
            mModNames.Clear();
            mModThumb.Clear();

            mMods = ModManager.Singleton.GetInstalledMods();
            foreach (var mod in mMods)
            {
                mModNames.Add(mod.Key);
                mModThumb.Add(mod.Value.MetaData.Thumb);
            }

            GameManager.Singleton.mTrayMgr.destroyAllWidgets();
            Label ModTitle = GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_LEFT, "ModTitle", "Mod Info");
            ModDescBox = GameManager.Singleton.mTrayMgr.createTextBox(TrayLocation.TL_LEFT, "ModInfo", "Mod Info", 250, 208);
            ModChooserMenu = GameManager.Singleton.mTrayMgr.createThickSelectMenu(TrayLocation.TL_LEFT, "SelMod", "Select Mod", 250, 10);
            ModSlider = GameManager.Singleton.mTrayMgr.createThickSlider(TrayLocation.TL_LEFT, "ModSlider", "Slider Samples", 250, 80, 0, 0, 0);
            ModChooserMenu.setItems(mModNames);
            ModChooserMenu.setCaption("Select Mod");
            if (mModNames.Count>0)
                ModTitle.setCaption(ModChooserMenu.getSelectedItem());

            GameManager.Singleton.mTrayMgr.showLogo(TrayLocation.TL_RIGHT);
            GameManager.Singleton.mTrayMgr.createSeparator(TrayLocation.TL_RIGHT, "LogoSep");
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "Play", "Play", 140);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "Configure", "Configure", 140);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "Quit", "Quit", 140);
            
            SetupModMenu();

            GameManager.Singleton.mMouse.MouseMoved += new MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Singleton.mMouse.MousePressed += new MouseListener.MousePressedHandler(mMouse_MousePressed);
            GameManager.Singleton.mMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mMouse_MouseReleased);
            GameManager.Singleton.mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(mRoot_FrameRenderingQueued);
        }

        bool mRoot_FrameRenderingQueued(FrameEvent evt)
        {
            selectedModName = ModChooserMenu.getSelectedItem();
            float carouselOffset = ModChooserMenu.getSelectionIndex() - mCarouselPlace;
            if ((carouselOffset <= 0.001) && (carouselOffset >= -0.001)) mCarouselPlace = ModChooserMenu.getSelectionIndex();
            else mCarouselPlace += carouselOffset * Clamp((float)evt.timeSinceLastFrame * 15.0f, -1.0f, 1.0f);

            for (int i = 0; i < mModThumbs.Count; i++)
            {
                float thumbOffset = mCarouselPlace - i;
                float phase = (thumbOffset / 2.0f) - 2.8f;

                if (thumbOffset < -5 || thumbOffset > 4)    // prevent thumbnails from wrapping around in a circle
                {
                    mModThumbs[i].Hide();
                    continue;
                }
                else mModThumbs[i].Show();

                float left = Mogre.Math.Cos(phase) * 200.0f;
                float top = Mogre.Math.Sin(phase) * 200.0f;
                float scale = 1.0f / Convert.ToSingle(System.Math.Pow((Mogre.Math.Abs(thumbOffset) + 1.0f), 0.75f));

                OverlayContainer.ChildContainerIterator xx = mModThumbs[i].GetChildContainerIterator();
                BorderPanelOverlayElement frame = (BorderPanelOverlayElement)xx.ElementAt(0);

                mModThumbs[i].SetDimensions(128.0f * scale, 96.0f * scale);
                frame.SetDimensions(mModThumbs[i].Width + 16.0f, mModThumbs[i].Height + 16.0f);
                mModThumbs[i].SetPosition((left - 80.0f - (mModThumbs[i].Width / 2.0f)),
                    (top - 5.0f - (mModThumbs[i].Height / 2.0f)));

                if (i == ModChooserMenu.getSelectionIndex())
                    frame.BorderMaterialName = "SdkTrays/Frame/Over";
                else
                    frame.BorderMaterialName = "SdkTrays/Frame";
            }

            GameManager.Singleton.mTrayMgr.frameRenderingQueued(evt);

            return true;
        }

        bool mMouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Singleton.mTrayMgr.injectMouseUp(arg, id);
        }

        bool mMouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Singleton.mTrayMgr.injectMouseDown(arg, id);
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {

            MouseState_NativePtr state = arg.state;
            if (arg.state.Z.rel != 0 && ModChooserMenu.getNumItems() != 0)
            {
                float newIndex = ModChooserMenu.getSelectionIndex() - arg.state.Z.rel / Mogre.Math.Abs((float)arg.state.Z.rel);
                float finalIndex = Clamp(newIndex, 0.0f, (float)(ModChooserMenu.getNumItems() - 1));
                ModChooserMenu.selectItem((uint)finalIndex);
                selectedModName = ModChooserMenu.getSelectedItem();
            }

            return GameManager.Singleton.mTrayMgr.injectMouseMove(arg);
        }

        public override bool pause()
        {
            return base.pause();
        }

        public override void resume()
        {
            base.resume();
        }

        public override void exit()
        {
            m_SceneMgr.DestroyCamera(m_Camera);
            if (m_SceneMgr != null)
                GameManager.Singleton.mRoot.DestroySceneManager(m_SceneMgr);

            GameManager.Singleton.mMouse.MouseMoved -= new MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Singleton.mRoot.FrameRenderingQueued -= new FrameListener.FrameRenderingQueuedHandler(mRoot_FrameRenderingQueued);
            foreach (BorderPanelOverlayElement bp in mModThumbs)
            {
                GameManager.Singleton.mTrayMgr.getTraysLayer().Remove2D(bp);
            }
        }

        public override void update(double timeSinceLastFrame)
        {
            if (isQuit == true)
            {
                shutdown();
                return;
            }
        }

        public override void buttonHit(Button button)
        {
            //if (button.getName() == "Play")
            //{
            //    if (GameManager.Singleton.mModMgr.ModStateChangedAction != null)
            //    {
            //        GameManager.Singleton.mModMgr.ModStateChangedAction(new ModEventArgs()
            //        {
            //            modState = ModState.Run,
            //            modName = ModChooserMenu.getSelectedItem(),
            //            modIndex = ModChooserMenu.getSelectionIndex()
            //        });
            //    }
            //}
            //else if (button.getName() == "Configure")
            //{
            //    ConfigureScreen();
            //}
            //else if (button.getName() == "Quit")
            //{
            //    isQuit = true;
            //}
        }

        void SetupModMenu()
        {
            MaterialPtr thumbMat = MaterialManager.Singleton.Create("ModThumbnail", "Essential");
            thumbMat.GetTechnique(0).GetPass(0).CreateTextureUnitState();
            MaterialPtr templateMat = MaterialManager.Singleton.GetByName("ModThumbnail");

            foreach ( string itr in mModThumb )
            {

                String name = "ModThumb" + (mModThumbs.Count + 1).ToString();

                MaterialPtr newMat = templateMat.Clone(name);

                TextureUnitState tus = newMat.GetTechnique(0).GetPass(0).GetTextureUnitState(0);
                if (ResourceGroupManager.Singleton.ResourceExists("Essential", itr + ".png"))
                    tus.SetTextureName(itr + ".png");
                else 
                    tus.SetTextureName("thumb_error.png");

                BorderPanelOverlayElement bp = null;
                if (!OverlayManager.Singleton.HasOverlayElement(itr))
                {
                    bp = (BorderPanelOverlayElement)
                        OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Picture", "BorderPanel", (itr));
                }
                else
                {
                    bp = (BorderPanelOverlayElement)OverlayManager.Singleton.GetOverlayElement(itr);
                }


                bp.HorizontalAlignment=(GuiHorizontalAlignment. GHA_RIGHT);
                bp.VerticalAlignment=(GuiVerticalAlignment. GVA_CENTER);
                bp.MaterialName=(name);
                GameManager.Singleton.mTrayMgr .getTraysLayer().Add2D(bp);

                mModThumbs.Add(bp);
            }  
        }

        void ConfigureScreen()
        {
            
        }

        protected float Clamp(float value, float min, float max)
        {
            if (value <= min)
                return min;
            else if (value >= max)
                return max;

            return value;
        }
        protected int Clamp(int value, int min, int max)
        {
            if (value <= min)
                return min;
            else if (value >= max)
                return max;

            return value;
        }
    }
}
