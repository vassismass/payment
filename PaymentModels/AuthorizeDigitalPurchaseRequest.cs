using Microsoft.SupplyChain.Care.PaymentModels;
using System;
using System.Runtime.Serialization;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/AuthorizeDigitalPurchaseRequest")]
    public class AuthorizeDigitalPurchaseRequest : SOMCorrelatableMessage,IExtensibleDataObject
    {
        [DataMember]
        public PaymentIdentity paymentIdentityInfo { get; set; }

        [DataMember]
        public LineItem SKUInfo { get; set; }

        [DataMember]
        public Guid PurchasetrackingGuid { get; set; }


        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
