using GameStore.Application.Extensions;
using GameStore.Application.Factories;
using GameStore.Application.Handlers;
using GameStore.Application.Abstractions;
using GameStore.Application.Models;
using GameStore.Domain.Entities;
using GameStore.Domain.Exceptions.DatabaseExceptions;

namespace GameStore.Application.Services;

public class PlatformService : IPlatformService
{
    private readonly IPlatformRepository platformRepository;

    public PlatformService(IPlatformRepository platformRepository)
    {
        this.platformRepository = platformRepository;
    }

    public async Task<IEnumerable<PlatformDto>> GetAll()
    {
        var list = await platformRepository.GetAllAsync();
        return list.Select(platform => platform.MapToDto());
    }

    public async Task<PlatformDto> GetByIdAsync(Guid id)
    {
        var platform = await GetPlatformOrThrow(id);
        return platform.MapToDto();
    }

    public async Task<IEnumerable<GameDto>> GetGamesAsync(Guid platformId)
    {
        await GetPlatformOrThrow(platformId);
        var games = await platformRepository.GetGamesAsync(platformId);
        return games.Select(g => g.MapToDto());
    }

    public async Task<PlatformDto> AddAsync(PlatformDto dto)
    {
        return await TryExecuteAndMapAsync(() => platformRepository.AddAsync(PlatformFactory.Create(dto.Type)), dto);
    }

    public async Task<PlatformDto> UpdateAsync(PlatformDto dto)
    {
        return await TryExecuteAndMapAsync(() => platformRepository.UpdateAsync(dto.MapToEntity()), dto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await GetPlatformOrThrow(id);
        await platformRepository.DeleteAsync(id);
    }

    private async Task<Platform> GetPlatformOrThrow(Guid id)
    {
       return await platformRepository.GetByIdAsync(id) ?? throw new ObjectNotFoundException(nameof(Platform), id);
    }

    private static async Task<PlatformDto> TryExecuteAndMapAsync(Func<Task<Platform>> action, PlatformDto dto)
    {
        var platform = await HandleUniqueConstraintAsync(action, dto.Type);
        return platform.MapToDto();
    }

    private static async Task<Platform> HandleUniqueConstraintAsync(Func<Task<Platform>> action, string value)
    {
        return await UniqueConstraintHandler.HandleAsync(action, nameof(Platform.Type), value);
    }
}
