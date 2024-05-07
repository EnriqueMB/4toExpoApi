using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace _4toExpoApi.DataAccess.Entities
{
    public class InformacionConfig
    {
        //PROPIEDADES DE LA ENTIDAD 
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Multimedia { get; set; }
        public string Texto { get; set; }

        //PROPIEDADES NECESARIAS DE LA ENTIDAD 
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public Boolean? Activo { get; set; }
    }
}
