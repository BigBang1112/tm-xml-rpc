using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TmXmlRpc;

[XmlRoot("root")]
public abstract class Request<T> : IXmlSerializable where T : RequestGame
{
    public T Game { get; init; }
    public RequestAuthor Author { get; set; }
    public string Auth { get; set; }

    public abstract string Name { get; }
    public int? Check { get; set; }
    public int? Count { get; set; }

    protected Request(T game, int? check = 0, int? count = 10)
    {
        Game = game;
        Check = check;
        Count = count;

        Author = new RequestAuthor();
    }

    public string ToXML()
    {
        var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var settings = new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true
        };
        using var str = new StringWriter();
        using var writer = XmlWriter.Create(str, settings);
        new XmlSerializer(GetType()).Serialize(writer, this, ns);
        return str.ToString();
    }

    /// <exception cref="HttpRequestException"/>
    protected async Task<TResponse> ProtectedRequestAsync<TResponse>() where TResponse : ResponseBase, new()
    {
        using var stream = new MemoryStream();
        await RequestXmlStreamAsync(stream);
        var response = new TResponse();
        response.FromXml(stream);
        return response;
    }


    public async Task RequestXmlStreamAsync(Stream output)
    {
        var xml = ToXML();

        using var content = new StringContent(xml);
        using var response = await MasterServer.Client.PostAsync(Game.MasterServerUri, content);

        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(output);

        output.Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// Requests the command and gives the XML response as a string.
    /// </summary>
    /// <returns>String containing XML data.</returns>
    /// <exception cref="HttpRequestException"/>
    public async Task<string> RequestXmlAsync()
    {
        using var stream = new MemoryStream();
        await RequestXmlStreamAsync(stream);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    XmlSchema IXmlSerializable.GetSchema()
    {
        return null;
    }

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
        throw new System.NotImplementedException();
    }

    protected virtual void WriteAuthorXml(XmlWriter writer)
    {
        if (Author.Login != null)
            writer.WriteElementString("login", Author.Login);

        if (Author.Session.HasValue)
            writer.WriteElementString("session", Author.Session.ToString());
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        var gameSerializer = new XmlSerializer(typeof(T));
        gameSerializer.Serialize(writer, Game, ns);

        writer.WriteStartElement("author");
        WriteAuthorXml(writer);
        writer.WriteEndElement();

        writer.WriteStartElement("request");
        writer.WriteElementString("name", Name);

        if (Check.HasValue)
            writer.WriteElementString("check", Check.ToString());

        if (Count.HasValue)
            writer.WriteElementString("count", Count.ToString());

        writer.WriteStartElement("params");

        WriteParametersXml(writer);

        writer.WriteEndElement();

        writer.WriteEndElement();

        if (!string.IsNullOrEmpty(Auth))
        {
            writer.WriteStartElement("auth");
            writer.WriteElementString("value", Auth);
            writer.WriteEndElement();
        }
    }

    protected abstract void WriteParametersXml(XmlWriter writer);
}
