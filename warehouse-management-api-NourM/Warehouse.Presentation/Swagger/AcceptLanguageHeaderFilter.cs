using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Warehouse.Presentation.Swagger;

//This file is like an instruction to Swagger to tell it to add an Accept-Language field to each request
public class AcceptLanguageHeaderFilter : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        if(operation.Parameters == null)
        {
            operation.Parameters = new List<IOpenApiParameter>();
        }

        operation.Parameters.Add(new OpenApiParameter {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Required = false,
            Description = "Supported languages: en, fr, ar",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            }
        });
    }
}