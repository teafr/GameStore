using GameStore.Application.Models;
using GameStore.Domain.Entities;

namespace GameStore.Application.Extensions;

public static class GameExtension
{
    public static GameDto MapToDto(this Game game)
    {
        return new GameDto
        {
            Id = game.Id,
            Name = game.Name,
            Key = game.Key,
            Description = game.Description,
            GenreIds = game.GameGenres.Select(gg => gg.GenreId).ToList(),
            PlatformIds = game.GamePlatforms.Select(gp => gp.PlatformId).ToList()
        };
    }

    public static Game MapToEntity(this GameDto gameDto)
    {
        return new Game
        {
            Id = gameDto.Id,
            Name = gameDto.Name,
            Key = gameDto.Key,
            Description = gameDto.Description,
            GameGenres = gameDto.GenreIds?.Select(genreId => new GameGenre { GenreId = genreId, GameId = gameDto.Id }).ToList() ?? [],
            GamePlatforms = gameDto.PlatformIds?.Select(platformId => new GamePlatform { PlatformId = platformId, GameId = gameDto.Id }).ToList() ?? []
        };
    }
}
