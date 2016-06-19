using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [Serializable]
    public class Brands
    {
        [XmlElement("Brand")]
        public List<Brand> BrandCollection { get; set; }
    }
    [Serializable]
    public class Brand
    {
        [XmlElement("ShipToCountry")]
        public List<ShipToCountry> ShipToCountryCollection { get; set; }

        [XmlAttribute]
        public string name { get; set; }

    }
    [Serializable]
    public class ShipToCountry
    {

        public String Country { get; set; }

        public String CountryCode { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String PostalCode { get; set; }

        public String Address1 { get; set; }

        public String Address2 { get; set; }

        public bool Default { get; set; }
        [XmlAttribute]
        public string name { get; set; }
    }

    //[Serializable]
    //public class ShipFromCollection : Collection<ShipFrom> { }


    public class ShipFromManager
    {
        private static string shipFromPath;
        //readonly static PaymentProvider paymentProvider = new CPPaymentProvider(); commented by me
        private static List<Brand> shipFromCollection = new List<Brand>();

        public static string ShipFromPath
        {
            get
            {
                return shipFromPath;
            }

            set
            {
                if (string.IsNullOrEmpty(shipFromPath))
                {
                    shipFromPath = value;
                }
            }
        }

        public static List<Brand> GetShipFromAddressList()
        {
            Brands maps = DeserializeFromXML();

            foreach (Brand r in maps.BrandCollection)
            {
                shipFromCollection.Add(r);
            }

            return shipFromCollection;
        }
        static Brands DeserializeFromXML()
        {
            var path = "C:\\ShipFromAddress.xml";//ShipFromPath;

            //if (!System.IO.File.Exists(path))
            //    path = String.Format("{0}{1}", System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, path);
            XmlSerializer deserializer = new XmlSerializer(typeof(Brands));
            using (var reader = new StreamReader(path))
            {
                return (Brands)deserializer.Deserialize(reader);
            }
        }
        //public static Brands DeserializeFromXML(Type type)
        //{
        //    var path = ShipFromPath;

        //    if (!System.IO.File.Exists(path))
        //        path = String.Format("{0}{1}", System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, path);

        //    var deserializer = new XmlSerializer(type);
        //    using (TextReader textReader = new StreamReader(path))
        //    {
        //        return deserializer.Deserialize(textReader);
        //    }
        //}
    }
}
