using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class UsuariosPromoVM
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public string Telefono { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Asociacion { get; set; }
        public string NombreRepresentante { get; set; }

    }
}
