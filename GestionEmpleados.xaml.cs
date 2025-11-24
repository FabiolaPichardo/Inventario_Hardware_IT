using System;
using System.Windows;
using System.Data;

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

        private void CargarEmpleados()
        {
            try
            {
                // Usamos tus columnas exactas: EmpleadoID, NombreCompleto, NumeroEmpleado
                gridEmpleados.ItemsSource = db.LeerDatos("SELECT * FROM Empleados").DefaultView;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                MessageBox.Show("Llena todos los campos."); return;
            }

            try
            {
                // INSERT usando tus columnas
                string query = $"INSERT INTO Empleados (NombreCompleto, NumeroEmpleado) VALUES ('{txtNombre.Text}', '{txtNumero.Text}')";
                db.EjecutarComando(query);

                MessageBox.Show("Empleado registrado.");
                txtNombre.Clear(); txtNumero.Clear();
                CargarEmpleados();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}