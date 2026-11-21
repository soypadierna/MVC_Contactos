using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class TBLFACTURADETALLE
    {
        [Key]
        public int IdDetalle { get; set; }

        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public int IntCantidad { get; set; }
        public decimal DecPrecioUnitario { get; set; }
        public decimal DecSubtotal { get; set; }

        public TBLFACTURAS? Factura { get; set; }
        public TBLPRODUCTOS? Producto { get; set; }
    }
}