using System;
using System.Windows;

namespace Inventario_Hardware_IT
{
    public partial class GestionMarcas : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionMarcas()
        {
            InitializeComponent();
            CargarMarcas();
        }

        private void CargarMarcas()
        {
            try
            {
                // NOTA: Si en tu base de datos la tabla se llama diferente, avísame.
                // Asumo que la tabla es 'Marcas'.
                string consulta = "SELECT * FROM Marcas";
                gridDatos.ItemsSource = db.LeerDatos(consulta).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar marcas: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreMarca.Text))
            {
                MessageBox.Show("Escribe un nombre para la marca.");
                return;
            }

            try
            {
                string nombre = txtNombreMarca.Text;
                
                // --- CORRECCIÓN IMPORTANTE AQUÍ ---
                // Usamos 'NombreMarca' porque así se llama en TU base de datos real
                string consulta = $"INSERT INTO Marcas (NombreMarca) VALUES ('{nombre}')";
                
                db.EjecutarComando(consulta);
                
                MessageBox.Show("Guardado con éxito.");
                txtNombreMarca.Clear();
                CargarMarcas(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}