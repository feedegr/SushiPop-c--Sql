﻿namespace _20241CYA12B_G3.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Direccion { get; set;}

        public string Telefono { get; set;}

        public DateTime FechaNacimiento { get; set;}

        public DateTime? FechaAlta { get; set;}

        public bool? Activo { get; set;}

        public string Email { get; set;}


    }
}
