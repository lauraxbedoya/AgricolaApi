using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Domain.Models
{
    public class Lot
    {
        [Required]
        [Key]
        public int Id { get; set; }

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

        public Farm Farm { get; set; } = null!;

        public IList<Group> Groups { get; } = new List<Group>();
    }
}