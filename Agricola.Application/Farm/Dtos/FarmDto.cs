using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Application;

public class FarmDto
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [StringLength(200)]
    public required string Location { get; set; }

    [Required]
    public required int Hectares { get; set; }

    [Required]
    [StringLength(500)]
    public required string Description { get; set; }
}
