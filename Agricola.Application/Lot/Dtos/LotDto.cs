using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Application;

public class LotDto
{
    [Required]
    public int FarmId { get; set; }

    [Length(5, 100)]
    [Required]
    public required string Name { get; set; }

    [Required]
    public required int Trees { get; set; }

    [Length(0, 100)]
    [Required]
    public required string Stage { get; set; }
}
