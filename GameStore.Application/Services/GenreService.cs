using GameStore.Application.Extensions;
using GameStore.Application.Factories;
using GameStore.Application.Handlers;
using GameStore.Application.Abstractions;
using GameStore.Application.Models;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;

namespace GameStore.Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        this.genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreDto>> GetAll()
    {
        var list = await genreRepository.GetAllAsync();
        return list.Select(genre => genre.MapToDto());
    }

    public async Task<GenreDto> GetByIdAsync(Guid id)
    {
        var genre = await GetGenreOrThrow(id);
        return genre.MapToDto();
    }

    public async Task<IEnumerable<GenreDto>> GetByParentIdAsync(Guid parentId)
    {
        await GetGenreOrThrow(parentId);
        var list = await genreRepository.GetByParentIdAsync(parentId);
        return list.Select(genre => genre.MapToDto());
    }

    public async Task<IEnumerable<GameDto>> GetGamesAsync(Guid genreId)
    {
        await GetGenreOrThrow(genreId);
        var games = await genreRepository.GetGamesAsync(genreId);
        return games.Select(g => g.MapToDto());
    }

    public async Task<GenreDto> Add(GenreDto dto)
    {
        await ValidateGenreReferenceAsync(dto.ParentGenreId);
        return await TryExecuteAndMapAsync(() => genreRepository.AddAsync(GenreFactory.Create(dto)), dto);
    }

    public async Task<GenreDto> UpdateAsync(GenreDto dto)
    {
        await ValidateGenreReferenceAsync(dto.ParentGenreId);
        return await TryExecuteAndMapAsync(() => genreRepository.UpdateAsync(dto.MapToEntity()), dto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await GetGenreOrThrow(id);
        await genreRepository.DeleteAsync(id);
    }

    private async Task<Genre> GetGenreOrThrow(Guid id)
    {
        return await genreRepository.GetByIdAsync(id) ?? throw new ObjectNotFoundException(nameof(Genre), id);
    }

    private async Task ValidateGenreReferenceAsync(Guid? genreId)
    {
        if (genreId is Guid validParentId)
        {
            var exists = await genreRepository.ExistsByIdAsync(validParentId);
            if (!exists)
            {
                throw new InvalidReferenceException(nameof(Genre), [validParentId]);
            }
        }
    }

    private static async Task<GenreDto> TryExecuteAndMapAsync(Func<Task<Genre>> action, GenreDto dto)
    {
        var genre = await HandleUniqueConstraintAsync(action, dto.Name);
        return genre.MapToDto();
    }

    private static async Task<Genre> HandleUniqueConstraintAsync(Func<Task<Genre>> action, string value)
    {
        return await UniqueConstraintHandler.HandleAsync(action, nameof(Genre.Name), value);
    }
}
