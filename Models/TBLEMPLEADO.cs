using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class TBLEMPLEADO
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Display(Name = "Nombre")]
        public string? strNombre { get; set; }

        [Display(Name = "Nro Documento")]
        public long NumDocumento { get; set; }

        [Display(Name = "Direccion")]
        public string? StrDireccion { get; set; }

        [Display(Name = "Telefono")]
        [Phone(ErrorMessage = "El teléfono no es válido.")]
        public string? StrTelefono { get; set; }

        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        public string? StrEmail { get; set; }

        [Display(Name = "Rol Empleado")]
        public int? IdRolEmpleado { get; set; }

        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtmIngreso { get; set; }

        [Display(Name = "Fecha retiro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DtmRetiro { get; set; }

        [Display(Name = "Datos adicionales")]
        public string? strDatosAdicionales { get; set; }

        public DateTime? DtmFechaModifica { get; set; }
        public string? StrUsuarioModifico { get; set; }

        public TBLROLES? TBLROLES { get; set; }
    }
}