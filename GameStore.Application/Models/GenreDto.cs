using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Models;

public class GenreDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public Guid? ParentGenreId { get; set; }
}
