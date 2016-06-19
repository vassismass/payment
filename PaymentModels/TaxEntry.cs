using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    /// <summary>
    /// Names of the properties exposed by the Payment entity.
    /// </summary>
    public class TaxEntry
    {
        public string Amount { get; set; }
        public string CITY { get; set; }
        public string Rate { get; set; }
    }
}