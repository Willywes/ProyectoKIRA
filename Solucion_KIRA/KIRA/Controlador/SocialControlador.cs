using KIRA.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIRA.Controlador
{
    public class SocialControlador
    {
        private static SocialDAO socialDAO = new SocialDAO();
        private Social social;
        public SocialControlador()
        {
            //socialDAO = new SocialDAO();
        }

        public static bool AgregarComandoSocial(Social social)
        {
            if (social.Comando == string.Empty)
            {
                throw new ArgumentException("Ingrese un comando");
            }
            else if (social.Respuestas == null)
            {
                throw new ArgumentException("Ingrese una respuesta");
            }
            else if(socialDAO.Agregar(social))
            {
                return true;
            }

            return false;
        }

        public static bool EliminarComandos(int id)
        {
            if (socialDAO.Mostrar(id) != null )
            {
                socialDAO.Eliminar(id);
                return true;
            }
            return false;
        }

        public static List<Social> ListarComandos()
        {
            List<Social> lista = new List<Social>();
            lista = socialDAO.Listar();
            return lista;
        }

        public static string[] RetornarComandoSociales()
        {
            return socialDAO.RetornarComandoSociales();
        }
    }
}
