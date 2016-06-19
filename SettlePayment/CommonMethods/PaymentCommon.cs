using CPProxy = Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP;
using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Care.PaymentModels;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CommonMethods
{
    public static class PaymentCommon
    {
        public static string GetCPExceptionMessage(CPProxy.AbstractResponse CPResponse)
        {
            string strCPResponse = "";
            if (CPResponse != null)
            {
                if (CPResponse.Error != null)
                {
                    strCPResponse = System.String.Format("{1} (CP ErrorCode:{0})",
                       CPResponse.Error.ErrorCode, CPResponse.Error.Detail);

                }
            }
            return strCPResponse;
        }

        public static CPProxy.Identity SelectIdentity(PaymentIdentity paymentIdentity)
        {
            var computeRequester = new CPProxy.Identity();

            if (!string.IsNullOrEmpty(paymentIdentity.GuestId))
                computeRequester = new CPProxy.Identity { IdentityValue = paymentIdentity.GuestId, IdentityType = CpConstants.AnonymousId };
            else
                computeRequester = new CPProxy.Identity { IdentityValue = paymentIdentity.Puid, IdentityType = CpConstants.Puid };

            return computeRequester;
        }
        /// <summary>
        /// Clean the WCF proxy
        /// </summary>
        /// <param name="proxy"></param>
        public static void CloseOrAbortProxy(CPProxy.ICommerceService proxy)
        {
            if (proxy != null)
            {
                if (proxy.State != CommunicationState.Faulted && proxy.State != CommunicationState.Closed)
                {
                    try
                    {
                        proxy.Close();
                    }
                    catch
                    {
                        proxy.Abort();
                    }
                }
                else
                {
                    proxy.Abort();
                }
            }
        }
    }
}
