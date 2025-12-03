namespace GameStore.API.ApiModels.Responses;

public interface IErrorResponse
{
    public string Title { get; init; }

    public int Status { get; init; }

    public string Message { get; init; }
}
