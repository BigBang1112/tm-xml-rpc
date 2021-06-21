using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class MasterServerTMUF : MasterServer
    {
        public Uri GetCampaignScoresUrl(string campaign, int zoneId)
        {
            return new Uri($"https://scores.trackmaniaforever.com/scores2/{campaign}/{zoneId}{zoneId}.gz");
        }
    }
}
