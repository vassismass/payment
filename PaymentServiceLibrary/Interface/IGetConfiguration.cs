using Microsoft.SupplyChain.Care.PaymentModels;
using System;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    public interface IGetConfiguration
    {
        PaymentConfiguration GetConfig();
    }
}