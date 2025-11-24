using System;
using System.Windows;
using System.Windows.Input; // <--- IMPORTANTE: Necesario para mover la ventana
using System.Data;
//using Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class Login : Window
    {
        ConexionDB db = new ConexionDB();

        public Login()
        {
            InitializeComponent();
        }

        // --- ESTA ES LA FUNCIÓN QUE TE FALTABA ---
        // Permite arrastrar la ventana aunque no tenga barra de título
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        // ------------------------------------------

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUser.Text;
            string pass = txtPass.Password;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("⚠️ Ingresa tus credenciales.", "Datos Incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Busca usuario y contraseña (usando 'Contrasena' como en tu base de datos)
                string query = $"SELECT RolID FROM Usuarios WHERE NombreUsuario = '{user}' AND Contrasena = '{pass}'";
                DataTable dt = db.LeerDatos(query);

                if (dt.Rows.Count > 0)
                {
                    int rolId = Convert.ToInt32(dt.Rows[0]["RolID"]);

                    // Abre el Menú Principal
                    MainWindow menu = new MainWindow(rolId);
                    menu.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPass.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("🚨 Error de conexión: " + ex.Message);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}