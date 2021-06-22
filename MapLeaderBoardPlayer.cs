using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BigBang1112.TmXmlRpc
{
    public class MapLeaderBoardPlayer
    {
        public int Rank { get; set; }
        public TimeSpan Time { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }
        public string FileName { get; set; }
        public string ReplayUrl { get; set; }

        public override string ToString()
        {
            return $"{Rank}) {Time.ToStringTM()} by {Nickname}";
        }

        public async Task<DateTimeOffset?> GetTimestampAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Head, ReplayUrl);
            var response = await MasterServer.Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response.Content.Headers.LastModified;
        }
    }
}
