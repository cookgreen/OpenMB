using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace AMOFGameEngine
{
    /// <summary>
    /// Character State
    /// </summary>
    public enum CharacterState
    {
        CHARA_INACTIVE,//Character is inactive(default value)
        CHARA_ALIVE,//Character is alive
        CHARA_DEAD//Character is dead
    }

    /// <summary>
    /// Character Description
    /// </summary>
    public class CharacterDesc
    {
        string charaName;
        string charaMeshName;
        int charaHealth;
        CharacterState charaState;

        public CharacterDesc()
        {
            charaName = "";
            charaHealth = 0;
            charaState = CharacterState.CHARA_INACTIVE;
        }
        public CharacterDesc(string charaName, string charaMeshName)
        {
            this.charaName = charaName;
            this.charaMeshName = charaMeshName;
            this.charaHealth = 100;
            this.charaState = CharacterState.CHARA_ALIVE;
        }
        public CharacterDesc(string charaName,string charaMeshName,int charaHealth,int charaState)
        {
            this.charaName = charaName;
            this.charaMeshName = charaMeshName;
            this.charaHealth = charaHealth;
            this.charaState = (CharacterState)charaState;
        }
        public string CharaMeshName
        {
            get { return charaMeshName; }
            set { charaMeshName = value; }
        }
        public CharacterState CharaState
        {
            get { return charaState; }
            set { charaState = value; }
        }
        public int CharaHealth
        {
            get { return charaHealth; }
            set { charaHealth = value; }
        }
        public string CharaName
        {
            get { return charaName; }
            set { charaName = value; }
        }
    }
}
