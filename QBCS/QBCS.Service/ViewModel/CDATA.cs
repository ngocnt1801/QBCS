using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QBCS.Service.ViewModel
{
    public class CDATA : IXmlSerializable
    {

        private string text;

        public CDATA()
        { }

        public CDATA(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get { return text; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            this.text = reader.ReadString();
            reader.Read(); // change in .net 2.0,
                           // without this line, you will lose value of all other fields
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteCData(this.text);
        }
        public override string ToString()
        {
            return this.text;
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
