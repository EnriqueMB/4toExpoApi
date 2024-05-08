

namespace _4toExpoApi.DataAccess.Entities
{
    public class TipoPaquete
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }
    }
}
