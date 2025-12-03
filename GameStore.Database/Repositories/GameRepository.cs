using GameStore.Application.Abstractions;
using GameStore.Infrastructure.Contexts;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameStore.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameStoreContext context;

    public GameRepository(GameStoreContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await context.Games.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await FindGameWithIncludesAsync(g => g.Id == id);
    }

    public async Task<Game?> GetByKeyAsync(string key)
    {
        return await FindGameWithIncludesAsync(g => g.Key == key);
    }

    public async Task<IEnumerable<Genre>> GetGenresAsync(string gameKey)
    {
        return await context.GameGenres
            .Where(gameGenre => gameGenre.Game!.Key == gameKey)
            .Include(gameGenre => gameGenre.Genre)
            .Select(gameGenre => gameGenre.Genre!)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Platform>> GetPlatformsAsync(string gameKey)
    {
        return await context.GamePlatforms
            .Where(gamePlatform => gamePlatform.Game!.Key == gameKey)
            .Include(gamePlatform => gamePlatform.Platform)
            .Select(gamePlatform => gamePlatform.Platform!)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Game> AddAsync(Game game)
    {
        context.Games.Add(game);
        await context.SaveChangesAsync();
        return game;
    }

    public async Task<Game> UpdateAsync(Game game)
    {
        var existingGame = await FindGameWithIncludesAsync(g => g.Id == game.Id) ?? throw new ObjectNotFoundException(nameof(Game), game.Key);

        RemoveRelations(existingGame);
        UpdateProperties(existingGame, game);

        await context.SaveChangesAsync();
        return existingGame;
    }

    public async Task DeleteByKeyAsync(string key)
    {
        var game = await context.Games.FirstOrDefaultAsync(g => g.Key == key);
        if (game != null)
        {
            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }
    }

    private async Task<Game?> FindGameWithIncludesAsync(Expression<Func<Game, bool>> filter)
    {
        return await context.Games
            .Include(game => game.GameGenres).ThenInclude(gameGenre => gameGenre.Genre)
            .Include(game => game.GamePlatforms).ThenInclude(gamePlatform => gamePlatform.Platform)
            .AsSplitQuery()
            .FirstOrDefaultAsync(filter);
    }

    private static void UpdateProperties(Game target, Game source)
    {
        target.Name = source.Name;
        target.Key = source.Key;
        target.Description = source.Description;
        target.GameGenres = source.GameGenres;
        target.GamePlatforms = source.GamePlatforms;
    }

    private void RemoveRelations(Game game)
    {
        RemoveGenres(game);
        RemovePlatforms(game);
    }

    private void RemoveGenres(Game game)
    {
        context.GameGenres.RemoveRange(game.GameGenres);
    }

    private void RemovePlatforms(Game game)
    {
        context.GamePlatforms.RemoveRange(game.GamePlatforms);
    }
}
