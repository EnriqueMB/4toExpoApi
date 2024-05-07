using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class HotelRequest
    {
        public string Nombre { set; get; }
        public string Tipo { set; get; }
        public string Ubicacion { set; get; }
        public int Telefono { set; get; }
        public string LinkWhatsapp { set; get; }
        public string CodigoReserva { set; get; }
        public string Imagen { set; get; }
        public string? Tarifa { set; get; }
    }
}
