using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc
{
    public abstract class RequestGame
    {
        [XmlElement("name")]
        public string Name { get; init; }
        [XmlElement("version")]
        public string Version { get; init; }
        [XmlElement("distro")]
        public string Distro { get; init; }
        [XmlElement("lang")]
        public string Lang { get; init; }

        public abstract Uri MasterServerUri { get; }

        protected RequestGame()
        {
            Lang = "en";
        }

        public RequestGame(string name, string version, string lang = "en") : this()
        {
            Name = name;
            Version = version;
            Lang = lang;
        }

        public override string ToString()
        {
            return $"{Name} {Version} {Distro}";
        }
    }
}