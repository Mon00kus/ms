using System.ComponentModel.DataAnnotations;

namespace ms.Backend.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Firtname { get; set; } = "";
        [Required]
        [StringLength(255)]
        public string Lastname { get; set; } = "";
        [Required]
        [StringLength(50)]
        public string Documnet_type { get; set; } = "";
        [Required]
        [StringLength(100)]
        public string Documnet { get; set; } = "";
        [Required]
        [StringLength(16)]
        public string Serial_tullave { get; set; } = "";
        public decimal Saldo { get; set; }
    }
}
