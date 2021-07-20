using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace TmXmlRpc
{
    public abstract class ResponseBase
    {
        public string Name { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }
        public string Message { get; private set; }
        public int? ByteSize { get; private set; }

        public void FromXml(XmlReader reader)
        {
            var r = reader;

            r.ReadStartElement("r");
            r.ReadStartElement("r");

            r.ReadStartElement("n");
            Name = r.ReadContentAsString();
            r.ReadEndElement();

            try
            {
                r.ReadStartElement("c");
                ReadContentXml(r);
                r.ReadEndElement();
            }
            catch (XmlException)
            {
                try
                {
                    r.ReadStartElement("e");

                    var exNum = r.ReadElementContentAsInt("v", "");

                    try
                    {
                        Message = r.ReadElementContentAsString("m", "");
                    }
                    catch (XmlException)
                    {

                    }

                    r.ReadEndElement();
                }
                catch
                {
                    throw;
                }
            }

            r.ReadEndElement();
            var executionTimeString = r.ReadElementContentAsString();
            executionTimeString = executionTimeString["execution time : ".Length..][0..^2];
            ExecutionTime = TimeSpan.FromSeconds(double.Parse(executionTimeString, CultureInfo.InvariantCulture));
            r.ReadEndElement();
        }

        public void FromXml(Stream stream)
        {
            ByteSize = (int)stream.Length;

            using var r = XmlReader.Create(stream);
            FromXml(r);
        }

        public void FromXml(string xml)
        {
            ByteSize = xml.Length;

            using var sr = new StringReader(xml);
            using var r = XmlReader.Create(sr);
            FromXml(r);
        }

        protected abstract void ReadContentXml(XmlReader reader);
    }
}
