using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Reservas
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Producto { get; set; }
        public int? IdTipoPaquete { get; set; }
        public int? IdUsuario {  get; set; }
        public string? NombreCompleto { get; set; }
        public int? Monto { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool? Activo { get; set; }

       
    }
}
