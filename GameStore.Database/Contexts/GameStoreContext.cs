using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Contexts;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options) { }

    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Platform> Platforms { get; set; }

    public DbSet<GameGenre> GameGenres { get; set; }

    public DbSet<GamePlatform> GamePlatforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureGameGenre(modelBuilder);
        ConfigureGamePlatform(modelBuilder);
        ConfigureGenreRelations(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureGameGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameGenre>().HasKey(gameGenre => new { gameGenre.GameId, gameGenre.GenreId });
        modelBuilder.Entity<GameGenre>()
            .HasOne(gameGenre => gameGenre.Game)
            .WithMany(game => game.GameGenres)
            .HasForeignKey(gameGenre => gameGenre.GameId);
        modelBuilder.Entity<GameGenre>()
            .HasOne(gameGenre => gameGenre.Genre)
            .WithMany(genre => genre.GameGenres)
            .HasForeignKey(gameGenre => gameGenre.GenreId);
    }

    private static void ConfigureGamePlatform(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GamePlatform>().HasKey(gamePlatform => new { gamePlatform.GameId, gamePlatform.PlatformId });
        modelBuilder.Entity<GamePlatform>()
            .HasOne(gamePlatform => gamePlatform.Game)
            .WithMany(game => game.GamePlatforms)
            .HasForeignKey(gamePlatform => gamePlatform.GameId);
        modelBuilder.Entity<GamePlatform>()
            .HasOne(gamePlatform => gamePlatform.Platform)
            .WithMany(platform => platform.GamePlatforms)
            .HasForeignKey(gamePlatform => gamePlatform.PlatformId);
    }

    private static void ConfigureGenreRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>()
            .HasOne(genre => genre.ParentGenre)
            .WithMany(genre => genre.SubGenres)
            .HasForeignKey(genre => genre.ParentGenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
