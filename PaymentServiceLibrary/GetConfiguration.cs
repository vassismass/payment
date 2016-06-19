using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.SupplyChain.Care.PaymentModels;

namespace Microsoft.SupplyChain.Care.Payment.ServiceLibrary
{
    class GetConfiguration : IGetConfiguration
    {
        public PaymentConfiguration GetConfig()
        {
            PaymentConfiguration config = new PaymentConfiguration();
            config.Config.Add("partnerGuid", ConfigurationManager.AppSettings["partnerGuid"]);
            config.Config.Add("productGuid", ConfigurationManager.AppSettings["productGuid"]);
            config.Config.Add("isZipCodeIncludedInCpComparision", ConfigurationManager.AppSettings["isZipCodeIncludedInCpComparision"]);
            config.Config.Add("enableFraudDetection", ConfigurationManager.AppSettings["enableFraudDetection"]);
            config.Config.Add("shipFromPath", ConfigurationManager.AppSettings["shipFromPath"]);
            config.Config.Add("alreadySettledErrMsg", ConfigurationManager.AppSettings["alreadySettledErrMsg"]);
            config.Config.Add("datacenterCurrent", ConfigurationManager.AppSettings["datacenterCurrent"]);
            config.Config.Add("geographyPath", ConfigurationManager.AppSettings["geographyPath"]);
            return config;
        }
    }
}
