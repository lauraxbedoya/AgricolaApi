using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Domain.Models
{
    public class Farm
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Length(5, 100)]
        [Required]
        public required string Name { get; set; }

        [Length(10, 200)]
        [Required]
        public required string Location { get; set; }

        [Required]
        public required int Hectares { get; set; }

        [Length(0, 500)]
        [Required]
        public required string Description { get; set; }

        public IList<Lot> Lots { get; } = new List<Lot>();
    }
}