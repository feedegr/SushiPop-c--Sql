using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "El maximo de longitud debe ser 255")]
        public string NombreCompleto { get; set; }

        [Required]
        // [DataType(DataType.Text, ErrorMessage = "Unicamente Texto")]
        // Debemos añadir una validacion para el tipo de Dato?
        public string Email { get; set; }

        // [DataType(DataType.Text, ErrorMessage = "Unicamente Texto")]
        [StringLength(10, MinimumLength = 10)]
        public string Telefono { get; set; }

        [Required]
        [MaxLength(int.MaxValue)]
        public string Mensaje { get; set; }

        [Required]
        // [DefaultValue(false)] Settear valor por default utilizando DataAnnotations.
        // Otra variante es: public bool Leido { get; set;} = true / false.
        public bool Leido { get; set; }
    }
}
