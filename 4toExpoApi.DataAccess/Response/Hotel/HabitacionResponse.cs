
namespace _4toExpoApi.DataAccess.Response.Hotel
{
    public class HabitacionResponse
    {
        public int Id { get; set; }
        public string Nombre { set; get; }
        public int? Precio { set; get; }
        public int? Impuesto { set; get; }
        public string? Adicional { set; get; }
        public string? incluye { set; get; }
    }
}
