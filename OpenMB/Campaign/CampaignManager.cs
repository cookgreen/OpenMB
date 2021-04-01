using OpenMB.Campaign.XML;
using OpenMB.Core;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Campaign
{
    public class CampaignManager : ISubSystemManager
    {
        private List<ICampaign> campaigns;
        private bool isCampaignsInited;

        private static CampaignManager instance;
        public static CampaignManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CampaignManager();
                }
                return instance;
            }
        }

        public void InitCampaignList(string campaignXmlFile)
        {
            if(!isCampaignsInited)
            {
                campaigns = new List<ICampaign>();

                CampaignListsXml campaignListsXml;

                isCampaignsInited = true;
                XmlObjectLoader loader = new XmlObjectLoader();
                loader.Load<CampaignListsXml>(campaignXmlFile, out campaignListsXml);

                foreach (var campaignListXml in campaignListsXml.Campaigns)
                {
                    ICampaign campaign;
                    if (campaignListXml.type == "Scriptable")
                    {
                        campaign = new ScriptableCampaign(campaignListXml.value);
                        campaign.Name = campaignListXml.id;
                        campaign.Desc = campaignListXml.desc;
                        campaigns.Add(campaign);
                    }
                }
            }
        }

        public ICampaign CurrentCampaign { get; set; }

        public void StartNewCampaign()
        {
            if (CurrentCampaign != null)
            {
                CurrentCampaign.Init();
            }
        }

        public void EndCurrentCampaign()
        {
            if (CurrentCampaign != null)
            {
                CurrentCampaign.End();
            }
        }

        public void Update(float timeSinceLastFrame)
        {
            if (CurrentCampaign != null)
            {
                CurrentCampaign.Update();
            }
        }
    }
}
