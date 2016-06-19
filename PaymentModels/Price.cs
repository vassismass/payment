using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    public class Price
    {
        public decimal Amount { get; set; }


        public string Currency { get; set; }


        public decimal Tax { get; set; }


        public string TaxCode { get; set; }

        //Incase of Deductible Advance Exchange , pick the ExtendedTaxCode

        public string ExtendedTaxCode { get; set; }
    }
}
