using System.ComponentModel.DataAnnotations;

namespace MVC_Contactos.Models
{
    public class TBLPRODUCTOS
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
        [Display(Name = "Nombre")]
        public string StrNombre { get; set; } = "";

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        [Display(Name = "Código")]
        public string StrCodigo { get; set; } = "";

        [Range(0.01, 999999999.99, ErrorMessage = "El precio de compra debe ser mayor a 0.")]
        [Display(Name = "Precio compra")]
        public decimal DecPrecioCompra { get; set; }

        [Range(0.01, 999999999.99, ErrorMessage = "El precio de venta debe ser mayor a 0.")]
        [Display(Name = "Precio venta")]
        public decimal DecPrecioVenta { get; set; }

        [Required(ErrorMessage = "Seleccione una categoría.")]
        [Display(Name = "Categoría")]
        public int? IdCategoria { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        [Display(Name = "Detalle")]
        public string? StrDetalle { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un entero mayor o igual a 0.")]
        [Display(Name = "Stock")]
        public int IntStock { get; set; }

        public string? StrFoto { get; set; }

        public DateTime? DtmFechaModifica { get; set; }
        public string? StrUsuarioModifico { get; set; }

        public TBLCATEGORIAS? Categoria { get; set; }
    }
}