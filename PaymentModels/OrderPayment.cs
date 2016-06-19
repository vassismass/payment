
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class OrderPayment
    {
        /// <summary>
        /// Payment status as PaymentPending, GTS Pending, GTS Rejected, Completed.
        /// </summary>
        public string PaymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the PaymentGateway 
        /// </summary>
        /// <value>PaymentGateway like CP, COD,NC etc.,</value>
        public string PaymentGateway { get; set; }

        /// <summary>
        /// Gets or sets the payment AccountID . Required when PayementGateway is CP
        /// </summary>
        /// <value>AccountID</value>
        public string AccountID { get; set; }

        /// <summary>
        /// Gets or sets the payment InstrumentID.Mandatory when PayementGateway is CP
        /// </summary>
        /// <value>InstrumentID</value>
        public string InstrumentID { get; set; }
    }
}