using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string StrNombre { get; set; } = string.Empty;

        [Display(Name = "Documento")]
        public long? NumDocumento { get; set; }

        [Display(Name = "Dirección")]
        public string? StrDireccion { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string? StrTelefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string? StrEmail { get; set; }

        [Display(Name = "Fecha modificación")]
        public DateTime? DtmFechaModifica { get; set; }

        [Display(Name = "Usuario modificación")]
        public string? StrUsuarioModifica { get; set; }
    }
}