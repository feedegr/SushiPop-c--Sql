using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }
        [Required]
        public decimal PrecioUnitarioConDescuento { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int CarritoId { get; set; }
        [Required]
        public Carrito? Carrito { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
