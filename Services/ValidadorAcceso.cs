using System;

namespace GymManager.Services
{
    public enum EstadoAcceso
    {
        Permitido,
        Tolerancia,
        RechazadoVencido
    }

    public static class ValidadorAcceso
    {
        public static EstadoAcceso Evaluar(DateTime? fechaVencimiento, int diasTolerancia, DateTime fechaActual)
        {
            if (!fechaVencimiento.HasValue)
                return EstadoAcceso.RechazadoVencido;

            DateTime vencimiento = fechaVencimiento.Value.Date;
            DateTime hoy = fechaActual.Date;
            DateTime limiteConGracia = vencimiento.AddDays(diasTolerancia);

            if (hoy > limiteConGracia)
                return EstadoAcceso.RechazadoVencido;

            if (hoy > vencimiento)
                return EstadoAcceso.Tolerancia;

            return EstadoAcceso.Permitido;
        }
    }
}