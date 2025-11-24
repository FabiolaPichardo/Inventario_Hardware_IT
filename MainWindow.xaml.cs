using System.Windows;

namespace Inventario_Hardware_IT
{
    public partial class MainWindow : Window
    {
        int _rolUsuario;

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
                txtRolUsuario.Text = "Colaborador";
            }
            else
            {
                txtRolUsuario.Text = "Administrador IT";
            }
        }

        private void BtnHardware_Click(object sender, RoutedEventArgs e)
        {
            new GestionHardware(_rolUsuario).ShowDialog();
        }

        private void BtnAsignaciones_Click(object sender, RoutedEventArgs e)
        {
            new GestionAsignaciones().ShowDialog();
        }

        private void BtnManto_Click(object sender, RoutedEventArgs e)
        {
            new GestionMantenimiento().ShowDialog();
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            new GestionEmpleados().ShowDialog();
        }

        // Aquí está la función que faltaba
        private void BtnUbicaciones_Click(object sender, RoutedEventArgs e)
        {
            new GestionUbicaciones().ShowDialog();
        }

        private void BtnModelos_Click(object sender, RoutedEventArgs e)
        {
            new GestionModelos().ShowDialog();
        }

        private void BtnMarcas_Click(object sender, RoutedEventArgs e)
        {
            new GestionMarcas().ShowDialog();
        }

        private void BtnTipos_Click(object sender, RoutedEventArgs e)
        {
            new GestionTipos().ShowDialog();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Seguro que deseas cerrar sesión?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }
    }
}