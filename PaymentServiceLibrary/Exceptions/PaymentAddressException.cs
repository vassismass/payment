using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    [Serializable]
   public class PaymentAddressException : PaymentException
    {
        public PaymentAddressException() : base() { }
        public PaymentAddressException(string message) : base(message) { }
        public PaymentAddressException(string message, Exception innerException) : base(message, innerException) { }
        public PaymentAddressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public PaymentAddressException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
            : base(message, exceptionCode, innerException) { }
    }
}
