using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using Microsoft.SupplyChain.Care.PaymentModels;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/PurchaseGoodsRequest")]
    public class PurchaseGoodsRequest : SOMCorrelatableMessage, IExtensibleDataObject
    {
        [DataMember]
        public string PurchaseType { get; set; }

        [DataMember]
        public Guid TrackingGuid { get; set; }

        //[DataMember]
        //public string Identity { get; set; }

        [DataMember]
        public PaymentIdentity paymentIdentityInfo { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Locale { get; set; }

        [DataMember]
        public AccountInfo Account { get; set; }

        [DataMember]
        public string SerialNumber { get; set; }

        //[DataMember]
        //public string PaymentInstrumentId { get; set; }

        [DataMember]
        public LineItem SKUInfo { get; set; }

        
       [DataMember]
        public Dictionary<string, string> FraudDetectionProperty { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
