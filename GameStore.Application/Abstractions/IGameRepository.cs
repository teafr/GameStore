using GameStore.Domain.Entities;

namespace GameStore.Application.Abstractions;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();

    Task<Game?> GetByIdAsync(Guid id);

    Task<Game?> GetByKeyAsync(string key);
    
    Task<IEnumerable<Genre>> GetGenresAsync(string gameKey);
    
    Task<IEnumerable<Platform>> GetPlatformsAsync(string gameKey);

    Task<Game> AddAsync(Game game);

    Task<Game> UpdateAsync(Game game);

    Task DeleteByKeyAsync(string key);
}