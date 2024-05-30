using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class BolsaTrabajo
    {

        [Key]

        public int IdBolsaTrabajo { get; set; }
        public string? Puesto { get; set; }
        public string? Tipo { get; set; }
        public string? Requisitos { get; set; }
        public string? Descripcion { get; set; }
        public string? DiasLaborales { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int IdPatrocinador { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaUpd { get; set; }
        public int? UserUpd { get; set; }
        public bool? Activo { get; set; }

    }
}
