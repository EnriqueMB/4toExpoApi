using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class UsuariosPromocion
    {
        [Key]
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        [MaxLength(250)]
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public string? Telefono { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? Asociacion { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public bool? Activo { get; set; }

        [ForeignKey("IdUsuario")]

        public Usuarios Usuarios { get; set; }

    }
}
