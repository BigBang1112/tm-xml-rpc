using TmEssentials;

namespace TmXmlRpc;

public struct CampaignScoresRecord
{
    public int Rank { get; set; }
    public TimeInt32? Time { get; set; }
    public string Login { get; set; }
    public string Nickname { get; set; }

    public override string ToString()
    {
        if (Nickname == null)
            return Time.ToTmString();
        return $"{Time} by {TextFormatter.Deformat(Nickname)}";
    }
}
