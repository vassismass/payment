using System;
using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{  
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Profile/2010/04/Warranty")]
    public class Warranty : ProfileBase
    {
        [DataMember]
        public DateTime? PurchaseDate { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string WarrantyType { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ExtendedServicePlanId { get; set; }

        [DataMember]
        public string ServiceRequestNumber { get; set; }

        [DataMember]
        public int ActivityCount { get; set; }

        [DataMember]
        public string PricePaid { get; set; }
    }
}
