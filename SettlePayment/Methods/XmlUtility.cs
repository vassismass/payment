using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SettlePayment
{
    public class XmlUtility
    {
        public XmlUtility()
        { }

        public static T DeserializeFromXml<T>(string file)
        {
            var deserializer = new XmlSerializer(typeof(T));
            using (var textReader = new StreamReader(file))
            {
                return (T)deserializer.Deserialize(textReader);
            }
        }

        public static string SerializeObject<T>(T obj, Type[] extraTypes = null)
        {
            try
            {
                using (StringWriter writer = new StringWriter())
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(writer, obj);
                    
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                return DataContractSerializeObject(obj);
            }
        }

        public static string DataContractSerializeObject<T>(T objectToSerialize)
        {
            try
            {
                using (MemoryStream memStm = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(objectToSerialize.GetType());

                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.OmitXmlDeclaration = true;
                    XmlWriter xmlWriter = XmlWriter.Create(memStm, xmlWriterSettings);

                    serializer.WriteObject(xmlWriter, objectToSerialize);
                    xmlWriter.Close();
                    memStm.Position = 0;

                    using (var streamReader = new StreamReader(memStm))
                    {
                        string result = streamReader.ReadToEnd();
                        //int _xmlDec = result.IndexOf("?xml");
                        //if (_xmlDec == -1)
                        //{
                        //    return "<?xml version='1.0' encoding='utf-16'?>" + result.Replace("&gt;", ">").Replace("&lt;", "<");
                        //}
                        //else
                        //{
                        //    return result.Replace("&gt;", ">").Replace("&lt;", "<");
                        //}
                        return FormatString(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return "DataContractSerializeObject " + " Type : " + typeof(T).ToString().ToLower() + " Name : " + typeof(T).AssemblyQualifiedName + " ErrorMsg : " + ex.Message;
            }
        }

        public class Input
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

      
        public static string FormatString(string serializedXML)
        {
            serializedXML = serializedXML.Replace("&gt;", ">").Replace("&lt;", "<");

            serializedXML = serializedXML.Substring(40, (serializedXML.Length - 40)).Replace("<?xml version='1.0' encoding='utf-16'?>", "");

            return "<?xml version='1.0' encoding='utf-16'?>" + serializedXML;
        }
    }
}
