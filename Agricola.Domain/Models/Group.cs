using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Domain.Models
{
    public class Group
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public required int LotId { get; set; }

        [Length(5, 100)]
        [Required]
        public required string Name { get; set; }

        public Lot Lot { get; set; } = null!;
    }
}