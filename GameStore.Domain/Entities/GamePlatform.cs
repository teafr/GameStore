using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GameStore.Domain.Entities;

[Index(nameof(GameId), nameof(PlatformId), IsUnique = true)]
public class GamePlatform
{
    public Guid GameId { get; set; }

    [JsonIgnore]
    public Game? Game { get; set; }

    public Guid PlatformId { get; set; }

    [JsonIgnore]
    public Platform? Platform { get; set; }
}
