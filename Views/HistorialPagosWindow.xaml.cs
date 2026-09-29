using System;
using System.Linq;
using System.Windows;
using GymManager.Data;
using GymManager.Models;

namespace GymManager.Views
{
    public partial class HistorialPagosWindow : Window
    {
        private readonly Socio _socio;

        public HistorialPagosWindow(Socio socio)
        {
            InitializeComponent();
            _socio = socio;
            TxtTitulo.Text = $"Historial de Pagos: {_socio.NombreCompleto}";
            CargarHistorial();
        }

        private void CargarHistorial()
        {
            using (var db = new GymDbContext())
            {
                var pagos = db.Pagos
                              .Where(p => p.SocioId == _socio.Id)
                              .OrderBy(p => p.FechaPago)
                              .ToList();

                GridPagos.ItemsSource = pagos;
            }
        }

        private void MenuEliminarPago_Click(object sender, RoutedEventArgs e)
        {
            if (GridPagos.SelectedItem is not Pago pagoSeleccionado) return;

            var confirmacion = MessageBox.Show(
                $"¿Estás seguro de que deseás anular este pago de {pagoSeleccionado.Monto:C2} registrado el {pagoSeleccionado.FechaPago:dd/MM/yyyy HH:mm}?\n\nEsta acción recalculará el vencimiento del socio.",
                "Confirmar anulación de pago",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes) return;

            using (var db = new GymDbContext())
            {
                // 1. Buscamos el pago en la base de datos
                var pagoDb = db.Pagos.Find(pagoSeleccionado.Id);
                if (pagoDb != null)
                {
                    db.Pagos.Remove(pagoDb);
                    db.SaveChanges(); // Eliminamos el pago erróneo
                }

                // 2. Buscamos al socio para recalcular su vencimiento
                var socioDb = db.Socios.Find(_socio.Id);
                if (socioDb != null)
                {
                    // Buscamos cuál es la fecha máxima de cobertura entre los pagos que le quedaron
                    var pagosRestantes = db.Pagos.Where(p => p.SocioId == _socio.Id).ToList();

                    if (pagosRestantes.Any())
                    {
                        socioDb.FechaVencimiento = pagosRestantes.Max(p => p.FechaVencimiento);
                    }
                    else
                    {
                        socioDb.FechaVencimiento = null; // No le quedan más pagos
                    }

                    db.SaveChanges();

                    // 3. Sincronizamos el objeto que está en memoria
                    _socio.FechaVencimiento = socioDb.FechaVencimiento;
                }
            }

            // Refrescamos la grilla del historial
            CargarHistorial();

            MessageBox.Show("Pago anulado con éxito y vencimiento recalculado.", "Operación completada", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}