using GameStore.Application.Factories;
using GameStore.Infrastructure.Contexts;
using GameStore.Infrastructure.Data;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure.Services;

public static class SeedGenresService
{
    public static async Task SeedGenresAsync(GameStoreContext context)
    {
        await AddGenres(context);
        await context.SaveChangesAsync();
    }

    private static async Task AddGenres(GameStoreContext context)
    {
        var existingGenres = await context.Genres.ToListAsync();
        var nameToId = GetDictionary(existingGenres);
        var genresToSeed = GetGenresToSeed(GetGenreNames(existingGenres));

        foreach (var (Name, Parent) in genresToSeed)
        {
            Genre newGenre = GenreFactory.Create(Name, TryToGetParentId(Parent, nameToId));
            context.Genres.Add(newGenre);
            nameToId[Name] = newGenre.Id;
        }
    }

    private static Dictionary<string, Guid> GetDictionary(List<Genre> genres)
    {
        return genres.ToDictionary(genre => genre.Name, genre => genre.Id, StringComparer.OrdinalIgnoreCase);
    }

    private static IEnumerable<(string, string?)> GetGenresToSeed(HashSet<string> existingNames)
    {
        return GenreSeedData.PredefinedGenres.Where(item => !existingNames.Contains(item.Name));
    }

    private static HashSet<string> GetGenreNames(List<Genre> genres)
    {
        return new HashSet<string>(genres.Select(genre => genre.Name), StringComparer.OrdinalIgnoreCase);
    }

    private static Guid? TryToGetParentId(string? parentName, Dictionary<string, Guid> nameToId)
    {
        if (parentName is null) return null;
        if (nameToId.TryGetValue(parentName, out var parentId)) return parentId;
        throw new ParentGenreNotFoundException(parentName);
    }
}
