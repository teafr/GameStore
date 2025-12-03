using GameStore.Application.Extensions;
using GameStore.Application.Abstractions;
using GameStore.Application.Models;
using GameStore.Domain.Exceptions.FileExceptions;
using System.Text.Json;

namespace GameStore.Application.Services;

public class GameFileManager : IGameFileManager
{
    private readonly IGameFileService gameFileService;

    public GameFileManager(IGameFileService fileService)
    {
        gameFileService = fileService;
    }

    public async Task GenerateFileAsync(GameDto game, CancellationToken token)
    {
        try
        {
            await gameFileService.GenerateFileAsync(game.MapToEntity(), token);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new FileGenerationException("Permission denied while creating game file.", ex);
        }
        catch (IOException ex)
        {
            throw new FileGenerationException("A storage error occurred while creating the game file.", ex);
        }
        catch (JsonException ex)
        {
            throw new FileGenerationException("Game data could not be serialized.", ex);
        }
    }
}
