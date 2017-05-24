using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;

namespace AMOFGameEngine.UI
{
    public class CharacterSelectionWindow : SdkTrayListener
    {
        /// <summary>
        /// Character List-key:Character Profession,value:Character Mesh FileName
        /// </summary>
        NameValuePairList characterLst;
        SdkTrayManager trayMgr;
        SceneManager scm;
        Camera cam;
        public CharacterSelectionWindow(NameValuePairList characterLst, SdkTrayManager trayMgr, Camera cam)
        {
            this.characterLst = characterLst;
            this.trayMgr = trayMgr;
            this.cam = cam;
            Mogre.Quaternion camDirection = cam.Orientation;
            GameManager.Singleton.mLog.LogMessage("Current Cam direction:\r\nx:" + camDirection.x
                + "\r\ny:" + camDirection.y + "\r\nz:" + camDirection.z + "\r\nw:" + camDirection.w + "\r\n");
            BuildUI();
            BuildCharacters();
        }

        public void BuildUI()
        {
            trayMgr.destroyAllWidgets();
            trayMgr.createLabel(TrayLocation.TL_TOP, "CharaName", "Select Your Character", 200);
            trayMgr.createButton(TrayLocation.TL_RIGHT | TrayLocation.TL_CENTER, "NextCharacter", "Next", 200);
            trayMgr.createButton(TrayLocation.TL_RIGHT | TrayLocation.TL_CENTER, "PrevCharacter", "Prev", 200);
            trayMgr.createButton(TrayLocation.TL_BOTTOMLEFT, "Exit", "Exit", 200);
            trayMgr.createButton(TrayLocation.TL_BOTTOMRIGHT, "Finish", "Finish", 200);
        }

        void BuildCharacters()
        {
            if (characterLst != null && characterLst.Count > 0)
            {
                foreach (KeyValuePair<string, string> kpl in characterLst)
                {
                    BuildCharacter(kpl.Key, kpl.Value);
                }
            }
        }

        void BuildCharacter(string characterName, string characterMeshName)
        {
            Entity characterEntity = cam.SceneManager.CreateEntity(characterName, string.Format("{0}.mesh", characterMeshName));
            SceneNode snCharacter = cam.SceneManager.RootSceneNode.CreateChildSceneNode();
            snCharacter.AttachObject(characterEntity);
            Mogre.Vector3 camPos = cam.Position;
            camPos.z = camPos.z - 100;
            snCharacter.SetPosition(camPos.x, camPos.y, camPos.z);
            Mogre.Vector3 currSacle = snCharacter.GetScale();
            GameManager.Singleton.mLog.LogMessage("Current Character :" + characterName + "\r\nCurrent Character Scale:\r\nx:" + currSacle.x
                + "\r\ny:" + currSacle.y + "\r\nz:" + currSacle.z + "\r\n");

            Quaternion characterDirection = snCharacter.Orientation;
            GameManager.Singleton.mLog.LogMessage("Current Cam :" + characterName + "\r\nCurrent Character Direction:\r\nx:" + characterDirection.x
                + "\r\ny:" + characterDirection.y + "\r\nz:" + characterDirection.z + "\r\nw:" + characterDirection.w + "\r\n");
        }

        public bool Update(float timeSinceLastFrame)
        {
            return true;
        }

        public void injectMouseDown(MouseEvent evt, MouseButtonID id)
        {

        }
        public override void buttonHit(Button button)
        {
            if (button.getName() == "NextCharacter")
            {

            }
            else if (button.getName() == "PrevCharacter")
            {

            }
            else if (button.getName() == "Exit")
            {

            }
            else if (button.getName() == "Finish")
            {

            }
        }
    }
}
