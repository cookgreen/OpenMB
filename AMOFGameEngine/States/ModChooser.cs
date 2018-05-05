using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using AMOFGameEngine.Localization;
using AMOFGameEngine.Mods;

namespace AMOFGameEngine.States
{
    using Mods = Dictionary<string, ModManifest>;

    class ModChooser : AppState
    {
        private bool isQuit;
        private SelectMenu ModChooserMenu;
        private Label ModTitle;
        private TextBox ModDescBox;
        private Slider ModSlider;
        private ProgressBar pbProcessBar;
        private StringVector mModNames;
        private StringVector mModDescs;
        private StringVector mModThumb;
        private Mods mMods;
        private List<OverlayContainer> mModThumbs;
        private float mCarouselPlace;
        private string selectedModName;

        public ModChooser()
        {
            isQuit = false;

            mModNames = new StringVector();
            mModThumb = new StringVector();
            mModDescs = new StringVector();
            mModThumbs = new List<OverlayContainer>();
        }

        public override void enter(ModData e = null)
        {
            m_SceneMgr = GameManager.Instance.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "ModChooserSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;

            m_Camera = m_SceneMgr.CreateCamera("ModChooserCam");
            m_Camera.SetPosition(0, 25, -50);
            Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance = 1;

            m_Camera.AspectRatio = GameManager.Instance.mViewport.ActualWidth / GameManager.Instance.mViewport.ActualHeight;

            GameManager.Instance.mViewport.Camera = m_Camera;
            mModNames.Clear();
            mModThumb.Clear();

            mMods = ModManager.Instance.GetInstalledMods();
            foreach (var mod in mMods)
            {
                mModNames.Add(mod.Key);
                mModDescs.Add(mod.Value.MetaData.Description);
                mModThumb.Add(mod.Value.MetaData.Thumb);
            }

            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            ModTitle = GameManager.Instance.mTrayMgr.createLabel(TrayLocation.TL_LEFT, "ModTitle", "Mod Info");
            ModTitle.setCaption("Mod Info");
            ModDescBox = GameManager.Instance.mTrayMgr.createTextBox(TrayLocation.TL_LEFT, "ModInfo", "Mod Info", 250, 208);
            ModDescBox.setCaption("Mod Info");
            ModChooserMenu = GameManager.Instance.mTrayMgr.createThickSelectMenu(TrayLocation.TL_LEFT, "SelMod", "Select Mod", 250, 10);
            ModChooserMenu.setCaption("Select Mod");
            ModChooserMenu.setItems(mModNames);
            ModSlider = GameManager.Instance.mTrayMgr.createThickSlider(TrayLocation.TL_LEFT, "ModSlider", "Slider Mods", 250, 80, 0, 0, 0);
            ModSlider.setCaption("Slider Mods");
            if (mModNames.Count > 0)
            {
                ModTitle.setCaption(ModChooserMenu.getSelectedItem());
            }

            GameManager.Instance.mTrayMgr.showLogo(TrayLocation.TL_RIGHT);
            GameManager.Instance.mTrayMgr.createSeparator(TrayLocation.TL_RIGHT, "LogoSep");
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "Play", LocateSystem.Singleton.GetLocalizedString(Localization.LocateFileType.GameString, "str_play"), 140);
            GameManager.Instance.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "Quit", LocateSystem.Singleton.GetLocalizedString(Localization.LocateFileType.GameString, "str_quit"), 140);
            
            SetupModMenu();

            GameManager.Instance.mMouse.MouseMoved += mMouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed += mMouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased += mMouse_MouseReleased;
            GameManager.Instance.mRoot.FrameRenderingQueued += mRoot_FrameRenderingQueued;

            ModManager.Instance.LoadingModStarted += new Action(LoadingModStarted);
            ModManager.Instance.LoadingModFinished+=new Action(LoadingModFinished);
            ModManager.Instance.LoadingModProcessing += new Action<int>(LoadingModProcessing);
        }

        void LoadingModProcessing(int obj)
        {
            switch (obj)
            {
                case 25:
                    pbProcessBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_processing_module_file"));
                    break;
                case 50:
                    pbProcessBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_loading_resource"));
                    break;
                case 75:
                    pbProcessBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_loading_module_data"));
                    break;
                case 100:
                    pbProcessBar.setComment(LocateSystem.Singleton.GetLocalizedString(LocateFileType.GameString, "str_finished"));
                    break;
            }
            pbProcessBar.setProgress(obj / 100);
        }


        void LoadingModFinished()
        {
            m_Data = ModManager.Instance.ModData;
            changeAppState(findByName("MainMenu"), m_Data);
        }

        void LoadingModStarted()
        {
            CreateLoadingScreen();
        }

        bool mRoot_FrameRenderingQueued(FrameEvent evt)
        {
            selectedModName = ModChooserMenu.getSelectedItem();
            float carouselOffset = ModChooserMenu.getSelectionIndex() - mCarouselPlace;
            if ((carouselOffset <= 0.001) && (carouselOffset >= -0.001)) mCarouselPlace = ModChooserMenu.getSelectionIndex();
            else mCarouselPlace += carouselOffset * AMOFGameEngine.Utilities.Helper.Clamp<float>(evt.timeSinceLastFrame * 15.0f, -1.0f, 1.0f);

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

            GameManager.Instance.mTrayMgr.frameRenderingQueued(evt);

            return true;
        }

        bool mMouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Instance.mTrayMgr.injectMouseUp(arg, id);
        }

        bool mMouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Instance.mTrayMgr.injectMouseDown(arg, id);
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {

            MouseState_NativePtr state = arg.state;
            if (arg.state.Z.rel != 0 && ModChooserMenu.getNumItems() != 0)
            {
                float newIndex = ModChooserMenu.getSelectionIndex() - arg.state.Z.rel / Mogre.Math.Abs((float)arg.state.Z.rel);
                float finalIndex = AMOFGameEngine.Utilities.Helper.Clamp<float>(newIndex, 0.0f, (float)(ModChooserMenu.getNumItems() - 1));
                ModChooserMenu.selectItem((uint)finalIndex);
                ModTitle.setCaption(ModChooserMenu.getSelectedItem());
                ModDescBox.setText(mModDescs[mModNames.ToList().IndexOf(ModChooserMenu.getSelectedItem())]);
                selectedModName = ModChooserMenu.getSelectedItem();
            }

            return GameManager.Instance.mTrayMgr.injectMouseMove(arg);
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
                GameManager.Instance.mRoot.DestroySceneManager(m_SceneMgr);

            GameManager.Instance.mMouse.MouseMoved -= new MouseListener.MouseMovedHandler(mMouse_MouseMoved);
            GameManager.Instance.mRoot.FrameRenderingQueued -= new FrameListener.FrameRenderingQueuedHandler(mRoot_FrameRenderingQueued);
            foreach (BorderPanelOverlayElement bp in mModThumbs)
            {
                GameManager.Instance.mTrayMgr.getTraysLayer().Remove2D(bp);
            }

            GameManager.Instance.mMouse.MouseMoved -= mMouse_MouseMoved;
            GameManager.Instance.mMouse.MousePressed -= mMouse_MousePressed;
            GameManager.Instance.mMouse.MouseReleased -= mMouse_MouseReleased;
            GameManager.Instance.mRoot.FrameRenderingQueued -= mRoot_FrameRenderingQueued;
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
            if (button.getName() == "Play")
            {
                ModManager.Instance.LoadMod(selectedModName);
            }
            else if (button.getName() == "Configure")
            {
                ConfigureScreen();
            }
            else if (button.getName() == "Quit")
            {
                isQuit = true;
            }
        }

        void SetupModMenu()
        {
            MaterialPtr thumbMat = MaterialManager.Singleton.Create("ModThumbnail", "General");
            thumbMat.GetTechnique(0).GetPass(0).CreateTextureUnitState();
            MaterialPtr templateMat = MaterialManager.Singleton.GetByName("ModThumbnail");

            foreach ( string itr in mModThumb )
            {
                String name = "ModThumb" + (mModThumbs.Count + 1).ToString();

                MaterialPtr newMat = templateMat.Clone(name);

                TextureUnitState tus = newMat.GetTechnique(0).GetPass(0).GetTextureUnitState(0);
                if (ResourceGroupManager.Singleton.ResourceExists("General", itr))
                    tus.SetTextureName(itr);
                else
                    tus.SetTextureName("thumb_error.png");

                BorderPanelOverlayElement bp = (BorderPanelOverlayElement)
                        OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Picture", "BorderPanel", (name));


                bp.HorizontalAlignment=(GuiHorizontalAlignment. GHA_RIGHT);
                bp.VerticalAlignment=(GuiVerticalAlignment. GVA_CENTER);
                bp.MaterialName=(name);
                GameManager.Instance.mTrayMgr.getTraysLayer().Add2D(bp);

                mModThumbs.Add(bp);
            }  
        }

        void ConfigureScreen()
        {
            
        }

        private void CreateLoadingScreen()
        {
            foreach (BorderPanelOverlayElement bp in mModThumbs)
            {
                GameManager.Instance.mTrayMgr.getTraysLayer().Remove2D(bp);
            }
            GameManager.Instance.mTrayMgr.destroyAllWidgets();
            pbProcessBar = GameManager.Instance.mTrayMgr.createProgressBar(TrayLocation.TL_CENTER, "pbProcessBar", "Loading", 500, 300);
            pbProcessBar.setComment("Loading Mod...Please be paient");
        }
    }
}
