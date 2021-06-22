using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BigBang1112.TmXmlRpc.Requests;

namespace BigBang1112.TmXmlRpc
{
    public static class MasterServer
    {
        public static HttpClient Client { get; }

        static MasterServer()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("TmXmlRpc / 0.1 by BigBang1112 (used fairly)");
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
    }
}
