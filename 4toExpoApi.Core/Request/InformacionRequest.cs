using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class InformacionRequest
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? SubTitulo { get; set; }
        public string? UrlImagen { get; set; }
        public IFormFile? ImagenFile { get; set; }
        public string? Texto { get; set; }
    }
}
