
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed by the Payment entity.
    /// </summary>
    public class TaxRequest
    {
        /// <summary>
        /// Payment Details
        /// </summary>
        public OrderPayment payment { get; set; }
        
        /// <summary>
        /// Device details 
        /// </summary>
        /// <value> Device key  like SN,IMEI and value</value>
        public DeviceKey Device { get; set; }
      
        /// <summary>
        /// Gets or sets the PaymentMethod like CC, COD
        /// </summary>        
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the RepairOfferSKU
        /// </summary>
        /// <value> RepairOfferSKU selected by the customer</value>  
        public string RepairOfferSKU { get; set; }

        /// <summary>
        /// Gets or sets the ShippingOfferSKU
        /// </summary>
        /// <value> ShippingOfferSKU selected by the customer</value>
        public string ShippingOfferSKU { get; set; }
        
        ///  <summary> 
        /// Gets or sets the StateCode
        /// </summary>
        /// <value>StateCode</value>
        public string StateCode { get; set; }
        
        /// <summary>
        /// Gets or sets the CountryCode
        /// </summary>
        /// <value> CountryCode like US,UK</value>
        public string CountryCode { get; set; }
    
       }
}