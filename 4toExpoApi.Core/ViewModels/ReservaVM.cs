

using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;

namespace _4toExpoApi.Core.ViewModels
{
    public class ReservaVM
    {
        //Paquete
        public string? NombrePaquete { get; set; }
        public int? IdTipoPaquete { get; set; }
        public decimal? Monto { get; set; }
        public string? Descripcion { get; set; }
        public List<IncluyePaqueteRequest>? Beneficios { get; set; }   

        //Usuario
        public int? IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public int? Edad { get; set; }
        public string? Telefono { get; set; }
        public string? Correo {  get; set; }

    }
}
