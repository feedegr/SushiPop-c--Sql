﻿namespace _20241CYA12B_G3.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Description { get; set; }

        public decimal Precio { get; set; }

        public string Foto { get; set; }

        public int Stock { get; set; }

        public decimal Costo { get; set; }

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public ICollection<Descuento> Descuentos { get; set; } // era una coleccion
    
        public ICollection<CarritoItem> CarritoItems { get; set; }
    }
}