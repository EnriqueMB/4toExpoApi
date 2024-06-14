using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
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
        public int? edad { get; set; }
        public string? producto { get; set; }
        public int? monto { get; set; } 
        public int? idUniversidad { get;set; }
        public int? semestre {  get; set; }
        public string? ciudad { get; set; }
        public string? estado { get; set; }
        public string? telefono { get; set; }

        public string correo { get; set; }
        public string? urlImg { get; set; }
        public string? asociacion { get; set; }
        public string? contactoEmergencia { get; set; }
        public string? alergia { get; set; }
        public bool? asesoria { get; set; }
        public string? sugerencia { get; set; }

        public string? Password { get; set; }
        public int? idTipoUsuario { get; set; }
        public int? idPaquete { get; set; }

        public IFormFile? ImagenFile { get; set; }


    }
}
