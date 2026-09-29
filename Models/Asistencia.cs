using System;

namespace GymManager.Models
{
    public class Asistencia
    {
        public int Id { get; set; }

        public int SocioId { get; set; }
        public Socio Socio { get; set; } = null!;

        public DateTime FechaHora { get; set; } = DateTime.Now;
    }
}