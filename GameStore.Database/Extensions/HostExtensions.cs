using GameStore.Infrastructure.Contexts;
using GameStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameStore.Infrastructure.Extensions;

public static class HostExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        return await ExecuteWithDbContextAsync(host, async context => await context.Database.MigrateAsync());
    }

    public static async Task<IHost> SeedDatabaseAsync(this IHost host)
    {
        return await ExecuteWithDbContextAsync(host, async context =>
        {
            await SeedPlatformsService.SeedPlatformsAsync(context);
            await SeedGenresService.SeedGenresAsync(context);
        });
    }

    private static async Task<IHost> ExecuteWithDbContextAsync(this IHost host, Func<GameStoreContext, Task> action)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await action(context);
        return host;
    }
}
