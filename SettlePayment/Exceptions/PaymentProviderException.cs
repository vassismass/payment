using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    public class PaymentProviderException : PaymentIntegrationException
    {
        public PaymentProviderException() : base() { }
        public PaymentProviderException(string message) : base(message) { }
        public PaymentProviderException(string message, Exception innerException) : base(message, innerException) { }
        public PaymentProviderException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
            : base(message, exceptionCode, innerException) { }
    }
}
