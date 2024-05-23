using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class TranferRequest
    {
        public int? idReserva { get; set; }
        public string? Producto { get; set; }
        public string? Apellidos { get; set; }
        public int? IdTipoPaquete { get; set; }
        public string? Nombre { get; set; }
        public int? Monto { get; set; }
        public string? TipoPago { get; set; }
        public string? LogRequest { get; set; }
        public string? LogResponse { get; set; }
        public string? StatusReserva { get; set; }
        public int? Cantidad { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? IdTransaction { get; set; }
        public string? apiResponse { get; set; }
        public string? claveBancaria { get; set; }
        public string? banco { get; set; }
        public string? cuenta { get; set; }
        public string? baucherPago { get; set; }
        public IFormFile? ImgFile { get; set; }
    }
}
