using System;
using System.Windows;
using System.Data;
// Asegúrate de poner aquí el using donde tengas tu clase ConexionDB, por ejemplo:
// using TuProyecto.Datos; 

namespace Inventario_Hardware_IT// <--- CAMBIA ESTO POR TU NAMESPACE REAL
{
    public partial class GestionMarcas : Window
    {
        // Instancia de nuestra clase de conexión (asegúrate de haberla creado en el paso anterior)
        Inventario_Hardware_IT.Datos.ConexionDB db = new Inventario_Hardware_IT.Datos.ConexionDB();

        public GestionMarcas()
        {
            InitializeComponent();
            CargarMarcas(); // Cargar la lista al abrir la ventana
        }

        // Función para leer datos de SQL y ponerlos en la tabla
        private void CargarMarcas()
        {
            try
            {
                string consulta = "SELECT * FROM Marcas";
                DataTable dt = db.LeerDatos(consulta);
                gridDatos.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
            }
        }

        // Función del botón Guardar
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombreMarca.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Por favor escribe un nombre.");
                return;
            }

            try
            {
                // Insertar en la base de datos
                string consulta = $"INSERT INTO Marcas (Nombre) VALUES ('{nombre}')";
                db.EjecutarComando(consulta);

                MessageBox.Show("¡Marca registrada con éxito!");

                // Limpiar y recargar
                txtNombreMarca.Clear();
                CargarMarcas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
    }
}