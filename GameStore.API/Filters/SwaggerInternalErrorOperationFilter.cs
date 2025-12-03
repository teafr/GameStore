using GameStore.API.ApiModels.Responses;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameStore.API.Filters;

public class SwaggerInternalErrorOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        const int InternalErrorCode = StatusCodes.Status500InternalServerError;
        operation.Responses[InternalErrorCode.ToString()] = new OpenApiResponse
        {
            Description = "Internal Server Error",
            Content = {
                ["application/json"] = new OpenApiMediaType { Schema = CreateErrorResponseSchema(context) }
            }
        };
    }

    private static OpenApiSchema CreateErrorResponseSchema(OperationFilterContext context)
    {
        return context.SchemaGenerator.GenerateSchema(typeof(ServerErrorResponse), context.SchemaRepository);
    }
}