using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIRA.DataBase
{
    public class ConexionDB
    {
        public static SqlConnection ObtenerConexion()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.Conexion);
            con.Open();
            return con;
        }
    }
}
