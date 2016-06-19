using Owin;
using System.Web.Http;
using Swashbuckle.Application;
using System;

namespace Microsoft.SupplyChain.Care.PaymentService
{
  
    public partial class Startup 
        {
            public void Configuration(IAppBuilder appBuilder)
            {
                
                HttpConfiguration config = new HttpConfiguration();

                config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "PaymentService");
                    c.OperationFilter<CustomOperationIdFilter>();
                    c.IncludeXmlComments(string.Format(@"{0}\Microsoft.SupplyChain.CustomerCare.PaymentService.XML", AppDomain.CurrentDomain.BaseDirectory));
                }
                )
                .EnableSwaggerUi();

                config.MapHttpAttributeRoutes();
     
                appBuilder.UseWebApi(config);

            }

        }
    
}