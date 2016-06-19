using Microsoft.SupplyChain.Care.PaymentModels;
using System;
using System.Runtime.Serialization;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/SettlePurchaseGoodsRequest")]
    public class SettlePurchaseGoodsRequest : SOMCorrelatableMessage, IExtensibleDataObject
    {
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Identity { get; set; }       

        [DataMember]
        public PaymentIdentity paymentIdentityInfo { get; set; }

        [DataMember]
        public LineItem SkuInfo { get; set; }

        [DataMember]
        public string PurchaseID { get; set; }

        [DataMember]
        public string AccountID { get; set; }

        [DataMember]
        public Guid TrackingGuid { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
