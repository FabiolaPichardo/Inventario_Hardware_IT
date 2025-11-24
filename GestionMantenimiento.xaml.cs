using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

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
                // Cargar Proveedores
                cbProveedor.ItemsSource = db.LeerDatos("SELECT * FROM Proveedores").DefaultView;

                // Cargar Equipos (Serie + Modelo)
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
            // 1. Validaciones Básicas
            if (cbEquipo.SelectedValue == null || cbProveedor.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Selecciona un Equipo y un Proveedor.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Validación de Costo (Evitar error si escriben letras)
            if (!decimal.TryParse(txtCosto.Text, out decimal costo))
            {
                MessageBox.Show("⚠️ El costo debe ser un número válido (Ej: 500.00).", "Error de Formato", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idHard = (int)cbEquipo.SelectedValue;
                int idProv = (int)cbProveedor.SelectedValue;
                string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");
                string desc = txtProblema.Text;

                // Guardar
                string insert = $"INSERT INTO Mantenimientos (HardwareID, ProveedorID, DescripcionProblema, FechaEntrada, Costo) VALUES ({idHard}, {idProv}, '{desc}', '{fecha}', {costo})";
                db.EjecutarComando(insert);

                // Actualizar estado del equipo
                db.EjecutarComando($"UPDATE Hardware SET Estado = 'En Reparación' WHERE HardwareID = {idHard}");

                MessageBox.Show("✅ Equipo enviado a taller correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                txtProblema.Clear();
                txtCosto.Clear();
                CargarHistorial();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnAddProv_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNuevoProv.Text))
            {
                try
                {
                    db.EjecutarComando($"INSERT INTO Proveedores (NombreProveedor) VALUES ('{txtNuevoProv.Text}')");
                    MessageBox.Show("Proveedor Agregado");
                    txtNuevoProv.Clear();
                    CargarListas(); // Recargar el combo para que aparezca
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }
    }
}