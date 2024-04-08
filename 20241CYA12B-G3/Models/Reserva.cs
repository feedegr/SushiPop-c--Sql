namespace _20241CYA12B_G3.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public DateTime FechaHora { get; set; }

        public bool Confirmada { get; set;}

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int ClienteId { get; set; }
    }
}
