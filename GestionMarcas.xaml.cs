using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

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

        private void CargarMarcas()
        {
            try
            {
                string consulta = "SELECT * FROM Marcas";
                DataTable dt = db.LeerDatos(consulta);
                gridDatos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombreMarca.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("⚠️ Escribe un nombre.", "Faltan Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string consulta = $"INSERT INTO Marcas (NombreMarca) VALUES ('{nombre}')";
                db.EjecutarComando(consulta);

                MessageBox.Show("✅ Marca registrada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                txtNombreMarca.Clear();
                CargarMarcas();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }
    }
}