using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Informacion
    {
        public int Id { get; set; }  
        public string? Titulo { get; set; }
        public string? SubTitulo { get; set; }
        public string? UrlImagen {  get; set; }
        public string? Texto { get; set; }
        //public int UserAlt { get; set; }
        //public DateTime? FechaAlt { get; set; }
        //public int? UserUpd { get; set; }
        //public DateTime? FechaUpd { get; set; }
        //public bool Activo { get; set; }
    }
}
