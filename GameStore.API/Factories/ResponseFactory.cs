using GameStore.API.ApiModels.Responses;

namespace GameStore.API.Factories;

public static class ResponseFactory
{
    public static ServerErrorResponse CreateServerError(Exception ex, bool isDevelopment, string message = "An unexpected error occurred.")
    {
        string title = "Internal Server Error";
        int code = StatusCodes.Status500InternalServerError;
        return new ServerErrorResponse(title, code, message, isDevelopment ? ex.Message : null, isDevelopment ? ex.StackTrace : null);
    }

    public static ClientErrorResponse CreateNotFound(string message = "The requested resource was not found.")
    {
        return new ClientErrorResponse("Not Found", StatusCodes.Status404NotFound, message);
    }

    public static ClientErrorResponse CreateConflict(string message = "A duplicate resource already exists.")
    {
        return new ClientErrorResponse("Conflict", StatusCodes.Status409Conflict, message);
    }

    public static ClientErrorResponse CreateBadRequest(string message)
    {
        return new ClientErrorResponse("Bad Request", StatusCodes.Status400BadRequest, message);
    }
}
