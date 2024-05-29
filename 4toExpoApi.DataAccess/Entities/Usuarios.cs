using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string? NombreCompleto { get; set; }
        public int? Edad { get; set; }
        public int? IdUniversidad { get; set; }
        public int? Semestre { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? Telefono { get; set; }
        public string? UrlImg { get; set; }
        public string? Asociacion { get; set; }
        public string? ContactoEmergencia { get; set; }
        public string? Alergia { get; set; }
        public bool? Asesoria { get; set; }
        public string? Sugerencia { get; set; }

        [MaxLength(250)]
        public string Correo { get; set; }
        public int IdTipoUsuario { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }
    }
}
