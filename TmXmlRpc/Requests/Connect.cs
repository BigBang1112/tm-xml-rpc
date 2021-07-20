using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Http;

namespace TmXmlRpc.Requests
{
    [XmlRoot("root")]
    public class Connect : Request<RequestGameManiaPlanet>
    {
        public override string Name => "Connect";

        public Connect(RequestGameManiaPlanet game, string login, int sessionId) : base(game, 0, 4)
        {
            Author.Login = login;
            Author.Session = sessionId;
        }

        internal Connect() : this(null, null, 0)
        {

        }

        /// <exception cref="HttpRequestException"/>
        public async Task<Response> RequestAsync()
        {
            return await ProtectedRequestAsync<Response>();
        }

        protected override void WriteParametersXml(XmlWriter writer)
        {
            
        }

        public class Response : ResponseBase
        {
            public int SessionId { get; set; }
            public string S { get; set; }
            public string K { get; set; }

            protected override void ReadContentXml(XmlReader reader)
            {
                
            }
        }
    }
}
