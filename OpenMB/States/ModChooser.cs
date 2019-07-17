using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Localization;
using OpenMB.Mods;

namespace OpenMB.States
{
    using Mods = Dictionary<string, ModManifest>;

    public class ModChooser : AppState
    {
        private bool isQuit;
        private SelectMenu modChooserMenu;
        private Label modTitle;
        private TextBox modDescBox;
        private Slider modSlider;
        private StringVector modNames;
        private StringVector modDescs;
        private StringVector modThumb;
        private Mods mods;
        private List<OverlayContainer> modThumbs;
        private float carouselPlace;
        private string selectedModName;

        public ModChooser()
        {
            isQuit = false;

            modNames = new StringVector();
            modThumb = new StringVector();
            modDescs = new StringVector();
            modThumbs = new List<OverlayContainer>();
        }

        public override void enter(ModData e = null)
        {
            sceneMgr = GameManager.Instance.root.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "ModChooserSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            sceneMgr.AmbientLight = cvAmbineLight;

            camera = sceneMgr.CreateCamera("ModChooserCam");
            camera.SetPosition(0, 25, -50);
            Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
            camera.LookAt(vectorCameraLookat);
            camera.NearClipDistance = 1;

            camera.AspectRatio = GameManager.Instance.viewport.ActualWidth / GameManager.Instance.viewport.ActualHeight;

            GameManager.Instance.viewport.Camera = camera;
            modNames.Clear();
            modThumb.Clear();

            mods = ModManager.Instance.GetInstalledMods();
            foreach (var mod in mods)
            {
                if (mod.Value.MetaData.DisplayInChooser)
                {
                    modNames.Add(mod.Key);
                    modDescs.Add(mod.Value.MetaData.Description);
                    modThumb.Add(mod.Value.MetaData.Thumb);
                }
            }

            GameManager.Instance.trayMgr.destroyAllWidgets();
            modTitle = GameManager.Instance.trayMgr.createLabel(TrayLocation.TL_LEFT, "ModTitle", "Mod Info");
            modTitle.setCaption("Mod Info");
            modDescBox = GameManager.Instance.trayMgr.createTextBox(TrayLocation.TL_LEFT, "ModInfo", "Mod Info", 250, 208);
            modDescBox.setCaption("Mod Info");
            modChooserMenu = GameManager.Instance.trayMgr.createThickSelectMenu(TrayLocation.TL_LEFT, "SelMod", "Select Mod", 250, 10);
            modChooserMenu.setCaption("Select Mod");
            modChooserMenu.setItems(modNames);
            modSlider = GameManager.Instance.trayMgr.createThickSlider(TrayLocation.TL_LEFT, "ModSlider", "Slider Mods", 250, 80, 0, 0, 0);
            modSlider.setCaption("Slider Mods");
            if (modNames.Count > 0)
            {
                modTitle.setCaption(modChooserMenu.getSelectedItem());
            }

            GameManager.Instance.trayMgr.showLogo(TrayLocation.TL_RIGHT);
            GameManager.Instance.trayMgr.createSeparator(TrayLocation.TL_RIGHT, "LogoSep");
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "Play", LocateSystem.Singleton.GetLocalizedString(Localization.LocateFileType.GameString, "str_play"), 140);
            GameManager.Instance.trayMgr.createButton(TrayLocation.TL_RIGHT, "Quit", LocateSystem.Singleton.GetLocalizedString(Localization.LocateFileType.GameString, "str_quit"), 140);
            
            setupModMenu();

            GameManager.Instance.mouse.MouseMoved += Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed += Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased += Mouse_MouseReleased;
            GameManager.Instance.root.FrameRenderingQueued += FrameRenderingQueued;
        }

        bool FrameRenderingQueued(FrameEvent evt)
        {
            selectedModName = modChooserMenu.getSelectedItem();
            float carouselOffset = modChooserMenu.getSelectionIndex() - carouselPlace;
            if ((carouselOffset <= 0.001) && (carouselOffset >= -0.001)) carouselPlace = modChooserMenu.getSelectionIndex();
            else carouselPlace += carouselOffset * OpenMB.Utilities.Helper.Clamp<float>(evt.timeSinceLastFrame * 15.0f, -1.0f, 1.0f);

            for (int i = 0; i < modThumbs.Count; i++)
            {
                float thumbOffset = carouselPlace - i;
                float phase = (thumbOffset / 2.0f) - 2.8f;

                if (thumbOffset < -5 || thumbOffset > 4)    // prevent thumbnails from wrapping around in a circle
                {
                    modThumbs[i].Hide();
                    continue;
                }
                else modThumbs[i].Show();

                float left = Mogre.Math.Cos(phase) * 200.0f;
                float top = Mogre.Math.Sin(phase) * 200.0f;
                float scale = 1.0f / Convert.ToSingle(System.Math.Pow((Mogre.Math.Abs(thumbOffset) + 1.0f), 0.75f));

                OverlayContainer.ChildContainerIterator xx = modThumbs[i].GetChildContainerIterator();
                BorderPanelOverlayElement frame = (BorderPanelOverlayElement)xx.ElementAt(0);

                modThumbs[i].SetDimensions(128.0f * scale, 96.0f * scale);
                frame.SetDimensions(modThumbs[i].Width + 16.0f, modThumbs[i].Height + 16.0f);
                modThumbs[i].SetPosition((left - 80.0f - (modThumbs[i].Width / 2.0f)),
                    (top - 5.0f - (modThumbs[i].Height / 2.0f)));

                if (i == modChooserMenu.getSelectionIndex())
                    frame.BorderMaterialName = "SdkTrays/Frame/Over";
                else
                    frame.BorderMaterialName = "SdkTrays/Frame";
            }

            GameManager.Instance.trayMgr.frameRenderingQueued(evt);

            return true;
        }

        bool Mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Instance.trayMgr.injectMouseUp(arg, id);
        }

        bool Mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            return GameManager.Instance.trayMgr.injectMouseDown(arg, id);
        }

        bool Mouse_MouseMoved(MOIS.MouseEvent arg)
        {

            MouseState_NativePtr state = arg.state;
            if (arg.state.Z.rel != 0 && modChooserMenu.getNumItems() != 0)
            {
                float newIndex = modChooserMenu.getSelectionIndex() - arg.state.Z.rel / Mogre.Math.Abs((float)arg.state.Z.rel);
                float finalIndex = OpenMB.Utilities.Helper.Clamp<float>(newIndex, 0.0f, (float)(modChooserMenu.getNumItems() - 1));
                modChooserMenu.selectItem((uint)finalIndex);
                modTitle.setCaption(modChooserMenu.getSelectedItem());
                modDescBox.setText(modDescs[modNames.ToList().IndexOf(modChooserMenu.getSelectedItem())]);
                selectedModName = modChooserMenu.getSelectedItem();
            }

            return GameManager.Instance.trayMgr.injectMouseMove(arg);
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
            sceneMgr.DestroyCamera(camera);
            if (sceneMgr != null)
                GameManager.Instance.root.DestroySceneManager(sceneMgr);

            GameManager.Instance.mouse.MouseMoved -= new MouseListener.MouseMovedHandler(Mouse_MouseMoved);
            GameManager.Instance.root.FrameRenderingQueued -= new FrameListener.FrameRenderingQueuedHandler(FrameRenderingQueued);
            foreach (BorderPanelOverlayElement bp in modThumbs)
            {
                GameManager.Instance.trayMgr.getTraysLayer().Remove2D(bp);
            }

            GameManager.Instance.mouse.MouseMoved -= Mouse_MouseMoved;
            GameManager.Instance.mouse.MousePressed -= Mouse_MousePressed;
            GameManager.Instance.mouse.MouseReleased -= Mouse_MouseReleased;
            GameManager.Instance.root.FrameRenderingQueued -= FrameRenderingQueued;
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
                GameManager.Instance.loadingData = new LoadingData(LoadingType.LOADING_MOD, "Loading Mod...Please wait", selectedModName, "MainMenu");
                changeAppState(findByName("Loading"));
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

        private void setupModMenu()
        {
            MaterialPtr thumbMat = MaterialManager.Singleton.Create("ModThumbnail", "General");
            thumbMat.GetTechnique(0).GetPass(0).CreateTextureUnitState();
            MaterialPtr templateMat = MaterialManager.Singleton.GetByName("ModThumbnail");

            foreach ( string itr in modThumb )
            {
                string name = "ModThumb" + (modThumbs.Count + 1).ToString();

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
                GameManager.Instance.trayMgr.getTraysLayer().Add2D(bp);

                modThumbs.Add(bp);
            }  
        }

        void ConfigureScreen()
        {
            
        }
    }
}
