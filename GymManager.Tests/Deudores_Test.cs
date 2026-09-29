using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GymManager.Models;

namespace GymManager.Tests
{
    public class MorosidadTests
    {
        [Fact]
        public void Socio_ConCuotaExpirada_DebeAparecerComoVencido()
        {
            DateTime hoy = DateTime.Today;
            var socio = new Socio
            {
                Id = 10,
                Nombre = "Carlos",
                Apellido = "Prueba",
                EstaActivo = true,
                Eliminado = false,
                FechaVencimiento = hoy.AddDays(-10) // Venció hace 10 días
            };

            string estado = socio.EstadoCuota;
            int diasVencido = (hoy - socio.FechaVencimiento.Value.Date).Days;

            Assert.Equal("Vencido", estado);
            Assert.Equal(10, diasVencido);
        }

        [Fact]
        public void Socio_ConCuotaVigente_NoDebeEstarVencido()
        {
            DateTime hoy = DateTime.Today;
            var socio = new Socio
            {
                Id = 11,
                Nombre = "Mariana",
                Apellido = "Gómez",
                EstaActivo = true,
                Eliminado = false,
                FechaVencimiento = hoy.AddDays(20)
            };

            Assert.Equal("Al día", socio.EstadoCuota);
        }

        [Fact]
        public void FiltroDeudores_DebeExcluirSociosAlDia_Y_ListarSoloMorosos()
        {
            DateTime hoy = DateTime.Today;

            var listaSimulada = new List<Socio>
            {
                new Socio { Id = 1, Nombre = "Socio Al Día", EstaActivo = true, FechaVencimiento = hoy.AddDays(15) },
                new Socio { Id = 2, Nombre = "Socio Vencido Ayer", EstaActivo = true, FechaVencimiento = hoy.AddDays(-1) },
                new Socio { Id = 3, Nombre = "Socio Sin Pagos", EstaActivo = true, FechaVencimiento = null },
                new Socio { Id = 4, Nombre = "Socio Inactivo", EstaActivo = false, FechaVencimiento = hoy.AddDays(-30) }
            };

            var deudores = listaSimulada
                .Where(s => s.EstaActivo && (!s.FechaVencimiento.HasValue || s.FechaVencimiento.Value.Date < hoy))
                .ToList();

            Assert.Equal(2, deudores.Count);
            Assert.DoesNotContain(deudores, s => s.Id == 1);
            Assert.DoesNotContain(deudores, s => s.Id == 4);
        }
    }
}