using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        [Required]
        [DefaultValue(30000)]
        public int NroPedido { get; set; }
        [Required]
        //  public DateTime? FechaCompra {  get; set; } = DateTime.Now 
        // es correcto settearle el valor .Now desde aca? le sacariamos el nulleable?
        public DateTime? FechaCompra {  get; set; }
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        [DefaultValue(80)]
        //Estándar: $ 80. Con lluvia o >5°C: $ 120 no sabemos de don
        public decimal GastoEnvio { get; set;}
        [Required]
        public decimal Total { get  ; set; }
        [Required]
        [Range(1,6,ErrorMessage = "Ingrese un valor entre 1 y 6")]
        [DefaultValue(1)]
        public int Estado { get; set;}
        [Required]
        public int CarritoId { get; set; }

        public Carrito? Carrito { get; set; }

        public virtual Reclamo? Reclamo { get; set; }
    }
}
