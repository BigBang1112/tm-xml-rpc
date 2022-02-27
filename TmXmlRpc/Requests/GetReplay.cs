using System.Xml;
using System.Xml.Serialization;

namespace TmXmlRpc.Requests;

[XmlRoot("root")]
public class GetReplay : Request<RequestGameTmuf>
{
    public override string Name => "GetReplay";

    public string MapUid { get; set; }
    public string Login { get; set; }

    public GetReplay(RequestGameTmuf game, string mapUid, string login) : base(game, null, null)
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
