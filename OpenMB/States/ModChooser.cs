using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;
using OpenMB.Localization;
using OpenMB.Mods;
using OpenMB.Screen;
using OpenMB.Widgets;

namespace OpenMB.States
{
    using Mods = Dictionary<string, ModManifest>;

    public class ModDisplayData
    {
        public readonly string ID;
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Thumb { get; set; }

        public ModDisplayData(string id)
        {
            ID = id;
        }
    }

    public class ModChooser : AppState
    {
        private bool isQuit;
        private SelectMenuWidget modChooserMenu;
        private LabelWidget modTitle;
        private StaticMultiLineTextBoxWidget modDescBox;
        private Slider modSlider;
        private List<ModDisplayData> modDisplayDataList;
        private Mods mods;
        private List<OverlayContainer> modThumbs;
        private float carouselPlace;
        private string selectedModName;

        public ModChooser()
        {
            isQuit = false;
            modThumbs = new List<OverlayContainer>();
            modDisplayDataList = new List<ModDisplayData>();
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
            modDisplayDataList.Clear();

            mods = ModManager.Instance.GetInstalledMods();
            foreach (var pair in mods)
            {
                if (pair.Value.MetaData.DisplayInChooser)
                {
                    ModDisplayData modDisplayData = new ModDisplayData(pair.Key);
                    modDisplayData.Name = pair.Value.MetaData.Name;
                    modDisplayData.Desc = pair.Value.MetaData.Description;
                    modDisplayData.Thumb = pair.Value.MetaData.Thumb;
                    modDisplayDataList.Add(modDisplayData);
                }
            }

            UIManager.Instance.DestroyAllWidgets();
            modTitle = UIManager.Instance.CreateLabel(UIWidgetLocation.TL_CENTER, "ModTitle", "Mod Info");
            modTitle.setCaption("Mod Info");
            modDescBox = UIManager.Instance.CreateTextBox(UIWidgetLocation.TL_CENTER, "ModInfo", "Mod Info", 250, 208);
            modDescBox.setCaption("Mod Info");
            modChooserMenu = UIManager.Instance.CreateThickSelectMenu(UIWidgetLocation.TL_CENTER, "SelMod", "Select Mod", 250, 10);
            modChooserMenu.setCaption("Select Mod");
            modChooserMenu.SetItems(modDisplayDataList.Select(o=>o.Name).ToList());
            modSlider = UIManager.Instance.CreateThickSlider(UIWidgetLocation.TL_CENTER, "ModSlider", "Slider Mods", 250, 80, 0, 0, 0);
            modSlider.setCaption("Slider Mods");
            if (modDisplayDataList.Count > 0)
            {
                modTitle.setCaption(modChooserMenu.getSelectedItem());
            }

            UIManager.Instance.ShowLogo(UIWidgetLocation.TL_RIGHT);
            UIManager.Instance.CreateSeparator(UIWidgetLocation.TL_RIGHT, "LogoSep");
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "Play", LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameString, "str_play"), 160);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "Mods", LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameString, "str_mods"), 160);
            UIManager.Instance.CreateButton(UIWidgetLocation.TL_RIGHT, "Quit", LocateSystem.Instance.GetLocalizedString(Localization.LocateFileType.GameString, "str_quit"), 160);
            
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

            UIManager.Instance.FrameRenderingQueued(evt);

            return true;
        }

        bool Mouse_MouseReleased(MouseEvent arg, MouseButtonID id)
        {
            UIManager.Instance.InjectMouseUp(arg, id);
			return true;
        }

        bool Mouse_MousePressed(MouseEvent arg, MouseButtonID id)
        {
            UIManager.Instance.InjectMouseDown(arg, id);
			return true;
        }

        bool Mouse_MouseMoved(MOIS.MouseEvent arg)
        {

            MouseState_NativePtr state = arg.state;
            if (arg.state.Z.rel != 0 && modChooserMenu.GetNumItems() != 0)
            {
                float newIndex = modChooserMenu.getSelectionIndex() - arg.state.Z.rel / Mogre.Math.Abs((float)arg.state.Z.rel);
                float finalIndex = OpenMB.Utilities.Helper.Clamp<float>(newIndex, 0.0f, (float)(modChooserMenu.GetNumItems() - 1));
               
                modChooserMenu.SelectItem((uint)finalIndex);
                //modTitle.setCaption("modChooserMenu.getSelectedItem()");
                modTitle.setCaption(
                    LocateSystem.Instance.GetLocalizedString(
                        "module_info_name", 
                        modChooserMenu.getSelectedItem(),
                        modDisplayDataList[modChooserMenu.Items.IndexOf(modChooserMenu.getSelectedItem())].ID
                    ));

                //modDescBox.setText(modDisplayDataList[modChooserMenu.Items.IndexOf(modChooserMenu.getSelectedItem())].Desc);
                modDescBox.setText(
                    LocateSystem.Instance.GetLocalizedString(
                        "module_info_desc",
                        modDisplayDataList[modChooserMenu.Items.IndexOf(modChooserMenu.getSelectedItem())].Desc,
                        modDisplayDataList[modChooserMenu.Items.IndexOf(modChooserMenu.getSelectedItem())].ID
                    ));

                selectedModName = modChooserMenu.getSelectedItem();
            }

			UIManager.Instance.InjectMouseMove(arg);
			return true;
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
                UIManager.Instance.GetTraysLayer().Remove2D(bp);
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

        public override void buttonHit(ButtonWidget button)
        {
            if (button.Name == "Play")
            {
                string modID = modDisplayDataList.Where(o => o.Name == selectedModName).First().ID;
                GameManager.Instance.loadingData = new LoadingData(LoadingType.LOADING_MOD, "Loading Mod...Please wait", modID, "MainMenu");
                changeAppState(findByName("Loading"));
            }
            else if (button.Name == "Configure")
            {
                ConfigureScreen();
            }
            else if (button.Name == "Mods")
            {
                ScreenManager.Instance.ChangeScreen("ModBrowser");
            }
            else if (button.Name == "Quit")
            {
                isQuit = true;
            }
        }

        private void setupModMenu()
        {
            MaterialPtr thumbMat = MaterialManager.Singleton.Create("ModThumbnail", "General");
            thumbMat.GetTechnique(0).GetPass(0).CreateTextureUnitState();
            MaterialPtr templateMat = MaterialManager.Singleton.GetByName("ModThumbnail");

            foreach ( var modDisplayData in modDisplayDataList )
            {
                string name = "ModThumb" + (modThumbs.Count + 1).ToString();

                MaterialPtr newMat = templateMat.Clone(name);

                TextureUnitState tus = newMat.GetTechnique(0).GetPass(0).GetTextureUnitState(0);
                if (ResourceGroupManager.Singleton.ResourceExists("General", modDisplayData.Thumb))
                    tus.SetTextureName(modDisplayData.Thumb);
                else
                    tus.SetTextureName("thumb_error.png");

                BorderPanelOverlayElement bp = (BorderPanelOverlayElement)
                        OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Picture", "BorderPanel", (name));


                bp.HorizontalAlignment=(GuiHorizontalAlignment. GHA_RIGHT);
                bp.VerticalAlignment=(GuiVerticalAlignment. GVA_CENTER);
                bp.MaterialName=(name);
                UIManager.Instance.GetTraysLayer().Add2D(bp);

                modThumbs.Add(bp);
            }  
        }

        void ConfigureScreen()
        {
            
        }
    }
}
