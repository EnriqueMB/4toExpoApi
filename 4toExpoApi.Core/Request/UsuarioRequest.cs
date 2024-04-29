using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class UsuarioRequest
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime UltimoAcceso { get; set; }
        public List<UsuarioPermisosVM> Permisos { get; set; }
    }
}
