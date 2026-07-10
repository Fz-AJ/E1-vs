using System;
using MySql.Data.MySqlClient;

namespace E1_vs
{
    public static class Conexion
    {
        private static string cadenaConexion = "Server=127.0.0.1;Database=musica;Uid=root;Pwd=pp123pp#A;";
        public static MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}
