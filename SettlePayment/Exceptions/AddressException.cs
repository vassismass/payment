using Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SettlePayment.Exceptions
{
    

   
        public class AddressException : PaymentException
        {
            public AddressException() : base() { }
            public AddressException(string message) : base(message) { }
            public AddressException(string message, Exception innerException) : base(message, innerException) { }
            public AddressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            public AddressException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
                : base(message, exceptionCode, innerException) { }
        }
   

}
