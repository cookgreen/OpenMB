using OpenMB.Game;
using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Script.Command
{
    public class SpawnScenePropScriptCommand : ScriptCommand
    {
        private string[] commandArgs;

        public override string CommandName
        {
            get
            {
                return "spawn_scene_prop";
            }
        }

        public override ScriptCommandType CommandType
        {
            get
            {
                return ScriptCommandType.Line;
            }
        }

        public override string[] CommandArgs
        {
            get
            {
                return commandArgs;
            }
        }

        public SpawnScenePropScriptCommand()
        {
            commandArgs = new string[]
            {
                "meshName",
                "Position Vector",
            };
        }

        public override void Execute(params object[] executeArgs)
        {
            string characterType = CommandArgs[0].StartsWith("%") ? Context.GetLocalValue(CommandArgs[0].Substring(1)) : CommandArgs[0];
            string meshName = CommandArgs[3].StartsWith("%") ? Context.GetLocalValue(CommandArgs[3].Substring(1)) : CommandArgs[3];
            string vectorName = CommandArgs[3].StartsWith("%") ? Context.GetLocalValue(CommandArgs[3].Substring(1)) : CommandArgs[3];
            GameWorld world = executeArgs[0] as GameWorld;
            var vector = world.GlobalValueTable.GetRecord(vectorName);

            world.CreateSceneProp(
                meshName,
                new Vector3(
                    float.Parse(vector.NextNodes[0].Value),
                    float.Parse(vector.NextNodes[1].Value),
                    float.Parse(vector.NextNodes[2].Value)));
        }
    }
}
