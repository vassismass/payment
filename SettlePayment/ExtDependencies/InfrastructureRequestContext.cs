using System;
using System.Xml.Serialization;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.ExtDependencies
{
    public class InfrastructureRequestContext
    {
        public Guid SasCorrelationId { get; set; }
        public string StoreName { get; set; }
        public string RequestSessionGuid { get; set; }
        [XmlIgnore]

        private string _brand;
        public string Brand
        {
            get { return _brand; }
            set
            {
                _brand = value;
            }
        }
    }
}
