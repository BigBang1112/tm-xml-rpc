using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BigBang1112.TmXmlRpc.Requests;

namespace BigBang1112.TmXmlRpc
{
    public class MasterServer<T> where T : RequestGame
    {
        public static HttpClient Client { get; }

        public T Game { get; }

        static MasterServer()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("TmXmlRpc / 0.1 by BigBang1112 (used fairly)");
        }

        public MasterServer(T game)
        {
            Game = game;
        }

        public async Task<GetLeagues<T>.Response> GetLeagues()
        {
            return await new GetLeagues<T>().RequestAsync();
        }
    }
}
