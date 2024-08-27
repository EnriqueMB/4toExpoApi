

using _4toExpoApi.DataAccess.Entities;

namespace _4toExpoApi.Core.ViewModels
{
    public class BannerVM
    {
        public int Id { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Descripcion { get; set; }
        public string? UrlVideo { get; set; }
        public List<RedPatrocinador>? RedesRequest { get; set; }

        public string? UrlRedSocial { get; set; }
    }
}
