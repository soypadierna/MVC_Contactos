using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Contactos.Models
{
    public class TBLFACTURAS
    {
        [Key]
        public int IdFactura { get; set; }

        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime DtmFecha { get; set; }
        public int IdEstado { get; set; }

        public decimal DecSubtotal { get; set; }
        public decimal DecDescuento { get; set; }
        public decimal DecIva { get; set; }
        public decimal DecTotal { get; set; }

        public DateTime? DtmFechaModifica { get; set; }
        public string? StrUsuarioModifico { get; set; }

        public TBLEMPLEADO? Empleado { get; set; }
        public List<TBLFACTURADETALLE> Detalles { get; set; } = new();

        [NotMapped]
        public string EstadoTexto => IdEstado switch
        {
            1 => "Pendiente",
            2 => "Pagada",
            3 => "Anulada",
            4 => "Vencida",
            _ => "Desconocido"
        };

        [NotMapped]
        public string EstadoClase => IdEstado switch
        {
            1 => "warning",
            2 => "success",
            3 => "default",
            4 => "danger",
            _ => "default"
        };
    }
}