using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMOFGameEngine.RPG.Data
{
    /// <summary>
    /// Dialogue Infomation
    /// </summary>
    public abstract class Dialogue
    {
        /// <summary>
        /// Who will start this dialogue
        /// </summary>
        /// <returns></returns>
        private DialogueStartType dialogueStartType;

        public DialogueStartType DialogueStartType
        {
            get { return dialogueStartType; }
            set { dialogueStartType = value; }
        }
        /// <summary>
        /// Last Dialogue, give null means it is a new dialogue
        /// </summary>
        /// <returns></returns>
        private string lastDialogueStatement;

        public string LastDialogueStatement
        {
            get { return lastDialogueStatement; }
            set { lastDialogueStatement = value; }
        }
        /// <summary>
        /// Current character Speak content
        /// </summary>
        /// <returns></returns>
        private string dialogueContent;

        public string DialogueContent
        {
            get { return dialogueContent; }
            set { dialogueContent = value; }
        }
        /// <summary>
        /// Avaliable Replies to reply this dialogue
        /// </summary>
        /// <returns></returns>
        private List<string> replies;

        public List<string> Replies
        {
            get { return replies; }
            set { replies = value; }
        }
    }
}
