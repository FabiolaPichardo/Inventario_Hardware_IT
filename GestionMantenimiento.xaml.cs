using System;
using System.Windows;
using System.Data;
//using Inventario_Hardware_IT.Datos//

namespace Inventario_Hardware_IT
{
    public partial class GestionMantenimiento : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionMantenimiento()
        {
            InitializeComponent();
            CargarListas();
            CargarHistorial();
            dpFecha.SelectedDate = DateTime.Now;
        }

        private void CargarListas()
        {
            try
            {
                // 1. Cargar Proveedores
                cbProveedor.ItemsSource = db.LeerDatos("SELECT * FROM Proveedores").DefaultView;

                // 2. Cargar Equipos (Mostramos Serie + Modelo)
                // OJO: Podrías filtrar solo los que NO están dados de baja, por ejemplo.
                string qEquipos = "SELECT H.HardwareID, (H.NumeroSerie + ' - ' + M.NombreModelo) AS Info FROM Hardware H INNER JOIN Modelos M ON H.ModeloID = M.ModeloID";
                DataTable dtEq = db.LeerDatos(qEquipos);

                cbEquipo.ItemsSource = dtEq.DefaultView;
                cbEquipo.DisplayMemberPath = "Info";
                cbEquipo.SelectedValuePath = "HardwareID";
            }
            catch (Exception ex) { MessageBox.Show("Error listas: " + ex.Message); }
        }

        private void CargarHistorial()
        {
            try
            {
                // Consulta completa para ver detalles
                string query = @"SELECT M.MantenimientoID, H.NumeroSerie, P.NombreProveedor, M.DescripcionProblema, M.FechaEntrada, M.Costo 
                                 FROM Mantenimientos M
                                 INNER JOIN Hardware H ON M.HardwareID = H.HardwareID
                                 INNER JOIN Proveedores P ON M.ProveedorID = P.ProveedorID";
                gridMto.ItemsSource = db.LeerDatos(query).DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error historial: " + ex.Message); }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (cbEquipo.SelectedValue == null || cbProveedor.SelectedValue == null || string.IsNullOrWhiteSpace(txtCosto.Text))
            {
                MessageBox.Show("Faltan datos."); return;
            }

            try
            {
                int idHard = (int)cbEquipo.SelectedValue;
                int idProv = (int)cbProveedor.SelectedValue;
                string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");
                decimal costo = Convert.ToDecimal(txtCosto.Text);
                string desc = txtProblema.Text;

                // 1. Guardar Mantenimiento
                string insert = $"INSERT INTO Mantenimientos (HardwareID, ProveedorID, DescripcionProblema, FechaEntrada, Costo) VALUES ({idHard}, {idProv}, '{desc}', '{fecha}', {costo})";
                db.EjecutarComando(insert);

                // 2. ACTUALIZAR ESTADO DEL EQUIPO A 'En Reparación'
                db.EjecutarComando($"UPDATE Hardware SET Estado = 'En Reparación' WHERE HardwareID = {idHard}");

                MessageBox.Show("Equipo enviado a taller.");
                txtProblema.Clear(); txtCosto.Clear();
                CargarHistorial();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnAddProv_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNuevoProv.Text))
            {
                db.EjecutarComando($"INSERT INTO Proveedores (NombreProveedor) VALUES ('{txtNuevoProv.Text}')");
                MessageBox.Show("Proveedor Agregado");
                txtNuevoProv.Clear();
                CargarListas(); // Recargar el combo
            }
        }
    }
}