
using _4toExpoApi.Core.ViewModels;

namespace _4toExpoApi.Core.Request
{
    public class HotelRequest
    {
        public string Nombre { set; get; }
        public string Tipo { set; get; }
        public string Ubicacion { set; get; }
        public int Telefono { set; get; }
        public string LinkWhatsapp { set; get; }
        public string CodigoReserva { set; get; }
        public string Correo { set; get; }
        public string Imagen { set; get; }
        public string? Tarifa { set; get; }

        public required List<RequestHabitacion> listaHabitacion { set; get; }
        public required List<RequestDistancia> listaDistancia { set; get; }

    }
}
