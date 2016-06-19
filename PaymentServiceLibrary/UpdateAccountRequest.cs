using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using System;


namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    internal class UpdateAccountRequest
    {
        public string AccountId { get; set; }
        public object AccountInfoInput { get; set; }
        public Identity Requester { get; set; }
        public Guid TrackingGuid { get; set; }
    }
}