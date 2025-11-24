using System;
using System.Windows;
using System.Data;
// using Inventario_Hardware_IT.Datos; <--- BORRÉ ESTA LÍNEA QUE TE DABA ERROR

namespace Inventario_Hardware_IT
{
    public partial class GestionModelos : Window
    {
        // Al quitar el 'using', el programa buscará ConexionDB aquí mismo, donde debe estar.
        ConexionDB db = new ConexionDB();

        public GestionModelos()
        {
            InitializeComponent();
            CargarListas();
            CargarModelos();
        }

        private void CargarListas()
        {
            try
            {
                // Usamos las consultas básicas para llenar los combos
                cbMarca.ItemsSource = db.LeerDatos("SELECT * FROM Marcas").DefaultView;
                cbTipo.ItemsSource = db.LeerDatos("SELECT * FROM TiposHardware").DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listas: " + ex.Message);
            }
        }

        private void CargarModelos()
        {
            try
            {
                // Usamos los nombres correctos de tus columnas (MarcaID, TipoID)
                string query = @"
                    SELECT M.ModeloID, M.NombreModelo, MA.NombreMarca as Marca, T.NombreTipo as Tipo 
                    FROM Modelos M
                    INNER JOIN Marcas MA ON M.MarcaID = MA.MarcaID
                    INNER JOIN TiposHardware T ON M.TipoID = T.TipoID";

                gridDatos.ItemsSource = db.LeerDatos(query).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error tabla: " + ex.Message);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cbMarca.SelectedValue == null || cbTipo.SelectedValue == null || string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                MessageBox.Show("Llena todos los campos.");
                return;
            }

            try
            {
                int idMarca = (int)cbMarca.SelectedValue;
                int idTipo = (int)cbTipo.SelectedValue;
                string nombre = txtModelo.Text.Trim();

                string query = $"INSERT INTO Modelos (NombreModelo, MarcaID, TipoID) VALUES ('{nombre}', {idMarca}, {idTipo})";
                db.EjecutarComando(query);

                MessageBox.Show("Modelo registrado.");
                txtModelo.Clear();
                CargarModelos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardar: " + ex.Message);
            }
        }
    }
}