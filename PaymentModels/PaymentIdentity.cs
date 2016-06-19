

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed for Payment Identity
    /// </summary>
    public class PaymentIdentity
    {
        /// <summary>
        /// Gets or sets the payment InstrumentID.Mandatory when PaymentGateway is CP
        /// </summary>
        /// <value>PaymentInstrumentId</value> 
        public string PaymentInstrumentId { get; set; }

        /// <summary>
        /// Gets or sets the payment AccountID . Required when PaymentGateway is CP
        /// </summary>
        /// <value>AccountID</value>
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets the IdentityType
        /// </summary>
        /// <value>IdentityType</value>
        public string IdentityType { get; set; }

        /// <summary>
        /// Gets or sets the Puid
        /// </summary>
        /// <value>Puid</value>
        public string Puid { get; set; }

        /// <summary>
        /// Gets or sets the Customer ID
        /// </summary>
        /// <value>Puid</value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the Geust ID
        /// </summary>
        /// <value>Geust ID</value>
        public string GuestId { get; set; }

    }
}