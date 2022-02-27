using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace TmXmlRpc.Requests;

[XmlRoot("root")]
public class GetLeagues<T> : Request<T> where T : RequestGame
{
    public override string Name => "GetLeagues";

    public GetLeagues(T game) : base(game, null, null)
    {
        
    }

    protected GetLeagues() : this(null)
    {

    }

    /// <exception cref="HttpRequestException"/>
    public async Task<Response> RequestAsync()
    {
        return await ProtectedRequestAsync<Response>();
    }

    protected override void WriteParametersXml(XmlWriter writer)
    {
        // No params
    }

    public class League
    {
        public string Zone { get; set; }
        public string Image { get; set; }

        public override string ToString()
        {
            return Zone;
        }
    }

    public class Response : ResponseBase, IReadOnlyCollection<League>
    {
        private ReadOnlyCollection<League> leagues;

        public ReadOnlyCollection<League> Leagues => leagues;
        public int Count => leagues.Count;

        public Response()
        {
            leagues = new ReadOnlyCollection<League>(new List<League>());
        }

        protected override void ReadContentXml(XmlReader reader)
        {
            var ls = new List<League>();

            while (reader.IsStartElement())
            {
                reader.ReadStartElement("l");

                var a = reader.ReadElementContentAsString("a", "");
                var b = reader.ReadElementContentAsString("b", "");
                var image = reader.ReadElementContentAsString("i", "");

                reader.ReadEndElement();

                var zone = default(string);

                if (string.IsNullOrEmpty(b))
                    zone = a;
                else
                    zone = b + "|" + a;

                ls.Add(new League
                {
                    Zone = zone,
                    Image = image
                });
            }

            ls = ls.OrderBy(x => x.Zone).ToList();

            leagues = new ReadOnlyCollection<League>(ls);
        }

        public IEnumerator<League> GetEnumerator()
        {
            return leagues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return leagues.GetEnumerator();
        }
    }
}
