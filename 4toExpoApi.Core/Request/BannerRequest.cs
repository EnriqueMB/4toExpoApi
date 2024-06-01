

using Microsoft.AspNetCore.Http;

namespace _4toExpoApi.Core.Request
{
    public class BannerRequest
    {
        public int Id { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Descripcion { get; set; }
        public string? UrlVideo { get; set; }
        public IFormFile? VideoFile { get; set; }
        public int IdRedSocial { get; set; }
        public string? UrlRedSocial {  get; set; }
        public int IdPatrocinador { get; set; }
        public bool Activo { get; set; }
    }
}
