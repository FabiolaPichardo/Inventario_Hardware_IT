using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Inventario_Hardware_IT.Datos // <--- 1. ESTO ARREGLA EL ERROR ROJO
{
    public class ConexionDB
    {
        // 2. Asegúrate de que este nombre ("CadenaInventario") sea IGUAL al que pusiste en App.config
        // Si en App.config dice name="CadenaPrincipal", cambia el texto de abajo.
        private string connectionStringName = "CadenaPrincipal";

        private string ObtenerCadena()
        {
            var connectionStringItem = ConfigurationManager.ConnectionStrings[connectionStringName];

            // Verificación de seguridad por si el nombre está mal
            if (connectionStringItem == null)
            {
                throw new Exception($"No se encontró la cadena de conexión '{connectionStringName}' en App.config.");
            }

            return connectionStringItem.ConnectionString;
        }

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(ObtenerCadena());
        }

        // --- FUNCIONES NECESARIAS PARA TUS VENTANAS (CRUD) ---

        // Función para leer tablas (SELECT)
        public DataTable LeerDatos(string query)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection con = ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                    adaptador.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error leyendo datos: " + ex.Message);
                }
            }
            return tabla;
        }

        // Función para guardar/editar/borrar (INSERT, UPDATE, DELETE)
        public void EjecutarComando(string query)
        {
            using (SqlConnection con = ObtenerConexion())
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error guardando datos: " + ex.Message);
                }
            }
        }
    }
}