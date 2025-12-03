using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Models;

public class GameDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Key { get; set; } = string.Empty;

    [StringLength(5000)]
    public string? Description { get; set; }

    [Required]
    [MinLength(1)]
    public List<Guid> GenreIds { get; set; } = new List<Guid>();

    [Required]
    [MinLength(1)]
    public List<Guid> PlatformIds { get; set; } = new List<Guid>();
}
