namespace _4toExpoApi.Core.Request
{
    public class ProgramaActividadesRequest
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
