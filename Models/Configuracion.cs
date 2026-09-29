namespace GymManager.Models
{
    public class Configuracion
    {
        public int Id { get; set; } = 1; // Registro único
        public string NombreGimnasio { get; set; } = "Gym Manager";
        public string TelefonoContacto { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal PrecioCuotaMensual { get; set; } = 25000m;
        public int DiasToleranciaMora { get; set; } = 0;
        public string MensajePieRecibo { get; set; } = "¡Gracias por entrenar con nosotros!";
        public string ClaveAdmin { get; set; } = string.Empty;
    }
}