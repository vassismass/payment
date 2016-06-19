using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    [Serializable]
    public abstract class PaymentServiceLibraryException : Exception, IPaymentServiceLibraryException
    {
        protected Dictionary<string, string> _paymentserviceLibraryContext = new Dictionary<string, string>();

        protected PaymentServiceLibraryException() : base() { }
        protected PaymentServiceLibraryException(string message) : base(message) { }
        protected PaymentServiceLibraryException(string message, PaymentExceptioncode exceptionCode)
            : this(message)
        {
            ExceptionCode = exceptionCode;
        }
        protected PaymentServiceLibraryException(string message, Exception innerException) : base(message, innerException) { }
        protected PaymentServiceLibraryException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
            : this(message, innerException)
        {
            ExceptionCode = exceptionCode;
        }
        protected PaymentServiceLibraryException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region IHighlanderException Members

        public Dictionary<string, string> PaymentServiceLibraryContext
        {
            get
            {
                return _paymentserviceLibraryContext;
            }
            set
            {
                if (value == null)
                    _paymentserviceLibraryContext = new Dictionary<string, string>();
                else
                    _paymentserviceLibraryContext = value;
            }
        }

        public void AddToExistingContext(Dictionary<string, string> paymentserviceLibraryContext)
        {
            try
            {
                if (paymentserviceLibraryContext == null)
                    return;
                foreach (string key in paymentserviceLibraryContext.Keys)
                {
                    if (_paymentserviceLibraryContext.ContainsKey(key))
                    {
                        string randomKey = key + "_RandomKey_" + Guid.NewGuid().ToString();
                        _paymentserviceLibraryContext.Add(randomKey, paymentserviceLibraryContext[key]);
                    }
                    else
                    {
                        _paymentserviceLibraryContext.Add(key, paymentserviceLibraryContext[key]);
                    }

                }
            }
            catch (Exception)
            {
                // Dont raise any exception from here. 
            }
        }

        public PaymentExceptioncode ExceptionCode { get; set; }

        #endregion

    }
}
