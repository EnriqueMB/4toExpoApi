

namespace _4toExpoApi.Core.ViewModels
{
    public class HotelVM
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

        public List<HabitacionVM> listaHabitacion { set; get; }
        public  List<DistanciaVM> listaDistancia { set; get; }
    }
}
