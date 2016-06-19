using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    [DataContract(Namespace = "http://EDD.Highlander.Commerce.ExtendedService/Profile/2010/04/ComponentWarranty")]
    public class ComponentDetails
    {
        [DataMember]
        public List<Warranty> ComponentWarrantyCollection { get; set; }

        public ComponentDetails()
        {
            this.ComponentWarrantyCollection = new List<Warranty>();
        }

        [DataMember]
        public string ComponentTypeCode { get; set;}

        [DataMember]
        public string ComponentMaterialNumberCount {get; set;}

        [DataMember]
        public string ComponentWarrantyStatus{get; set;}

        [DataMember]
        public string ComponentPartNumber { get; set; }

        [DataMember]
        public string ComponentDesciprtion { get; set; }
        
    }
}
