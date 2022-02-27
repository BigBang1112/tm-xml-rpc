namespace TmXmlRpc;

public class CampaignScoresMap
{
    public string MapUid { get; set; }
    public Dictionary<string, CampaignScoresMapZone> Zones { get; set; }

    public override string ToString()
    {
        return Zones.FirstOrDefault().Value.ToString();
    }

    public bool Equals(CampaignScoresMap map)
    {
        if (map is null) return false;
        return map.MapUid == MapUid;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as CampaignScoresMap);
    }

    public override int GetHashCode()
    {
        return MapUid?.GetHashCode() ?? base.GetHashCode();
    }
}
