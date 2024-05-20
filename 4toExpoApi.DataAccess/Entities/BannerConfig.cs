using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class BannerConfig
    {
        [Key]
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? SubTitulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public int? CantidadExpositores { get; set; }
        public int? CantidadParticipantes { get; set; }
        public int? Orden{ get; set; }
        public string? CenaGala { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }
        public int? Dias { get; set; }
        public int? CantidadConstructoras { get; set; }





    }
}
