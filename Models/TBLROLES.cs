using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class TBLROLES
    {
        [Key]
        public int IdRolEmpleado { get; set; }
        public string? StrDescripcion { get; set; }
    }
}