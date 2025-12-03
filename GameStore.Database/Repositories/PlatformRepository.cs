using GameStore.Application.Abstractions;
using GameStore.Infrastructure.Contexts;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Repositories;

public class PlatformRepository : IPlatformRepository
{
    private readonly GameStoreContext context;

    public PlatformRepository(GameStoreContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        return await context.Platforms.AsNoTracking().ToListAsync();
    }

    public async Task<Platform?> GetByIdAsync(Guid id)
    {
        return await context.Platforms.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Game>> GetGamesAsync(Guid platformId)
    {
        return await context.GamePlatforms
            .Where(gp => gp.PlatformId == platformId)
            .Include(gp => gp.Game)
            .Select(gp => gp.Game!)
            .Where(game => game != null)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        return await context.Platforms.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();
    }

    public async Task<Platform> AddAsync(Platform platform)
    {
        context.Platforms.Add(platform);
        await context.SaveChangesAsync();
        return platform;
    }

    public async Task<Platform> UpdateAsync(Platform platform)
    {
        var existing = await context.Platforms.FirstOrDefaultAsync(p => p.Id == platform.Id) ?? throw new ObjectNotFoundException(nameof(Platform), platform.Id);
        UpdateProperties(existing, platform);
        await context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(Guid id)
    {
        var platform = await context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        if (platform != null)
        {
            context.Platforms.Remove(platform);
            await context.SaveChangesAsync();
        }
    }

    private static void UpdateProperties(Platform existing, Platform updated)
    {
        existing.Type = updated.Type;
    }
}
