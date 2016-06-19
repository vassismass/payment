using Microsoft.SupplyChain.Care.PaymentModels;
using System;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    public interface IBillingServices

    {
        AccountInfo GetBillingAccount(string country, PaymentIdentity paymentIdentity);

        AccountInfo UpdateBillingAccount(AccountInfo accountInfo, PaymentIdentity paymentIdentity, string locale, string currency);
    }
}
