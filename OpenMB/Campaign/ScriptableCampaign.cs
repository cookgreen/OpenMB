using Mogre;
using OpenMB.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Campaign
{
    public class ScriptableCampaign : ICampaign
    {
        private ScriptFile scriptFile;

        public bool HasNext { get { return NextCampaign != null; } }

        public ICampaign NextCampaign { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public ScriptableCampaign(string scriptFileName)
        {
            scriptFile = new ScriptFile(scriptFileName);
            scriptFile.Parse(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
        }

        public void Init()
        {
            scriptFile.FindFunction("campaign_init").Execute();
        }

        public void End()
        {
            scriptFile.FindFunction("campaign_end").Execute();
        }

        public void GoNext()
        {
            if (HasNext)
            {
                End();
                CampaignManager.Instance.CurrentCampaign = NextCampaign;
                CampaignManager.Instance.StartNewCampaign();
            }
        }

        public void Update()
        {
            scriptFile.FindFunction("campaign_update").Execute();
        }
    }
}
