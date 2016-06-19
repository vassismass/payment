namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Error entity
    /// </summary>
    public class PaymentError
    {
        public string ErrorCodeOrType { get; set; }
        public string Description { get; set; }
        public string EventId { get; set; }
    }
}