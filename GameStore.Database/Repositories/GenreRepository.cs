using GameStore.Application.Abstractions;
using GameStore.Infrastructure.Contexts;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly GameStoreContext context;

    public GenreRepository(GameStoreContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await context.Genres.AsNoTracking().ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(Guid id)
    {
        return await context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentGenreId)
    {
        return await context.Genres.Where(g => g.ParentGenreId == parentGenreId).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Game>> GetGamesAsync(Guid genreId)
    {
        return await context.GameGenres
            .Where(gg => gg.GenreId == genreId)
            .Include(gg => gg.Game)
            .Select(gg => gg.Game!)
            .Where(game => game != null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        return await context.Genres.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        context.Genres.Add(genre);
        await context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre> UpdateAsync(Genre genre)
    {
        var existing = await context.Genres.FirstOrDefaultAsync(g => g.Id == genre.Id) ?? throw new ObjectNotFoundException(nameof(Genre), genre.Id);
        UpdateProperties(existing, genre);
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var genre = await context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre != null)
        {
            context.Genres.Remove(genre);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await context.Genres.AnyAsync(g => g.Id == id);
    }

    private static void UpdateProperties(Genre existing, Genre updated)
    {
        existing.Name = updated.Name;
        existing.ParentGenreId = updated.ParentGenreId;
    }
}
