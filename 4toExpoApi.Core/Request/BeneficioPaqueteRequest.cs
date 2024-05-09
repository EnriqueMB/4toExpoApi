
namespace _4toExpoApi.Core.Request
{
    public class BeneficioPaqueteRequest
    {
        public int Id { get; set; }
        public string TipoBeneficio { get; set; }
        public string Descripcion { get; set; }
        public int? IdPaquetePatrocinador { get; set; }
    }
}
