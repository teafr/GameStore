using GameStore.Domain.Entities;

namespace GameStore.Application.Abstractions;

public interface IPlatformRepository
{
    Task<IEnumerable<Platform>> GetAllAsync();

    Task<Platform?> GetByIdAsync(Guid id);

    Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids);

    Task<IEnumerable<Game>> GetGamesAsync(Guid platformId);
    
    Task<Platform> UpdateAsync(Platform platform);

    Task<Platform> AddAsync(Platform platform);

    Task DeleteAsync(Guid id);
}