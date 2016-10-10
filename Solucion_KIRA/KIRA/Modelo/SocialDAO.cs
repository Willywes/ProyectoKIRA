using KIRA.DataBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIRA.Modelo
{
    public class SocialDAO
    {
        public SocialDAO()
        {
        }

        private int CalcularID()
        {
            SqlConnection con = ConexionDB.ObtenerConexion();
            SqlCommand cmd = new SqlCommand(string.Format("SELECT MAX(id) FROM Social;"), con);

            SqlDataReader rs = cmd.ExecuteReader();

            if (rs.Read())
            {
                return rs.GetInt32(0) + 1;
            }
            return 0;
        }

        public bool Agregar(Social social)
        {

            SqlConnection con = ConexionDB.ObtenerConexion();

            int id = CalcularID();

            SqlCommand cmd = new SqlCommand(string.Format("INSERT INTO Social(id,comando) Values ('{0}','{1}');", id, social.Comando), con);

            if (cmd.ExecuteNonQuery() > 0)
            {
                foreach (var respuesta in social.Respuestas)
                {
                    SqlCommand cmd2 = new SqlCommand(string.Format("INSERT INTO RespuestasSocial(id,respuesta) Values ('{0}','{1}');", id, respuesta), con);
                    cmd2.ExecuteNonQuery();
                }
                return true;
            }
            con.Close();
            return false;
        }

        public bool Eliminar(int id)
        {

            SqlConnection con = ConexionDB.ObtenerConexion();

            SqlCommand cmd = new SqlCommand(string.Format("DELETE FROM RespuestaSocial Where id = {0};", id), con);

            if (cmd.ExecuteNonQuery() > 0)
            {                
                    cmd = new SqlCommand(string.Format("DELETE FROM Social Where id = {0};", id), con);
                    cmd.ExecuteNonQuery();               
                return true;
            }
            con.Close();
            return false;
        }

        public bool Actualizar(Social social)
        {
            return false;
        }
        public Social Mostrar(int id)
        {
            Social s = null;
            if (id == 1)
            {
                s = new Social(1,null,null);
            }
            
            return s;
        }
        public List<Social> Listar()
        {
            List<Social> lista = new List<Social>();

            SqlConnection con = ConexionDB.ObtenerConexion();

            SqlCommand cmd = new SqlCommand(string.Format("SELECT * FROM Social;"), con);

            SqlDataReader rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                lista.Add(new Social(rs.GetInt32(0), rs.GetString(1), ListarRespuestasSocial(rs.GetInt32(0))));
            }
            con.Close();
            return lista;
        }
        public string[] RetornarComandoSociales()
        {
            string[] lista = new string[0];

            SqlConnection con = ConexionDB.ObtenerConexion();

            SqlCommand cmd = new SqlCommand(string.Format("SELECT comando FROM Social;"), con);

            SqlDataReader rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                Array.Resize(ref lista, lista.Length + 1);
                lista[lista.Length - 1] = (rs.GetString(0));
            }

            con.Close();
            return lista;
        }
        

        private string[] ListarRespuestasSocial(int id)
        {
            string[] lista = new string[0];

            SqlConnection con = ConexionDB.ObtenerConexion();

            SqlCommand cmd = new SqlCommand(string.Format("SELECT respuesta FROM RespuestasSocial WHERE id_social = {0};", id), con);

            SqlDataReader rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                Array.Resize(ref lista, lista.Length + 1);
                lista[lista.Length - 1] = (rs.GetString(0));
            }
            return lista;
        }
    }
}
