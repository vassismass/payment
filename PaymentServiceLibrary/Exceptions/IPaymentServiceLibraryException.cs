using System.Collections.Generic;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    public interface IPaymentServiceLibraryException
    {
        Dictionary<string, string> PaymentServiceLibraryContext
        {
            get;
            set;
        }

        PaymentExceptioncode ExceptionCode { get; set; }
    }
}
