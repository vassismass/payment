using Microsoft.SupplyChain.Care.PaymentModels;
using System.Runtime.Serialization;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/PurchaseGoodsResponse")]
    public class PurchaseGoodsResponse : IExtensibleDataObject
    {        
        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public string PurchaseId { get; set; }

        [DataMember]
        public decimal LineItemTax { get; set; }

        [DataMember]
        public decimal ShippmentTax { get; set; }

        [DataMember]
        public Order Order { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
