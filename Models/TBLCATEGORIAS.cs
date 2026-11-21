using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class TBLCATEGORIAS
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        [Display(Name = "Descripción")]
        public string StrDescripcion { get; set; } = "";

        public DateTime? DtmFechaModifica { get; set; }
        public string? StrUsuarioModifico { get; set; }
    }
}