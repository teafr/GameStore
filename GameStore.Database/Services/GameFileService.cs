using GameStore.Application.Abstractions;
using GameStore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Slugify;
using System.Text.Json;

namespace GameStore.Infrastructure.Services;

public class GameFileService : IGameFileService
{
    private const string FileNameTemplate = "_{0}.{1}";
    private const string FileExtension = "txt";
    private const string FilePathKey = "GameFilePath";

    private readonly string fileDirectory;
    private readonly SlugHelper slugHelper = new SlugHelper();

    /// <summary>
    /// Initializes the game file service, ensuring the directory exists.
    /// </summary>
    /// <param name="configuration">Application configuration for file directory key.</param>
    public GameFileService(IConfiguration configuration)
    {
        fileDirectory = configuration.GetValue<string>(FilePathKey) ?? "GameFiles";
        Directory.CreateDirectory(fileDirectory);
    }

    /// <summary>
    /// Generates a .txt file for the given game, with serialized content.
    /// </summary>
    /// <param name="game">The game to serialize.</param>
    /// <returns>The full file path of the generated file.</returns>
    /// <exception cref="IOException">Thrown if the file cannot be written.</exception>
    public async Task<string> GenerateFileAsync(Game game, CancellationToken token)
    {
        string key = string.IsNullOrWhiteSpace(game.Key) ? SlugifyGameName(game.Name) : game.Key;
        string filePath = GetFullFilePath(key);

        await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(game), token);
        return filePath;
    }

    public string GetFileName(string gameKey)
    {
        return string.Format(FileNameTemplate, gameKey, FileExtension);
    }

    private string SlugifyGameName(string gameName)
    {
        return slugHelper.GenerateSlug(gameName);
    }

    private string GetFullFilePath(string gameKey)
    {
        return Path.Combine(fileDirectory, GetFileName(gameKey));
    }
}
