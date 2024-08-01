
using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Promocion
    {
        [Key]
        public int IdPromocion { get; set; }
        public string? Descripcion { get; set; }
        public int? Porcentage { get; set; }
        public int? NumeroPases { get; set; }
        public int? PasesUsados { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt {  get; set; }
        public int? UserUpd {  get; set; }
        public DateTime? FechaUpd {  get; set; }
        public bool Activo {  get; set; }
    }
}
