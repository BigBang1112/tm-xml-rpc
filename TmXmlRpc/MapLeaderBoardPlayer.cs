using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TmEssentials;

namespace TmXmlRpc;

public class MapLeaderBoardPlayer
{
    public int Rank { get; set; }
    public TimeInt32 Time { get; set; }
    public string Login { get; set; }
    public string Nickname { get; set; }
    public string FileName { get; set; }
    public string ReplayUrl { get; set; }

    public override string ToString()
    {
        return $"{Rank}) {Time} by {Nickname}";
    }

    public async Task<DateTimeOffset?> GetTimestampAsync()
    {
        var response = await MasterServer.Client.HeadAsync(ReplayUrl);
        response.EnsureSuccessStatusCode();
        return response.Content.Headers.LastModified;
    }
}
