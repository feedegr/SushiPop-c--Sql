using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public string Local { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }
        
        [Required]
        public bool? Confirmada { get; set;}

        
        public string Nombre { get; set; }
       
        
        public string Apellido { get; set; }

        [Required]
        public int ClienteId { get; set; }

        
        public Cliente? Cliente { get; set; }
    }
}
