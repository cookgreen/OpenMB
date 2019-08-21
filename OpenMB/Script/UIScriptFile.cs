using OpenMB.Script.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script
{
    public class UIScriptFile : ScriptFile
    {
        public void ExecuteUpdate(object[] executeArgs, float timeSinceLastFrame)
        {
            if (Commands.Count > 0)
            {
                var uiScriptCommand = Commands[0] as UIScriptCommand;
                if (uiScriptCommand.SubCommands.Count == 2)
                {
                    var uiUpdateScriptCommand = uiScriptCommand.SubCommands[1] as UIUpdateScriptCommand;
                    if (uiUpdateScriptCommand == null)
                    {
                        GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
                        return;
                    }
                    uiUpdateScriptCommand.Execute(executeArgs);
                }
                else
                {
                    GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
                }
            }
            else
            {
                GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
            }
        }

        public void ExecuteSetup(object[] executeArgs)
        {
            if (Commands.Count > 0)
            {
                var uiScriptCommand = Commands[0] as UIScriptCommand;
                if (uiScriptCommand.SubCommands.Count == 2)
                {
                    var uiSetupScriptCommand = uiScriptCommand.SubCommands[1] as UISetupScriptCommand;
                    if (uiSetupScriptCommand == null)
                    {
                        GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
                        return;
                    }
                    uiSetupScriptCommand.Execute(executeArgs);
                }
                else
                {
                    GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
                }
            }
            else
            {
                GameManager.Instance.log.LogMessage("Invalid UI Script File", LogMessage.LogType.Error);
            }
        }
    }
}
