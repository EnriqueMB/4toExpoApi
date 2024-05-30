
namespace _4toExpoApi.Core.Request
{
    public class ServicioRequest
    {
        public int Id { get; set; }
        public string Servicio { get; set; }
        public string Descripcion { get; set; }
        public string DiasLaborales { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public int IdPatrocinador { get; set; }
    }
}
