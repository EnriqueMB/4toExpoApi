
namespace _4toExpoApi.DataAccess.Entities
{
    public class BeneficioPaquete
    {
        public int Id { get; set; }
        public string TipoBeneficio { get; set;}
        public string Descripcion {  get; set;}
        public int? IdPaquetePatrocinador { get; set;}
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }
    }
}
