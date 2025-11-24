using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class GestionEmpleados : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionEmpleados()
        {
            InitializeComponent();
            CargarEmpleados();
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

        private void CargarEmpleados()
        {
            try
            {
                gridEmpleados.ItemsSource = db.LeerDatos("SELECT * FROM Empleados").DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                MessageBox.Show("⚠️ Por favor, llena todos los campos.", "Faltan Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string query = $"INSERT INTO Empleados (NombreCompleto, NumeroEmpleado) VALUES ('{txtNombre.Text}', '{txtNumero.Text}')";
                db.EjecutarComando(query);

                MessageBox.Show("✅ Empleado registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                txtNombre.Clear();
                txtNumero.Clear();
                CargarEmpleados();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}