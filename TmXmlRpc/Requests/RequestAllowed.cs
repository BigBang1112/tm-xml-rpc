using System.Xml;
using System.Xml.Serialization;

namespace TmXmlRpc.Requests;

[XmlRoot("root")]
public class RequestAllowed : Request<RequestGameTmuf>
{
    public override string Name => "RequestAllowed";

    public string RequestName { get; set; }
    public string Uid { get; set; }

    public RequestAllowed(RequestGameTmuf game, string requestName, string uid) : base(game, null, null)
    {
        RequestName = requestName;
        Uid = uid;
    }

    internal RequestAllowed() : this(null, null, null)
    {

    }

    protected override void WriteParametersXml(XmlWriter writer)
    {
        writer.WriteElementString("r", RequestName);
        writer.WriteElementString("uid", Uid);
    }
}
