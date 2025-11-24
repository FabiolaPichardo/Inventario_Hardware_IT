using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
//sing Inventario_Hardware_IT.Datos;

namespace Inventario_Hardware_IT
{
    public partial class GestionEmpleados : Window
    {
        ConexionDB db = new ConexionDB();

        // VARIABLES CLAVE PARA CONTROL
        int _rolUsuario;
        int _idEmpleadoSeleccionado = 0; // 0 = Nuevo, >0 = Editar

        // Modificamos el constructor para pedir el Rol
        public GestionEmpleados(int rol = 1)
        {
            InitializeComponent();
            _rolUsuario = rol;

            // CONFIGURAR PERMISOS
            if (_rolUsuario == 1) // Admin
            {
                btnEliminar.Visibility = Visibility.Visible; // Admin puede ver el botón rojo
            }
            else
            {
                btnEliminar.Visibility = Visibility.Collapsed; // Trabajador NO lo ve
            }

            CargarEmpleados();
        }

        // --- VISUALES ---
        private void Border_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void CargarEmpleados()
        {
            try { gridEmpleados.ItemsSource = db.LeerDatos("SELECT * FROM Empleados").DefaultView; }
            catch (Exception ex) { MessageBox.Show("Error al cargar: " + ex.Message); }
        }

        // --- LÓGICA DE GUARDAR (INSERT O UPDATE) ---
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtNumero.Text))
            {
                MessageBox.Show("⚠️ Llena todos los campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string query = "";

                if (_idEmpleadoSeleccionado == 0)
                {
                    // MODO INSERTAR (NUEVO)
                    query = $"INSERT INTO Empleados (NombreCompleto, NumeroEmpleado) VALUES ('{txtNombre.Text}', '{txtNumero.Text}')";
                }
                else
                {
                    // MODO EDITAR (UPDATE)
                    query = $"UPDATE Empleados SET NombreCompleto = '{txtNombre.Text}', NumeroEmpleado = '{txtNumero.Text}' WHERE EmpleadoID = {_idEmpleadoSeleccionado}";
                }

                db.EjecutarComando(query);
                MessageBox.Show("✅ Datos guardados correctamente.");

                LimpiarFormulario(); // Resetear todo
                CargarEmpleados();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        // --- LÓGICA PARA EDITAR (DOBLE CLIC) ---
        private void GridEmpleados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridEmpleados.SelectedItem != null)
            {
                DataRowView fila = (DataRowView)gridEmpleados.SelectedItem;

                // 1. Llenar campos con datos de la fila
                txtNombre.Text = fila["NombreCompleto"].ToString();
                txtNumero.Text = fila["NumeroEmpleado"].ToString();

                // 2. Guardar el ID en memoria para saber a quién actualizar
                _idEmpleadoSeleccionado = Convert.ToInt32(fila["EmpleadoID"]);

                // 3. Cambiar título visualmente
                lblTitulo.Text = "✏️ Editando a: " + txtNombre.Text;
            }
        }

        // --- LÓGICA ELIMINAR (SOLO ADMIN) ---
        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_idEmpleadoSeleccionado == 0)
            {
                MessageBox.Show("⚠️ Primero da DOBLE CLIC en un empleado de la lista para seleccionarlo.", "Selección requerida");
                return;
            }

            if (MessageBox.Show("¿Eliminar a este empleado? Si tiene equipos asignados, fallará.", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    db.EjecutarComando($"DELETE FROM Empleados WHERE EmpleadoID = {_idEmpleadoSeleccionado}");
                    MessageBox.Show("🗑️ Empleado eliminado.");
                    LimpiarFormulario();
                    CargarEmpleados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ No se puede eliminar: Es posible que tenga equipos asignados. Retírale los equipos primero.", "Error de Integridad");
                }
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtNumero.Clear();
            _idEmpleadoSeleccionado = 0; // Volver a modo "Nuevo"
            lblTitulo.Text = "Registrar Nuevo Colaborador";
        }
    }
}