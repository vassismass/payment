using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary.CP
{
    public partial interface ICommerceService
    {
        void Abort();

        void Close();

        System.ServiceModel.CommunicationState State { get; }
    }
}
