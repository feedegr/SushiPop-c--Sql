namespace _20241CYA12B_G3.Models
{
    public class DetallePedidoViewModel
    {
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GastoEnvio { get; set;}
        public decimal Total { get; set; }
        public List<string> Productos { get; set; }
    }
}

    