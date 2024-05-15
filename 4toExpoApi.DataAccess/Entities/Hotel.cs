using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Hotel: FieldsDefault
    {
        
       public Hotel() {
            Nombre = "";
            Tipo = "";
            Ubicacion = "";
            Telefono = 0;
            LinkWhatsapp = "";
            CodigoReserva = "";
            Correo = "";
            Imagen = "";
        }

        [Key]
        public int Id {set; get;}
        public string Nombre {set; get;}
        public string Tipo { set; get; }
        public string Ubicacion { set; get; }
        public int Telefono { set; get; }
        public string LinkWhatsapp { set; get; }
        public string CodigoReserva { set; get; }
        public string Correo { set; get; }
        public string Imagen { set; get; }
        public string? Tarifa { set; get; }

    }
}
