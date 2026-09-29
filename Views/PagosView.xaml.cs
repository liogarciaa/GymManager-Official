using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using GymManager.Data;
using GymManager.Models;

namespace GymManager.Views
{
    public partial class PagosView : UserControl
    {
        private bool _inicializado = false;

        public PagosView()
        {
            InitializeComponent();

            // Filtro inicial: del 1 del mes actual hasta el día de hoy
            DpDesde.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DpHasta.SelectedDate = DateTime.Today;

            _inicializado = true;
            CargarPagos();
        }

        private void Fondo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject depObj)
            {
                // Permitimos la interacción dentro del ComboBox, sus ítems y popups emergentes
                bool esControlEntrada = EsDescendienteDe<DatePicker>(depObj) || 
                                       EsDescendienteDe<ComboBox>(depObj) || 
                                       EsDescendienteDe<ComboBoxItem>(depObj) ||
                                       EsDescendienteDe<Popup>(depObj) ||
                                       EsDescendienteDe<Calendar>(depObj) ||
                                       EsDescendienteDe<Button>(depObj);

                if (!esControlEntrada)
                {
                    Keyboard.ClearFocus();
                }
            }
        }

        private static bool EsDescendienteDe<T>(DependencyObject? elemento) where T : DependencyObject
        {
            DependencyObject? actual = elemento;
            while (actual != null)
            {
                if (actual is T) return true;
                actual = VisualTreeHelper.GetParent(actual);
            }
            return false;
        }

        private void CargarPagos()
        {
            if (!_inicializado) return;

            DateTime? desde = DpDesde.SelectedDate;
            // Para que 'Hasta' incluya todos los cobros del último día hasta las 23:59:59
            DateTime? hasta = DpHasta.SelectedDate?.Date.AddDays(1).AddTicks(-1);

            // Obtenemos el texto limpio seleccionado en el ComboBox
            string metodoSeleccionado = "Todos";
            if (CmbMetodoFiltro.SelectedItem is ComboBoxItem item && item.Content != null)
            {
                metodoSeleccionado = item.Content.ToString()!.Trim();
            }

            using (var db = new GymDbContext())
            {
                var query = db.Pagos.Include(p => p.Socio).AsQueryable();

                // Filtro de fechas
                if (desde.HasValue)
                {
                    query = query.Where(p => p.FechaPago >= desde.Value);
                }

                if (hasta.HasValue)
                {
                    query = query.Where(p => p.FechaPago <= hasta.Value);
                }

                // Filtro por Método de Cobro
                if (!string.Equals(metodoSeleccionado, "Todos", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(p => p.FormaDePago != null && 
                                            EF.Functions.Like(p.FormaDePago.Trim(), metodoSeleccionado));
                }

                var lista = query.OrderByDescending(p => p.FechaPago).ToList();
                GridPagos.ItemsSource = lista;

                // Sumamos la recaudación total del conjunto filtrado
                decimal total = lista.Sum(p => p.Monto);
                TxtTotalRecaudado.Text = total.ToString("C2");
            }
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            CargarPagos();
        }

        private void BtnHoy_Click(object sender, RoutedEventArgs e)
        {
            DpDesde.SelectedDate = DateTime.Today;
            DpHasta.SelectedDate = DateTime.Today;
        }

        private void BtnEsteMes_Click(object sender, RoutedEventArgs e)
        {
            DpDesde.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DpHasta.SelectedDate = DateTime.Today;
        }
    }
}