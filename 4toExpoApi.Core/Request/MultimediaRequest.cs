using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class MultimediaRequest
    {
        public int Id { get; set; }
        public string Calidad { get; set; }
        public string Resolucion { get; set; }
        public DateTime FechaAlt { get; set; }
        public string UserAlt { get; set; }
        public DateTime? FechaUpd { get; set; }
        public string UserUpd { get; set; }
        public int IdTipo { get; set; }
        public bool Activo { get; set; }
    }
}
