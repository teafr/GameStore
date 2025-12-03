using GameStore.Domain.Entities;

namespace GameStore.Application.Abstractions;

public interface IGameFileService
{
    Task<string> GenerateFileAsync(Game game, CancellationToken token);

    string GetFileName(string gameKey);
}