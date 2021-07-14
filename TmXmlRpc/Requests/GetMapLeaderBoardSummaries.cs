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

namespace TmXmlRpc.Requests
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

        public GetMapLeaderBoardSummaries(T game, string mapUid, string context = "", string zone = "World")
            : this(game, new List<Map> { new Map(mapUid, context, zone) })
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
                            case nameof(Map.Context):
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
                writer.WriteElementString(tElementName + i, map.Context);
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
            public string Context { get; init; }
            [XmlElement("s")]
            public string S { get; init; }

            public Map(string mapUid, string context = "", string zone = "World")
            {
                MapUid = mapUid;
                Context = context;
                Zone = zone;

                C = string.Empty;
                S = "MapRecord";
            }
        }        

        public class Response : ResponseBase, IReadOnlyCollection<MapLeaderBoard>
        {
            public ReadOnlyCollection<MapLeaderBoard> Leaderboards { get; private set; }

            public int Count => Leaderboards.Count;

            public Response()
            {
                Leaderboards = new ReadOnlyCollection<MapLeaderBoard>(new List<MapLeaderBoard>());
            }

            protected override void ReadContentXml(XmlReader reader)
            {
                var lbs = new List<MapLeaderBoard>();

                while (reader.IsStartElement())
                {
                    reader.ReadStartElement("s");

                    var r = reader.ReadElementContentAsInt("r", "");
                    var context = reader.ReadElementContentAsString("c", "");
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
                    var records = new List<MapLeaderBoardPlayer>();

                    using (var ms = new MemoryStream(leaderboard))
                    using (var rr = new ResponseReader(ms))
                    {
                        var numRecords = rr.ReadInt32();

                        for (var i = 0; i < numRecords; i++)
                        {
                            records.Add(new MapLeaderBoardPlayer
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

                    lbs.Add(new MapLeaderBoard(records)
                    {
                        R = r,
                        Context = context,
                        Zone = zone,
                        MapUid = mapUid,
                        S = s,
                        D = d,
                        Times = times,
                        TotalCount = n
                    });

                    reader.ReadEndElement();
                }

                Leaderboards = new ReadOnlyCollection<MapLeaderBoard>(lbs);
            }

            public IEnumerator<MapLeaderBoard> GetEnumerator()
            {
                return Leaderboards.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Leaderboards.GetEnumerator();
            }
        }
    }
}
