﻿namespace _20241CYA12B_G3.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public bool? Procesado { get; set; }
        public bool? Cancelado { get; set; }
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public virtual Pedido? Pedido { get; set; }

        public List<CarritoItem>? CarritoItems { get; set; }
    }
}
