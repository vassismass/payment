using System;
using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.Exceptions
{
    public class ConfigurationException : PaymentIntegrationException
    {
        public ConfigurationException() { }
        public ConfigurationException(string message) { }
        public ConfigurationException(string message, Exception innerException) { }
        public ConfigurationException(SerializationInfo info, StreamingContext context) { }
    }
}