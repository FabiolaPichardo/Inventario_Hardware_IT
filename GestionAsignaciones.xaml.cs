using System;
using System.Windows;
using System.Windows.Input; // NECESARIO PARA MOVER
using System.Data;
//using Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class GestionAsignaciones : Window
    {
        ConexionDB db = new ConexionDB();

        public GestionAsignaciones()
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
                // 1. Cargar Empleados
                cbEmpleado.ItemsSource = db.LeerDatos("SELECT * FROM Empleados").DefaultView;

                // 2. Cargar SOLO Hardware Disponible
                string queryHW = @"SELECT H.HardwareID, (H.NumeroSerie + ' - ' + M.NombreModelo) AS InfoEquipo 
                                   FROM Hardware H 
                                   INNER JOIN Modelos M ON H.ModeloID = M.ModeloID 
                                   WHERE H.Estado = 'Disponible'";

                DataTable dtHw = db.LeerDatos(queryHW);

                // --- DIAGNÓSTICO: AVISAR SI NO HAY EQUIPOS ---
                if (dtHw.Rows.Count == 0)
                {
                    // Esto agregará un ítem falso para avisarte visualmente
                    // Pero lo ideal es que registres equipos o cambies su estado a 'Disponible'
                }

                cbHardware.ItemsSource = dtHw.DefaultView;
                cbHardware.DisplayMemberPath = "InfoEquipo"; // ESTO ES LO QUE SE VE
                cbHardware.SelectedValuePath = "HardwareID"; // ESTO ES LO QUE SE GUARDA
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error listas: " + ex.Message);
            }
        }

        private void CargarHistorial()
        {
            try
            {
                string query = @"SELECT A.AsignacionID, H.NumeroSerie, E.NombreCompleto, A.FechaAsignacion 
                                 FROM Asignaciones A
                                 INNER JOIN Hardware H ON A.HardwareID = H.HardwareID
                                 INNER JOIN Empleados E ON A.EmpleadoID = E.EmpleadoID";
                gridAsignaciones.ItemsSource = db.LeerDatos(query).DefaultView;
            }
            catch (Exception ex) { MessageBox.Show("Error historial: " + ex.Message); }
        }

        private void BtnAsignar_Click(object sender, RoutedEventArgs e)
        {
            if (cbHardware.SelectedValue == null || cbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("⚠️ Selecciona un Equipo y un Empleado.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int idHard = (int)cbHardware.SelectedValue;
                int idEmp = (int)cbEmpleado.SelectedValue;
                string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");

                // 1. Insertar Asignación
                string qInsert = $"INSERT INTO Asignaciones (HardwareID, EmpleadoID, FechaAsignacion) VALUES ({idHard}, {idEmp}, '{fecha}')";
                db.EjecutarComando(qInsert);

                // 2. Cambiar estado a 'Asignado'
                string qUpdate = $"UPDATE Hardware SET Estado = 'Asignado' WHERE HardwareID = {idHard}";
                db.EjecutarComando(qUpdate);

                MessageBox.Show("✅ Equipo asignado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarListas(); // Recargar para que el equipo desaparezca de la lista
                CargarHistorial();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}