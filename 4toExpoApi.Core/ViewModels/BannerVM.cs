﻿

namespace _4toExpoApi.Core.ViewModels
{
    public class BannerVM
    {
        public int Id { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Descripcion { get; set; }
        public string? UrlVideo { get; set; }
        public int? IdRedSocial { get; set; }
        public string? NombreRedSocial { get; set; }
        public string? UrlRedSocial { get; set; }
        public int? IdPatrocinador { get; set; }
        public bool Activo { get; set; }
    }
}
