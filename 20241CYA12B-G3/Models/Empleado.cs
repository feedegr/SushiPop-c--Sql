using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _20241CYA12B_G3.Models
{
    public class Empleado : Usuario
    {
        [Required]
        [DefaultValue(99000)]
        public int? Legajo { get; set; }

       
    }
}
