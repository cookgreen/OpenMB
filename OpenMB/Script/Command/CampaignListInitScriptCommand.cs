using Mogre;
using OpenMB.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Command
{
    public class CampaignListInitScriptCommand : ScriptCommand
    {
		private string[] commandArgs;
		public override string CommandName
		{
			get
			{
				return "campaign_list_init";
			}
		}

		public override string[] CommandArgs
		{
			get
			{
				return commandArgs;
			}
		}

		public override ScriptCommandType CommandType
		{
			get
			{
				return ScriptCommandType.Line;
			}
		}

		public CampaignListInitScriptCommand()
        {
			commandArgs = new string[]
			{
				"CampaignXmlFileName"
			};
        }

        public override void Execute(params object[] executeArgs)
        {
			string campaignFileName = getVariableValue(commandArgs[0]).ToString();
			DataStreamPtr dataStream = ResourceGroupManager.Singleton.OpenResource(campaignFileName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
			string dataString = dataStream.AsString;
			CampaignManager.Instance.InitCampaignList(dataString);
		}
    }
}
