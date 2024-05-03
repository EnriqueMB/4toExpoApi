using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public  class Clientes
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string? Nombre { get; set; }

        public int? IdReserva { get; set; }

        public string? Apellidos { get; set; }
        public string? Identificador { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public DateTime? FechaAlt { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }
    }
}
