using GameStore.Application.Models;

namespace GameStore.Application.Services;

public interface IGenreService
{
    Task<GenreDto> Add(GenreDto dto);
    
    Task DeleteAsync(Guid id);
    
    Task<IEnumerable<GenreDto>> GetAll();
    
    Task<GenreDto> GetByIdAsync(Guid id);
    
    Task<IEnumerable<GenreDto>> GetByParentIdAsync(Guid parentId);
    
    Task<IEnumerable<GameDto>> GetGamesAsync(Guid genreId);
    
    Task<GenreDto> UpdateAsync(GenreDto dto);
}