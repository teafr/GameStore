using GameStore.Domain.Entities;

namespace GameStore.Application.Factories;

public static class PlatformFactory
{
    public static Platform Create(string type)
    {
        return new Platform
        {
            Id = Guid.NewGuid(),
            Type = type
        };
    }
}
