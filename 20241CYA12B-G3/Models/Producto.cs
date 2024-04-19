using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage ="Nombre debe tener maximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [MinLength(20, ErrorMessage = "Descripcion debe tener minimo 20 caracteres")]
        [MaxLength(250, ErrorMessage = "Descripcion debe tener maximo 250 caracteres")]
        public string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [DefaultValue("https://onedrive.live.com/?authkey=%21AKHpa5tj0ZHxwc0&id=6341A9F77420C628%21178983&cid=6341A9F77420C628&parId=root&parQt=sharedby&o=OneUp")]
        public string Foto { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public decimal Costo { get; set; }

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public ICollection<Descuento>? Descuentos { get; set; } 
    
        public ICollection<CarritoItem>? CarritoItems { get; set; }
    }
}
