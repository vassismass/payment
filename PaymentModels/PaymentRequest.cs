
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed by the Payment entity.
    /// </summary>
    public class PaymentRequest
    {
       public OrderPayment payment{ get; set; } 
       public ServiceOrder order { get; set; } 
       string Puid { get; set; } 
       string GuestID { get; set; }
       string trackingGuid { get; set; }
    }
}