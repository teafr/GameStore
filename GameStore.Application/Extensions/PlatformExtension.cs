using GameStore.Application.Models;
using GameStore.Domain.Entities;

namespace GameStore.Application.Extensions;

public static class PlatformExtension
{
    public static PlatformDto MapToDto(this Platform platform)
    {
        return new PlatformDto
        {
            Id = platform.Id,
            Type = platform.Type
        };
    }

    public static Platform MapToEntity(this PlatformDto platformDto)
    {
        return new Platform
        {
            Id = platformDto.Id,
            Type = platformDto.Type
        };
    }
}
