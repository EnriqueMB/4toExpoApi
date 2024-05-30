

namespace _4toExpoApi.DataAccess.Entities
{
    public class Servicios
    {
        public int Id { get; set; }
        public string Servicio { get; set; }
        public string Descripcion { get; set; }
        public string DiasLaborales {  get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal {  get; set; }
        public int IdPatrocinador { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }


    }
}
