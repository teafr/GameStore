using GameStore.Application.Models;

namespace GameStore.Application.Services;

public interface IPlatformService
{
    Task<PlatformDto> AddAsync(PlatformDto dto);
    
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<PlatformDto>> GetAll();
    
    Task<PlatformDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<GameDto>> GetGamesAsync(Guid platformId);
    
    Task<PlatformDto> UpdateAsync(PlatformDto dto);
}