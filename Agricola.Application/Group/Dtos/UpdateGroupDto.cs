using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AgricolaApi.Application;

public class UpdateGroupDto
{
    public required string Name { get; set; }
}
