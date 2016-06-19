using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.ExtDependencies
{

    public class RequestContext : InfrastructureRequestContext
    {
        public RequestContext()
        {
            FraudDetectionPropertyList = new List<string>();
        }

        public Guid CorrelationId { get; set; }
        public string Locale { get; set; }
        public string CustomerId { get; set; }
        public string AgentTier { get; set; }
        public string Email { get; set; }
        public Channel Channel { get; set; }
        [XmlIgnore]
        public Dictionary<string, string> FraudDetectionProperty { get; set; }
        public List<string> FraudDetectionPropertyList { get; set; }


        private string _brand;
        public string Brand
        {
            get { return _brand; }
            set
            {
                _brand = value;
                StoreName = StoreName + Brand;
            }
        }



        public bool IsCustomer
        {
            get
            {
                return Channel == Channel.Customer;
            }
        }
    }
}
