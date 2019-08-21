namespace OpenMB.Script.Command
{
    public class UISetupScriptCommand : ScriptCommand
    {
        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Block;
            }
        }

        public override string CommandName
        {
            get
            {
                return "ui_setup";
            }
        }

        public override string[] CommandArgs
        {
            get
            {
                return new string[] { };
            }
        }


        public override void Execute(params object[] executeArgs)
        {
            if (!ParentCommand.GetType().Equals(typeof(UIScriptCommand)))
            {
                GameManager.Instance.log.LogMessage("Invalid UI Setup Command", LogMessage.LogType.Error);
                return;
            }

            for (int i = 0; i < SubCommands.Count; i++)
            {
                SubCommands[i].Execute(executeArgs);
            }
        }
    }
}