using System;
using System.Collections.Generic;

namespace GymManager.Models
{
    public class ReporteMensualDto
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public decimal TotalRecaudado { get; set; }
        public int CantidadPagosTotal { get; set; }
        public int CantidadBonificaciones { get; set; }
        public int NuevosSocios { get; set; }
        public int TotalAsistencias { get; set; }
        public int TotalDeudoresActivos { get; set; }
        public List<ResumenMetodoPagoDto> DesgloseMetodos { get; set; } = new();
    }

    public class ResumenMetodoPagoDto
    {
        public string Metodo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}