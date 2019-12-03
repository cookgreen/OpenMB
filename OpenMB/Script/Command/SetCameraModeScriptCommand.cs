using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class SetCameraModeScriptCommand : ScriptCommand
    {
        private string[] commandArgs;
        public override string CommandName
        {
            get
            {
                return "set_camera_mode";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Line;
            }
        }

        public SetCameraModeScriptCommand()
        {
            commandArgs = new string[]
            {
                "cameraMode, 0-Free, 1-Manual"
            };
        }

        public override string[] CommandArgs
        {
            get
            {
                return commandArgs;
            }
        }

        public override void Execute(params object[] executeArgs)
        {
            GameWorld world = executeArgs[0] as GameWorld;
            world.ChangeCameraMode(getParamterValue(commandArgs[0], world));
        }
    }
}
