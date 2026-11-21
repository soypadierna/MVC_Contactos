using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class FacturaNuevaVM
    {
        [Required(ErrorMessage = "Seleccione un cliente.")]
        [Display(Name = "Cliente")]
        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "Seleccione el empleado emisor.")]
        [Display(Name = "Emisor")]
        public int? IdEmpleado { get; set; }

        [Display(Name = "Estado")]
        public int IdEstado { get; set; } = 1;

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        [Display(Name = "Descuento (%)")]
        public decimal DescuentoPorcentaje { get; set; }

        public List<LineaFacturaVM> Lineas { get; set; } = new();
    }

    public class LineaFacturaVM
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}