using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{

    public class Patrocinadores
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string NombreEmpresa { get; set; }

        public string? UrlLogo { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }

        public Boolean? Activo { get; set; }

        public int? IdUsuario { get; set; }

        [ForeignKey ("IdUsuario")]
        public virtual Usuarios? Usuarios { get; set; }

    }
}
