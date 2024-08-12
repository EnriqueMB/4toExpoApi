using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Estados
    {
        [Key]
        public string Clave { get; set; }
        public string Estado { get; set; }
        public string Abreviatura { get; set; }
    }
}
