namespace GameStore.API.ApiModels.Responses;

public abstract record ErrorResponse(string Title, int Status, string Message) : IErrorResponse;