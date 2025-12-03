using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GameStore.Domain.Entities;

[Index(nameof(GameId), nameof(GenreId), IsUnique = true)]
public class GameGenre
{
    public Guid GameId { get; set; }

    [JsonIgnore]
    public Game? Game { get; set; }

    public Guid GenreId { get; set; }

    [JsonIgnore]
    public Genre? Genre { get; set; }
}
