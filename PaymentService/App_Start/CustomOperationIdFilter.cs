using System.Linq;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using System.Globalization;

namespace Microsoft.SupplyChain.Care.PaymentService
{
    internal class CustomOperationIdFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                // Select the capitalized parameter names
                var parameters = operation.parameters.Select(
                    p => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.name));

                // Set the operation id to match the format "OperationByParam1AndParam2"
                operation.operationId = string.Format(
                    "{0}By{1}",
                    operation.operationId,
                    string.Join("And", parameters));
            }

            if (operation.operationId.Length > 300)
                operation.operationId = operation.operationId.Substring(0, 300);

        }

    }
}