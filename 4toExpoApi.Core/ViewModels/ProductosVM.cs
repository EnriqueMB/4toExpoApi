using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class ProductosVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Caracteristica { get; set; }
        public int Precio { get; set; }
        public string Descripcion { get; set; }
        public int TotalArticulo { get; set; }
        public int IdPatrocinador { get; set; }
    }
}
