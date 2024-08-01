

namespace _4toExpoApi.Core.Request
{
    public class PromocionRequest
    {
        public int IdPromocion { get; set; }
        public string? Descripcion { get; set; }
        public int? Porcentage { get; set; }
        public int? NumeroPases { get; set; }
        public int? PasesUsados { get; set; }
    }
}
