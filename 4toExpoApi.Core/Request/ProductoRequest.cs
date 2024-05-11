using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class ProductoRequest
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Articulo { get; set; }
        public int Precio { get; set; }
        public string Caracteristica { get; set; }
        public string Detalles { get; set; }
    }
}
