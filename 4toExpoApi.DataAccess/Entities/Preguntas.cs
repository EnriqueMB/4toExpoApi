using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Preguntas
    {
        public int Id { get; set; }
        public string Pregunta { get; set; }
        public string Descripcion { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }

        public Boolean? Activo { get; set; }
    }
}
