using System.ComponentModel.DataAnnotations;

namespace GameStore.Application.Models;

public class PlatformDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Type { get; set; } = string.Empty;
}