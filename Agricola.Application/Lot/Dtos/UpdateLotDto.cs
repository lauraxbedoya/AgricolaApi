using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AgricolaApi.Application;

public class UpdateLotDto
{
    public string? Name { get; set; }
    public int Trees { get; set; }
    public string? Stage { get; set; }
}
