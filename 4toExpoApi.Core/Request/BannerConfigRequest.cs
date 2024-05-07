
namespace _4toExpoApi.Core.Request
{
    public class BannerConfigRequest
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public int CantidadExpositores { get; set; }
        public int CantidadParticipantes { get; set; }
        public int Orden { get; set; }
    }
}
