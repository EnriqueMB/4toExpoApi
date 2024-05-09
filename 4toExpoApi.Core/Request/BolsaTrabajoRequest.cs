using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class BolsaTrabajoRequest
    {
        public int IdBolsaTrabajo { get; set; }
        public string Puesto { get; set; }
        public string Tipo { get; set; }
        public string Requisitos { get; set; }
        public string Descripcion { get; set; }
        public DateTime DiasLaborales { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinal { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
    }
}
