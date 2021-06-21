using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc.Requests
{
    [XmlRoot("root")]
    public class GetMapLeaderBoardSummaries<T> : Request<T> where T : RequestGame
    {
        public override string Name => "GetMapLeaderBoardSummaries";

        public int B { get; init; }
        public List<Map> Maps { get; init; }

        public GetMapLeaderBoardSummaries(T game, IList<Map> maps) : base(game)
        {
            B = 1;
            Maps = new List<Map>(maps);
        }

        public GetMapLeaderBoardSummaries(T game, string mapUid, string zone = "World", string type = "")
            : this(game, new List<Map> { new Map(mapUid, zone, type) })
        {

        }

        internal GetMapLeaderBoardSummaries() : this(null, new List<Map>())
        {

        }

        public async Task<Response> RequestAsync()
        {
            return await ProtectedRequestAsync<Response>();
        }

        protected override void WriteParametersXml(XmlWriter writer)
        {
            writer.WriteElementString("b", B.ToString());

            var cElementName = default(string);
            var mElementName = default(string);
            var zElementName = default(string);
            var tElementName = default(string);
            var sElementName = default(string);

            for (var i = 1; i <= Maps.Count; i++)
            {
                var map = Maps[i - 1];

                if (i == 1)
                {
                    var type = map.GetType();

                    foreach (var property in type.GetProperties())
                    {
                        switch (property.Name)
                        {
                            case nameof(Map.C):
                                cElementName = property.GetCustomAttribute<XmlElementAttribute>().ElementName;
                                break;
                            case nameof(Map.MapUid):
                                mElementName = property.GetCustomAttribute<XmlElementAttribute>().ElementName;
                                break;
                            case nameof(Map.Zone):
                                zElementName = property.GetCustomAttribute<XmlElementAttribute>().ElementName;
                                break;
                            case nameof(Map.Type):
                                tElementName = property.GetCustomAttribute<XmlElementAttribute>().ElementName;
                                break;
                            case nameof(Map.S):
                                sElementName = property.GetCustomAttribute<XmlElementAttribute>().ElementName;
                                break;
                        }
                    }
                }

                writer.WriteElementString(cElementName + i, map.C);
                writer.WriteElementString(mElementName + i, map.MapUid);
                writer.WriteElementString(zElementName + i, map.Zone);
                writer.WriteElementString(tElementName + i, map.Type);
                writer.WriteElementString(sElementName + i, map.S);
            }
        }

        public class Map
        {
            [XmlElement("c")]
            public string C { get; init; }
            [XmlElement("m")]
            public string MapUid { get; init; }
            [XmlElement("z")]
            public string Zone { get; init; }
            [XmlElement("t")]
            public string Type { get; init; }
            [XmlElement("s")]
            public string S { get; init; }

            public Map(string mapUid, string zone = "World", string type = "")
            {
                MapUid = mapUid;
                Zone = zone;

                C = string.Empty;
                Type = type;
                S = "MapRecord";
            }
        }

        public class Leaderboard : IReadOnlyCollection<LeaderboardPlayer>
        {
            public int R { get; set; }
            public string C { get; set; }
            public string MapUid { get; set; }
            public string Zone { get; set; }
            public string S { get; set; }
            public int D { get; set; }
            public (int time, int count)[] Times { get; set; }
            public int N { get; set; }
            public ReadOnlyCollection<LeaderboardPlayer> Records { get; }
            public int TotalCount { get; set; }

            public int Count => Records.Count;

            public Leaderboard(IList<LeaderboardPlayer> records)
            {
                Records = new ReadOnlyCollection<LeaderboardPlayer>(records);
            }

            public override string ToString()
            {
                return $"Leaderboard [{MapUid}]";
            }

            public IEnumerator<LeaderboardPlayer> GetEnumerator()
            {
                return Records.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Records.GetEnumerator();
            }
        }

        public class LeaderboardPlayer
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

        public class Response : ResponseBase, IReadOnlyCollection<Leaderboard>
        {
            private ReadOnlyCollection<Leaderboard> leaderboards;

            public int Count => leaderboards.Count;

            protected override void ReadContentXml(XmlReader reader)
            {
                var lbs = new List<Leaderboard>();

                while (reader.IsStartElement())
                {
                    reader.ReadStartElement("s");

                    var r = reader.ReadElementContentAsInt("r", "");
                    var c = reader.ReadElementContentAsString("c", "");
                    var mapUid = reader.ReadElementContentAsString("m", "");
                    var zone = reader.ReadElementContentAsString("z", "");
                    var s = reader.ReadElementContentAsString("s", "");
                    var d = reader.ReadElementContentAsInt("d", "");

                    var info = Convert.FromBase64String(reader.ReadElementContentAsString("i", ""));
                    var times = default((int time, int count)[]);

                    using (var ms = new MemoryStream(info)) 
                    using (var rr = new ResponseReader(ms))
                    {
                        var num = rr.ReadInt32();
                        times = new (int time, int count)[num];

                        for (var i = 0; i < num; i++)
                        {
                            var time = rr.ReadInt32();
                            var count = rr.ReadInt32();

                            times[i] = new(time, count);
                        }
                    }

                    var leaderboard = Convert.FromBase64String(reader.ReadElementContentAsString("t", ""));
                    var records = new List<LeaderboardPlayer>();

                    using (var ms = new MemoryStream(leaderboard))
                    using (var rr = new ResponseReader(ms))
                    {
                        var numRecords = rr.ReadInt32();

                        for (var i = 0; i < numRecords; i++)
                        {
                            records.Add(new LeaderboardPlayer
                            {
                                Rank = rr.ReadInt32(),
                                Time = TimeSpan.FromMilliseconds(rr.ReadInt32()),
                                Login = rr.ReadString(),
                                Nickname = rr.ReadString(),
                                FileName = rr.ReadString(),
                                ReplayUrl = rr.ReadString()
                            });
                        }
                    }

                    var n = reader.ReadElementContentAsInt("n", "");

                    lbs.Add(new Leaderboard(records)
                    {
                        R = r,
                        C = c,
                        MapUid = mapUid,
                        Zone = zone,
                        S = s,
                        D = d,
                        Times = times,
                        N = n,
                        TotalCount = times.Sum(x => x.count)
                    });

                    reader.ReadEndElement();
                }

                leaderboards = new ReadOnlyCollection<Leaderboard>(lbs);
            }

            public IEnumerator<Leaderboard> GetEnumerator()
            {
                return leaderboards.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return leaderboards.GetEnumerator();
            }
        }
    }
}
