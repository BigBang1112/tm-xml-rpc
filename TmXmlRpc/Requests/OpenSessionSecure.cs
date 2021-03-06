using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TmXmlRpc.Requests;

[XmlRoot("root")]
public class OpenSessionSecure : Request<RequestGameManiaPlanet>
{
    public override string Name => "OpenSessionSecure";

    public string Password { get; set; }

    public OpenSessionSecure(RequestGameManiaPlanet game, string login, string password) : base(game, 0, 4)
    {
        Author.Login = login;
        Password = password;
    }

    internal OpenSessionSecure() : this(null, null, null)
    {

    }

    /// <exception cref="HttpRequestException"/>
    public async Task<Response> RequestAsync()
    {
        return await ProtectedRequestAsync<Response>();
    }

    protected override void WriteParametersXml(XmlWriter writer)
    {
        writer.WriteElementString("cr", "");
        writer.WriteElementString("p", Password);
    }

    public class Response : ResponseBase
    {
        public int SessionId { get; set; }
        public string S { get; set; }
        public string K { get; set; }

        protected override void ReadContentXml(XmlReader reader)
        {
            SessionId = reader.ReadElementContentAsInt("i", "");
            S = reader.ReadElementContentAsString("s", "");

            for (var i = 0; i < 3; i++)
            {
                reader.ReadStartElement("p");
                reader.ReadElementContentAsInt("i", "");
                reader.ReadElementContentAsInt("t", "");
                reader.ReadElementContentAsString("k", "");
                reader.ReadEndElement();
            }

            K = reader.ReadElementContentAsString("k", "");
        }
    }
}
