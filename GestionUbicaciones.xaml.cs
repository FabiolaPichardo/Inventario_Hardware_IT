using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
//using Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class GestionUbicaciones : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionUbicaciones()
        {
            InitializeComponent();
            CargarTodo();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CargarTodo()
        {
            try
            {
                DataTable dtSedes = db.LeerDatos("SELECT * FROM Sedes");
                gridSedes.ItemsSource = dtSedes.DefaultView;
                cbSedes.ItemsSource = dtSedes.DefaultView;

                string queryUbi = "SELECT U.UbicacionID, U.NombreUbicacion, S.NombreSede FROM Ubicaciones U INNER JOIN Sedes S ON U.SedeID = S.SedeID";
                gridUbicaciones.ItemsSource = db.LeerDatos(queryUbi).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos: " + ex.Message);
            }
        }

        private void BtnGuardarSede_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSede.Text)) return;

            try
            {
                db.EjecutarComando($"INSERT INTO Sedes (NombreSede) VALUES ('{txtSede.Text}')");
                MessageBox.Show("✅ Sede registrada correctamente.");
                txtSede.Clear();
                CargarTodo();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnGuardarUbicacion_Click(object sender, RoutedEventArgs e)
        {
            if (cbSedes.SelectedValue == null || string.IsNullOrWhiteSpace(txtUbicacion.Text))
            {
                MessageBox.Show("⚠️ Selecciona una Sede y escribe un nombre.");
                return;
            }

            try
            {
                int idSede = (int)cbSedes.SelectedValue;
                db.EjecutarComando($"INSERT INTO Ubicaciones (NombreUbicacion, SedeID) VALUES ('{txtUbicacion.Text}', {idSede})");
                MessageBox.Show("✅ Ubicación registrada correctamente.");
                txtUbicacion.Clear();
                CargarTodo();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}