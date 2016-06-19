
using Microsoft.SupplyChain.Care.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Microsoft.SupplyChain.Care.PaymentService
{
    /// <summary>
    /// Service with capabilities to get Payment Methods, Get Tax , Process Physical/Digital Payment, Settle, Release and Refund Payment
    /// </summary>
    //[RoutePrefix("return/payment")]
    public class PaymentController : ApiController
    {
        /// <summary>
        /// Returns list of payment methods like CC,COD based on country, state or postalcode
        /// </summary>
        /// <param name="countryCode"> [optional] Country eg: US</param>
        /// <param name="postalCode"> [optional] Postal Code</param>
        /// <param name="stateCode"> [optional] State Code</param>
        /// <returns>List of payment methods</returns>
        [System.Web.Http.Route("PaymentMethods")]
        public List<string> GetPaymentMethods([FromUri]string countryCode, [FromUri] string postalCode, [FromUri]string stateCode)
        {
            return new List<string>();
        }

        /// <summary>
        /// Calculates Tax amount based on payment details like PaymentMethod, OfferSKU, ShippingSKU, SerialNumber,
        /// ProductType, OrderType, StateCode, CountryCode, brand
        /// </summary>
        /// <param name="payment">Payment</param>
        /// <returns>List of tax amounts for respective SKUs</returns>
        [HttpGet]
        [System.Web.Http.Route("Tax")]
        public TaxInfo Tax([FromUri]TaxRequest payment)
        {
            return new TaxInfo();
        }

        /// <summary>
        /// Block payment allows to hold/authorize the amount 
        /// </summary>
        /// <param name="paymentRequest">paymentRequest</param>
        /// <returns>PurchaseID</returns>
        [System.Web.Http.Route("Block")]
        public string Block([FromUri]BlockPaymentRequest paymentRequest)
        {
            return string.Empty;
        }

        /// <summary>
        /// ChargePayment which allows to settle/charge the amount by passing Purchase ID
        /// </summary>
        /// <param name="PurchaseID">PurchaseID</param>
        [System.Web.Http.Route("Charge/{PurchaseID}")]
        public void Charge(string PurchaseID)
        {
        }

        /// <summary>
        /// Refund Payment for the PurchaseID based on payment identity details PaymentInstrumentId, AccountId, IdentityType, Puid, CustomerId, GuestId
        /// </summary>
        /// <param name="purchaseID">PurchaseID</param> 
        /// <param name="paymentIdentity">paymentIdentity</param> 
        [System.Web.Http.Route("Refund/{purchaseID}")]
        public void Refund(string purchaseID, [FromUri]PaymentIdentity paymentIdentity)
        {

        }

        /// <summary>
        /// Release Physical Payment for the PurchaseID
        /// </summary>
        /// <param name="purchaseID">PurchaseID</param>
        [System.Web.Http.Route("Release/{PurchaseID}")]
        public void Release(string purchaseID)
        {
        }
    }
}