using System.Windows;

namespace Inventario_Hardware_IT
{
    public partial class MainWindow : Window
    {
        int _rolUsuario; // Variable para guardar si es Admin (1) o Trabajador (2)

        // Constructor modificado que recibe el ID
        public MainWindow(int rolId)
        {
            InitializeComponent();
            _rolUsuario = rolId;
            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            if (_rolUsuario == 2)
            {
                Title = "Sistema de Inventario IT - Modo Trabajador (Acceso Limitado)";
            }
            else
            {
                Title = "Sistema de Inventario IT - Modo Administrador (Control Total)";
            }
        }

        private void BtnHardware_Click(object sender, RoutedEventArgs e)
        {
            // Pasamos el rol a la ventana de Hardware para que sepa si activar el botón borrar
            GestionHardware ventana = new GestionHardware(_rolUsuario);
            ventana.ShowDialog();
        }

        // El resto de ventanas no necesitan rol por ahora
        private void BtnModelos_Click(object sender, RoutedEventArgs e) { new GestionModelos().ShowDialog(); }
        private void BtnMarcas_Click(object sender, RoutedEventArgs e) { new GestionMarcas().ShowDialog(); }
        private void BtnTipos_Click(object sender, RoutedEventArgs e) { new GestionTipos().ShowDialog(); }

        private void BtnSalir_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
    }
}