
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed by the Payment entity.
    /// </summary>
    public class BlockPaymentRequest
    {
        /// <summary>
        /// Payment 
        /// </summary>
        public OrderPayment payment { get; set; }
       /// <summary>
       /// ServiceOrder 
       /// </summary>
      public Order order { get; set; }
       /// <summary>
       /// Puid of the customer 
       /// </summary>
       string Puid { get; set; }
       /// <summary>
       /// GuestID of the customer 
       /// </summary>
       string GuestID { get; set; }
       /// <summary>
       /// TrackingGuid of the customer 
       /// </summary>
       string trackingGuid { get; set; }
    }
}