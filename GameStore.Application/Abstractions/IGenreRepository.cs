using GameStore.Domain.Entities;

namespace GameStore.Application.Abstractions;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();

    Task<Genre?> GetByIdAsync(Guid id);

    Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentGenreId);

    Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids);

    Task<IEnumerable<Game>> GetGamesAsync(Guid genreId);

    Task<Genre> AddAsync(Genre genre);

    Task<Genre> UpdateAsync(Genre genre);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsByIdAsync(Guid id);
}