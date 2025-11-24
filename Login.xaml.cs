using System;
using System.Windows;
using System.Data;


namespace Inventario_Hardware_IT
{
    public partial class Login : Window
    {
        ConexionDB db = new ConexionDB();

        public Login()
        {
            InitializeComponent();
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUser.Text;
            string pass = txtPass.Password;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Ingresa usuario y contraseña.");
                return;
            }

            try
            {
                // Buscamos el usuario y su rol
                string query = $"SELECT RolID FROM Usuarios WHERE NombreUsuario = '{user}' AND Password = '{pass}'";
                DataTable dt = db.LeerDatos(query);

                if (dt.Rows.Count > 0)
                {
                    // ¡Login Exitoso!
                    int rolId = Convert.ToInt32(dt.Rows[0]["RolID"]);

                    // Abrimos el Menú Principal y le pasamos el ROL
                    MainWindow menu = new MainWindow(rolId);
                    menu.Show();
                    this.Close(); // Cerramos el login
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}