
using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Distancia: FieldsDefault
    {
        [Key]
        public int Id { get; set; }
        public int IdHotel { get; set; }
        public string Tipo { get; set; }
        public string Tiempo { get; set; }
        public string Icono { get; set; } 
    }
}
