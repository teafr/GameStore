using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameStore.Domain.Entities;

[Index(nameof(Type), IsUnique = true)]
public class Platform
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Type { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
