

namespace _4toExpoApi.DataAccess.Response.Hotel
{
    public class HotelResponse
    {
        public int Id { get; set; }
        public string Nombre { set; get; }
        public string Tipo { set; get; }
        public string Ubicacion { set; get; }
        public int Telefono { set; get; }
        public string LinkWhatsapp { set; get; }
        public string CodigoReserva { set; get; }
        public string Correo { set; get; }
        public string Imagen { set; get; }
        public string? Tarifa { set; get; }

        public List<HabitacionResponse> listaHabitacion { set; get; }
        public List<DistanciaResponse> listaDistancia { set; get; }
    }
}
