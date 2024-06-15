using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class UsuarioPromoRequest
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string correo { get; set; }
        public int edad { get; set; }
        public string? telefono { get; set; }
        public string? ciudad { get; set; }
        public string? estado { get; set; }
        public string? asociacion { get; set; }
        public string? nombreRepresentante { get; set; }
    }
}
