using Microsoft.SupplyChain.Care.PaymentModels;
using Microsoft.SupplyChain.Care.Payment.ServiceLibrary;
using System;

namespace Microsoft.SupplyChain.Care.Payment.TestTax
{
    public class MockedGetConfiguration : IGetConfiguration
    {
        public Func<PaymentConfiguration> GetConfigFunc = null;
        public PaymentConfiguration GetConfig()
        {
            if (GetConfigFunc != null)
                return GetConfigFunc();
            return null;
        }
    }
}
