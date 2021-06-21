using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace BigBang1112.TmXmlRpc
{
    public class CampaignScoresMap
    {
        [YamlIgnore]
        public string MapUid { get; set; }
        public Dictionary<string, CampaignScoresMapZone> Zones { get; set; }

        public override string ToString()
        {
            return Zones.FirstOrDefault().Value.ToString();
        }
    }
}