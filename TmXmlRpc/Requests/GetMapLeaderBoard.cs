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
    public class GetMapLeaderBoard<T> : Request<T> where T : RequestGame
    {
        public override string Name => "GetMapLeaderBoard";

        [XmlElement("f")]
        public int Offset { get; init; }

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

        public GetMapLeaderBoard(T game, string mapUid, string context = "", string zone = "World",
            int offset = 0, int count = 10) : base(game)
        {
            MapUid = mapUid;
            Zone = zone;
            Context = context;

            C = string.Empty;
            S = "MapRecord";

            Offset = offset;
            Count = count;
        }

        protected GetMapLeaderBoard() : this(null, null)
        {

        }

        public async Task<Response> RequestAsync()
        {
            return await ProtectedRequestAsync<Response>();
        }

        protected override void WriteParametersXml(XmlWriter writer)
        {
            writer.WriteElementString("f", Offset.ToString());
            writer.WriteElementString("n", Count.ToString());

            writer.WriteElementString("c", C);
            writer.WriteElementString("m", MapUid);
            writer.WriteElementString("z", Zone);
            writer.WriteElementString("t", Context);
            writer.WriteElementString("s", S);
        }      

        public class Response : ResponseBase
        {
            public MapLeaderBoard Leaderboard { get; private set; }

            public Response()
            {
                Leaderboard = new MapLeaderBoard();
            }

            protected override void ReadContentXml(XmlReader reader)
            {
                var records = new List<MapLeaderBoardPlayer>();

                while (reader.IsStartElement())
                {
                    reader.ReadStartElement("i");

                    var rank = reader.ReadElementContentAsInt("r", "");
                    var login = reader.ReadElementContentAsString("l", "");
                    var nickname = reader.ReadElementContentAsString("n", "");
                    var time = reader.ReadElementContentAsInt("s", "");
                    var fileName = reader.ReadElementContentAsString("f", "");
                    var replayUrl = reader.ReadElementContentAsString("u", "");

                    reader.ReadEndElement();

                    records.Add(new MapLeaderBoardPlayer
                    {
                        Rank = rank,
                        Time = TimeSpan.FromMilliseconds(time),
                        Login = login,
                        Nickname = nickname,
                        FileName = fileName,
                        ReplayUrl = replayUrl
                    });
                }

                Leaderboard = new MapLeaderBoard(records);
            }
        }
    }
}
