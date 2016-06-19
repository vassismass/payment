//=====================================================================================================
//   Class          : Fulfilment Details            
//   Developed By   : v-hasure
//   Purpose        : Fulfilment Details
//   Date           : 12/24/2010            
//=====================================================================================================

using System.Runtime.Serialization;


namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Fulfilment Details
    /// </summary>
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Profile/2010/04/FulfilmentDetails")]
    public class FulfilmentDetails : ProfileBase
    {

        [DataMember]
        public string ComponentTypeCode { get; set; }

        [DataMember]
        public int FulfilmentCountNumber { get; set; }

    }
}
