namespace TmXmlRpc;

public class MasterServerTmuf : MasterServer<RequestGameTmuf>
{
    public MasterServerTmuf() : base(new RequestGameTmuf())
    {

    }

    /// <summary>
    /// Gets the URL where the leaderboard data is available.
    /// </summary>
    /// <param name="set">The set number. Can be 1 to 7.</param>
    /// <param name="campaign">Official campaign name.</param>
    /// <param name="zoneId">ID of the zone</param>
    /// <returns>The URI of the leaderboard data download.</returns>
    public static Uri GetCampaignScoresUrl(int set, string campaign, int zoneId)
    {
        return new Uri($"https://scores.trackmaniaforever.com/scores{set}/{campaign}/{campaign}{zoneId}.gz");
    }
}
