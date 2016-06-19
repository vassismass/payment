using System;
using System.Runtime.Serialization;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/AuthorizeDigitalPurchaseResponse")]
    public class AuthorizeDigitalPurchaseResponse : IExtensibleDataObject
    {
        [DataMember]
        public Guid TrackingGuid { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string PurchaseId { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
