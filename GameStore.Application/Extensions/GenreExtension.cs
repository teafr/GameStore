using GameStore.Application.Models;
using GameStore.Domain.Entities;

namespace GameStore.Application.Extensions;

public static class GenreExtension
{
    public static GenreDto MapToDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            ParentGenreId = genre.ParentGenreId
        };
    }

    public static Genre MapToEntity(this GenreDto genreDto)
    {
        return new Genre
        {
            Id = genreDto.Id,
            Name = genreDto.Name,
            ParentGenreId = genreDto.ParentGenreId
        };
    }
}
