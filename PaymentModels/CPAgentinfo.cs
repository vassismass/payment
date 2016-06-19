using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class CPAgentInfo
    {
        public bool isZipCodeIncludedInCpComparision { get; set; }
        public Guid productGuid { get; set; }
        public Guid partnerGuid { get; set; }
        public string dataCenter { get; set; }
        public string shipFromPath { get; set; }
    }
}
