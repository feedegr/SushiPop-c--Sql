namespace _20241CYA12B_G3.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set;}

        public  ICollection<Producto> Productos { get; set; }

    }
}
