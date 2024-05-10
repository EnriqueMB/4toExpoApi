
namespace _4toExpoApi.Core.Request
{
    public class PaquetePatrocinadoresRequest
    {
        public int Id { get; set; }
        public int IdTipoPaquete { get; set; }
        public string NombrePaquete { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
    }
}
