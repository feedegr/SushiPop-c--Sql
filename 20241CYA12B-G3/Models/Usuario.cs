using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public abstract class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Nombre debe tener mas de 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Nombre debe tener menos de 30 caracteres")]

        public string Nombre { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Apellido debe tener mas de 5 caracteres")]
        [MaxLength(30, ErrorMessage = "Apellido debe tener menos de 30 caracteres")]
        public string Apellido { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Nombre debe tener menos de 100 caracteres")]
        public string Direccion { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Telefono debe tener mas de 10 caracteres")]
        [MaxLength(10, ErrorMessage = "Telefono debe tener menos de 10 caracteres")]
        public string Telefono { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }
       
        public DateTime FechaAlta { get; set;}
        public bool? Activo { get; set;}
        
        public string? Email { get; set;}


    }
}
