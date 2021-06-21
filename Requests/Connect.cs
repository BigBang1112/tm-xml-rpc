using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc.Requests
{
    [XmlRoot("root")]
    public class Connect : Request<RequestGameMP>
    {
        public override string Name => "Connect";

        public Connect(RequestGameMP game, string login, int sessionId) : base(game, 0, 4)
        {
            Author.Login = login;
            Author.Session = sessionId;
        }

        internal Connect() : this(null, null, 0)
        {

        }

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
