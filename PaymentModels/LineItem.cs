

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class LineItem
    {

        public string Description { get; set; }


        public string Sku { get; set; }

        /// <summary>
        /// Revenuse incase of LineItem and Expense in Case of Shipping
        /// </summary>

        public string PaymentSku { get; set; }

        //Incase of DAE , pick the ExtendedPaymentSku

        public string ExtendedPaymentSku { get; set; }



        public Address AddressInfo { get; set; }


        public Price PriceInfo { get; set; }


        public LineItem ShippingInfo { get; set; }

    }
}
