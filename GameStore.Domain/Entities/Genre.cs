using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameStore.Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Genre
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public Guid? ParentGenreId { get; set; }

    public Genre? ParentGenre { get; set; }

    public ICollection<Genre> SubGenres { get; set; } = new List<Genre>();

    [JsonIgnore]
    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
}
