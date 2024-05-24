namespace _4toExpoApi.Core.ViewModels
{
    public class ProgramaActividadesVM
    {
        public int IdProgramaActividades { get; set; }
        public string? Orden { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public string? Detalles { get; set; }

    }
}
