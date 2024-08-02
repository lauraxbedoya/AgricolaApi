using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Application;

public class GroupDto
{
    [Required]
    public int LotId { get; set; }

    [Length(5, 100)]
    [Required]
    public required string Name { get; set; }
}
