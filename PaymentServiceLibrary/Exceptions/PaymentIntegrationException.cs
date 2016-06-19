using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
 
    [Serializable]
    public class PaymentIntegrationException : PaymentServiceLibraryException
    {
        public PaymentIntegrationException() : base() { }
        public PaymentIntegrationException(string message) : base(message) { }
        public PaymentIntegrationException(string message, Exception innerException) : base(message, innerException) { }
        public PaymentIntegrationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public PaymentIntegrationException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
            : this(message, innerException)
        {
            ExceptionCode = exceptionCode;
        }
     //   public Severity Recoverability { get; set; }
    }
}
