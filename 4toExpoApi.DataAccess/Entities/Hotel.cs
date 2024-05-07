using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Hotel
    {
        
       /** public Hotel() {
            Nombre = "";
            Tipo = "";
            Ubicacion = "";
            Telefono = 0;
            LinkWhatsapp = "";
            CodigoReserva = "";
            Imagen = "";
        }**/

        [Key]
        public int Id {set; get;}
        public string Nombre {set; get;}
        public string Tipo { set; get; }
        public string Ubicacion { set; get; }
        public int Telefono { set; get; }
        public string LinkWhatsapp { set; get; }
        public string CodigoReserva { set; get; }
        public string Imagen { set; get; }
        public string? Tarifa { set; get; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }

    }
}
