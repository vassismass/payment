
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed by the Payment entity.
    /// </summary>
    public class GetTaxRequest
    {
        /// <summary>
        /// Gets or sets the PaymentMethod like CC, COD
        /// </summary>
        public PaymentMethods PaymentMethod { get; set; }

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

        /// <summary>
        /// Registered Device serial number 
        /// </summary>
        /// <value> Device serial number</value>
        public string DeviceSerialNumber { get; set; }

        /// <summary>
        /// ProductType for the device
        /// </summary>
        /// <value> ProductType like Xbox 360</value>
        public string ProductType { get; set; }

        /// <summary>
        /// Order type like Repair Order,Advance Exchange
        /// </summary>
        /// <value>Order type</value>
        public string OrderType { get; set; }

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

        /// <summary>
        /// Gets or sets the Brand
        /// </summary>
        /// <value> Brand like Xbox, surface</value>
        public string brand { get; set; }

    
       }
}