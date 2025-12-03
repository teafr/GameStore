using GameStore.Application.Factories;
using GameStore.Infrastructure.Contexts;
using GameStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Services;

public static class SeedPlatformsService
{
    public static async Task SeedPlatformsAsync(GameStoreContext context)
    {
        var existingTypes = await GetExistingPlatforms(context);
        var toAdd = PlatformSeedData.PredefinedPlatforms.Where(type => !existingTypes.Contains(type)).Select(PlatformFactory.Create);
        await context.Platforms.AddRangeAsync(toAdd);
        await context.SaveChangesAsync();
    }

    private async static Task<HashSet<string>> GetExistingPlatforms(GameStoreContext context)
    {
        return (await context.Platforms.Select(p => p.Type).ToListAsync()).ToHashSet();
    }
}
