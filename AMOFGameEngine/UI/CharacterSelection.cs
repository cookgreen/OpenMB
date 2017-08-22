using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural.MogreBites;
using AMOFGameEngine.RPG;

namespace AMOFGameEngine.UI
{
    public class CharacterSelection : UIWindiw
    {
        /// <summary>
        /// Characters will show 
        /// </summary>
        List<Character> characters;
        Character currentCharacter;

        public CharacterSelection(List<Character> characterLst,Camera cam) : base(cam)
        {
            this.characters = characterLst;
            this.trayMgr = trayMgr;
            this.cam = cam;
            currentCharacter = null;
            Mogre.Quaternion camDirection = cam.Orientation;
            //GameManager.Singleton.mLog.LogMessage("Current Cam direction:\r\nx:" + camDirection.x
            //    + "\r\ny:" + camDirection.y + "\r\nz:" + camDirection.z + "\r\nw:" + camDirection.w + "\r\n");
        }

        public override void enter()
        {
            BuildUI();
        }

        public override void close()
        {
            base.close();
        }

        private void BuildUI()
        {
            trayMgr.destroyAllWidgets();
            trayMgr.createLabel(TrayLocation.TL_TOP, "CharaName", "Select Your Character", 200);
            trayMgr.createButton(TrayLocation.TL_RIGHT | TrayLocation.TL_CENTER, "NextCharacter", "Next", 200);
            trayMgr.createButton(TrayLocation.TL_RIGHT | TrayLocation.TL_CENTER, "PrevCharacter", "Prev", 200);
            trayMgr.createButton(TrayLocation.TL_BOTTOMLEFT, "Exit", "Exit", 200);
            trayMgr.createButton(TrayLocation.TL_BOTTOMRIGHT, "Finish", "Finish", 200);
        }

        public override void update(double timeSinceLastFrame)
        {
            if (shutdown)
            {
                Shutdown();
            }

            foreach (Character chara in characters)
            {
                chara.Update((float)timeSinceLastFrame);
            }
        }

        public void injectMouseDown(MouseEvent evt, MouseButtonID id)
        {

        }

        public override void buttonHit(Button button)
        {
            if (button.getName() == "NextCharacter")
            {
                int index = characters.IndexOf(currentCharacter);
                if (characters.Count > 0)
                {
                    if (index != characters.Count - 1)
                    {
                        currentCharacter = characters[index + 1];
                    }
                    else
                    {
                        currentCharacter = characters[0];
                    }
                    SwitchCharacter(currentCharacter);
                }
            }
            else if (button.getName() == "PrevCharacter")
            {
                int index = characters.IndexOf(currentCharacter);
                if (characters.Count > 0)
                {
                    if (index != 0)
                    {
                        currentCharacter = characters[index - 1];
                    }
                    else
                    {
                        currentCharacter = characters[0];
                    }
                    SwitchCharacter(currentCharacter);
                }
            }
            else if (button.getName() == "Exit")
            {
                shutdown = true;
            }
            else if (button.getName() == "Finish")
            {
                shutdown = true;
            }
        }

        private void SwitchCharacter(Character character)
        {
            currentCharacter.Hide();

            character.Show();

            currentCharacter = character;
        }
    }
}
