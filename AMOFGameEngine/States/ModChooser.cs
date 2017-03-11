using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre_Procedural.MogreBites;
using MOIS;

namespace AMOFGameEngine.States
{
    class ModChooser : AppState
    {
        SelectMenu m_pMenu;
        TextBox mDescBox;
        Slider mSampleSlider;
        StringVector m_SampleNames;
        List<OverlayContainer> m_Thumbs;
        float m_CarouselPlace;

        public ModChooser()
        {
            m_SampleNames = new StringVector();
            m_Thumbs = new List<OverlayContainer>();
            m_SampleNames.Add("thumb_bezier");
            m_SampleNames.Add("thumb_bsp");
            m_SampleNames.Add("thumb_bump");
            m_SampleNames.Add("thumb_camtrack");
            m_SampleNames.Add("thumb_cel");
        }

        public override void enter()
        {
            m_SceneMgr = GameManager.Singleton.mRoot.CreateSceneManager(Mogre.SceneType.ST_GENERIC, "ModChooserSceneMgr");

            ColourValue cvAmbineLight = new ColourValue(0.7f, 0.7f, 0.7f);
            m_SceneMgr.AmbientLight = cvAmbineLight;

            m_Camera = m_SceneMgr.CreateCamera("MenuCam");
            m_Camera.SetPosition(0, 25, -50);
            Mogre.Vector3 vectorCameraLookat = new Mogre.Vector3(0, 0, 0);
            m_Camera.LookAt(vectorCameraLookat);
            m_Camera.NearClipDistance = 1;//setNearClipDistance(1);

            m_Camera.AspectRatio = GameManager.Singleton.mViewport.ActualWidth / GameManager.Singleton.mViewport.ActualHeight;

            GameManager.Singleton.mViewport.Camera = m_Camera;
            mDescBox = GameManager.Singleton.mTrayMgr.createTextBox(TrayLocation.TL_LEFT, "ModInfo", "Mod Info", 250, 208);
            m_pMenu = GameManager.Singleton.mTrayMgr.createThickSelectMenu(TrayLocation.TL_LEFT, "Mod", "Mod", 250, 10);
            mSampleSlider = GameManager.Singleton.mTrayMgr.createThickSlider(TrayLocation.TL_LEFT, "SampleSlider", "Slide Samples", 250, 80, 0, 0, 0);
            m_pMenu.setItems(m_SampleNames);

            GameManager.Singleton.mTrayMgr.showLogo(TrayLocation.TL_RIGHT);
            GameManager.Singleton.mTrayMgr.createSeparator(TrayLocation.TL_RIGHT, "LogoSep");
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnStart", "Play",140);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnConfigure", "Config", 140);
            GameManager.Singleton.mTrayMgr.createButton(TrayLocation.TL_RIGHT, "btnExit", "Exit", 140);

            //GameManager.Singleton.mTrayMgr.createLabel(TrayLocation.TL_TOP, "lbMod", "Choose your mod");
            SetupModMenu();

            GameManager.Singleton.mMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(mMouse_MouseMoved);
        }

        bool mMouse_MouseMoved(MOIS.MouseEvent arg)
        {
            MouseState_NativePtr state = arg.state;
            if (arg.state.Z.rel != 0 && m_pMenu.getNumItems() != 0)
            {
                int newIndex = m_pMenu.getSelectionIndex() - arg.state.Z.rel / System.Math.Abs(arg.state.Z.rel);
                float finalIndex = GameManager.Singleton.Clamp((float)newIndex, 0f, (float)(m_pMenu.getNumItems() - 1));
                m_pMenu.selectItem((uint)finalIndex);
            }

            if (GameManager.Singleton.mTrayMgr.injectMouseMove(arg))
                return true;
            return false;
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
            base.exit();
        }

        public override void update(double timeSinceLastFrame)
        {
            base.update(timeSinceLastFrame);

            float carouselOffset = m_pMenu.getSelectionIndex() - m_CarouselPlace;
            if ((carouselOffset <= 0.001) && (carouselOffset >= -0.001)) m_CarouselPlace = m_pMenu.getSelectionIndex();
            else m_CarouselPlace += carouselOffset * AMOFGameEngine.GameManager.Singleton. Clamp((float)timeSinceLastFrame * 0.015f, -1.0f, 1.0f);

            for (int i = 0; i < (int)m_Thumbs.Count; i++)
            {
                float thumbOffset = m_CarouselPlace - i;
                float phase = (thumbOffset / 2.0f) - 2.8f;

                if (thumbOffset < -5 || thumbOffset > 4)    // prevent thumbnails from wrapping around in a circle
                {
                    m_Thumbs[i].Hide();
                    continue;
                }
                else m_Thumbs[i].Show();

                float left = (float)(System.Math.Cos(phase) * 200.0);
                float top = (float)(System.Math.Sin(phase) * 200.0);
                float scale = (float)(1.0 / System.Math.Pow((System.Math.Abs(thumbOffset) + 1.0), 0.75));

                BorderPanelOverlayElement frame = (BorderPanelOverlayElement)m_Thumbs[i].GetChildIterator().ElementAt(0);

                m_Thumbs[i].SetDimensions(128.0f * scale, 96.0f * scale);
                frame.SetDimensions(m_Thumbs[i].Width + 16.0f, m_Thumbs[i].Height + 16.0f);
                m_Thumbs[i].SetPosition((int)(left - 80.0 - (m_Thumbs[i].Width / 2.0)),
                    (int)(top - 5.0 - (m_Thumbs[i].Height / 2.0)));

                if (i == m_pMenu.getSelectionIndex()) frame.BorderMaterialName=("SdkTrays/Frame/Over");
                else frame.BorderMaterialName=("SdkTrays/Frame");
            }
        }

        void SetupModMenu()
        {
            MaterialPtr thumbMat = MaterialManager.Singleton.Create("SampleThumbnail", "Essential");
            thumbMat.GetTechnique(0).GetPass(0).CreateTextureUnitState();
            MaterialPtr templateMat = MaterialManager.Singleton.GetByName("SampleThumbnail");

            foreach ( string itr in m_SampleNames )
            {

                String name = "SampleThumb" + (m_Thumbs.Count + 1).ToString();

                // clone a new material for sample thumbnail
                MaterialPtr newMat = templateMat.Clone(name);

                TextureUnitState tus = newMat.GetTechnique(0).GetPass(0).GetTextureUnitState(0);
                if (ResourceGroupManager.Singleton.ResourceExists("Essential", itr + ".png"))
                    tus.SetTextureName(itr + ".png");
                else 
                    tus.SetTextureName("thumb_error.png");

                BorderPanelOverlayElement bp = (BorderPanelOverlayElement)
                    OverlayManager.Singleton.CreateOverlayElementFromTemplate("SdkTrays/Picture", "BorderPanel", (itr));

                //Ogre::ResourceGroupManager::getSingletonPtr()->loadResourceGroup("Essential");

                bp.HorizontalAlignment=(GuiHorizontalAlignment. GHA_RIGHT);
                bp.VerticalAlignment=(GuiVerticalAlignment. GVA_CENTER);
                bp.MaterialName=(name);
                GameManager.Singleton.mTrayMgr .getTraysLayer().Add2D(bp);

                m_Thumbs.Add(bp);
            }
        }
    }
}
