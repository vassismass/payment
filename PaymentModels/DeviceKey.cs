

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Device key details
    /// </summary>
    public class DeviceKey
    {
        /// <summary>
        /// Device key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Ket type - IMEI or SN
        /// </summary>
        public string KeyType { get; set; }
    }
}