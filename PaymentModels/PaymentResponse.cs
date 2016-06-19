

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class PaymentResponse
    {
        /// <summary>
        /// Gets or sets the Acknowledgement returned from CP response
        /// </summary>
        public string Ack { get; set; }

        /// <summary>
        /// Gets or sets the IsSameTrackingGuidRetry returned from CP response
        /// </summary>
        public string IsSameTrackingGuidRetry { get; set; }

        /// <summary>
        /// Gets or sets the currency of the payment
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the TotalAmountWithoutTax
        /// </summary>
        public string TotalAmountWithoutTax { get; set; }

        /// <summary>
        /// Gets or sets the TotalTax
        /// </summary>
        public string TotalTax { get; set; }

        /// <summary>
        /// Gets or sets the TaxEntries
        /// </summary>
        public TaxEntry TaxEntries { get; set; }
    }
}