using GameStore.API.Factories;
using GameStore.Domain.Exceptions.DatabaseExceptions;

namespace GameStore.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private const string ContentType = "application/json";

    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;
    private readonly IHostEnvironment environment;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
    {
        this.next = next;
        this.logger = logger;
        this.environment = environment;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (InvalidReferenceException ex)
        {
            await FillResponseAsync(httpContext, StatusCodes.Status400BadRequest, ResponseFactory.CreateBadRequest(ex.Message));
        }
        catch (ObjectNotFoundException ex)
        {
            await FillResponseAsync(httpContext, StatusCodes.Status404NotFound, ResponseFactory.CreateNotFound(ex.Message));
        }
        catch (DuplicateObjectException ex)
        {
            await FillResponseAsync(httpContext, StatusCodes.Status409Conflict, ResponseFactory.CreateConflict(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            int code = StatusCodes.Status500InternalServerError;
            await FillResponseAsync(httpContext, code, ResponseFactory.CreateServerError(ex, environment.IsDevelopment()));
        }
    }

    private static async Task FillResponseAsync(HttpContext httpContext, int statusCode, object response)
    {
        if (!httpContext.Response.HasStarted)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = ContentType;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}