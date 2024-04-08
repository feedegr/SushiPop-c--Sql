namespace _20241CYA12B_G3.Models
{
    public class Descuento
    {
        public int Id { get; set; }

        public DateTime Dia { get; set;}

        public decimal Porcentaje {  get; set;}
        
        public decimal DescuentoMaximo { get; set;}

        public bool Activo { get; set;}
        
        public int ProductoId { get; set; }
    }
}
