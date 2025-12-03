using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities;

[Index(nameof(Key), IsUnique = true)]
public class Game
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Key { get; set; } = string.Empty;

    [StringLength(5000, MinimumLength = 0)]
    public string? Description { get; set; }

    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
