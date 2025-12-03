using GameStore.Application.Extensions;
using GameStore.Application.Models;
using GameStore.Domain.Entities;

namespace GameStore.Application.Factories;

public static class GenreFactory
{
    public static Genre Create(string name, Guid? parentId = null)
    {
        return new Genre { Id = Guid.NewGuid(), Name = name, ParentGenreId = parentId };
    }

    public static Genre Create(GenreDto dto)
    {
        var genre = dto.MapToEntity();
        genre.Id = Guid.NewGuid();
        return genre;
    }
}
