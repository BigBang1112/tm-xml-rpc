using System.Xml.Serialization;

namespace TmXmlRpc;

[XmlRoot("game")]
public class RequestGameManiaPlanet : RequestGame
{
    [XmlElement("build")]
    public string Build { get; init; }
    [XmlElement("title")]
    public string Title { get; set; }
    [XmlElement("network")]
    public int Network { get; init; }
    [XmlElement("dedicated")]
    public int Dedicated { get; init; }

    public override Uri MasterServerUri { get; } = new("http://relay01.v04.maniaplanet.com/game/request.php");

    public RequestGameManiaPlanet(string title)
    {
        Name = "ManiaPlanet";
        Version = "3.3.0";
        Distro = "KOTOF";
        Build = "2019-11-19_18_50";
        Title = title;
        Network = 395;
        Dedicated = 0;
    }

    public RequestGameManiaPlanet() : this(null)
    {

    }
}
