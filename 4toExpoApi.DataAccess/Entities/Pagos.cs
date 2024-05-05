using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Pagos
    {
        [Key]
        public int IdPago { get; set; }

        public int? IdReserva { get; set; }
        public string? IdTransaccion { get; set; }
        public string? TitularTarjeta { get; set; }
        public string? EmailTarjeta { get; set; }
        public int? Monto { get; set; }
        public string? StatusPago { get; set; }
        public string? RefBancaria { get; set; }
        public string? Pasarela { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }



    }
}
