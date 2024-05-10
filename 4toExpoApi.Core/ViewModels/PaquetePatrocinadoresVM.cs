
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;

namespace _4toExpoApi.Core.ViewModels
{
    public class PaquetePatrocinadoresVM
    {
        public PaquetePatrocinadoresRequest PaquetePatrocinador {  get; set; }   
        public List<BeneficioPaqueteRequest>? BeneficioPaquete { get; set; }
    }
}
