using System;
using System.Xml.Serialization;

namespace BigBang1112.TmXmlRpc
{
    [XmlRoot("game")]
    public class RequestGameTMTurbo : RequestGameMP
    {
        [XmlElement("beta")]
        public int Beta { get; init; }

        public MasterServerTmTurbo MasterServerType { get; set; }
        public int MasterServerNum { get; set; }

        public override Uri MasterServerUri => new($"http://mp{MasterServerNum:000}-{MasterServerType.ToString().ToLower()}.turbo.trackmania.com/game/request.php");

        /// <summary>
        /// Constructs a new TMTurbo game specification for the request.
        /// </summary>
        /// <param name="type">Type of the platform.</param>
        /// <param name="num">PC has servers num 1-3, PS4 and XB1 1-8.</param>
        public RequestGameTMTurbo(MasterServerTmTurbo type = MasterServerTmTurbo.PC, int num = 3) : base("TMTurbo@nadeolabs")
        {
            Distro = "UPLAY";
            Build = "2016-11-07_16_15";
            Network = 394;

            MasterServerType = type;
            MasterServerNum = num;
        }

        public RequestGameTMTurbo() : this(MasterServerTmTurbo.PC, 3)
        {

        }
    }
}
