using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class GestionModelos : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionModelos()
        {
            InitializeComponent();
            CargarListas();
            CargarModelos();
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

        private void CargarListas()
        {
            try
            {
                cbMarca.ItemsSource = db.LeerDatos("SELECT * FROM Marcas").DefaultView;
                cbTipo.ItemsSource = db.LeerDatos("SELECT * FROM TiposHardware").DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error cargando listas: " + ex.Message); }
        }

        private void CargarModelos()
        {
            try
            {
                // Usamos los nombres correctos de tus columnas
                string query = @"
                    SELECT M.ModeloID, M.NombreModelo, MA.NombreMarca as Marca, T.NombreTipo as Tipo 
                    FROM Modelos M
                    INNER JOIN Marcas MA ON M.MarcaID = MA.MarcaID
                    INNER JOIN TiposHardware T ON M.TipoID = T.TipoID";

                gridDatos.ItemsSource = db.LeerDatos(query).DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error cargando tabla: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cbMarca.SelectedValue == null || cbTipo.SelectedValue == null || string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                MessageBox.Show("⚠️ Por favor, selecciona Marca, Tipo y escribe el Modelo.", "Faltan Datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idMarca = (int)cbMarca.SelectedValue;
                int idTipo = (int)cbTipo.SelectedValue;
                string nombre = txtModelo.Text.Trim();

                string query = $"INSERT INTO Modelos (NombreModelo, MarcaID, TipoID) VALUES ('{nombre}', {idMarca}, {idTipo})";
                db.EjecutarComando(query);

                MessageBox.Show("✅ Modelo registrado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                txtModelo.Clear();
                CargarModelos();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }
    }
}