using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc.Requests
{
    [XmlRoot("root")]
    public class GetCampaignScores : Request<RequestGameTMUF>
    {
        public override string Name => "GetCampaignScores";

        public string Campaign { get; set; }
        public string Zone { get; set; }
        public DateTimeOffset S { get; set; }
        public int T { get; set; }

        public GetCampaignScores(RequestGameTMUF game, string campaign, string zone) : base(game, null, null)
        {
            Campaign = campaign;
            Zone = zone;
            S = DateTimeOffset.UtcNow.Date + TimeSpan.FromSeconds(2);
            T = 3;
        }

        internal GetCampaignScores() : this(null, null, null)
        {

        }

        public async Task<Response> RequestAsync()
        {
            return await ProtectedRequestAsync<Response>();
        }

        protected override void WriteParametersXml(XmlWriter writer)
        {
            var dateTime = S.DateTime + S.Offset;

            writer.WriteElementString("n", Campaign);
            writer.WriteElementString("f0", Zone);
            writer.WriteElementString("s", dateTime.ToString("yyyy:MM:dd:HH:mm:ss"));
            writer.WriteElementString("t", T.ToString());
        }

        public class Response : ResponseBase
        {
            public string Campaign { get; private set; }
            public string Zone { get; private set; }
            public DateTime? Timestamp { get; private set; }
            public int? T { get; private set; }
            public string FileName { get; private set; }
            public string DownloadUrl { get; private set; }

            protected override void ReadContentXml(XmlReader reader)
            {
                reader.ReadStartElement("a");
                Campaign = reader.ReadContentAsString();
                reader.ReadEndElement();

                try
                {
                    reader.ReadStartElement("d");

                    Zone = reader.ReadElementContentAsString("f", "");
                    Timestamp = DateTime.ParseExact(reader.ReadElementContentAsString("u", ""), "yyyyMMddHHmmss", null);
                    T = reader.ReadElementContentAsInt("t", "");

                    reader.ReadEndElement();

                    reader.ReadStartElement("s");

                    FileName = reader.ReadElementContentAsString("f", "");
                    DownloadUrl = reader.ReadElementContentAsString("u", "");

                    reader.ReadEndElement();
                }
                catch (XmlException)
                {

                }
            }

            public async Task<CampaignScores> GetScoresAsync()
            {
                if (DownloadUrl == null) return null;

                var response = await MasterServer.Client.GetAsync(DownloadUrl);

                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsByteArrayAsync();

                return CampaignScores.Parse(data);
            }

            public async Task<HttpContentHeaders> GetScoresHeadersAsync()
            {
                if (DownloadUrl == null) return null;

                var request = new HttpRequestMessage(HttpMethod.Head, DownloadUrl);
                var response = await MasterServer.Client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                return response.Content.Headers;
            }
        }
    }
}
