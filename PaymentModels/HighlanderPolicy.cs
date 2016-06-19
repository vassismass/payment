using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Care.PaymentModels
{
    #region Assembly Microsoft.ServiceOrder.Infrastructure.ExceptionManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2223b85afcd2b3d9
    #endregion

        public static class HighlanderPolicy
        {
            public const string AsyncDataLoadBoundaryPolicy = "AsyncDataLoadBoundaryPolicy";
            public const string BatchProcessorPolicy = "BatchProcessorPolicy";
            public const string CommerceServerPolicy = "CommerceServerPolicy";
            public const string ControllerBoundaryPolicy = "ControllerBoundaryPolicy";
            public const string DataBoundaryPolicy = "DataBoundaryPolicy";
            public const string FrameworkPolicy = "FrameworkPolicy";
            public const string GeneralExceptionPolicy = "GeneralExceptionPolicy";
            public const string IntegrationPolicy = "IntegrationPolicy";
            public const string OpSequenceBoundaryPolicy = "OpSequenceBoundaryPolicy";
            public const string PresentationBoundaryPolicy = "PresentationBoundaryPolicy";
            public const string ProviderPolicy = "ProviderPolicy";
            public const string UIBoundaryPolicy = "UIBoundaryPolicy";
            public const string WCFBoundaryPolicy = "WCFBoundaryPolicy";
        }
    
}
