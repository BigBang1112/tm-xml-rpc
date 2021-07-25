using System.Collections.Generic;
using System.Linq;

namespace TmXmlRpc
{
    public class CampaignScoresMap
    {
        public string MapUid { get; set; }
        public Dictionary<string, CampaignScoresMapZone> Zones { get; set; }

        public override string ToString()
        {
            return Zones.FirstOrDefault().Value.ToString();
        }
    }
}