using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class ModeloPersona
    {
        [Key]
        public int PersonaId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        [Phone(ErrorMessage = "El teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [StringLength(150)]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = string.Empty;
    }
}