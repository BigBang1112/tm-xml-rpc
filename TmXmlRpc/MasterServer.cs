using System.Net;
using System.Net.Http.Headers;
using TmXmlRpc.Requests;

namespace TmXmlRpc;

public static class MasterServer
{
    public static HttpClient Client { get; }

    static MasterServer()
    {
        var handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        Client = new HttpClient(handler);
        Client.DefaultRequestHeaders.UserAgent.ParseAdd("TmXmlRpc / 0.1 by BigBang1112 (used fairly)");
        Client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = true,
            MustRevalidate = true,
            MaxAge = TimeSpan.Zero
        };
        Client.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
    }
}

public class MasterServer<T> where T : RequestGame
{
    public T Game { get; }

    public MasterServer(T game)
    {
        Game = game;
    }

    public async Task<GetLeagues<T>.Response> GetLeaguesAsync()
    {
        return await new GetLeagues<T>(Game).RequestAsync();
    }

    public async Task<GetMapLeaderBoardSummaries<T>.Response>
        GetMapLeaderBoardSummariesAsync(IList<GetMapLeaderBoardSummaries<T>.Map> maps)
    {
        return await new GetMapLeaderBoardSummaries<T>(Game, maps).RequestAsync();
    }

    public async Task<GetMapLeaderBoardSummaries<T>.Response>
        GetMapLeaderBoardSummariesAsync(string mapUid, string context = "", string zone = "World")
    {
        return await new GetMapLeaderBoardSummaries<T>(Game, mapUid, context, zone).RequestAsync();
    }

    public async Task<GetMapLeaderBoard<T>.Response>
        GetMapLeaderBoardAsync(string mapUid, string context = "", string zone = "World", int offset = 0, int count = 10)
    {
        return await new GetMapLeaderBoard<T>(Game, mapUid, context, zone, offset, count).RequestAsync();
    }
}
