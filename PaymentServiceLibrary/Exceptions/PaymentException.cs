using Microsoft.SupplyChain.Care.PaymentModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    [Serializable]
    public class PaymentException : PaymentIntegrationException
    {
        public PaymentError PaymentError { get; set; }

        public PaymentException(PaymentError PaymentError)
        {
            this.PaymentError = PaymentError;
        }
        public PaymentException() : base() { }
        public PaymentException(string message) : base(message) { }
        public PaymentException(string message, Exception innerException) : base(message, innerException) { }
        public PaymentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public PaymentException(string message, PaymentExceptioncode exceptionCode, Exception innerException)
            : base(message, exceptionCode, innerException)
        { }

        public void HandleException(Exception exception, Dictionary<string, string> context)
        {
            PaymentServiceLibraryException derived = null;

            if (exception is EndpointNotFoundException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentEndpointNotFoundException,
                                                      exception);
            }
            else if (exception is TimeoutException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentTimeoutException,
                                                      exception);
            }

            else if (exception is System.Web.Services.Protocols.SoapException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.PaymentSoapException,
                                                      exception);
            }
            else if (exception is ConfigurationException)
            {
                derived = new PaymentException(exception.Message, PaymentExceptioncode.CPRequiredConfigValuesUnavailable,
                                                      exception);
            }
            else if (exception is PaymentException)
            {
                derived = new PaymentException(exception.Message, ((PaymentServiceLibraryException)(exception)).ExceptionCode,
                                                      exception);
            }
            else
            {
                derived = exception as PaymentIntegrationException ??
                          new PaymentException(exception.Message, PaymentExceptioncode.GenericPaymentException, exception);
            }

            derived.AddToExistingContext(context);

           // if (ExceptionPolicy.HandleException(derived, HighlanderPolicy.ProviderPolicy))
                throw derived;
        }

    }
}
