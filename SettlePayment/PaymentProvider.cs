//=====================================================================================================
//   Class Name     : PaymentProvider               
//   Developed By   : v-pasaga
//   Purpose        : To implement provider model for CP payment calls
//   Date           : 04/09/2013                       
//=====================================================================================================

namespace SettlePayment
{
    #region Using
    //using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    
    using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
    using Microsoft.SupplyChain.Care.PaymentModels;
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>
    /// Represents the base for all Payment Providers
    /// </summary>
    public abstract class PaymentProvider 
    {
        private const string PaymentProviderError = "Unhandled error occurred in PaymentProvider";

        /// <summary>
        /// Performs the purchase for the requested order
        /// </summary>
        /// <param name="request">PurchaseGoodsRequest</param>
        /// <returns>PurchaseGoodsResponse</returns>
        public abstract PurchaseGoodsResponse PurchaseGoods(PurchaseGoodsRequest request);

        /// <summary>
        /// Performs the settlement of the physical goods purchased
        /// </summary>
        /// <param name="request">SettlePurchaseGoodsRequest</param>
        /// <returns>SettlePurchaseGoodsResponse</returns>
        public abstract SettlePurchaseGoodsResponse SettlePurchaseGoods(SettlePurchaseGoodsRequest request);

        /// <summary>
        /// Generic way to handle exception(s)
        /// </summary>
        protected void HandleException(Exception exception, PaymentExceptioncode exceptionCode, Dictionary<string, string> context)
        {
            PaymentServiceLibraryException derived = new PaymentProviderException(PaymentProviderError, exceptionCode, exception);

            derived.AddToExistingContext(context);
            throw derived;
        }
    }
}
