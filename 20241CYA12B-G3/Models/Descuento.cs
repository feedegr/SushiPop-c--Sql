using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Descuento
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 7, ErrorMessage = "Ingresa un número válido entre 1 y 7")]
        public int Dia { get; set;}
        [Required]
        [DefaultValue(0)]
        public int Porcentaje {  get; set;}
        [DefaultValue(1000)]
        public decimal DescuentoMaximo { get; set;}
        [Required]
        [DefaultValue(true)]
        public bool? Activo { get; set;}
        
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }
    }
}
