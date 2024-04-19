using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Nombre de categoria debe tener un maximo de 100")]
        public string Nombre { get; set; }
        
        [MaxLength(int.MaxValue)] //consultar si se completa el ()
        public string Descripcion { get; set;}

        public ICollection<Producto>? Productos { get; set; }

    }
}
