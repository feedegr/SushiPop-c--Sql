using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Descuento
    {
        public int Id { get; set; }

        [Display(Name = "Día")]
        [Required]
        [Range(1, 7, ErrorMessage = "Ingresa un número válido entre 1 y 7")]
        public int Dia { get; set;}

        [Display(Name = "Porcentaje")]
        [Required]
        [Range(0, 50, ErrorMessage = "El porcentaje máximo permitido es 50%")]
        [DefaultValue(0)]
        public int Porcentaje {  get; set;}

        [Display(Name = "Descuento máximo")]
        [Range(0, 3000, ErrorMessage = "El descuento máximo permitido es $3000")]
        [DefaultValue(1000)]
        public decimal DescuentoMaximo { get; set;}
         
        [Display(Name = "Activo")]
        [Required]
        [DefaultValue(true)]
        public bool Activo { get; set;}
        
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }
    }
}
