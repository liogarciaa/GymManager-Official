using System.Windows;
using System.Windows.Input;

namespace GymManager.Views
{
    public partial class PasswordPromptDialog : Window
    {
        private readonly string _claveCorrecta;
        public bool Autenticado { get; private set; } = false;

        public PasswordPromptDialog(string claveCorrecta)
        {
            InitializeComponent();
            _claveCorrecta = claveCorrecta;
            Loaded += (s, e) => TxtPasswordInput.Focus();
        }

        private void ValidarClave()
        {
            if (TxtPasswordInput.Password == _claveCorrecta)
            {
                Autenticado = true;
                DialogResult = true;
                Close();
            }
            else
            {
                TxtError.Text = "Contraseña incorrecta.";
                TxtError.Visibility = Visibility.Visible;
                TxtPasswordInput.SelectAll();
                TxtPasswordInput.Focus();
            }
        }

        private void BtnAcceder_Click(object sender, RoutedEventArgs e) => ValidarClave();

        private void TxtPasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidarClave();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}