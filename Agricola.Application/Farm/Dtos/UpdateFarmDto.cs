using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AgricolaApi.Application;

public class UpdateFarmDto
{
    public string? Name { get; set; }

    public string? Location { get; set; }

    public int? Hectares { get; set; }

    public string? Description { get; set; }
}
