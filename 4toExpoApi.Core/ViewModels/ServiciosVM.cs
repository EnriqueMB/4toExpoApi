using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class ServiciosVM
    {
        public int Id { get; set; }
        public string Servicio { get; set; }
        public string Descripcion { get; set; }
        public string DiasLaborales { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public int IdPatrocinador { get; set; }

    }
}
