using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
   // [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService.ContractLibrary.DataContract.Payment/2012/06/GetBillingAccountRequest")]
#region Existing
    //public class GetBillingAccountRequest : IExtensibleDataObject
    //{
    //    [DataMember]
    //    public string Puid { get; set; }

    //    public virtual ExtensionDataObject ExtensionData { get; set; }
    //}
#endregion
    public class GetBillingAccountRequest : SOMCorrelatableMessage, IExtensibleDataObject
    {
        /// <summary>
        /// This is the passport unique identifier (Puid)
        /// Note: Currently this is - Puid - renamed property to Identity
        /// </summary>
        [DataMember]
        public string Identity { get; set; }

        [DataMember]
        public string Country { get; set; }


        public virtual ExtensionDataObject ExtensionData { get; set; }
    }
}
