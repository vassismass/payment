namespace Microsoft.SupplyChain.Care.PaymentModels
{ 
    /// <summary>
    /// Carrier details for an inbound or Outbound shipment
    /// </summary>
    public class Carrier
    {
        /// <summary>
        /// Airway bill number
        /// </summary>
        public string AWBNumber { get; set; }
        /// <summary>
        /// Carrier name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tracking url
        /// </summary>
        public string TrackingUrl { get; set; }
    }
}