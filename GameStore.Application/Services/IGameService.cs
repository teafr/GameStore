using GameStore.Application.Models;

namespace GameStore.Application.Services;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllAsync();
    
    Task<GameDto> GetByIdAsync(Guid id);
    
    Task<GameDto> GetByKeyAsync(string key);
    
    Task<IEnumerable<GenreDto>> GetGenresAsync(string key);
    
    Task<IEnumerable<PlatformDto>> GetPlatformsAsync(string key);

    Task<GameDto> AddAsync(GameDto dto);

    Task<GameDto> UpdateAsync(GameDto dto);

    Task DeleteByKeyAsync(string key);
}