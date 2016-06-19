using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.SupplyChain.Care.PaymentModels
{   
    ///Represent sales order document which customer has in case he has bought customised accessory or device from marketplace
    public class SalesOrderDoc
    {   
        /// <summary>
        /// Sale order number
        /// </summary>
        public string SalesOrderNumber { get; set; }

        /// <summary>
        /// Line item number containing the actual purchase detail of the accessory or device as one SalesOrderDoc can have multiple items
        /// </summary>
        public string LineItemNumber { get; set; }

        /// <summary>
        /// When was the sale executed
        /// </summary>
        public DateTime PurcahseDate { get; set; }
    }
}