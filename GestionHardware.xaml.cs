using System;
using System.Windows;
using System.Data;


namespace Inventario_Hardware_IT
{
    public partial class GestionHardware : Window
    {
        ConexionDB db = new ConexionDB();
        int _rol;

        // Constructor modificado: si no recibe nada es 1 (admin), pero normalmente recibirá desde el menú
        public GestionHardware(int rolUsuario = 1)
        {
            InitializeComponent();
            _rol = rolUsuario;

            CargarModelosCombo();
            CargarInventario();
            dpFecha.SelectedDate = DateTime.Now;

            // SEGURIDAD: Solo mostramos el botón rojo si es Admin (Rol 1)
            if (_rol == 1)
            {
                btnEliminar.Visibility = Visibility.Visible;
            }
        }

        private void CargarModelosCombo()
        {
            try
            {
                DataTable dt = db.LeerDatos("SELECT ModeloID, NombreModelo FROM Modelos");
                cbModelo.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error modelos: " + ex.Message); }
        }

        private void CargarInventario()
        {
            try
            {
                string query = @"SELECT H.HardwareID, H.NumeroSerie, H.EtiquetaActivo, M.NombreModelo, MA.NombreMarca, H.Estado, H.FechaCompra 
                                FROM Hardware H
                                INNER JOIN Modelos M ON H.ModeloID = M.ModeloID
                                INNER JOIN Marcas MA ON M.MarcaID = MA.MarcaID";
                gridDatos.ItemsSource = db.LeerDatos(query).DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error inventario: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cbModelo.SelectedValue == null || string.IsNullOrWhiteSpace(txtSerie.Text))
            {
                MessageBox.Show("Faltan datos obligatorios."); return;
            }
            try
            {
                int modeloId = (int)cbModelo.SelectedValue;
                string serie = txtSerie.Text;
                string etiqueta = txtEtiqueta.Text;
                string estado = cbEstado.Text;
                string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");

                string query = $"INSERT INTO Hardware (NumeroSerie, EtiquetaActivo, ModeloID, FechaCompra, Estado) VALUES ('{serie}', '{etiqueta}', {modeloId}, '{fecha}', '{estado}')";
                db.EjecutarComando(query);

                MessageBox.Show("Guardado.");
                txtSerie.Clear(); txtEtiqueta.Clear();
                CargarInventario();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message); }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gridDatos.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una fila primero."); return;
            }

            if (MessageBox.Show("¿Borrar este equipo?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    DataRowView fila = (DataRowView)gridDatos.SelectedItem;
                    int idHardware = Convert.ToInt32(fila["HardwareID"]);
                    db.EjecutarComando($"DELETE FROM Hardware WHERE HardwareID = {idHardware}");

                    MessageBox.Show("Eliminado.");
                    CargarInventario();
                }
                catch (Exception ex) { MessageBox.Show("Error al eliminar: " + ex.Message); }
            }
        }
    }
}