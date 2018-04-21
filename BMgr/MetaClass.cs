using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BMgr
{
    public class MetaClass
    {
        public string name { get; set; }
        public string author { get; set; }
        public string tagstring { get; set; }
        public Int64 page { get; set; }
        public string path { get; set; }
        public string cover { get; set; }

        public static string XmlSerialize(object obj)
        {
            StringBuilder buffer = new StringBuilder();
            using (TextWriter writer = new StringWriter(buffer))
            {
                XmlSerializer xmlSrlzer = new XmlSerializer(obj.GetType());
                xmlSrlzer.Serialize(writer, obj);
            }
            return buffer.ToString();
        }
        public static TClass XmlDeserialize<TClass>(string xml)
        {
            TClass obj;
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer xmlSrlzer = new XmlSerializer(typeof(TClass));
                obj = (TClass)xmlSrlzer.Deserialize(reader);
            }
            return obj;
        }

    }
}
