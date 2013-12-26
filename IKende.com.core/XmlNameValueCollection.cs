using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IKende.com.core
{
    [Serializable]
    public class XMLNameValueCollection : System.Collections.Specialized.NameValueCollection, System.Xml.Serialization.IXmlSerializable
    {
        #region IXmlSerializable 成员

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = Utils.GetXmlSerializer(typeof(string), null);
            XmlSerializer valueSerializer = Utils.GetXmlSerializer(typeof(string), null);
            string KeyValue;
            string value;


            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                KeyValue = (string)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                value = (string)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(KeyValue, value);
                reader.ReadEndElement();
                reader.MoveToContent();

            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            string value;
            XmlSerializer keySerializer = Utils.GetXmlSerializer(typeof(string), null);
            XmlSerializer valueSerializer = Utils.GetXmlSerializer(typeof(string), null);
            foreach (string key in Keys)
            {
                value = this[key];
                if (value != null)
                {
                    writer.WriteStartElement("item");

                    writer.WriteStartElement("key");
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();
                    writer.WriteStartElement("value");
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }
}
