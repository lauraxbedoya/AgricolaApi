using System.ComponentModel.DataAnnotations;

namespace AgricolaApi.Domain.Models
{
    public class User
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Length(10, 100)]
        [Required]
        public required string Email { get; set; }

        [Length(10, 30)]
        [Required]
        public required string? Password { get; set; }
    }
}