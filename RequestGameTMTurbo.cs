using System;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc
{
    [XmlRoot("game")]
    public class RequestGameTmTurbo : RequestGameManiaPlanet
    {
        [XmlElement("beta")]
        public int Beta { get; init; }

        public MasterServerPlatform MasterServerPlatform { get; set; }
        public int MasterServerNum { get; set; }

        public override Uri MasterServerUri => new($"http://mp{MasterServerNum:000}-{MasterServerPlatform.ToString().ToLower()}.turbo.trackmania.com/game/request.php");

        /// <summary>
        /// Constructs a new TMTurbo game specification for the request.
        /// </summary>
        /// <param name="type">Type of the platform.</param>
        /// <param name="num">PC has servers num 1-3, PS4 and XB1 1-8.</param>
        public RequestGameTmTurbo(MasterServerPlatform platform = MasterServerPlatform.PC, int num = 3) : base("TMTurbo@nadeolabs")
        {
            Distro = "UPLAY";
            Build = "2016-11-07_16_15";
            Network = 394;

            MasterServerPlatform = platform;
            MasterServerNum = num;
        }

        public RequestGameTmTurbo() : this(MasterServerPlatform.PC, 3)
        {

        }
    }
}
