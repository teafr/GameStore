using GameStore.Application.Extensions;
using GameStore.Application.Models;
using GameStore.Domain.Entities;

namespace GameStore.Application.Factories;

public static class GameFactory
{
    public static Game Create(string name, string key, string? description = null)
    {
        return new Game
        {
            Id = Guid.NewGuid(),
            Name = name,
            Key = key,
            Description = description
        };
    }

    public static Game Create(GameDto dto)
    {
        var game = dto.MapToEntity();
        game.Id = Guid.NewGuid();
        return game;
    }
}
