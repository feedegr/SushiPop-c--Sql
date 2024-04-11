namespace _20241CYA12B_G3.Models
{
    public class Reclamo
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Telefono {  get; set; }
        public string DetalleReclamo { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set;}
    }
}
