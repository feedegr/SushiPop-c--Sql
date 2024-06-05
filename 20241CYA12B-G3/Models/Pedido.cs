using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Display(Name = "Número de pedido")]
        [Required]
        [DefaultValue(30000)]
        public int NroPedido { get; set; }

        [Display(Name = "Fecha")]
        [Required]
        //  public DateTime? FechaCompra {  get; set; } = DateTime.Now 
        // es correcto settearle el valor .Now desde aca? le sacariamos el nulleable?
        public DateTime? FechaCompra { get; set; }

        [Display(Name = "Subtotal")]
        [Required]
        public decimal Subtotal { get; set; }

        [Display(Name = "Envío")]
        [Required]
        [DefaultValue(80)]
        //Estándar: $ 80. Con lluvia o >5°C: $ 120 no sabemos de don
        public decimal GastoEnvio { get; set; }

        [Display(Name = "Total")]
        [Required]
        public decimal Total { get; set; }

        [Display(Name = "Estado")]
        [Required]
        [Range(1, 6, ErrorMessage = "Ingrese un valor entre 1 y 6")]
        [DefaultValue(1)]
        public int Estado { get; set; }

        [Required]
        public int CarritoId { get; set; }

        public Carrito? Carrito { get; set; }

        public virtual Reclamo? Reclamo { get; set; }

    }
}
