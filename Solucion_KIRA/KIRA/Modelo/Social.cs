using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIRA.Modelo
{
    public class Social
    {
        public int Id { get; set; }
        public string Comando { get; set; }
        public string[] Respuestas { get; set; }

        public Social(int id, string comando, string[] respuestas)
        {
            this.Id = id;
            this.Comando = comando;
            this.Respuestas = respuestas;
        }
    }
}
