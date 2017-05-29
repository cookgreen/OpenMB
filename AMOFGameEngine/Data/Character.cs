using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine.Data
{
    public class Character
    {
        string charaTypeID;
        string charaID;
        string charaName;
        string charaMeshName;
        CharacterState charaState;
        LevelInfo level;
        int hitpoint;
        InventoryInfo inventory;

        public LevelInfo Level
        {
            get { return level; }
            set { level = value; }
        }
        public int HitPoint
        {
            get { return hitpoint; }
            set { hitpoint = value; }
        }
        public CharacterState CharaState
        {
            get { return charaState; }
            set { charaState = value; }
        }
        public string CharaMeshName
        {
            get { return charaMeshName; }
            set { charaMeshName = value; }
        }
        public string CharaName
        {
            get { return charaName; }
            set { charaName = value; }
        }
        public string CharaID
        {
            get { return charaID; }
        }
        public string CharaTypeID
        {
            get { return charaTypeID; }
            set { charaTypeID = value; }
        }

        public void Attack<T>() where T : Item
        {

        }

        public void MoveTolocation(Mogre.Vector3 location)
        {

        }
    }
}
