using System;
using Xunit;
using GymManager.Services;

namespace GymManager.Tests
{
    public class AccesoTolerancia_Test
    {
        private readonly DateTime _hoy = new DateTime(2026, 9, 28);

        [Fact]
        public void SocioConCuotaVigente_DebeTenerAccesoPermitido()
        {
            // Vence dentro de 3 días
            DateTime vencimiento = _hoy.AddDays(3);
            int diasTolerancia = 5;

            var resultado = ValidadorAcceso.Evaluar(vencimiento, diasTolerancia, _hoy);

            Assert.Equal(EstadoAcceso.Permitido, resultado);
        }

        [Fact]
        public void SocioVencidoHaceDosDias_ConToleranciaDeCinco_DebeTenerAccesoEnTolerancia()
        {
            // Venció hace 2 días (el 26/09). Con 5 días de tolerancia, está dentro del margen de gracia
            DateTime vencimiento = _hoy.AddDays(-2);
            int diasTolerancia = 5;

            var resultado = ValidadorAcceso.Evaluar(vencimiento, diasTolerancia, _hoy);

            Assert.Equal(EstadoAcceso.Tolerancia, resultado);
        }

        [Fact]
        public void SocioVencidoHaceSeisDias_ConToleranciaDeCinco_DebeSerRechazado()
        {
            // Venció hace 6 días (el 22/09). Superó los 5 días de tolerancia
            DateTime vencimiento = _hoy.AddDays(-6);
            int diasTolerancia = 5;

            var resultado = ValidadorAcceso.Evaluar(vencimiento, diasTolerancia, _hoy);

            Assert.Equal(EstadoAcceso.RechazadoVencido, resultado);
        }

        [Fact]
        public void SocioVencidoAyer_ConToleranciaCero_DebeSerRechazado()
        {
            // Venció ayer. Con tolerancia 0, debe bloquear el acceso de inmediato
            DateTime vencimiento = _hoy.AddDays(-1);
            int diasTolerancia = 0;

            var resultado = ValidadorAcceso.Evaluar(vencimiento, diasTolerancia, _hoy);

            Assert.Equal(EstadoAcceso.RechazadoVencido, resultado);
        }
    }
}