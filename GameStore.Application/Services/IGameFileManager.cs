using GameStore.Application.Models;

namespace GameStore.Application.Services;

public interface IGameFileManager
{
    Task GenerateFileAsync(GameDto game, CancellationToken token);
}