using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc.Requests
{
    [XmlRoot("root")]
    public class GetReplay : Request<RequestGameTMUF>
    {
        public override string Name => "GetReplay";

        public string MapUid { get; set; }
        public string Login { get; set; }

        public GetReplay(RequestGameTMUF game, string mapUid, string login) : base(game, null, null)
        {
            MapUid = mapUid;
            Login = login;
        }

        public GetReplay() : this(null, null, null)
        {

        }

        protected override void WriteParametersXml(XmlWriter writer)
        {
            writer.WriteElementString("uid", MapUid);
            writer.WriteElementString("l", Login);
        }
    }
}
