using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public  class ClientesRequest
    {
        public string? Nombre { get; set; }

        public string? Apellidos { get; set; }
        public string? Identificador { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
 
    }
}
