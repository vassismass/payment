
namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class TaxInfo
    {
        /// <summary>
        /// Repair Offer Price 
        /// </summary>
        public string RepairOfferPrice { get; set; }

        /// <summary>
        /// Repair Offer tax 
        /// </summary>
        public string RepairOfferTax { get; set; }

        /// <summary>
        /// Shipping Offer Price 
        /// </summary>
        public string ShippingOfferPrice { get; set; }

        /// <summary>
        /// Shipping Offer tax 
        /// </summary>
        public string ShippingOfferTax { get; set; }

        /// <summary>
        /// Repair Extended Price
        /// </summary>
        public string RepairExtndPrice { get; set; }

        /// <summary>
        /// Repair Extended Tax
        /// </summary>
        public string RepairExtndTax { get; set; }

        /// <summary>
        /// Total amount excluding extended price and tax
        /// </summary>             
        public string TotalAmoutWithoutTax { get; set; }

        /// <summary>
        /// Total amount includingTax and excluding extended price
        /// </summary>
        public string TotalAmount { get; set; }

    }
}