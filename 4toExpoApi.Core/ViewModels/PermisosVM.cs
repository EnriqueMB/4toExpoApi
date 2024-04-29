using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class PermisosVM
    {
        public long Id { get; set; }
        public string Permiso { get; set; }
        public long PermisoPapaId { get; set; }
        public long Orden { get; set; }
        public string Vista { get; set; }
        public bool EsPapa { get; set; }
        public int TipoPermiso { get; set; }
    }
}
