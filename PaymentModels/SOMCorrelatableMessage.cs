using System;
using System.Runtime.Serialization;


namespace Microsoft.SupplyChain.Care.PaymentModels
{

   [DataContract]
    public abstract class SOMCorrelatableMessage
    {
       [DataMember]
        public Guid? CorrelationId { get; set; }
    }
}
