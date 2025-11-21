using System.Windows;

namespace Inventario_Hardware_IT
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PRUEBA(object sender, RoutedEventArgs e)
        {

        }

        private void PRUEBA_CLICK(object sender, RoutedEventArgs e)
        {
            ConexionDB db = new ConexionDB();

            if (db.ProbarConexion())
            {
                MessageBox.Show("¡Conexión Exitosa! Sistema listo.");
            }
            else
            {
                MessageBox.Show("Error al conectar. Verifica el nombre de la base de datos en App.config");
            }
        }

        //Console.WriteLine("Hello, C# Academyl! l");
    }
}
