

using System.ComponentModel.DataAnnotations.Schema;

namespace _4toExpoApi.DataAccess.Entities
{
    public class PaquetePatrocinadores
    {
        public int Id { get; set; }
        public int? IdTipoPaquete {  get; set; }
        public string? NombrePaquete { get; set; }
        public string? Descripcion {  get; set; }
        public int Precio {  get; set; }
        public int? UserAlt {  get; set; }
        public DateTime? FechaAlt {  get; set; }
        public int? UserUpd {  get; set; }  
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<IncluyePaquete> Incluyes { get; set; }

        [ForeignKey(nameof(IdTipoPaquete))]
        public virtual TipoPaquete TipoPaquete { get; set;}

    }
}
