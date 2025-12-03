using GameStore.Application.Extensions;
using GameStore.Application.Handlers;
using GameStore.Application.Abstractions;
using GameStore.Application.Models;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;

namespace GameStore.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository gameRepository;
    private readonly IGenreRepository genreRepository;
    private readonly IPlatformRepository platformRepository;

    public GameService(IGameRepository gameRepository, IGenreRepository genreRepository, IPlatformRepository platformRepository)
    {
        this.gameRepository = gameRepository;
        this.genreRepository = genreRepository;
        this.platformRepository = platformRepository;
    }

    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        var games = await gameRepository.GetAllAsync();
        return games.Select(game => game.MapToDto());
    }

    public async Task<GameDto> GetByKeyAsync(string key)
    {
        var game = await GetGameByKeyOrThrow(key);
        return game.MapToDto();
    }

    public async Task<GameDto> GetByIdAsync(Guid id)
    {
        var game = await gameRepository.GetByIdAsync(id) ?? throw new ObjectNotFoundException(nameof(Game), id);
        return game.MapToDto();
    }

    public async Task<IEnumerable<GenreDto>> GetGenresAsync(string key)
    {
        await GetGameByKeyOrThrow(key);
        var genres = await gameRepository.GetGenresAsync(key);
        return genres.Select(g => g.MapToDto());
    }

    public async Task<IEnumerable<PlatformDto>> GetPlatformsAsync(string key)
    {
        await GetGameByKeyOrThrow(key);
        var platforms = await gameRepository.GetPlatformsAsync(key);
        return platforms.Select(p => p.MapToDto());
    }

    public async Task<GameDto> AddAsync(GameDto dto)
    {
        dto.Id = Guid.NewGuid();
        await ValidateGenreAndPlatformIdsAsync(dto.GenreIds, dto.PlatformIds);

        Game newGame = await HandleUniqueConstraintAsync(
            () => gameRepository.AddAsync(dto.MapToEntity()), 
            dto.Key
        );
        return newGame.MapToDto();
    }

    public async Task<GameDto> UpdateAsync(GameDto dto)
    {
        Game updatedGame = await HandleUniqueConstraintAsync(
            () => gameRepository.UpdateAsync(dto.MapToEntity()), 
            dto.Key
        );
        return updatedGame.MapToDto();
    }

    public async Task DeleteByKeyAsync(string key)
    {
        await GetGameByKeyOrThrow(key);
        await gameRepository.DeleteByKeyAsync(key);
    }

    private async Task<Game> GetGameByKeyOrThrow(string key)
    {
        return await gameRepository.GetByKeyAsync(key) ?? throw new ObjectNotFoundException(nameof(Game), key);
    }

    private static async Task<Game> HandleUniqueConstraintAsync(Func<Task<Game>> action, string value)
    {
        return await UniqueConstraintHandler.HandleAsync(action, nameof(Game.Key), value);
    }

    private async Task ValidateGenreAndPlatformIdsAsync(IEnumerable<Guid> genreIds, IEnumerable<Guid> platformIds)
    {
        await ValidateGenreIdsAsync(genreIds);
        await ValidatePlatformIdsAsync(platformIds);
    }

    private async Task ValidateGenreIdsAsync(IEnumerable<Guid> genreIds)
    {
        await ValidateEntityIdsAsync<Genre>(genreIds, genreRepository.GetExistingIdsAsync);
    }

    private async Task ValidatePlatformIdsAsync(IEnumerable<Guid> platformIds)
    {
        await ValidateEntityIdsAsync<Platform>(platformIds, platformRepository.GetExistingIdsAsync);
    }

    private static async Task ValidateEntityIdsAsync<TEntity>(
        IEnumerable<Guid> ids, 
        Func<IEnumerable<Guid>, Task<IEnumerable<Guid>>> getExistingIds)
    {
        var idSet = ids.ToHashSet();
        var missingIds = idSet.Except(await getExistingIds(ids));
        if (missingIds.Any()) throw new InvalidReferenceException(typeof(TEntity).Name, missingIds);
    }
}
