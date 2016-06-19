using System.Runtime.Serialization;

namespace SettlePayment
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/SettlePurchaseGoodsResponse")]
    public class SettlePurchaseGoodsResponse : IExtensibleDataObject
    {

        [DataMember]
        public string Status { get; set; }

        public virtual ExtensionDataObject ExtensionData { get; set; }
        
    }
}
