using System;

namespace GymManager.Models
{
    public class Pago
    {
        public int Id { get; set; }

        public int SocioId { get; set; }
        public Socio Socio { get; set; } = null!;

        public DateTime FechaPago { get; set; } = DateTime.Now;
        
        // Dinero entregado efectivamente
        public decimal Monto { get; set; }
        
        // Valor total de la cuota (útil si paga una parte)
        public decimal MontoTotal { get; set; }
        
        // Saldo que queda debiendo de este pago puntual
        public decimal SaldoPendiente => MontoTotal - Monto;

        public string FormaDePago { get; set; } = "Efectivo";

        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public string? Observaciones { get; set; }
    }
}