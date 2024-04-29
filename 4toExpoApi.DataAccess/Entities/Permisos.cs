using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Permisos
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        public string Permiso { get; set; }
        public long PermisoPapaId { get; set; }
        public long Orden { get; set; }
        [MaxLength(100)]
        public string Vista { get; set; }
        public bool EsPapa { get; set; }
        public int TipoPermiso { get; set; }
    }
}
