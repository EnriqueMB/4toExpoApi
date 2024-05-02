using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class PagoRequest
    {
        public string nombre { get; set; }
        public string apellidos { get; set;}
        public int edad { get; set; }
        public string telefono { get; set;}
        public string correo { get; set; }
        public int monto { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
    }
}
