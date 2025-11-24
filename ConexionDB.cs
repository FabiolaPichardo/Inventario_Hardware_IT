using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Inventario_Hardware_IT // <--- CORREGIDO: Se quitó ".Datos" para que MainWindow lo reconozca
{
    public class ConexionDB
    {
        // IMPORTANTE: Asegúrate que en tu App.config la conexión se llame igual que aquí: "CadenaPrincipal"
        private string connectionStringName = "CadenaPrincipal";

        private string ObtenerCadena()
        {
            // Busca la cadena de conexión en el archivo App.config
            var connectionStringItem = ConfigurationManager.ConnectionStrings[connectionStringName];

            // Verificación de seguridad por si el nombre está mal o no existe en App.config
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

        // --- FUNCIONES PARA TUS VENTANAS (CRUD) ---

        // Función para leer tablas (SELECT) - Devuelve una tabla con datos
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

        // Función para guardar/editar/borrar (INSERT, UPDATE, DELETE) - No devuelve datos
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