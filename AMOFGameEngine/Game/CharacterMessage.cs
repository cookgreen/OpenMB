using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.Game
{
    public enum MessageLevel
    {
        low,
        medium,
        heigh,
        veryhigh,
    }

    public enum MessageType
    {
        enemy_spotted,
        need_backup,
    }
    public class CharacterMessage
    {
        private MessageLevel level;
        private MessageType type;

        public MessageLevel Level
        {
            get
            {
                return level;
            }
        }

        public MessageType Type
        {
            get
            {
                return type;
            }
        }

        public CharacterMessage(MessageLevel level, MessageType type)
        {
            this.level = level;
            this.type = type;
        }
    }
}
