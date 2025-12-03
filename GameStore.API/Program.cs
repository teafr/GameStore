using GameStore.API.Extensions;
using GameStore.API.Middlewares;
using GameStore.Infrastructure.Extensions;

namespace GameStore.API;

 static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureControllers().ConfigureVersioning().AddEndpointsApiExplorer().ConfigureSwaggerGen();
        builder.Services.AddResponseCaching();
        builder.Services.ConfigureDatabase(builder.Configuration).AddGameStoreServices().ConfigureHostedServices();

        var app = builder.Build();

        await app.MigrateDatabaseAsync();
        await app.SeedDatabaseAsync();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseResponseCaching();

        app.MapControllers();

        await app.RunAsync();
    }
}
