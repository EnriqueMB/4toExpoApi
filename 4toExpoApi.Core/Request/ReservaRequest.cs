using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class ReservaRequest
    {
        public string Nombre { get; set; }
        public string? Apellidos { get; set;}
        public int? IdTipoPaquete { get; set; }
        public string? NombreTitular { get; set; }
        public int? Monto { get; set; }
        public string? TipoPago { get; set; }
        public string? LogRequest { get; set; }
        public string? LogResponse { get; set; }
        public string? StatusReserva { get; set; }
        public int? Cantidad { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

    }
}
