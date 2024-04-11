using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ms.Backend.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        [MaxLength(100, ErrorMessage = "El campo {0} No puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Firtsname { get; set; } = null!;

        [Display(Name = "Apellido Completo")]
        [MaxLength(100, ErrorMessage = "El campo {0} No puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Lastname { get; set; } = null!;

        [Display(Name = "Tipo de documento")]
        [MaxLength(100, ErrorMessage = "El campo {0} No puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string DocumentType { get; set; } = null!;

        [Display(Name = "Documento")]
        [MaxLength(100, ErrorMessage = "El campo {0} No puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Document { get; set; } = null!;

        [Display(Name = "Serial de la tarjeta")]
        [MaxLength(100, ErrorMessage = "El campo {0} No puede tener mas de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Serial_tullave { get; set; } = null!;

        [Display(Name = "Saldo de la tarjeta")]        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Saldo { get; set; }
    }
}