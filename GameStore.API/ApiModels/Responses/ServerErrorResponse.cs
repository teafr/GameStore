namespace GameStore.API.ApiModels.Responses;

public record ServerErrorResponse(
    string Title,
    int Status,
    string Message,
    string? Detail = null,
    string? StackTrace = null
) : ErrorResponse(Title, Status, Message);
