using _4toExpoApi.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class PaqueteGeneralVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        public List<IncluyePaqueteRequest> listaPaquetes {  get; set; }

        public string PrecioFormateado => Precio.ToString("N2");
    }
}
