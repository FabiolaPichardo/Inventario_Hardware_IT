using System;
using System.Windows;
using System.Data;

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

        private void CargarListas()
        {
            try
            {
                // 1. Cargar Empleados
                cbEmpleado.ItemsSource = db.LeerDatos("SELECT * FROM Empleados").DefaultView;

                // 2. Cargar SOLO Hardware Disponible (Para no asignar uno que ya tiene dueño)
                // Hacemos un JOIN para mostrar Modelo y Serie en el texto
                string queryHW = @"SELECT H.HardwareID, (H.NumeroSerie + ' - ' + M.NombreModelo) AS InfoEquipo 
                                   FROM Hardware H 
                                   INNER JOIN Modelos M ON H.ModeloID = M.ModeloID 
                                   WHERE H.Estado = 'Disponible'"; // <--- FILTRO IMPORTANTE

                DataTable dtHw = db.LeerDatos(queryHW);
                cbHardware.ItemsSource = dtHw.DefaultView;
                cbHardware.DisplayMemberPath = "InfoEquipo";
                cbHardware.SelectedValuePath = "HardwareID";
            }
            catch (Exception ex) { MessageBox.Show("Error listas: " + ex.Message); }
        }

        private void CargarHistorial()
        {
            try
            {
                // Consulta completa para ver quién tiene qué
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
                MessageBox.Show("Selecciona Equipo y Empleado."); return;
            }

            try
            {
                int idHard = (int)cbHardware.SelectedValue;
                int idEmp = (int)cbEmpleado.SelectedValue;
                string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");

                // PASO 1: Guardar en Asignaciones
                string qInsert = $"INSERT INTO Asignaciones (HardwareID, EmpleadoID, FechaAsignacion) VALUES ({idHard}, {idEmp}, '{fecha}')";
                db.EjecutarComando(qInsert);

                // PASO 2: Actualizar el estado del Hardware a 'Asignado' (Para que no salga en la lista de nuevo)
                string qUpdate = $"UPDATE Hardware SET Estado = 'Asignado' WHERE HardwareID = {idHard}";
                db.EjecutarComando(qUpdate);

                MessageBox.Show("Equipo Asignado Correctamente.");
                CargarListas();     // Recargar para quitar el equipo asignado del combo
                CargarHistorial();  // Verlo en la tabla
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}