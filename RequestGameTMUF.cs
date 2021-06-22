using System;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc
{
    [XmlRoot("game")]
    public class RequestGameTmuf : RequestGame
    {
        public override Uri MasterServerUri => new("http://game.trackmaniaforever.com/online_game/request.php");

        public RequestGameTmuf()
        {
            Name = "TmForever";
            Version = "2.11.25";
            Distro = "TAHOR";
            Lang = "en";
        }
    }
}
