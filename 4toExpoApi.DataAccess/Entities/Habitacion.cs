

using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Habitacion: FieldsDefault
    {
        [Key]
        public int Id { set; get; }
        public int IdHotel { set; get; }
        public string Nombre { set; get; }
        public int Precio { set; get; }
        public int Impuesto { set; get; }
        public string Adicional { set; get; }
        public string incluye { set; get; }

    }
}
