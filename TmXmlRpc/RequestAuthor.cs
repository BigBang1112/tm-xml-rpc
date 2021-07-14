using System.Xml.Serialization;

namespace TmXmlRpc
{
    [XmlRoot("author")]
    public class RequestAuthor
    {
        [XmlElement("login")]
        public string Login { get; set; }
        [XmlElement("session")]
        public int? Session { get; set; }

        public bool ShouldSerializeSession()
        {
            return Session.HasValue;
        }
    }
}
