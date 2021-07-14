using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmXmlRpc
{
    public class MasterServerTmuf : MasterServer<RequestGameTmuf>
    {
        public MasterServerTmuf() : base(new RequestGameTmuf())
        {

        }

        public Uri GetCampaignScoresUrl(string campaign, int zoneId)
        {
            return new Uri($"https://scores.trackmaniaforever.com/scores4/{campaign}/{zoneId}{zoneId}.gz");
        }
    }
}
