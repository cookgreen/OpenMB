using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Campaign
{
    public interface ICampaign
    {
        string Name { get; set; }

        string Desc { get; set; }

        bool HasNext { get; }

        ICampaign NextCampaign { get; set; }

        void Init();

        void End();

        void Update();

        void GoNext();
    }
}
