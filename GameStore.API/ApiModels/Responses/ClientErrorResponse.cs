namespace GameStore.API.ApiModels.Responses;

public record ClientErrorResponse(string Title, int Status, string Message) : ErrorResponse(Title, Status, Message);