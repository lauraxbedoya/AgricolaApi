using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AgricolaApi.Application;

public class UpdateUserDto
{
    [StringLength(40)]
    public string? Name { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; } = null!;
}
