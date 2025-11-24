using System;
using System.Windows;
using System.Data;


namespace Inventario_Hardware_IT
{
    public partial class GestionTipos : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionTipos()
        {
            InitializeComponent();
            CargarTipos();
        }

        private void CargarTipos()
        {
            try
            {
                // Consultamos la tabla de Tipos
                DataTable dt = db.LeerDatos("SELECT * FROM TiposHardware");
                gridDatos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombreTipo.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Escribe un nombre (Ej: Laptop, Monitor).");
                return;
            }

            try
            {
                // Guardamos en la tabla TiposHardware
                // CÓDIGO CORREGIDO
                string query = $"INSERT INTO TiposHardware (NombreTipo) VALUES ('{nombre}')";
                db.EjecutarComando(query);

                MessageBox.Show("Tipo guardado con éxito.");
                txtNombreTipo.Clear();
                CargarTipos(); // Recargar la tabla para ver el nuevo
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
    }
}