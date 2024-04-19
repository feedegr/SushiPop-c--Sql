using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Reclamo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Debe tener maximo 255 caracteres")]
        public string NombreCompleto { get; set; }

        [Required]
        // Debemos añadir una validacion para el tipo de Dato?
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 50)]
        public string DetalleReclamo { get; set; }

        [Required]
        // Debemos añadir una validacion para el tipo de Dato?
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
