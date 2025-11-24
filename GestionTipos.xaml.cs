using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

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

        // --- FUNCIONES VISUALES ---
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // --------------------------

        private void CargarTipos()
        {
            try
            {
                DataTable dt = db.LeerDatos("SELECT * FROM TiposHardware");
                gridDatos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombreTipo.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("⚠️ Por favor, escribe un nombre.", "Faltan Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string query = $"INSERT INTO TiposHardware (NombreTipo) VALUES ('{nombre}')";
                db.EjecutarComando(query);

                MessageBox.Show("✅ Tipo registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                txtNombreTipo.Clear();
                CargarTipos();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }
    }
}