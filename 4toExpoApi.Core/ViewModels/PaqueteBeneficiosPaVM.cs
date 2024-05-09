

using _4toExpoApi.Core.Request;

namespace _4toExpoApi.Core.ViewModels
{
    public class PaqueteBeneficiosPaVM
    {
        public int Id { get; set; }
     
        public int IdTipoPaquete {  get; set; }
        public string TipoPaquete { get; set; }
        public string NombrePaquete { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }

        public List<BeneficioPaqueteRequest> Beneficios {  get; set; }
    }
}
