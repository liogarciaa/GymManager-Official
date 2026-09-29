using System;
using System.Windows;
using System.Windows.Controls;
using GymManager.Models;
using GymManager.Data;
namespace GymManager.Views
{
    public partial class RegistrarPagoWindow : Window
    {
    private readonly Socio _socio;
    private DateTime _fechaInicioCalculada;
    private DateTime _fechaVencimientoCalculada;

        public Pago? PagoRealizado { get; private set; }

        public RegistrarPagoWindow(Socio socio)
        {
            InitializeComponent();
            _socio = socio;

            TxtTituloSocio.Text = $"Socio: {_socio.NombreCompleto}";
            CalcularFechaVencimiento(30);
            CargarPrecioCuotaConfigurado();
        }

        private void CargarPrecioCuotaConfigurado()
        {
            try
            {
                using (var db = new GymDbContext())
                {
                    var config = db.Configuraciones.FirstOrDefault();
                    if (config != null)
                    {
                        TxtMonto.Text = config.PrecioCuotaMensual.ToString("0.##");
                    }
                }
            }
            catch
            {
                // Si ocurre algún problema de lectura, no interrumpe la carga de la ventana
            }
        }

private void CalcularFechaVencimiento(int dias)
        {
            if (TxtNuevoVencimiento == null) return;

            // Si tiene cuota vigente, arranca en la misma fecha de corte. Si no, arranca hoy.
            _fechaInicioCalculada = (_socio.FechaVencimiento.HasValue && _socio.FechaVencimiento.Value.Date >= DateTime.Today)
                ? _socio.FechaVencimiento.Value.Date
                : DateTime.Today;

            _fechaVencimientoCalculada = _fechaInicioCalculada.AddDays(dias);
            TxtNuevoVencimiento.Text = _fechaVencimientoCalculada.ToString("dd/MM/yyyy");
        }

        private void CmbDuracion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDuracion?.SelectedItem is ComboBoxItem item && int.TryParse(item.Tag?.ToString(), out int dias))
            {
                CalcularFechaVencimiento(dias);
            }
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validar que sea un número válido y no negativo
            if (!decimal.TryParse(TxtMonto.Text.Trim(), out decimal monto) || monto < 0)
            {
                MessageBox.Show("El monto no puede ser negativo.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Si el monto es $0, validar confirmación
            if (monto == 0)
            {
                var confirmacion = MessageBox.Show(
                    "Está registrando un período bonificado ($0).\n¿Desea continuar y extender el vencimiento del socio?",
                    "Confirmar Bonificación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirmacion != MessageBoxResult.Yes)
                    return;
            }

            string formaPago = ((ComboBoxItem)CmbFormaPago.SelectedItem).Content.ToString() ?? "Efectivo";

            PagoRealizado = new Pago
            {
                SocioId = _socio.Id,
                Monto = monto,
                FechaPago = DateTime.Now,
                FechaInicio = _fechaInicioCalculada,
                FechaVencimiento = _fechaVencimientoCalculada,
                FormaDePago = formaPago
            };

            DialogResult = true;
            Close();
        }
    }
}