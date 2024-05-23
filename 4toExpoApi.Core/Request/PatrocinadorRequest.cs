using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class PatrocinadorRequest
    {
        public int? Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string NombreEmpresa { get; set; }
        public string? UrlLogo { get; set; }
        public IFormFile? UrlImg { get; set; }
        //public int? UserAlt { get; set; }
        //public DateTime? FechaAlt { get; set; }
        //public int? UserUpd { get; set; }
        //public DateTime? FechaUpd { get; set; }

        //public Boolean? Activo { get; set; }

        //Datos para en la tabla  usuario 
        public string? Telefono { get; set; }
        public string Password { get; set; }
    }
}
