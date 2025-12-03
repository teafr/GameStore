using Asp.Versioning;
using Asp.Versioning.Conventions;
using GameStore.API.Filters;
using GameStore.Application.Abstractions;
using GameStore.Application.BackgroundTasks;
using GameStore.Application.Services;
using GameStore.Infrastructure.BackgroundTasks;
using GameStore.Infrastructure.Contexts;
using GameStore.Infrastructure.Repositories;
using GameStore.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GameStore.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddGameStoreServices(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPlatformRepository, PlatformRepository>();

        services.AddScoped<IGameFileService, GameFileService>();
        services.AddScoped<IGameFileManager, GameFileManager>();

        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPlatformService, PlatformService>();

        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GameStoreContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("GameStoreDB"));
        });

        return services;
    }

    public static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "GameStore.API.xml"));
            options.OperationFilter<SwaggerInternalErrorOperationFilter>();
        });

        return services;
    }

    public static IServiceCollection ConfigureHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<QueuedHostedService>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddSingleton<ITaskTracker, InMemoryTaskTracker>();
        services.AddSingleton<IBackgroundJobService, BackgroundJobService>();

        return services;
    }

    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.CacheProfiles.Add("Default", new CacheProfile
            {
                Duration = 60,
                Location = ResponseCacheLocation.Any
            });
        }).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }

    public static IServiceCollection ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        }).AddMvc(options => options.Conventions.Add(new VersionByNamespaceConvention())).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
