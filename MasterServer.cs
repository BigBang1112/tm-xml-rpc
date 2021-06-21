using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class MasterServer
    {
        public static HttpClient Client { get; }

        static MasterServer()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("TmXmlRpc / 0.1 by BigBang1112 (used fairly)");
        }
    }
}
